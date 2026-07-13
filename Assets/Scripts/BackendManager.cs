using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class BackendManager : MonoBehaviour
{
    [Header("Backend")]
    [SerializeField] private string baseURL = "http://localhost:8080";

    [Header("UI")]
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TextMeshProUGUI statusText;

    private const string JwtTokenKey = "jwt_token";
    private const string UsernameKey = "username";
    private const string IsGuestKey = "is_guest";

    [Serializable]
    public class AuthRequest
    {
        public string username;
        public string password;
    }

    [Serializable]
    public class AuthResponse
    {
        public string token;
    }

    [Serializable]
    public class ErrorResponse
    {
        public string error;
        public string message;
    }

    private void Start()
    {
        SetStatus("");

        if (!GameSession.UseBackend)
        {
            EnterGuestMode();
            LoadMainMenu();
            return;
        }


        bool savedGuestMode = PlayerPrefs.GetInt(IsGuestKey, 0) == 1;

        if (savedGuestMode)
        {
            GameSession.IsGuest = true;
            GameSession.Token = "";
            GameSession.Username = "";
            GameSession.Progress = null;
            GameSession.SelectedLevelNumber = 0;

            Debug.Log("Guest mode detected. Skipping auto-login.");
            return;
        }

        StartCoroutine(TryAutoLogin());
    }

    private void EnterGuestMode()
    {
        GameSession.IsGuest = true;
        GameSession.Token = "";
        GameSession.Username = "Guest";
        GameSession.Progress = null;
        GameSession.SelectedLevelNumber = 0;

        PlayerPrefs.SetInt(IsGuestKey, 1);
        PlayerPrefs.DeleteKey(JwtTokenKey);
        PlayerPrefs.DeleteKey(UsernameKey);
        PlayerPrefs.Save();

        Debug.Log("Backend disabled. Entering guest mode.");
    }

    public void Login()
    {
        if (!GameSession.UseBackend)
        {
            SetStatus("Backend disabled in WebGL build.");
            return;
        }

        StartCoroutine(AuthCoroutine("/api/auth/login", false));
    }

    public void Register()
    {
        if (!GameSession.UseBackend)
        {
            SetStatus("Backend disabled in WebGL build.");
            return;
        }

        StartCoroutine(AuthCoroutine("/api/auth/register", true));
    }

    public void OnPlayAsGuest()
    {
        EnterGuestMode();
        LoadMainMenu();
    }

    private IEnumerator TryAutoLogin()
    {
        string savedToken = PlayerPrefs.GetString(JwtTokenKey, "");
        string savedUsername = PlayerPrefs.GetString(UsernameKey, "");

        if (string.IsNullOrWhiteSpace(savedToken))
        {
            yield break;
        }

        GameSession.IsGuest = false;
        GameSession.Token = savedToken;
        GameSession.Username = savedUsername;

        SetStatus("Loading account...");

        UnityWebRequest request = UnityWebRequest.Get(baseURL + "/api/player/progress");
        request.SetRequestHeader("Authorization", "Bearer " + savedToken);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            if (request.responseCode == 0)
            {
                Debug.LogWarning("Backend is offline. Keeping saved token.");
                SetStatus("Server offline. Please try again later.");
                yield break;
            }

            if (request.responseCode == 401 || request.responseCode == 403)
            {
                ClearLoggedUserSession();
                SetStatus("");
                yield break;
            }

            SetStatus("Something went wrong.");
            Debug.LogError("Auto-login failed: " + request.responseCode + " " + request.downloadHandler.text);
            yield break;
        }

        PlayerProgressResponse progress =
            JsonUtility.FromJson<PlayerProgressResponse>(request.downloadHandler.text);

        if (progress == null || progress.levels == null)
        {
            Debug.LogError("Invalid progress response: " + request.downloadHandler.text);

            ClearLoggedUserSession();
            SetStatus("");
            yield break;
        }

        GameSession.Progress = progress;

        if (MainGameManager.Instance != null)
        {
            MainGameManager.Instance.SyncFromBackendProgress();
        }

        Debug.Log("Auto-login successful. Max unlocked level: " + progress.maxLevelUnlocked);

        LoadMainMenu();
    }

    private IEnumerator AuthCoroutine(string endpoint, bool isRegister)
    {
        string username = usernameInput.text.Trim();
        string password = passwordInput.text;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            SetStatus("Username and password are required.");
            yield break;
        }

        if (!IsValidUsername(username))
        {
            SetStatus("Username: 3-20 letters, numbers or underscore.");
            yield break;
        }

        if (!IsValidPassword(password))
        {
            SetStatus("Password: 4-30 characters, no spaces.");
            yield break;
        }

        AuthRequest authRequest = new AuthRequest
        {
            username = username,
            password = password
        };

        string json = JsonUtility.ToJson(authRequest);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(baseURL + endpoint, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        SetStatus("Connecting...");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            string friendlyMessage = GetFriendlyErrorMessage(request.downloadHandler.text, request.responseCode);
            SetStatus(friendlyMessage);

            Debug.LogError("Auth error: " + request.responseCode + " " + request.downloadHandler.text);
            yield break;
        }

        AuthResponse response = JsonUtility.FromJson<AuthResponse>(request.downloadHandler.text);

        if (response == null || string.IsNullOrWhiteSpace(response.token))
        {
            SetStatus("Invalid server response.");
            Debug.LogError("Server response did not contain a token: " + request.downloadHandler.text);
            yield break;
        }

        GameSession.IsGuest = false;
        GameSession.Token = response.token;
        GameSession.Username = username;
        GameSession.Progress = null;
        GameSession.SelectedLevelNumber = 0;

        PlayerPrefs.SetInt(IsGuestKey, 0);
        PlayerPrefs.SetString(JwtTokenKey, response.token);
        PlayerPrefs.SetString(UsernameKey, username);
        PlayerPrefs.Save();

        SetStatus(isRegister ? "Account created. Loading progress..." : "Login successful. Loading progress...");

        Debug.Log("Token saved.");

        yield return StartCoroutine(LoadProgressAndEnterGame());
    }

    private IEnumerator LoadProgressAndEnterGame()
    {
        if (GameSession.IsGuest)
        {
            Debug.Log("Guest mode active. Skipping backend progress loading.");
            LoadMainMenu();
            yield break;
        }

        string token = GetToken();

        if (string.IsNullOrWhiteSpace(token))
        {
            SetStatus("Missing token.");
            yield break;
        }

        UnityWebRequest request = UnityWebRequest.Get(baseURL + "/api/player/progress");
        request.SetRequestHeader("Authorization", "Bearer " + token);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            SetStatus("Invalid progress data.");
            Debug.LogError("Invalid progress response: " + request.responseCode + " " + request.downloadHandler.text);
            yield break;
        }

        PlayerProgressResponse progress =
            JsonUtility.FromJson<PlayerProgressResponse>(request.downloadHandler.text);

        if (progress == null || progress.levels == null)
        {
            SetStatus("Invalid progress data.");
            Debug.LogError("Invalid progress response: " + request.downloadHandler.text);
            yield break;
        }

        GameSession.Progress = progress;

        if (MainGameManager.Instance != null)
        {
            MainGameManager.Instance.SyncFromBackendProgress();
        }

        Debug.Log("Progress loaded. Max level: " + progress.maxLevelUnlocked);

        LoadMainMenu();
    }

    private void LoadMainMenu()
    {
        if (SceneTransitionerManager.Instance != null)
        {
            SceneTransitionerManager.Instance.LoadScene("MainMenu");
            return;
        }

        SceneManager.LoadScene("MainMenu");
    }

    private void ClearLoggedUserSession()
    {
        PlayerPrefs.DeleteKey(JwtTokenKey);
        PlayerPrefs.DeleteKey(UsernameKey);
        PlayerPrefs.SetInt(IsGuestKey, 0);
        PlayerPrefs.Save();

        GameSession.IsGuest = false;
        GameSession.Token = "";
        GameSession.Username = "";
        GameSession.Progress = null;
        GameSession.SelectedLevelNumber = 0;
    }

    private bool IsValidUsername(string username)
    {
        return Regex.IsMatch(username, "^[a-zA-Z0-9_]{3,20}$");
    }

    private bool IsValidPassword(string password)
    {
        return Regex.IsMatch(password, @"^\S{4,30}$");
    }

    private string GetFriendlyErrorMessage(string responseText, long statusCode)
    {
        if (statusCode == 0)
        {
            return "Cannot connect to server.";
        }

        if (string.IsNullOrWhiteSpace(responseText))
        {
            return "Something went wrong.";
        }

        try
        {
            ErrorResponse errorResponse = JsonUtility.FromJson<ErrorResponse>(responseText);

            if (errorResponse != null && !string.IsNullOrWhiteSpace(errorResponse.message))
            {
                return errorResponse.message;
            }
        }
        catch
        {
        }

        string lowerResponse = responseText.ToLower();

        if (lowerResponse.Contains("username already exists"))
        {
            return "Username already exists.";
        }

        if (lowerResponse.Contains("invalid username or password"))
        {
            return "Invalid username or password.";
        }

        if (lowerResponse.Contains("must not be blank"))
        {
            return "Username and password are required.";
        }

        if (lowerResponse.Contains("username"))
        {
            return "Invalid username.";
        }

        if (lowerResponse.Contains("password"))
        {
            return "Invalid password.";
        }

        if (statusCode == 400)
        {
            return "Invalid input.";
        }

        if (statusCode == 401)
        {
            return "Invalid username or password.";
        }

        if (statusCode == 403)
        {
            return "Access denied.";
        }

        if (statusCode == 409)
        {
            return "Username already exists.";
        }

        return "Something went wrong.";
    }

    public string GetToken()
    {
        if (GameSession.IsGuest)
        {
            return "";
        }

        return PlayerPrefs.GetString(JwtTokenKey, "");
    }

    private void SetStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }

        if (!string.IsNullOrWhiteSpace(message))
        {
            Debug.Log(message);
        }
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game...");

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
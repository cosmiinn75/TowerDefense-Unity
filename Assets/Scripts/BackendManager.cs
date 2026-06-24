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
        StartCoroutine(TryAutoLogin());
    }

    public void Login()
    {
        StartCoroutine(AuthCoroutine("/api/auth/login", false));
    }

    public void Register()
    {
        StartCoroutine(AuthCoroutine("/api/auth/register", true));
    }

    private IEnumerator TryAutoLogin()
    {
        string savedToken = PlayerPrefs.GetString("jwt_token", "");
        string savedUsername = PlayerPrefs.GetString("username", "");
        if (string.IsNullOrWhiteSpace(savedToken))
        {
            yield break;
        }

        GameSession.Token = savedToken;
        GameSession.Username = savedUsername;
        SetStatus("Loading account");
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
                PlayerPrefs.DeleteKey("jwt_token");
                PlayerPrefs.DeleteKey("username");
                PlayerPrefs.Save();

                GameSession.Token = "";
                GameSession.Username = "";
                GameSession.Progress = null;
                GameSession.SelectedLevelNumber = 0;

                SetStatus("");
                yield break;
            }

            SetStatus("Something went wrong.");
            yield break;
        }
        PlayerProgressResponse progress =
      JsonUtility.FromJson<PlayerProgressResponse>(request.downloadHandler.text);

        if (progress == null || progress.levels == null)
        {
            Debug.LogError("Invalid progress response: " + request.downloadHandler.text);

            PlayerPrefs.DeleteKey("jwt_token");
            PlayerPrefs.Save();

            GameSession.Token = "";
            GameSession.Progress = null;
            GameSession.SelectedLevelNumber = 0;

            SetStatus("");
            yield break;
        }

        GameSession.Progress = progress;

        if (MainGameManager.Instance != null)
        {
            MainGameManager.Instance.SyncFromBackendProgress();
        }

        Debug.Log("Auto-login successful. Max unlocked level: " + progress.maxLevelUnlocked);

        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator AuthCoroutine(string endpoint, bool isRegister)
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            SetStatus("");
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

        PlayerPrefs.SetString("jwt_token", response.token);
        PlayerPrefs.SetString("username", username);
        PlayerPrefs.Save();

        GameSession.Token = response.token;
        GameSession.Username = username;

        SetStatus(isRegister ? "Account created. Loading progress..." : "Login successful. Loading progress...");

        Debug.Log("Token saved: " + response.token);

        yield return StartCoroutine(LoadProgressAndEnterGame());
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
        return PlayerPrefs.GetString("jwt_token", "");
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

    private IEnumerator LoadProgressAndEnterGame()
    {
        string token = GetToken();

        if (string.IsNullOrWhiteSpace(token)){
            SetStatus("Missing token.");
            yield break;
        }

        UnityWebRequest request = UnityWebRequest.Get(baseURL + "/api/player/progress");
        request.SetRequestHeader("Authorization", "Bearer " + token);
        yield return request.SendWebRequest();

        if(request.result != UnityWebRequest.Result.Success)
        {
            SetStatus("Invalid progress data.");
            Debug.LogError("Invalid progress response: " + request.downloadHandler.text);
            yield break;
        }
        PlayerProgressResponse progress = JsonUtility.FromJson<PlayerProgressResponse>(request.downloadHandler.text);
        if (progress == null || progress.levels == null)
        {
            SetStatus("Invalid progress data.");
            Debug.LogError("Invalid progress response: " + request.downloadHandler.text);
            yield break;
        }
        GameSession.Progress = progress;

        Debug.Log("Progress loaded. Max level: " + progress.maxLevelUnlocked);

        SceneManager.LoadScene("MainMenu");
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
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : Menu
{
    [Header("Menus")]
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject resetMenu;
    [SerializeField] private Button resetButton;
    [SerializeField] private GameObject tutorialMenu;
    [SerializeField] private TutorialManager tutorialManager;
    [SerializeField] private GameObject audioMenu;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI usernameText;
    [SerializeField] private Button signOutButton;

    [Header("Scenes")]
    [SerializeField] private string worldMapSceneName = "WorldMap";
    [SerializeField] private string loginSceneName = "Login";

    private const string JwtTokenKey = "jwt_token";
    private const string UsernameKey = "username";
    private const string IsGuestKey = "is_guest";

    private bool openWorldMapAfterTutorial = false;

    private void Start()
    {
        SyncSessionFromPlayerPrefs();
        UpdateUsernameText();
        UpdateSignOutButtonState();

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.backgroundClip);
        }

        if (settingsMenu != null)
        {
            settingsMenu.SetActive(false);
        }

        if (resetMenu != null)
        {
            resetMenu.SetActive(false);
        }

        if (tutorialMenu != null)
        {
            tutorialMenu.SetActive(false);
        }

        if (audioMenu != null)
        {
            audioMenu.SetActive(false);
        }

        if (tutorialManager != null)
        {
            tutorialManager.OnTutorialClosed += HandleTutorialClosed;
        }
    }

    private void OnDestroy()
    {
        if (tutorialManager != null)
        {
            tutorialManager.OnTutorialClosed -= HandleTutorialClosed;
        }
    }

    private void SyncSessionFromPlayerPrefs()
    {
        bool savedGuestMode = PlayerPrefs.GetInt(IsGuestKey, 0) == 1;

#if UNITY_WEBGL && !UNITY_EDITOR
        savedGuestMode = true;
#endif

        if (savedGuestMode)
        {
            GameSession.IsGuest = true;
            GameSession.Token = "";
            GameSession.Username = "Guest";
            GameSession.Progress = null;
            GameSession.SelectedLevelNumber = 0;

            PlayerPrefs.SetInt(IsGuestKey, 1);
            PlayerPrefs.Save();

            return;
        }

        GameSession.IsGuest = false;

        if (string.IsNullOrWhiteSpace(GameSession.Username))
        {
            GameSession.Username = PlayerPrefs.GetString(UsernameKey, "");
        }

        if (string.IsNullOrWhiteSpace(GameSession.Token))
        {
            GameSession.Token = PlayerPrefs.GetString(JwtTokenKey, "");
        }
    }

    private void UpdateUsernameText()
    {
        if (usernameText == null)
        {
            return;
        }

        if (GameSession.IsGuest)
        {
            usernameText.text = "Username: Guest";
            return;
        }

        string username = GameSession.Username;

        if (string.IsNullOrWhiteSpace(username))
        {
            username = PlayerPrefs.GetString(UsernameKey, "");
        }

        if (string.IsNullOrWhiteSpace(username))
        {
            usernameText.text = "Username: Unknown";
        }
        else
        {
            usernameText.text = "Username: " + username;
        }
    }

    private void UpdateSignOutButtonState()
    {
        if (signOutButton == null)
        {
            return;
        }

        signOutButton.interactable = CanUseSignOut();
    }

    private bool CanUseSignOut()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        return false;
#else
        return !GameSession.IsGuest;
#endif
    }

    public void OnWorldMap()
    {
        if (!TutorialManager.HasSeenCurrentTutorial())
        {
            openWorldMapAfterTutorial = true;

            if (tutorialMenu != null)
            {
                tutorialMenu.SetActive(true);
            }

            return;
        }

        LoadWorldMap();
    }

    public void SignOut()
    {
        if (!CanUseSignOut())
        {
            Debug.Log("Sign out is disabled in guest mode.");
            return;
        }

        if (GameSession.IsGuest)
        {
            SignOutGuest();
        }
        else
        {
            SignOutLoggedUser();
        }

        Time.timeScale = 1f;
        LoadLoginScene();
    }

    private void SignOutGuest()
    {
        PlayerPrefs.SetInt(IsGuestKey, 0);
        PlayerPrefs.Save();

        GameSession.IsGuest = false;
        GameSession.Token = "";
        GameSession.Username = "";
        GameSession.Progress = null;
        GameSession.SelectedLevelNumber = 0;

        Debug.Log("Signed out from guest mode.");
    }

    private void SignOutLoggedUser()
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

        Debug.Log("Signed out from logged account.");
    }

    public void OnTutorial()
    {
        openWorldMapAfterTutorial = false;

        if (tutorialMenu != null)
        {
            tutorialMenu.SetActive(true);
        }
    }

    private void HandleTutorialClosed()
    {
        if (openWorldMapAfterTutorial)
        {
            openWorldMapAfterTutorial = false;
            LoadWorldMap();
        }
    }

    private void LoadWorldMap()
    {
        if (SceneTransitionerManager.Instance != null)
        {
            SceneTransitionerManager.Instance.LoadScene(worldMapSceneName);
            return;
        }

        SceneManager.LoadScene(worldMapSceneName);
    }

    private void LoadLoginScene()
    {
        if (SceneTransitionerManager.Instance != null)
        {
            SceneTransitionerManager.Instance.LoadScene(loginSceneName);
            return;
        }

        SceneManager.LoadScene(loginSceneName);
    }

    public void OnSettings()
    {
        if (settingsMenu != null)
        {
            settingsMenu.SetActive(true);
        }

        if (resetButton != null)
        {
            resetButton.interactable = false;
        }
    }

    public void OnBackSettings()
    {
        if (settingsMenu != null)
        {
            settingsMenu.SetActive(false);
        }

        if (resetButton != null)
        {
            resetButton.interactable = true;
        }
    }

    public void OnExit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OnReset()
    {
        if (resetMenu != null)
        {
            resetMenu.SetActive(true);
        }
    }

    public void OnAudio()
    {
        if (settingsMenu != null)
        {
            settingsMenu.SetActive(false);
        }

        if (audioMenu != null)
        {
            audioMenu.SetActive(true);
        }
    }

    public void OnBackAudio()
    {
        if (settingsMenu != null)
        {
            settingsMenu.SetActive(true);
        }

        if (audioMenu != null)
        {
            audioMenu.SetActive(false);
        }
    }
}
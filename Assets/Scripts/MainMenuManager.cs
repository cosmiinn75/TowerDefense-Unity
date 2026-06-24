using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : Menu
{
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject resetMenu;
    [SerializeField] private Button resetButton;
    [SerializeField] private GameObject tutorialMenu;
    [SerializeField] private TutorialManager tutorialManager;
    [SerializeField] private GameObject audioMenu;
    [SerializeField] private TextMeshProUGUI usernameText;

    [SerializeField] private string worldMapSceneName = "WorldMap";
    [SerializeField] private string loginSceneName = "Login";

    private bool openWorldMapAfterTutorial = false;

    private void Start()
    {
        usernameText.text = "Username: " + GameSession.Username;
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.backgroundClip);
        }

        settingsMenu.SetActive(false);
        resetMenu.SetActive(false);
        tutorialMenu.SetActive(false);
        audioMenu.SetActive(false);

        tutorialManager.OnTutorialClosed += HandleTutorialClosed;
    }

    public void OnWorldMap()
    {
        if (!TutorialManager.HasSeenCurrentTutorial())
        {
            openWorldMapAfterTutorial = true;
            tutorialMenu.SetActive(true);
            return;
        }

        LoadWorldMap();
    }

    public void SignOut()
    {
        PlayerPrefs.DeleteKey("jwt_token");
        PlayerPrefs.DeleteKey("username");
        PlayerPrefs.Save();

        GameSession.Token = "";
        GameSession.Username = "";
        GameSession.Progress = null;
        GameSession.SelectedLevelNumber = 0;

        Time.timeScale = 1f;

        if(SceneTransitionerManager.Instance != null)
        {
            SceneTransitionerManager.Instance.LoadScene(loginSceneName);
            return;
        }
        SceneManager.LoadScene(loginSceneName);
    }


    public void OnTutorial()
    {
        openWorldMapAfterTutorial = false;
        tutorialMenu.SetActive(true);
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
        if(SceneTransitionerManager.Instance != null)
        {
            SceneTransitionerManager.Instance.LoadScene(worldMapSceneName);
            return;
        }
        SceneManager.LoadScene(worldMapSceneName);
    }

    public void OnSettings()
    {
        settingsMenu.SetActive(true);
        resetButton.interactable = false;
    }

    public void OnBackSettings()
    {
        settingsMenu.SetActive(false);
        resetButton.interactable = true;
    }

    public void OnExit()
    {
        Application.Quit();
    }

    public void OnReset()
    {
        resetMenu.SetActive(true);
    }

    public void OnAudio()
    {
        settingsMenu.SetActive(false);
        audioMenu.SetActive(true);
    }

    public void OnBackAudio()
    {
        settingsMenu.SetActive(true);
        audioMenu.SetActive(false);
    }

}
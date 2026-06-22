using System;
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

    [SerializeField] private string worldMapSceneName = "WorldMap";

    private bool openWorldMapAfterTutorial = false;

    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.backgroundClip);
        }

        settingsMenu.SetActive(false);
        resetMenu.SetActive(false);
        tutorialMenu.SetActive(false);

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
        SceneManager.LoadScene(worldMapSceneName);
    }

    public void OnSettings()
    {
        settingsMenu.SetActive(true);
        resetButton.interactable = false;
    }

    public void OnBack()
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
}
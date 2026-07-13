using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : Menu
{
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject tutorialMenu;
    private void Start()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
        tutorialMenu.SetActive(false);
    }
   
    public void OnSettings()
    {
        pauseMenu.SetActive(false);
        tutorialMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
    public void OnTutorial()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
        tutorialMenu.SetActive(true);
    }


    public void OnBackFromTutorial()
    {
        if (tutorialMenu != null)
        {
            tutorialMenu.SetActive(false);
        }
    }
    public void OnBack() {

        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void OnResume()
    {
        OnBack();
        if (GameManager.Instance != null)
        {
            Time.timeScale = GameManager.Instance.currentSpeed;
        }
        else
        {
            Time.timeScale = 1f;
        }
        GameManager.Instance.isPaused = false;
        gameObject.SetActive(false);
    }

    public void OnMasterChanged(float sliderValue)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMasterVolume(sliderValue);
        }
    }
    public void OnMusicChanged(float sliderValue)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(sliderValue);
        }
    }
    public void OnSFXChanged(float sliderValue)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(sliderValue);
        }
    }

}

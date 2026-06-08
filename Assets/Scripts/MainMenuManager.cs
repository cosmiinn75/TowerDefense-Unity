using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject resetMenu;
    [SerializeField] Button resetButton;
    private void Start()
    {
        settingsMenu.SetActive(false);
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
    public void OnWorldMap()
    {
        SceneManager.LoadScene("WorldMap");
        Time.timeScale = 1f;
    }
    
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject settingsMenu;
    private void Start()
    {
        settingsMenu.SetActive(false);
    }
    public void OnSettings()
    {

        settingsMenu.SetActive(true);

    }

    public void OnExit()
    {
        Application.Quit();
    }
    
    public void OnWorldMap()
    {
        SceneManager.LoadScene("WorldMap");
        Time.timeScale = 1f;
    }
    
}

using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Menu : MonoBehaviour
{
    public void OnWorldMap()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("WorldMap");
    }
    public void OnMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void OnRetry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnNextLevel()
    {
        Time.timeScale = 1f;


        int nextIndex = GameManager.Instance.currentLevelIndex + 1;


        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            PlayerPrefs.SetInt("LastAccesedLevel", nextIndex);
            PlayerPrefs.Save();
            SceneManager.LoadScene(nextIndex);
        }
    }
}

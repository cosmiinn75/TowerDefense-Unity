using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Menu : MonoBehaviour
{
    public void OnWorldMap()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
        }
        Time.timeScale = 1f;
        SceneTransitionerManager.Instance?.LoadScene("WorldMap");
    }
    public void OnMainMenu()
    {
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
        }
        Time.timeScale = 1f;
        SceneTransitionerManager.Instance?.LoadScene("MainMenu");
    }
    public void OnRetry()
    {
        Time.timeScale = 1f;
        SceneTransitionerManager.Instance?.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnNextLevel()
    {
        Time.timeScale = 1f;
        int nextIndex = GameManager.Instance.currentLevelIndex + 1;


        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            PlayerPrefs.SetInt("LastAccesedLevel", nextIndex);
            PlayerPrefs.Save();
            SceneTransitionerManager.Instance?.LoadScene("Level" + nextIndex);
        }
    }
}

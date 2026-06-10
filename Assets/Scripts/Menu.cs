using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Menu : MonoBehaviour
{
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
    public void OnMenu(string menuName)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
        }
        Time.timeScale = 1f;
        SceneTransitionerManager.Instance?.LoadScene(menuName);
    }

}


using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetMenuManager : MonoBehaviour
{
    public void DeleteAllProgress() {

        PlayerPrefs.DeleteKey("MaxLevelReached");
        PlayerPrefs.DeleteKey("LastAccessedLevel");
        int levelButtons = 10;

        for(int i = 1; i <= levelButtons; i++)
        {
            PlayerPrefs.DeleteKey("StarsUnlocked" + i);
        }
        if(MainGameManager.Instance != null)
        {
            MainGameManager.Instance.maxLevelReached = 1;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}

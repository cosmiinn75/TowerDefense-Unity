using UnityEngine;

public class MainGameManager : MonoBehaviour
{
    public static MainGameManager Instance;
    public int maxLevelReached = 1;
    [HideInInspector] public bool justUnlockedLevel;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            maxLevelReached = PlayerPrefs.GetInt("MaxLevelReached", 1);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UnlockNextLevel(int completedLevelIndex) {
        if (completedLevelIndex == maxLevelReached) {
            maxLevelReached++;
            justUnlockedLevel = true;
            PlayerPrefs.SetInt("MaxLevelReached", maxLevelReached);
            PlayerPrefs.Save();
        }
    }

}

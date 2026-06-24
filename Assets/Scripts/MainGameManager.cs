using UnityEngine;

public class MainGameManager : MonoBehaviour
{
    public static MainGameManager Instance;

    public int maxLevelReached = 1;

    [HideInInspector] public bool justUnlockedLevel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

  
            DontDestroyOnLoad(gameObject);


            SyncFromBackendProgress();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SyncFromBackendProgress()
    {
        if (GameSession.Progress != null)
        {
            maxLevelReached = GameSession.Progress.maxLevelUnlocked;
        }
        else
        {
            maxLevelReached = 1;
        }
    }

    public void SetProgressFromBackend(PlayerProgressResponse progress)
    {
        int oldMaxLevel = maxLevelReached;

        GameSession.Progress = progress;
        maxLevelReached = progress.maxLevelUnlocked;

        justUnlockedLevel = maxLevelReached > oldMaxLevel;
    }


    public void UnlockNextLevel(int completedLevelIndex)
    {
        Debug.LogWarning("UnlockNextLevel is deprecated. Progress should be saved through backend.");

        if (completedLevelIndex == maxLevelReached)
        {
            maxLevelReached++;
            justUnlockedLevel = true;
        }
    }
}
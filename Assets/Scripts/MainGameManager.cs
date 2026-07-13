using UnityEngine;

public class MainGameManager : MonoBehaviour
{
    public static MainGameManager Instance;

    public int maxLevelReached = 1;

    [HideInInspector] public bool justUnlockedLevel;

    [Header("Campaign")]
    [SerializeField] private int maxCampaignLevel = 10;

    private const string IsGuestKey = "is_guest";
    private const string GuestMaxLevelUnlockedKey = "guest_max_level_unlocked";
    private const string GuestLevelStarsPrefix = "guest_level_";
    private const string GuestLevelStarsSuffix = "_stars";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SyncProgress();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SyncProgress()
    {
        SyncGuestStateFromPlayerPrefs();

        if (GameSession.IsGuest)
        {
            SyncFromGuestProgress();
        }
        else
        {
            SyncFromBackendProgress();
        }
    }

    private void SyncGuestStateFromPlayerPrefs()
    {
        bool savedGuestMode = PlayerPrefs.GetInt(IsGuestKey, 0) == 1;

        if (savedGuestMode)
        {
            GameSession.IsGuest = true;
            GameSession.Token = "";
            GameSession.Username = "Guest";
            GameSession.Progress = null;
        }
    }

    public void SyncFromBackendProgress()
    {
        if (GameSession.IsGuest)
        {
            SyncFromGuestProgress();
            return;
        }

        if (GameSession.Progress != null)
        {
            maxLevelReached = Mathf.Clamp(
                GameSession.Progress.maxLevelUnlocked,
                1,
                maxCampaignLevel
            );
        }
        else
        {
            maxLevelReached = 1;
        }

        justUnlockedLevel = false;
    }

    public void SetProgressFromBackend(PlayerProgressResponse progress)
    {
        if (progress == null)
        {
            Debug.LogWarning("Cannot set backend progress. Progress is null.");
            return;
        }

        int oldMaxLevel = maxLevelReached;

        GameSession.IsGuest = false;
        GameSession.Progress = progress;

        PlayerPrefs.SetInt(IsGuestKey, 0);
        PlayerPrefs.Save();

        maxLevelReached = Mathf.Clamp(
            progress.maxLevelUnlocked,
            1,
            maxCampaignLevel
        );

        justUnlockedLevel = maxLevelReached > oldMaxLevel;
    }

    public void SyncFromGuestProgress()
    {
        int oldMaxLevel = maxLevelReached;

        maxLevelReached = PlayerPrefs.GetInt(GuestMaxLevelUnlockedKey, 1);
        maxLevelReached = Mathf.Clamp(maxLevelReached, 1, maxCampaignLevel);

        justUnlockedLevel = maxLevelReached > oldMaxLevel;
    }

    public void SaveGuestLevelResult(int completedLevelIndex, int earnedStars)
    {
        if (completedLevelIndex < 1 || completedLevelIndex > maxCampaignLevel)
        {
            Debug.LogWarning("Invalid guest level index: " + completedLevelIndex);
            return;
        }

        earnedStars = Mathf.Clamp(earnedStars, 0, 3);

        GameSession.IsGuest = true;
        GameSession.Token = "";
        GameSession.Username = "Guest";
        GameSession.Progress = null;

        PlayerPrefs.SetInt(IsGuestKey, 1);

        string starsKey = GetGuestStarsKey(completedLevelIndex);
        int previousStars = PlayerPrefs.GetInt(starsKey, 0);

        if (earnedStars > previousStars)
        {
            PlayerPrefs.SetInt(starsKey, earnedStars);
        }

        int oldMaxLevel = PlayerPrefs.GetInt(GuestMaxLevelUnlockedKey, 1);
        int newMaxLevel = oldMaxLevel;

        if (completedLevelIndex >= oldMaxLevel && completedLevelIndex < maxCampaignLevel)
        {
            newMaxLevel = completedLevelIndex + 1;
            PlayerPrefs.SetInt(GuestMaxLevelUnlockedKey, newMaxLevel);
        }

        PlayerPrefs.Save();

        maxLevelReached = Mathf.Clamp(newMaxLevel, 1, maxCampaignLevel);
        justUnlockedLevel = maxLevelReached > oldMaxLevel;

        Debug.Log(
            "Guest progress saved. Level: " +
            completedLevelIndex +
            ", Stars: " +
            earnedStars +
            ", Max unlocked: " +
            maxLevelReached
        );
    }

    public int GetGuestStarsForLevel(int levelNumber)
    {
        if (levelNumber < 1 || levelNumber > maxCampaignLevel)
        {
            return 0;
        }

        return PlayerPrefs.GetInt(GetGuestStarsKey(levelNumber), 0);
    }

    private string GetGuestStarsKey(int levelNumber)
    {
        return GuestLevelStarsPrefix + levelNumber + GuestLevelStarsSuffix;
    }

    public void ResetGuestProgress()
    {
        PlayerPrefs.DeleteKey(GuestMaxLevelUnlockedKey);

        for (int i = 1; i <= maxCampaignLevel; i++)
        {
            PlayerPrefs.DeleteKey(GetGuestStarsKey(i));
        }

        PlayerPrefs.Save();

        maxLevelReached = 1;
        justUnlockedLevel = false;

        Debug.Log("Guest progress reset.");
    }

    public void UnlockNextLevel(int completedLevelIndex)
    {
        if (GameSession.IsGuest)
        {
            SaveGuestLevelResult(completedLevelIndex, 1);
            return;
        }

        Debug.LogWarning("UnlockNextLevel is deprecated. Logged user progress should be saved through backend.");

        if (completedLevelIndex == maxLevelReached && maxLevelReached < maxCampaignLevel)
        {
            maxLevelReached++;
            justUnlockedLevel = true;
        }
    }
}
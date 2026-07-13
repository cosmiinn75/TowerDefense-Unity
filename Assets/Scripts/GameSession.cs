using System;

public static class GameSession
{
    public static string Token;
    public static string Username;

    public static PlayerProgressResponse Progress;

    public static int SelectedLevelNumber;

    public static bool IsGuest = false;


    public static bool UseBackend
    {
        get
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return false;
#else
            return true;
#endif
        }
    }

}

[Serializable]
public class PlayerProgressResponse
{
    public int maxLevelUnlocked;
    public LevelProgressResponse[] levels;
}

[Serializable]
public class LevelProgressResponse
{
    public int levelNumber;
    public int stars;
}
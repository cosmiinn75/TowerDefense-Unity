using System;

public static class GameSession
{
    public static string Token;
    public static string Username;

    public static PlayerProgressResponse Progress;

    public static int SelectedLevelNumber;
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
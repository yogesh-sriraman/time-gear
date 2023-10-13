[System.Serializable]
public class PlayerProfile
{
    public int playerID;
    public int numberOfStars;
    public GameChapters[] gameChapters;

}

[System.Serializable]
public class GameChapters
{
    public int chapterID;
    public string chapterName;
    public int starToUnlock;
    public bool isUnlocked;
    public string chapterLevelID;
}


[System.Serializable]
public class PlayerLevels
{
    public int playerID;
    public PlayerLevel[] playerLevels;
}

[System.Serializable]
public class PlayerLevel
{
    public int levelID;
    public int chapterID;
    public string levelName;
    public bool isUnlocked;
    public bool isCompleted;
    public int minimumTime;
    public int numberOfOrbs;
}

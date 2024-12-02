using UnityEngine;

public class LevelManager
{
    private const string LVL_KEY = "S_LVL";
    private const string LVL_UP = "S_IS_LVL_UP";
    
    public static void LevelUp()
    {
        PlayerPrefs.SetInt(LVL_UP, 1);
    }

    public static (int, bool) GetLevel()
    {
        return (PlayerPrefs.GetInt(LVL_KEY, 1), PlayerPrefs.GetInt(LVL_UP) == 1);
    }

    public static void ResetLevelUp()
    {
        PlayerPrefs.SetInt(LVL_UP, 1);
        PlayerPrefs.SetInt(LVL_KEY, GetLevel().Item1 + 1);
    }

    public static void SaveLevel()
    {
        PlayerPrefs.SetInt(LVL_KEY, GetLevel().Item1);
    }
}
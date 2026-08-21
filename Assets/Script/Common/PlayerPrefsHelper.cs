using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//±£¥ÊªÒ»°
public static class PlayerPrefsHelper
{
    public static string CurrentLevelKey = "Currentlevel";
    public static void SaveCurrentLevelIndex(int idx)
    {
        PlayerPrefs.SetInt(CurrentLevelKey, idx);
    }
    public static int GetCurrentLevelIdx()
    {
        return PlayerPrefs.GetInt(CurrentLevelKey);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSaver : MonoBehaviour
{
    public static int ReadCatagoryCurrentIndexValues(string name)
    {
        var value = -1;
        if (PlayerPrefs.HasKey(name))
            value = PlayerPrefs.GetInt(name);

        return value;
    }

    public static void SaveCatagoryData(string CatagoryName, int currentIndex)
    {
        PlayerPrefs.SetInt(CatagoryName, currentIndex);
        PlayerPrefs.Save();
    }

    public static void ClearGameData(GameLevelData levelData)
    {
        foreach (var data in levelData.data)
        {
            PlayerPrefs.SetInt(data.catagoryName, -1);
        }

        //Unlock first level
        PlayerPrefs.SetInt(levelData.data[0].catagoryName, 0);
        PlayerPrefs.Save();
    }
}

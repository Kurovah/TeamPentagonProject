using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string playerName;
    public int medals;
    public RangerCustomSettings rangerCustom;

    public PlayerData()
    {
        playerName = "Default Name";
        medals = 0;
        rangerCustom = new RangerCustomSettings();
    }
}

[System.Serializable]
public class RangerCustomSettings
{
    public int skinIndex;
    public int bodyIndex;
    public RangerCustomSettings()
    {
        skinIndex = 1;
        bodyIndex = 0;
    }
}

[System.Serializable]
public class AlienCustomSettings
{
    public int skinIndex, bodyIndex, shipIndex1, shipIndex2;
    public AlienCustomSettings()
    {
        skinIndex = 0;
        bodyIndex = 0;
        shipIndex1 = 0;
        shipIndex2 = 0;
    }
}

[System.Serializable]
public class ColorListing
{
    public string c_Name;
    public Color color;
    public bool collapsed;
    public ColorListing()
    {
        c_Name = "Default";
        color = Color.white;
        collapsed = false;
    }
}

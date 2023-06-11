using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string playerName;
    public int medals;
    public float battlePassExp;
    public RangerCustomSettings rangerCustom;
    public List<bool> unlocked = new List<bool> { false, false, false };
    public int HeadGearSetting = 0;
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

[System.Serializable]
public class GameItem
{
    public string itemName;
}

[System.Serializable]
public class CosmeticItem : GameItem
{
    public GameObject itemToSpawn;
    public bool unlocked;
}

[System.Serializable]
public class PassItem
{
    public string itemName;
    public int value;
    public bool gotten;
    public enum EPassItemType
    {
        bundle,
        cosmeticUnlock
    }

    EPassItemType itemType = EPassItemType.bundle;

    public PassItem(EPassItemType _itemType)
    {
        itemType = _itemType;
        switch (itemType)
        {
            case EPassItemType.bundle:
                itemName = $"Cosmo coin Bundle X{value}";
                break;
            case EPassItemType.cosmeticUnlock:
                itemName = $"{GameManager.instance.cosmetics[value].name}";
                break;
        }
    }
}
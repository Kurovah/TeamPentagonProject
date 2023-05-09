using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using JetBrains.Annotations;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Image blackOutImage;
    public PlayerData playerData;
    public UnityAction onRangerColorChanged;
    public ColourList colList;

    public List<GameObject> headGear = new List<GameObject>();

    public bool loadingDone;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        playerData = LoadData();
        SceneManager.LoadSceneAsync("StartScreen", LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    List<AsyncOperation> loadingops = new List<AsyncOperation>();
    public void LoadNewScenewithFade(string _scenetoLoad)
    {
        loadingDone = false;
        blackOutImage.CrossFadeAlpha(1, 0, true);
        var currentScenesNo = SceneManager.sceneCount;
        for(int i = 0; i < SceneManager.sceneCount; i++)
        {
            if(gameObject.scene != SceneManager.GetSceneAt(i))
            {
                loadingops.Add(SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i)));
            }
        }

        loadingops.Add(SceneManager.LoadSceneAsync(_scenetoLoad, LoadSceneMode.Additive));
        Debug.Log($"Loading {_scenetoLoad}");
        StartCoroutine(LoadingProgress());
    }

    public void ChangeCurrency(int amount)
    {
        playerData.medals += amount;
    }

    IEnumerator LoadingProgress()
    {
        for (int i = 0; i < loadingops.Count; i++)
        {
            while (!loadingops[i].isDone)
            {
                yield return null;
            }
        }

        loadingDone = true;
    }

    public void FadeIn()
    {
        //fade back in
        blackOutImage.CrossFadeAlpha(0, 0.1f, true);
    }

    public Color GetColorFromList(int index)
    {
        return colList.colors[index].color;
    }


    public enum CustomSlots
    {
        skin,
        body
    }

    public void UnlockItem(int index)
    {
        index = Mathf.Clamp(index, 0, playerData.unlocked.Count);
        playerData.unlocked[index] = true;
    }

    public void SetColorIndex(int index, CustomSlots slot)
    {
        switch (slot)
        {
            case CustomSlots.body:
                playerData.rangerCustom.bodyIndex = index;
                break;
            case CustomSlots.skin:
                playerData.rangerCustom.skinIndex = index;
                break;
        }
        onRangerColorChanged?.Invoke();
    }

    public void SetHeadGearIndex(int index)
    {
        playerData.HeadGearSetting = index;
    }

    public void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerdata.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, playerData);
        stream.Close();
    }

    public PlayerData LoadData()
    {
        PlayerData data;
        string path = Application.persistentDataPath + "/playerdata.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

        } else
        {
            data = new PlayerData();
            
        }
        return data;
    }
}

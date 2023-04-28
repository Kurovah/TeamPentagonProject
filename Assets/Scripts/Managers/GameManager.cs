using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Image blackOutImage;
    public PlayerData playerData;
    public UnityAction onRangerColorChanged;
    public ColourList colList;

    public bool TestMode;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        playerData = new PlayerData();

        if(!TestMode)
            SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    List<AsyncOperation> loadingops = new List<AsyncOperation>();
    public void LoadNewScenewithFade(string _scenetoLoad)
    {
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
        StartCoroutine(LoadingProgress());
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
}

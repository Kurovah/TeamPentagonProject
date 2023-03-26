using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Image blackOutImage;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
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
}

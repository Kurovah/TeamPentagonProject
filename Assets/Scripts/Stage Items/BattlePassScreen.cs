using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattlePassScreen : MonoBehaviour
{
    public Image fillImage;
    public TMP_Text LevelText;


    public List<Button> unlockButtons = new List<Button>();
    public List<GameObject> boughtBanners = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        fillImage.fillAmount = GameManager.instance.playerData.battlePassExp / 30;
        CheckBPProgress();
    }

    // Update is called once per frame
    void Update()
    {
        LevelText.text = (GameManager.instance.playerData.battlePassExp / 10).ToString();
    }

    void CheckBPProgress()
    {
        for(int i = 0; i < unlockButtons.Count; i++)
        {
            unlockButtons[i].enabled = i < Mathf.Floor(GameManager.instance.playerData.battlePassExp / 10) && !GameManager.instance.playerData.unlocked[i];
            boughtBanners[i].SetActive(GameManager.instance.playerData.unlocked[i]);
        }
    }

    public void UnlockItem(int index)
    {
        GameManager.instance.UnlockItem(index);
        CheckBPProgress();
    }

    public void BackToMain()
    {
        GameManager.instance.SaveData();
        GameManager.instance.LoadNewScenewithFade("MainMenu");
    }
}

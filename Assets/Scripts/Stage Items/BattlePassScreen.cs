using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattlePassScreen : MonoBehaviour
{
    public Image fillImage;
    public TMP_Text LevelText;
    public GameObject itemBox;
    public Transform battlepassContent;
    // Start is called before the first frame update
    void Start()
    {
        fillImage.fillAmount = GameManager.instance.playerData.battlePassExp / 30;
        LevelText.text = (GameManager.instance.playerData.battlePassExp / 10).ToString();
        foreach(var item in GameManager.instance.currentBattlePass.passItems)
        {
            var i = Instantiate(itemBox, battlepassContent);
            i.GetComponent<UnlockBanner>().item = item;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackToMain()
    {
        GameManager.instance.SaveData();
        GameManager.instance.LoadNewScenewithFade("MainMenu");
    }
}

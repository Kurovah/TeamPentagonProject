using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattlePassScreen : MonoBehaviour
{
    public Image fillImage;
    public TMP_Text LevelText;
    // Start is called before the first frame update
    void Start()
    {
        fillImage.fillAmount = GameManager.instance.playerData.battlePassExp / 30;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckBPProgress()
    {
        
    }

    public void UnlockItem(int index)
    {

    }
}

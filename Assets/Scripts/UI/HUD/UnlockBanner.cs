using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnlockBanner : MonoBehaviour
{
    public Button unlockButton;
    public PassItem item;
    public TMP_Text itemText;
    public GameObject unlockedNotifier;
    // Start is called before the first frame update
    void Start()
    {
        unlockButton.enabled = !(item.gotten || transform.GetSiblingIndex() + 1 > Mathf.Floor(GameManager.instance.playerData.battlePassExp));
        unlockedNotifier.SetActive(item.gotten);
        itemText.text = item.GetItemName();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UnlockItem()
    {
        GameManager.instance.currentBattlePass.passItems[transform.GetSiblingIndex()].gotten = true;
        unlockedNotifier.SetActive(true);
        unlockButton.enabled = false;

        switch (item.itemType)
        {
            case PassItem.EPassItemType.bundle:
                GameManager.instance.ChangeCurrency(item.value);
                break;
            case PassItem.EPassItemType.cosmeticUnlock:

                break;
        }
    }
}

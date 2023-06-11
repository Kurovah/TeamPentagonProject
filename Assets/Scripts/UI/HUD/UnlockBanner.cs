using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockBanner : MonoBehaviour
{
    public string itemName;
    bool available, unlocked;
    public Button unlockButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UnlockItem()
    {
        unlocked = true;
    }
}

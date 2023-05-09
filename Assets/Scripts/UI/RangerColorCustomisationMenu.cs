using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RangerColorCustomisationMenu : MonoBehaviour
{
    public GameObject colorChangeButtonPrefab;
    public Transform skinChangeTransform, headGearPlace;
    // Start is called before the first frame update
    void Start()
    {
        AddButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddButtons()
    {
       
        foreach(var i in GameManager.instance.headGear)
        {
            var a = Instantiate(colorChangeButtonPrefab, skinChangeTransform);
            if(i == null)
            {
                a.GetComponentInChildren<TMP_Text>().text = "None";
            } else
            {
                a.GetComponentInChildren<TMP_Text>().text = i.name;
            }
            
            a.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.SetHeadGearIndex(a.transform.GetSiblingIndex()));
            a.GetComponent<Button>().onClick.AddListener(() => PlaceHeadGear(a.transform.GetSiblingIndex()));
        }
    }

    void PlaceHeadGear(int index)
    {
        if(headGearPlace.childCount > 0)
        {
            Destroy(headGearPlace.GetChild(0).gameObject);
        }

        if(GameManager.instance.headGear[index] != null)
        {
            var i = Instantiate(GameManager.instance.headGear[index], headGearPlace);
            i.transform.localScale = Vector3.one * 0.1f;
        }
        
    }
}

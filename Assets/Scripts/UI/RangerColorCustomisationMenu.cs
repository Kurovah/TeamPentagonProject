using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangerColorCustomisationMenu : MonoBehaviour
{
    public GameObject colorChangeButtonPrefab;
    public Transform skinChangeTransform, bodyChangeTransform;
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
       
        foreach(var i in GameManager.instance.colList.colors)
        {
            var a = Instantiate(colorChangeButtonPrefab, skinChangeTransform);
            a.GetComponent<Image>().color = i.color;
            a.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.SetColorIndex(a.transform.GetSiblingIndex(), GameManager.CustomSlots.skin));

            var b = Instantiate(colorChangeButtonPrefab, bodyChangeTransform);
            b.GetComponent<Image>().color = i.color;
            b.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.SetColorIndex(b.transform.GetSiblingIndex(), GameManager.CustomSlots.body));
        }
    }
}

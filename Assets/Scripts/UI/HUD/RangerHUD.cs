using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangerHUD : MonoBehaviour
{
    public GameObject hpPip;
    public Transform hpArea;
    bool matchManagerAvailable;
    // Start is called before the first frame update
    void Start()
    {
        //creating HealthPips
        matchManagerAvailable = FindObjectOfType<MatchManager>() != null;
        if (matchManagerAvailable)
        {
            for (int i = 0; i < MatchManager.instance.rangerHP; i++)
            {
                Instantiate(hpPip, hpArea);
            }
        } else
        {

        }
    }
        

    // Update is called once per frame
    void Update()
    {
        if (matchManagerAvailable)
        {
            UpdatePips(MatchManager.instance.rangerHP);
        }
        {
            UpdatePips(OnboardingManager.instance.rangerHP);
        }
    }

    void UpdatePips(int Hp)
    {
        foreach(Transform t in hpArea)
        {
            t.GetComponent<Image>().color = t.GetSiblingIndex() < Hp ? Color.cyan : Color.gray;
        }
    }
}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public class OnboardingManager : MonoBehaviour
{
    public static OnboardingManager instance;
    public int rangerHP = 6;
    public int alienResource = 0;
    public GameObject toolTip;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
    public void ChangeRangerHP(int amount)
    {
        rangerHP += amount;
    }
    public void ChangeAlienResource(int amount)
    {
        alienResource += amount;
    }
    public void ShowPopUp(string text)
    {

    }
    public void HidePopUp()
    {

    }
}

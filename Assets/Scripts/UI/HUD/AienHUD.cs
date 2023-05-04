using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AienHUD : MonoBehaviour
{
    public TMP_Text resourceText;
    AlienBehaviour alien;
    // Start is called before the first frame update
    void Start()
    {
        SetAlienTarget(FindObjectOfType<AlienBehaviour>());
    }

    // Update is called once per frame
    void Update()
    {
        resourceText.text = GetResource().ToString();     
    }

    int GetResource()
    {
        if (FindObjectOfType<MatchManager>())
        {
            return MatchManager.instance.alienResource;
        }
        else
        {
            return OnboardingManager.instance.alienResource;
        }
    }

    public void SetAlienTarget(AlienBehaviour _alien)
    {
        alien = _alien;
    }
}

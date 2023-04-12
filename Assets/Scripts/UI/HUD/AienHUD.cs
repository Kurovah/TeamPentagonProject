using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AienHUD : MonoBehaviour
{
    public TMP_Text destronText;
    public TMP_Text animaText;
    public TMP_Text aquidiaText;

    AlienBehaviour alien;
    // Start is called before the first frame update
    void Start()
    {
        SetAlienTarget(FindObjectOfType<AlienBehaviour>());
    }

    // Update is called once per frame
    void Update()
    {
        destronText.text = alien.resources[ResourceTypes.Destron].ToString();   
        animaText.text = alien.resources[ResourceTypes.Anima].ToString();   
        aquidiaText.text = alien.resources[ResourceTypes.Aquidia].ToString();   
    }

    public void SetAlienTarget(AlienBehaviour _alien)
    {
        alien = _alien;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    //
    public Slider soundSlider, musicSlider;
    // Start is called before the first frame update
    void Start()
    {
        soundSlider.value = SoundManager.instance.effectsSource.volume;
        musicSlider.value = SoundManager.instance.musicSource.volume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMusicVolume(float newValue)
    {
        SoundManager.instance.musicSource.volume = newValue;
    }

    public void SetFXVolume(float newValue)
    {
        SoundManager.instance.effectsSource.volume = newValue;
    }

    public void BackToMain()
    {
        GameManager.instance.LoadNewScenewithFade("MainMenu");
    }

}

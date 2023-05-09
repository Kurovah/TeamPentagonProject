using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource musicSource, effectsSource;

    public List<AudioClip> music = new List<AudioClip>();
    public List<AudioClip> soundFX = new List<AudioClip>();
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        PlaySong(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySong(int index)
    {
        musicSource.clip = music[index];
        musicSource.Play();
    }

    public void PlaySound(int index)
    {
        effectsSource.PlayOneShot(soundFX[index]);
    }
}

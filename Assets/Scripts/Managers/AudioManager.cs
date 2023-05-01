using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioSource soundSource;
    public AudioSource musicSource;

    // MAKE IT A STATIC TO BE ACCESSABLE EVERYWHERE
    // Singleton too, I guess
    public static AudioManager Instance = null;

    public void Awake()
    {
        // Check if there's an AudioManager already.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        // If any other Instances, destroy it if it's not this.
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Play a single clip through the sound effects source
    public void Play(AudioClip audioClip)
    {
        soundSource.PlayOneShot(audioClip);
    }

    // Play a sound track through the music source
    public void PlayMusic(AudioClip audioClip)
    {
        musicSource.clip = audioClip;
        musicSource.Play();
        musicSource.loop = true;
    }

    public void Stop()
    {
        musicSource.Stop();
    }
}

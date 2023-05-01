using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public AudioMixerGroup effects;
    public AudioMixerGroup music;

    public Slider soundEffects;
    public Slider soundMusic;

    public void Start()
    {
        if (soundEffects == null)
        {
            OnMusicVolumeChange();
        }

        if (soundMusic == null)
        {
            OnEffectVolumeChange();
        }
    }

    public void OnEffectVolumeChange()
    {
        // Start with the slider value
        float newVolume = soundEffects.value;
        if (newVolume <= 0) 
        {
            // If we are at zero, lowest volume
            newVolume = -80;
        }
        else
        {
            // We are >0, so find the log10 value
            newVolume = Mathf.Log10(newVolume);
            // Make it in the 0-20db range
            newVolume = newVolume * 20;
        }

        // Set the volume to the new volume setting
        effects.audioMixer.SetFloat("SFXVol", newVolume);
    }

    public void OnMusicVolumeChange()
    {
        // Start with the slider value
        float newVolume = soundMusic.value;
        if (newVolume <= 0)
        {
            // If we are at zero, lowest volyme
            newVolume = -80;
        }
        else
        {
            // We are >0, so find the log10 value
            newVolume = Mathf.Log10(newVolume);
            // Make it in the 0-20db range
            newVolume = newVolume * 20;
        }

        // Set the volume to the new volume setting
        music.audioMixer.SetFloat("MusicVol", newVolume);
    }
}

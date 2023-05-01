using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOptions : MonoBehaviour
{
    public AudioClip buttonSound;

    // Go to options
    public void OptionsMenu()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ActivateOptionsScreen();
        }

        // Play sounds
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.Play(buttonSound);
        }
    }
}

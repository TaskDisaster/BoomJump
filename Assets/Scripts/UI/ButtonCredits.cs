using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCredits : MonoBehaviour
{
    public AudioClip buttonSound;

    // Go to credits
    public void CreditsMenu()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ActivateCreditsScreen();
        }

        // Play sounds
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.Play(buttonSound);
        }
    }
}

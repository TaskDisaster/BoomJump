using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonStart : MonoBehaviour
{
    public AudioClip buttonSound;

    // The start button
    public void StartGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ActivateGameplayState();
        }

        // Play sounds
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.Play(buttonSound);
        }
    }
}

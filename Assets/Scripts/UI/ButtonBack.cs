using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBack : MonoBehaviour
{
    public AudioClip buttonSound;

    // Button to go back to the menu
    public void MainMenu()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ActivateMainMenuScreen();
        }

        // Play sounds
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.Play(buttonSound);
        }
    }
}

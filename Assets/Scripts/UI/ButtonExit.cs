using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonExit : MonoBehaviour
{
    public AudioClip buttonSound;

    // Exit the game :(
    public void ExitGame()
    {
        // Play sounds
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.Play(buttonSound);
        }

        Debug.Log("EXIT");
        Application.Quit();
    }
}

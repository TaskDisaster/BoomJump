using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesManager : MonoBehaviour
{
    public Controller player;
    public Text livesText;
    public int lives;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        // Set player to PlayerController
        if (player == null)
        {
            TargetPlayer();
            UpdateLives();
        }
    }

    public void TargetPlayer()
    {
        // If the GameManager exists
        if (GameManager.Instance != null)
        {
            // And there are players in it
            if (GameManager.Instance.playerController1 != null)
            {
                player = GameManager.Instance.playerController1;
            }
        }
    }

    // Updates UI text
    public void UpdateLives()
    {
        if (player != null)
        {
            lives = player.lives;
            livesText.text = lives.ToString();
        }
    }
}

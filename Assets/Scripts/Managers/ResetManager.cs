using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetManager : MonoBehaviour
{
    public KeyCode reset;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.resetManager = this.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    public void ProcessInput()
    {
        if (Input.GetKey(reset))
        {
            // Reset the game
            GameManager.Instance.timeToReset = true;
            GameManager.Instance.gameState = GameManager.GameState.MainMenu;

        }
    }
}

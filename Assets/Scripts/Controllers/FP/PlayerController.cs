using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerController : Controller
{
    public KeyCode moveForwardKey;
    public KeyCode moveBackwardKey;
    public KeyCode rightMovementKey;
    public KeyCode leftMovementKey;
    public KeyCode shootKey;
    public KeyCode jumpKey;
    public LivesManager livesText;

    // Start is called before the first frame update
    public override void Start()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.playerController1 == null)
            {
                GameManager.Instance.playerController1 = this;
                lives = 2;
            }
        }

        // Connect controller with livesManager
        livesText = FindObjectOfType<LivesManager>();
    }

    // Remove itself upon death
    public void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.playerController1 != null)
            {
                GameManager.Instance.playerController1 = null;
            }
        }
    }

    // Update doesn't break my GetKeyDownInputs for some reason?????
    public override void Update()
    {
        ProcessGetKeyDownInputs();

        // Constantly check if player is dead
        if (pawn == null)
        {
            // Decrement Lives
            lives -= 1;

            // Respawn if I have the lives to do so
            if (lives > 0)
            {
                livesText.UpdateLives();
                GameManager.Instance.RespawnPlayer(this);
            }
            else if (GameManager.Instance.won != true)
            {
                // Mark the player as dead
                isDead = true;
            }
            else if (GameManager.Instance.won == true)
            {
                // Don't mark the player dead
                isDead = false;
            }
        }
    }
    
    // FixedUpdate works better for physic based things
    public void FixedUpdate()
    {
        ProcessGetKeyInputs();
    }

    public void ProcessGetKeyDownInputs()
    {
        if (Input.GetKeyDown(shootKey))
        {
            pawn.Shoot();
        }

        if (Input.GetKeyDown(jumpKey))
        {
            pawn.Jump();
        }
    }

    public void ProcessGetKeyInputs()
    {
        if (Input.GetKey(moveForwardKey))
        {
            pawn.MoveForward();
        }

        if (Input.GetKey(moveBackwardKey))
        {
            pawn.MoveBackward();
        }

        if (Input.GetKey(rightMovementKey))
        {
            pawn.RightMovement();
        }

        if (Input.GetKey(leftMovementKey))
        {
            pawn.LeftMovement();
        }
    }
}

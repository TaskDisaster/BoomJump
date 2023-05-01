using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;

    // Update is called once per frame
    private void Update()
    {
        if (cameraPosition == null)
        {
            if (GameManager.Instance.playerController1 != null)
            {
                // Find the player pawn
                GameObject player = GameManager.Instance.playerController1.pawn.gameObject;
                // Find the player's child of CameraPos
                GameObject playerCamPos = player.transform.GetChild(1).gameObject;
                // Set that position to cameraPosition
                cameraPosition = playerCamPos.GetComponent<Transform>();
            }
        }

        if (cameraPosition != null) 
        { 
            transform.position = cameraPosition.position;
        }

    }
}

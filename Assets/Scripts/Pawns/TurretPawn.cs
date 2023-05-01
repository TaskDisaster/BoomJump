using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurretPawn : Pawn
{
    #region Variables
    public GameObject turretGun;
    public Shooter shooter;
    public GameObject shellPrefab;
    public float fireForce;
    public float damageDone;
    public float shellLifeSpan;
    public float fireCooldown = 1.5f;
    #endregion Variables

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        shooter = GetComponent<Shooter>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void MoveForward()
    {
        if (mover != null)
        {
            mover.Move(transform.forward, moveSpeed);
        }
        else
        {
            Debug.LogWarning("Warning: No Mover in TankPawn.MoveForward()!");
        }
    }

    public override void MoveBackward()
    {
        if (mover != null)
        {
            mover.Move(transform.forward, -moveSpeed);
        }
        else
        {
            Debug.LogWarning("Warning: No Mover in TankPawn.MoveBackward()!");
        }
    }

    public override void RightMovement()
    {
        if (mover != null)
        {
            mover.Rotate(turnSpeed);
        }
        else
        {
            Debug.LogWarning("Warning: No Mover in TankPawn.RotateClockwise()!");
        }
    }

    public override void LeftMovement()
    {
        if (mover != null)
        {
            mover.Rotate(-turnSpeed);
        }
        else
        {
            Debug.LogWarning("Warning: No Mover in TankPawn.RotateCounterClockwise()!");
        }
    }

    public override void RotateTowards(Vector3 targetPosition)
    {
        // Gets the vector from the pawn to the target
        Vector3 vectorToTarget = targetPosition - transform.position;

        // Gets the rotation from the pawn to the target
        Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget, Vector3.up);
        
        // Locks the rotation so that only the treads rotate to the location
        targetRotation.x = 0;
        targetRotation.z = 0;

        // Sets the rotation of the pawn to the target using the turnSpeed multiplied by delta time
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    public override void RotateGunTo(Vector3 targetPosition)
    {
        // Get the vector to target
        Vector3 vectorToTarget = targetPosition - turretGun.transform.position;

        // Get the rotation from the pawn to the target
        Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget, Vector3.up);

        /*
        // Locks the rotation so that it can only point up at the target
        targetRotation.y = this.transform.rotation.y;
        targetRotation.z = 0;
        */

        // Sets the rotation of the pawn to the target using the turnSpeed multiplied by delta time
        turretGun.transform.rotation = Quaternion.RotateTowards(turretGun.transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    public void ResetGun()
    {
        turretGun.transform.rotation = gameObject.transform.localRotation;
    }

    public override void Shoot()
    {
        shooter.Shoot(shellPrefab, fireForce, damageDone, shellLifeSpan);
    }
}

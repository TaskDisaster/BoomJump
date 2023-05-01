using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FirstPersonPawn : Pawn
{


    #region Variables
    public Transform camPos;
    public Shooter shooter;
    public GameObject shellPrefab;
    public float fireForce;
    public float damageDone;
    public float shellLifeSpan;
    public float fireCooldown;
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
        mover.Rotate(turnSpeed);
    }

    public override void MoveForward()
    {
        if (mover != null)
        {
            mover.Move(mover.orientation.forward, moveSpeed);
        }
        else
        {
            Debug.LogWarning("Warning: No Mover in FirstPersonPawn");
        }
    }

    public override void MoveBackward()
    {
        if (mover != null)
        {
            mover.Move(mover.orientation.forward, -moveSpeed);
        }
        else
        {
            Debug.LogWarning("Warning: No Mover in FirstPersonPawn");
        }
    }

    public override void RightMovement()
    {
        if (mover != null)
        {
            mover.Move(mover.orientation.right, moveSpeed);
        }
        else
        {
            Debug.LogWarning("Warning: No Mover in FirstPersonPawn");
        }
    }

    public override void LeftMovement()
    {
        if (mover != null)
        {
            mover.Move(mover.orientation.right, -moveSpeed);
        }
        else
        {
            Debug.LogWarning("Warning: No Mover in FirstPersonPawn");
        }
    }

    public override void RotateTowards(Vector3 target)
    {
        // Not used by First Person
    }

    public override void RotateGunTo(Vector3 target)
    {
        // Not used by First Person
    }

    public override void Shoot()
    {
        shooter.Shoot(shellPrefab, fireForce, damageDone, shellLifeSpan);
    }

    public override void Jump()
    {
        if (mover != null)
        {
            mover.Jump();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : MonoBehaviour
{

    #region Variables
    public Controller controller;   // Holds Controller
    public Mover mover;             // Holds Mover
    public float moveSpeed;
    public float turnSpeed;

    #endregion Variables

    // Start is called before the first frame update
    public virtual void Start()
    {
        mover = GetComponent<Mover>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public abstract void MoveForward();
    public abstract void MoveBackward();
    public abstract void RightMovement();
    public abstract void LeftMovement();
    public abstract void Shoot();
    public abstract void RotateTowards(Vector3 target);
    public abstract void RotateGunTo(Vector3 target);
    public virtual void Jump()
    {

    }
}

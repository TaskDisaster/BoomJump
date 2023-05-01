using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : MonoBehaviour
{
    public Transform orientation;
    public Transform verticalOrientation;

    // Start is called before the first frame update
    public abstract void Start();
    public abstract void Move(Vector3 direction, float speed);
    public abstract void Rotate(float turnSpeed);
    public virtual void Jump()
    {

    }
}

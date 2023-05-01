using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMover : Mover
{
    #region Variables
    private Rigidbody rb;

    #endregion

    // Start is called before the first frame update
    public override void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    public override void Move(Vector3 direction, float speed)
    {
        // Calculate a forward vector
        Vector3 moveVector = direction.normalized * speed * Time.deltaTime;
        rb.MovePosition(rb.position + moveVector);
    }

    public override void Rotate(float turnSpeed)
    {
        transform.Rotate(0, turnSpeed * Time.deltaTime, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{
    #region Variables
    public float forceAmount;
    public Pawn owner;
    public float explodeRange;
    #endregion Variables

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);

        Pawn otherPawn = other.GetComponent<Pawn>();

        // Check if the collider we're colliding with isn't the owner's
        if (otherPawn != owner)
        {
            // Get the distance between the owner and rocket
            float distance = Vector3.Distance(transform.position, owner.transform.position);

            // Check if it's within range of us to actually add force
            if (distance < explodeRange)
            {
                // Then check if the owner still exists
                if (owner != null)
                {
                    Rigidbody rb = owner.GetComponent<Rigidbody>();

                    // Get the direction of the force and multiply it by the force amount
                    Vector3 direction = owner.transform.position - gameObject.transform.position;
                    Vector3 force = direction.normalized * forceAmount;

                    // Add force away from this
                    rb.AddForce(force, ForceMode.Impulse);
                }
            }
        }
    }
}

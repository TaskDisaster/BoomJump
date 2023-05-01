using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class WinTrigger : MonoBehaviour
{
    public float damageDone;
    public Pawn owner;

    public void OnTriggerEnter(Collider other)
    {
        Pawn otherPawn = other.GetComponent<Pawn>();

        // Check if we're hitting ourselves and skip this if we are
        if (otherPawn == owner)
        {        
            GameManager.Instance.won = true;

            // Get the health of the collided object
            Health otherHealth = other.gameObject.GetComponent<Health>();

            // If it has health, then damage it
            if (otherHealth != null)
            {
                otherHealth.TakeDamage(damageDone, owner);
            }
        }

    }
}

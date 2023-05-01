using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnHit : MonoBehaviour
{
    #region Variables
    public float damageDone;
    public Pawn owner;
    public AudioClip audioClip;
    #endregion Variables

    public void OnTriggerEnter(Collider other)
    {
        Pawn otherPawn = other.GetComponent<Pawn>();

        // Check if we're hitting ourselves and skip this if we are
        if (otherPawn != owner)
        {
            // Get the health of the collided object
            Health otherHealth = other.gameObject.GetComponent<Health>();

            // If it has health, then damage it
            if (otherHealth != null)
            {
                otherHealth.TakeDamage(damageDone, owner);
            }

            if (audioClip != null)
            {
                AudioManager.Instance.Play(audioClip);
            }

            // Then destroy self
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShooter : Shooter
{
    #region Variables
    public Transform firepointTransform;
    private TurretPawn turretPawn;
    private float lastTimeShot;
    public AudioClip audioClip;

    #endregion

    // Start is called before the first frame update
    public override void Start()
    {
        turretPawn = GetComponent<TurretPawn>();

        // Set the last time shot
        lastTimeShot = Time.time;
    }

    // Update is called once per frame
    public override void Update()
    {
        
    }

    public override void Shoot(GameObject shellPrefab, float fireForce, float damageDone, float lifeSpan)
    {
        if (Time.time > lastTimeShot + turretPawn.fireCooldown)
        {
            // Instantiate our projectile
            GameObject newShell = Instantiate(shellPrefab, firepointTransform.position, firepointTransform.rotation);

            // Get the DamageOnHit
            DamageOnHit doh = newShell.GetComponent<DamageOnHit>();

            if (doh != null)
            {
                doh.damageDone = damageDone;
                doh.owner = GetComponent<Pawn>();
            }

            Rigidbody rb = newShell.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(firepointTransform.forward * fireForce);
            }
            
            if (audioClip != null)
            {
                AudioManager.Instance.Play(audioClip);
            }

            // Set lastTimeShot to current time
            lastTimeShot = Time.time;

            // Destroy shell
            Destroy(newShell, lifeSpan);
        }
    }
}

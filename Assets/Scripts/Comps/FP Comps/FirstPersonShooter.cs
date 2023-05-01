using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonShooter : Shooter
{
    #region Variables
    public Transform firepointTransform;
    public FirstPersonPawn firstPersonPawn;
    public  float forceAmount;
    private NoiseMaker noiseMaker;
    private float lastTimeShot;
    public AudioClip audioClip;

    #endregion Variables

    // Start is called before the first frame update
    public override void Start()
    {
        firstPersonPawn = GetComponent<FirstPersonPawn>();

        lastTimeShot = Time.time;
    }

    // Update is called once per frame
    public override void Update()
    {

    }

    public override void Shoot(GameObject shellPrefab, float fireForce, float damageDone, float lifeSpan)
    {
        if (Time.time > lastTimeShot + firstPersonPawn.fireCooldown)
        {
            // Instantiate our projectile
            GameObject newShell = Instantiate(shellPrefab, firepointTransform.position, firstPersonPawn.camPos.rotation);

            // Get the DamageOnHit
            DamageOnHit doh = newShell.GetComponent<DamageOnHit>();
            
            // Get the Exploder
            Exploder exploder = newShell.GetComponent<Exploder>();

            if (doh != null)
            {
                doh.damageDone = damageDone;
                doh.owner = GetComponent<Pawn>();
            }

            if (exploder != null)
            {
                exploder.forceAmount = forceAmount;
                exploder.owner = GetComponent<Pawn>();
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

            // Make some noise
            if (noiseMaker != null)
            {
                noiseMaker.volumeDistance = 20;
            }

            // Set lastTimeShot to current time
            lastTimeShot = Time.time;

            // Destroy shell
            Destroy(newShell, lifeSpan);
        }
    }
}

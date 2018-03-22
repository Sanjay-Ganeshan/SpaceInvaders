using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A general purpose Turret that can fire Projectiles.
/// </summary>
public class Turret : MonoBehaviour {

    /// <summary>
    /// A prefab of the Projectile that will be instantiated each shot
    /// </summary>
    public Projectile ProjectilePrefab;

    /// <summary>
    /// The ship that this turret is attached to
    /// </summary>
    public Ship MyShip;

    /// <summary>
    /// The amount of cooldown/reload time left on this ship. It cannot fire unless Cooldown = 0.
    /// </summary>
    public float Cooldown { get; private set; }

    /// <summary>
    /// The amount of charge on this ship.
    /// </summary>
    public float Charge { get; private set; }

    /// <summary>
    /// The audio source containing the firing sfx of this turret.
    /// </summary>
    public AudioSource sfx;

    private bool isCharging;

    public Transform[] spawnPositions;

	// Use this for initialization
	void Start () {
        sfx = GetComponent<AudioSource>();
        this.Cooldown = 0.0f;
        this.Charge = 0.0f;
        this.isCharging = false;
	}
	
	// Update is called once per frame
	void Update () {
        // TODO: Consider changing for efficiency
        this.Cooldown = Mathf.Max(0.0f, this.Cooldown - Time.deltaTime);	
	}

    // Late Update is called every frame, after every script has called Update().
    void LateUpdate()
    {
        if(!this.isCharging)
        {
            ResetCharge();
        }
        else
        {
            this.isCharging = false;
        }
    }


    /// <summary>
    /// Resets the charge amount on this turret
    /// </summary>
    void ResetCharge()
    {
        this.Charge = 0.0f;
        this.isCharging = false;
    }

    /// <summary>
    /// Determines if this turret can currently fire (not on cooldown)
    /// </summary>
    /// <returns>If this turret can validly fire</returns>
    public bool CanFire()
    {
        return Mathf.Approximately(Cooldown, 0.0f);
    }

    /// <summary>
    /// Fire a projectile, instantiating it at the turret's position.
    /// Fails silently if CanFire() would return false.
    /// </summary>
    public void Fire()
    {
        // Break out if we can't fire / charge.
        if(!CanFire())
        {
            return;
        }

        // Set that we were charging this frame
        this.isCharging = true;

        // Charge up
        this.Charge += Time.deltaTime;

        // If you have enough charge to release a shot, do it
        if (this.Charge > ProjectilePrefab.ChargeTime)
        {
            // If we can fire, do so
            foreach(Transform projectileSpawn in this.spawnPositions) {
                Projectile newProjectile = GameObject.Instantiate(ProjectilePrefab.gameObject, projectileSpawn.position, projectileSpawn.rotation, null).GetComponent<Projectile>();
                newProjectile.Fire(MyShip);
            }

            // Add the reload time to the cooldown
            this.Cooldown += ProjectilePrefab.ReloadTime;

            // Deduct the energy needed to fire the bullet from the ship.
            MyShip.DamageMe(ProjectilePrefab.EnergyCost);

            // We've fired our projectile, this eliminates our charge
            ResetCharge();

            // Play firing SFX
            PlaySound();
        }
    }

    /// <summary>
    /// Plays the SFX associated with this turret
    /// </summary>
    void PlaySound()
    {
        // If we have sound effects, and we're supposed to play them
        if(sfx != null && GameConfig.SFXEnabled)
        {
            // Then play them
            sfx.Play();
        }
    }
}

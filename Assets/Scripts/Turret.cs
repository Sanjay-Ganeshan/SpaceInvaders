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

    /// <summary>
    /// Whether or not this weapon is charging up
    /// </summary>
    private bool isCharging;

    /// <summary>
    /// All positions from which to spawn a projectile
    /// </summary>
    public Transform[] SpawnPositions;

    private List<Projectile> HeldProjectiles;

	// Use this for initialization
	void Start () {
        sfx = GetComponent<AudioSource>();
        this.Cooldown = 0.0f;
        this.Charge = 0.0f;
        this.isCharging = false;
        this.HeldProjectiles = new List<Projectile>();
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
        this.DestroyHeldProjectiles();
    }

    /// <summary>
    /// Determines if this turret can currently fire (not on cooldown)
    /// </summary>
    /// <returns>If this turret can validly fire</returns>
    public bool CanFire()
    {
        return Mathf.Approximately(Cooldown, 0.0f);
    }

    void DestroyHeldProjectiles()
    {
        if(this.HeldProjectiles == null)
        {
            return;
        }
        for(int i = 0; i < this.HeldProjectiles.Count; i++)
        {
            try
            {
                this.HeldProjectiles[i].DestroyMe();
            }
            catch(MissingReferenceException mre)
            {
                // Already destroyed
            }
        }
        if(ProjectilePrefab.LaunchType == ProjectileLaunchType.SUSTAINED)
        {
            if(this.sfx != null)
            {
                this.sfx.Stop();
            }
        }
        this.HeldProjectiles.Clear();
    }

    bool IsHoldingProjectiles()
    {
        return this.HeldProjectiles.Count > 0;
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

            if (ProjectilePrefab.LaunchType == ProjectileLaunchType.FIRE_AND_FORGET)
            {
                foreach (Transform projectileSpawn in this.SpawnPositions)
                {
                    Projectile newProjectile = GameObject.Instantiate(ProjectilePrefab.gameObject, projectileSpawn.position, projectileSpawn.rotation, null).GetComponent<Projectile>();
                    newProjectile.Fire(MyShip);
                }

                // Add the reload time to the cooldown
                this.Cooldown += GetCooldownMax();

                // Deduct the energy needed to fire the bullet from the ship.
                // Shouldn't kill the ship outright

                float damage = ProjectilePrefab.EnergyCost;
                if (!MyShip.IsControlledByAI())
                {
                    damage = Mathf.Min(damage, Mathf.Max(0.0f, MyShip.Energy - 1.0f));
                }
                MyShip.DamageMe(damage);

                // We've fired our projectile, this eliminates our charge
                ResetCharge();

                // Play firing SFX
                PlaySound();
            }

            else if (ProjectilePrefab.LaunchType == ProjectileLaunchType.SUSTAINED)
            {
                if(!this.IsHoldingProjectiles())
                {
                    // We haven't spawned in our held sustained projectiles yet
                    foreach (Transform projectileSpawn in this.SpawnPositions)
                    {
                        Projectile newProjectile = GameObject.Instantiate(ProjectilePrefab.gameObject).GetComponent<Projectile>();
                        newProjectile.Fire(MyShip);
                        newProjectile.transform.parent = projectileSpawn;
                        this.HeldProjectiles.Add(newProjectile);
                    }

                }

                // Pretty unelegant, but it wasn't working otherwise
                // Move the held projectiles to follow the ship
                foreach (Projectile heldProj in this.HeldProjectiles)
                {
                    try
                    {
                        heldProj.transform.localPosition = Vector3.zero;
                        heldProj.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    }
                    catch(MissingReferenceException mre)
                    {
                        //Already been destroyed
                        DestroyHeldProjectiles();
                        break;
                    }
                }
                // Deduct the energy needed to sustain the beam from the ship.
                float damage = ProjectilePrefab.EnergyCost * Time.deltaTime;
                if(!MyShip.IsControlledByAI())
                {
                    damage = Mathf.Min(damage, Mathf.Max(0.0f, MyShip.Energy - 1.0f));
                }
                MyShip.DamageMe(damage);

                // Play firing SFX
                PlaySound();
            }

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
            if (ProjectilePrefab.LaunchType == ProjectileLaunchType.FIRE_AND_FORGET)
            {
                sfx.Play();
            }
        }
    }

    /// <summary>
    /// Returns the type of projectile that this turret fires.
    /// </summary>
    /// <returns>The ProjectileType fired from this turret</returns>
    public ProjectileType GetTurretType()
    {
        return ProjectilePrefab.PType;
    }

    private float GetCooldownMax()
    {
        return ProjectilePrefab.ReloadTime * MyShip.Mods.ReloadMultiplier;
    }

    public float GetReadyPercentage()
    {
        if(this.ProjectilePrefab.ReloadTime > 0)
        {
            return Mathf.Clamp(1.0f - (this.Cooldown / GetCooldownMax()), 0.0f, 1.0f);
        }
        else
        {
            return 1.0f;
        }
    }
}

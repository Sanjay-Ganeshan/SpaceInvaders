using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// A superclass for all projectile types. A projectile is anything launched from a turret.
/// </summary>
public class Projectile : MonoBehaviour {

    /// <summary>
    /// How much damage this deals on hit.
    /// </summary>
    public float Damage;

    /// <summary>
    /// How long this bullet will last 
    /// before disappating.
    /// </summary>
    public float Lifetime { get; protected set; }

    /// <summary>
    /// How long this bullet will last, maximum.
    /// After this time, the bullet will disappate.
    /// </summary>
    public float MaxLifetime;

    /// <summary>
    /// How long after launching one projectile of this kind one must
    /// wait before launching another.
    /// </summary>
    public float ReloadTime;

    /// <summary>
    /// The type of projectile
    /// </summary>
    public ProjectileType PType;


    /// <summary>
    /// How the turret should launch this projectile
    /// </summary>
    public ProjectileLaunchType LaunchType;

    /// <summary>
    /// The cost, in energy, of firing a projectile
    /// </summary>
    public float EnergyCost;

    /// <summary>
    /// The amount of time that a ship must charge up before the projectile
    /// is actually launched
    /// </summary>
    public float ChargeTime;

    /// <summary>
    /// The ship from which this projectile was fired.
    /// </summary>
    protected Ship FiredBy;

    /// <summary>
    /// The rigidbody attached to this projectile.
    /// </summary>
    protected Rigidbody2D rb;

    // Use this for initialization
    void Start() {
        // Projectile Start - this will never
        // actually run, because everything subclasses Projectile,
        // but, this can serve as a template for subclasses

        CommonStart();
    }

    // Update is called once per frame
    void Update() {
        // Projectile Update

        CommonUpdate();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Projectile Collision

        CommonCollision(other);
    }

    /// <summary>
    /// Performs operations in Update common to all projectiles.
    /// </summary>
    protected void CommonUpdate()
    {
        if (this.MaxLifetime > 0)
        {
            // Update the current lifetime to the best of our knowledge
            this.Lifetime -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Performs operations in OnTriggerEnter2D(Collider2D other) common to all projectiles.
    /// </summary>
    /// <param name="other">The collider that this projectile collided with</param>
    protected void CommonCollision(Collider2D other)
    {
        // If it's a ship, it has a Ship component
        Ship otherShip = other.gameObject.GetComponent<Ship>();

        // If it IS a ship, and the ship isn't the original firing 
        // ship (since the projectile starts on top of the firing ship)
        // This projectile hit an enemy!
        if (otherShip != null && otherShip != this.FiredBy && otherShip.IsControlledByAI() != this.FiredBy.IsControlledByAI())
        {
            // Call an overridable handler
            this.OnHit(otherShip);
        }
    }

    /// <summary>
    /// Overridable method that activates whenever this projectile hit's a 
    /// ship (no self-attacks)
    /// </summary>
    /// <param name="hit">The sihp that this projectile hit</param>
    protected virtual void OnHit(Ship hit)
    {
        // No default behavior - overridden by children
    }

    /// <summary>
    /// Performs operations in Start() common 
    /// to all projectiles.
    /// </summary>
    protected void CommonStart()
    {
        if (this.MaxLifetime > 0)
        {
            // Set this object to be destroyed after it's lifetime
            Destroy(this.gameObject, this.MaxLifetime);
        }

        // Get the rigidbody - super common among subclasses
        rb = GetComponent<Rigidbody2D>();

        // Set the remaining lifetime to it's maximum
        this.Lifetime = this.MaxLifetime;
    }


    /// <summary>
    /// Get's the Lifetime left in this projectile
    /// as a fraction of its maximum.
    /// </summary>
    /// <returns>The percent of lifetime this projectile has remaining.</returns>
    protected float LifetimeFraction()
    {
        return this.Lifetime / this.MaxLifetime;
    }

    /// <summary>
    /// Determines if the Lifetime fraction of this 
    /// projectile is in between the two given endpoints (inclusive).
    /// </summary>
    /// <param name="minimum">The minimum inclusive bound for the lifetime fraction, from 0.0 to 1.0</param>
    /// <param name="maximum">The maximum inclusive bound for the lifetime fraction, from 0.0 to 1.0</param>
    /// <returns>If this projectile's remaining lifetime is between the fractions given.</returns>
    protected bool LifetimeFractionBetween(float minimum, float maximum)
    {
        // Get the fraction
        float fraction = LifetimeFraction();

        // Check minimum <= fraction <= maximum
        return fraction >= minimum && fraction <= maximum;
    }

    /// <summary>
    /// Fire this projectile from the given ship.
    /// </summary>
    /// <param name="from">The ship this projectile was fired from</param>
    public virtual void Fire(Ship from)
    {
        this.FiredBy = from;
    }

    /// <summary>
    /// Instantly destroys this projectile.
    /// </summary>
    public void DestroyMe()
    {
        // We need to use a timed destroy, 
        // because objects are not allowed to instantly destroy themselves.
        // Use the common destruction time.
        try
        {
            Destroy(this.gameObject, Common.DestroyTime);
        }
        catch (Exception e)
        {
            // Already marked for death
        }
    }

}

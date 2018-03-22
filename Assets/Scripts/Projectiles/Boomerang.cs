using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class Boomerang: Projectile
{
    /// <summary>
    /// The speed at which the simple shot will move
    /// </summary>
    public float Speed = 30.0f;

    public float DamagePerTime = 1.0f;
    public float VelocityPerTime = 1.0f;

    public float ReversalPoint = 0.8f;

    private bool hasReversed = false;

    // Use this for initialization
    void Start()
    {
        // Projectile Initialization
        CommonStart();

        // Start the bullet's velocity
        DoStart();
    }

    // Update is called once per frame
    void Update()
    {
        // Projectile update
        CommonUpdate();
        ReverseIfNeeded();
        this.rb.velocity += (VelocityPerTime * Time.deltaTime * this.rb.velocity.normalized);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Default Projectile Collision
        CommonCollision(other);
        Ship otherShip = other.gameObject.GetComponent<Ship>();
        if (hasReversed && otherShip != null && otherShip == FiredBy)
        {
            //Catch the boomerang
            rb.velocity = Vector2.zero;
            DestroyMe();
        }
    }

    /// <summary>
    /// Initializes the SimpleShot after being fired
    /// </summary>
    private void DoStart()
    {
        this.rb.velocity = this.transform.right.normalized * Speed;
    }

    private void ReverseIfNeeded()
    {
        if (!hasReversed && this.LifetimeFractionBetween(0.0f, ReversalPoint))
        {
            if(FiredBy != null)
            {
                hasReversed = true;
                //this.rb.velocity *= -1;
                this.transform.TurnToFace(FiredBy.transform.position);
                this.rb.velocity = this.rb.velocity.magnitude * this.transform.right;
            }
            else
            {
                hasReversed = true;
                this.rb.velocity *= -1;
                DestroyMe();
            }
            
        }
    }

    /// <summary>
    /// Triggered when this projectile hits another. Just damages it.
    /// </summary>
    /// <param name="hit">The ship that was hit</param>
    protected override void OnHit(Ship hit)
    {
        float damageToDeal = this.Damage + this.DamagePerTime * Mathf.Max(0.0f, ((this.MaxLifetime * ReversalPoint) - this.Lifetime));
        if (FiredBy != null)
        {
            damageToDeal *= FiredBy.Mods.BulletMagnitudeMultiplier;
        }
        // Damage the hit ship
        hit.DamageMe(amount: damageToDeal , showShield: true);

        // Stop it from piercing another target before being destroyed.
        rb.velocity = Vector2.zero;

        // And destroy the bullet
        DestroyMe();
    }

    public override void Fire(Ship from)
    {
        base.Fire(from);
        this.hasReversed = false;
    }
}

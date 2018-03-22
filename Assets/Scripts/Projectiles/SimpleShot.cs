using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleShot : Projectile {

    /// <summary>
    /// The speed at which the simple shot will move
    /// </summary>
    public float Speed = 30.0f;

	// Use this for initialization
	void Start () {
        // Projectile Initialization
        CommonStart();

        // Start the bullet's velocity
        DoStart();
	}
	
	// Update is called once per frame
	void Update () {
        // Projectile update
        CommonUpdate();
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        // Default Projectile Collision
        CommonCollision(other);
    }

    /// <summary>
    /// Initializes the SimpleShot after being fired
    /// </summary>
    private void DoStart()
    {
        this.rb.velocity = this.transform.right.normalized * Speed * FiredBy.Mods.BulletMagnitudeMultiplier;
    }

    /// <summary>
    /// Triggered when this projectile hits another. Just damages it.
    /// </summary>
    /// <param name="hit">The ship that was hit</param>
    protected override void OnHit(Ship hit)
    {
        // Damage the hit ship
        hit.DamageMe(amount: this.Damage, showShield: true);

        // Stop it from piercing another target before being destroyed.
        rb.velocity = Vector2.zero;

        // And destroy the bullet
        DestroyMe();
    }


}

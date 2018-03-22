using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A boarding shot, when it hits a ship, will transfer player control to that 
/// ship. 
/// 
/// Do NOT fire this shot from an AI - it will add a second player-controlled ship.
/// </summary>
public class BoardingShot : Projectile {

    /// <summary>
    /// The speed at which the boarding shot will move
    /// </summary>
    public float Speed = 30.0f;

    // Use this for initialization
    void Start()
    {
        // Projectile initialization
        CommonStart();

        // Start the shot's velocity
        DoStart();
    }

    
    void Update()
    {
        // Projectile update.
        CommonUpdate();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Projectile Collision defaults
        CommonCollision(other);
    }

    /// <summary>
    /// Initializes the SimpleShot after being fired
    /// </summary>
    private void DoStart()
    {
        // Just make it go forward at it's start speed.
        this.rb.velocity = this.transform.right.normalized * Speed;
    }

    /// <summary>
    /// Triggered when this projectile hits another. Just damages it.
    /// </summary>
    /// <param name="hit">The ship that was hit</param>
    protected override void OnHit(Ship hit)
    {
        // Destroy the firing ship - there should only be 1 player.
        this.FiredBy.DestroyMe();

        // Set the hit ship to be player controlled.
        hit.ChangeController(isAI: false);

        hit.Mods.CopyFrom(FiredBy.Mods);

        // Stop it from piercing another target before it is destroyed.
        rb.velocity = Vector2.zero;
        
        // Lastly, destroy the shot
        DestroyMe();
    }
}

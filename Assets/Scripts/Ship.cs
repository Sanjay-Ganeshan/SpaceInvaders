using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains all functionality needed to control any ship. Keeps 
/// track of ship stats, modifiers, movement, and combat.
/// </summary>
public class Ship : MonoBehaviour {

    /// <summary>
    /// The amount of energy left in this ship.
    /// </summary>
    public float Energy = 100.0f;
    
    /// <summary>
    /// The maximum possible energy this ship can have.
    /// </summary>
    public float MaxEnergy = 100.0f;

    /// <summary>
    /// All Turrets that can be used by this ship.
    /// </summary>
    public Turret[] Turrets;

    /// <summary>
    /// The velocity this ship goes at, in any direction.
    /// </summary>
    public float Thrust = 3.0f;

    public float BoosterMultiplier = 6.0f;

    public float BoosterCost = 10.0f;

    /// <summary>
    /// The GameObject that houses all of the turrets of this ship. 
    /// It will be rotated when targetting points.
    /// </summary>
    public GameObject TurretBase;

    /// <summary>
    /// The Explosion sprite to place underneath this when this ship dies.
    /// </summary>
    public Explosion ExplosionSprite;

    /// <summary>
    /// The sprite used to render the shield of the ship
    /// </summary>
    public Shield MyShield;



    /// <summary>
    /// The rigidbody attached to this component.
    /// </summary>
    Rigidbody2D rb;

    Collider2D collider2d;

    /// <summary>
    /// A reference to the radar, which knows the locations of all ships
    /// </summary>
    public Radar GlobalRadar;

    /// <summary>
    /// The type of ship this is
    /// </summary>
    public ShipType SType;

    /// <summary>
    /// A unique identifier for this ship, generated at runtime.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// A list of ShipControllers attached to this ship, 
    /// that will be polled when changing controllers
    /// </summary>
    private ShipController[] controllers;

    /// <summary>
    /// Stat modifiers that belong to this ship
    /// </summary>
    public StatsMods Mods;


    /// <summary>
    /// Not definitive, just stored for convenience
    /// </summary>
    public bool isAI;

    /// <summary>
    /// The indicator that will appear on the minimap
    /// </summary>
    public MinimapIndicator Indicator;

	// Use this for initialization
	void Start () {

        // Create a new unique identifier for this ship. 
        this.Id = Guid.NewGuid();

        // Get the rigidbody assigned to this ship.
        this.rb = GetComponent<Rigidbody2D>();

        this.collider2d = GetComponent<Collider2D>();

        // Find the radar and get a reference to it.
        GlobalRadar = GameObject.FindGameObjectWithTag("Radar").GetComponent<Radar>();

        // Get all the possible controllers of this ship.
        this.controllers = GetComponents<ShipController>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Inflicts a given amount of damage onto this ship.
    /// If the ship runs out of energy, it will be destroyed.
    /// </summary>
    /// <param name="amount">The amount of damage to deal to this ship</param>
    /// <param name="showShield">Whether or not to show an animating shield around the ship</param>
    public void DamageMe(float amount, bool showShield, bool trueDamage)
    {
        // Reduce the ship's energy by the damage inflicted

        // Divide damage by the energy mod = multiply energy by energy mod
        if (!trueDamage)
        {
            amount /= Mods.EnergyMultiplier;
        }
        this.Energy -= amount;

        if (showShield)
        {
            MyShield.Activate(this.Energy / this.MaxEnergy);
        }
        

        // Check that the ship still has energy remaining. If it doesn't,
        // Destroy it
        if(this.Energy <= 0)
        {
            this.DestroyMe();
        }
    }

    /// <summary>
    /// Inflicts a given amount of damage onto this ship.
    /// If the ship runs out of energy, it will be destroyed.
    /// </summary>
    /// <param name="amount">The amount of damage to deal to this ship</param>
    public void DamageMe(float amount)
    {
        DamageMe(amount, false, false);
    }

    public void DamageMe(float amount, bool showShield)
    {
        DamageMe(amount, showShield, false);
    }



    /// <summary>
    /// Restores energy to this ship.
    /// </summary>
    /// <param name="amount">The amount of energy to restore to the ship</param>
    public void RepairMe(float amount)
    {
        // Healing is just the opposite of damaging
        this.DamageMe(-1.0f * amount);
    }

    /// <summary>
    /// Destroys this ship
    /// </summary>
    public void DestroyMe()
    {
        // Tell the radar we're going down
        GlobalRadar.KillShip(this);

        if (this.ExplosionSprite != null)
        {
            this.ExplosionSprite.transform.parent = null;
            this.ExplosionSprite.StartTimer();
            this.ExplosionSprite.gameObject.SetActive(true);
        }

        // And mark this ship for death
        Destroy(this.gameObject, Common.DestroyTime);
    }

    /// <summary>
    /// Move in the given 2D world-direction. The ship will rotate 
    /// to point where it is moving.
    /// </summary>
    /// <param name="direction">The direction for the ship to move in.</param>
    public void Move(Vector2 direction, bool useBoost = false)
    {
        // Velocity = Speed * Direction Vector

        float speed = Thrust * this.Mods.SpeedMultiplier;
        if(useBoost)
        {
            float damage = BoosterCost * Time.deltaTime;
            if(damage < this.Energy)
            {
                this.DamageMe(damage, false, true);
                speed *= BoosterMultiplier;
            }
        }

        this.rb.velocity = (direction.normalized * speed);

        // If we're actually moving, turn to face the direction we're moving in
        if (!Mathf.Approximately(this.rb.velocity.magnitude, 0.0f))
        {
            this.transform.TurnToFaceDirection(direction);
        }
    }

    /// <summary>
    /// Moves this ship towards the given target point in 2D space. Only 
    /// sets the velocity once, so it must be called multiple times to stop behind,
    /// or to follow, the target.
    /// </summary>
    /// <param name="target"></param>
    public void MoveTowards(Vector2 target, bool useBoost = false)
    {
        // Get the direction to the target, and move in that direction
        Move(target - this.transform.position.ToVector2());
    }

    /// <summary>
    /// Target the given 2D world space point with this ship's turrets.
    /// </summary>
    /// <param name="target">A 2D point to point the turrets at</param>
    public void Target(Vector2 target)
    {
        // Turn our turret to face our target
        TurretBase.transform.TurnToFace(target);
    }

    /// <summary>
    /// Tries firing or enabling the given turret number on this ship.
    /// </summary>
    /// <param name="number">The turret number to fire on this ship. Fails silently if not enough turrets.</param>
    public void FireTurret(int number)
    {
        // Check that the turret index is within our turret array
        if(number >= 0 && number < this.Turrets.Length)
        {
            // This is a valid turret. Send the fire signal!
            Turrets[number].Fire();
        }
        else
        {
            // We don't have that turret, fail silently
        }
    }

    /// <summary>
    /// Fire the default turret on this ship.
    /// </summary>
    public void FireDefaultTurret()
    {
        FireTurret(GetDefaultTurretNumber());
    }

    /// <summary>
    /// Determines if we can currently fire the turret of the given number
    /// </summary>
    /// <param name="number">The index of the turret to fire</param>
    /// <returns>True if we have this turret, and it can currently fire.</returns>
    public bool CanFireTurret(int number)
    {
        // We can't fire a turret if we don't have it
        if(!HasTurret(number))
        {
            return false;
        }
        // If we do have the turret, check that it can fire right now
        else
        {
            return Turrets[number].CanFire();
        }
    }

    /// <summary>
    /// Determines if a given turret number is valid for this ship.
    /// </summary>
    /// <param name="number">The number of the turret</param>
    /// <returns>If this is a valid turret for this ship.</returns>
    public bool HasTurret(int number)
    {
        // Check that the index is in our array
        return number >= 0 && number < Turrets.Length;
    }

    /// <summary>
    /// Determines if this ship can fire its default turret.
    /// </summary>
    /// <returns>If the default turret can be fired. False if this ship has no turrets.</returns>
    public bool CanFireDefaultTurret()
    {
        // Get the turret number, then check if it can fire.
        return CanFireTurret(GetDefaultTurretNumber());
    }

    /// <summary>
    /// Get the number of turrets attached to this ship.
    /// </summary>
    /// <returns>The number of turrets on this ship (n). 0 to n-1 inclusive are valid turret numbers. </returns>
    public int NumberOfTurrets()
    {
        return this.Turrets.Length;
    }

    /// <summary>
    /// Get the turret number of the default turret
    /// </summary>
    /// <returns>The default turret's number</returns>
    private int GetDefaultTurretNumber()
    {
        return 0;
    }

    /// <summary>
    /// Change the controller of this ship to AI or player.
    /// </summary>
    /// <param name="isAI">Whether this ship should be an AI or a Player</param>
    public void ChangeController(bool isAI)
    {
        // Iterate through all controllers attached to this ship,
        // regardless of if they are currently acting
        foreach(ShipController c in this.GetComponents<ShipController>())
        {
            // If it's an AI, turn it on if and only if we should change controls to AI.
            c.enabled = (isAI == c.isAI());
        }
    }

    public bool IsControlledByAI()
    {
        /*
        foreach(ShipController c in this.controllers)
        {
            if (c.enabled == true && c.isAI() == true)
            {
                return true;
            }
        }
        return false;
        */

        // Right 99 / 100 times and is much faster to compute
        return this.isAI;
    }

    public void SetAI(bool isAI)
    {
        this.isAI = isAI;
        this.Indicator.SetAI(isAI);
    }

    public float GetTurretReadyPercentage(int turretNumber)
    {
        if(HasTurret(turretNumber))
        {
            return this.Turrets[turretNumber].GetReadyPercentage();
        }
        else
        {
            return 0.0f;
        }
    }

    public float GetDefaultTurretReadyPercentage()
    {
        return GetTurretReadyPercentage(GetDefaultTurretNumber());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Ship otherShip = other.GetComponent<Ship>();
        if(otherShip != null)
        {
            otherShip.DestroyMe();
            this.DestroyMe();
        }
    }
}

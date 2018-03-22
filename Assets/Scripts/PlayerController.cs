using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the ship according to a player's keyboard and mouse input
/// </summary>
public class PlayerController : ShipController {

    /// <summary>
    /// The Horizontal Axis configured in Input.
    /// This will control horizontal direction
    /// </summary>
    private const string HORIZONTAL_INPUT = "Horizontal";

    /// <summary>
    /// The Vertical Axis configured in Input.
    /// This will control vertical direction.
    /// </summary>
    private const string VERTICAL_INPUT = "Vertical";

    /// <summary>
    /// The button configured to fire your primary weapon
    /// </summary>
    private const string FIRE1_INPUT = "Fire1";

    /// <summary>
    /// The button configured to fire your secondary weapon
    /// </summary>
    private const string FIRE2_INPUT = "Fire2";

    CrosshairFollow Crosshair;


    /// <summary>
    /// The turret to fire as primary
    /// </summary>
    //private const int FIRE1_TURRET = 0;

    /// <summary>
    /// The turret to fire as secondary
    /// </summary>
    //private const int FIRE2_TURRET = 1;

	void Start () {
        // Let the parent class initialize
        CommonStart();
        Crosshair = GameObject.FindGameObjectWithTag("Crosshair").GetComponent<CrosshairFollow>();
	}
	
	// Update is called once per frame
	void Update () {
        // Let the parent class update
        CommonUpdate();

        // Handle moving based on keyboard
        DoMovement();

        // Rotate the turret to your target
        DoTargetting();

        // And fire if necessary
        DoFiring();
	}

    /// <summary>
    /// Handles moving the ship based on player input.
    /// </summary>
    private void DoMovement()
    {
        // The direction we want to go in is the inputted <x,y>
        float h = Input.GetAxis(HORIZONTAL_INPUT);
        float v = Input.GetAxis(VERTICAL_INPUT);
        bool shouldBoost = Input.GetButton("Boost");

        MyShip.Move(new Vector2(h, v), shouldBoost);
    }

    /// <summary>
    /// Handles setting the turret target based on player input.
    /// </summary>
    private void DoTargetting()
    {
        // We always target the mouse
        //Vector2 target = Common.GetMousePosition();
        Vector2 target = Crosshair.GetTarget();

        MyShip.Target(target);
    }

    /// <summary>
    /// Handles firing weapons based on player input.
    /// </summary>
    private void DoFiring()
    {
        // Arbitrarily assign left click greater priority than right click

        // Using GetButton so that the user can hold to repeatedly shoot

        // If the player is holding down FIRE1 (Default: Left Click),
        // Fire the primary turret
        if (Input.GetAxis(FIRE1_INPUT) > 0)
        {
            MyShip.FireTurret(GameConfig.PRIMARY_TURRET_NUMBER);
        }

        // If they are not already firing the primary turret, 
        // and they are holding down FIRE2,
        // Fire the second turret
        else if (Input.GetAxis(FIRE2_INPUT) > 0)
        {
            MyShip.FireTurret(GameConfig.SECONDARY_TURRET_NUMBER);
        }

        // Note that this does not guarentee the weapons fire,
        // they are given the fire INPUT.
        // They will fire / charge up / cool down as needed
    }

    /// <summary>
    /// Determines if this controller is an AI.
    /// </summary>
    /// <returns>Whether this controller is an AI</returns>
    public override bool isAI()
    {
        // This is a player controller, so return false.
        return false;
    }
}

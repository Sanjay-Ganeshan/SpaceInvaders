using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A superclass for all ship controllers. Ship Controllers
/// Have access to a sihp, and send commands to that ship.
/// </summary>
public class ShipController : MonoBehaviour {

    public Ship MyShip;

	// Use this for initialization
	void Start () {
        CommonStart();
	}

    /// <summary>
    /// Performs Start() operations common to all controllers
    /// </summary>
    protected void CommonStart()
    {
        // If we haven't already assigned the ship, 
        // try getting a ship attached to this game object.
        if (MyShip == null)
        {
            MyShip = GetComponent<Ship>();
        }
    }

    /// <summary>
    /// Returns if this ship is an AI or not.
    /// </summary>
    /// <returns>Whether or not this ship is an AI</returns>
    public virtual bool isAI()
    {
        return false;
    }
	
	// Update is called once per frame
	void Update () {
        CommonUpdate();
	}

    /// <summary>
    /// Shared behavior between all Ship Controllers
    /// </summary>
    protected void CommonUpdate()
    {
        // Let the radar know if we're a player or not
        MyShip.GlobalRadar.UpdateShipInfo(MyShip, isAI());
        MyShip.SetAI(isAI());
    }
}

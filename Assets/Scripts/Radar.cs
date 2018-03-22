using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The radar keeps track of all of the ships in the game at any time.
/// </summary>
public class Radar : MonoBehaviour {

    // The player's ship
    private Ship PlayerShip;

    // A knowledge base of all ships on the map
    private Dictionary<Guid, Ship> AllShips;

    // The camera follower
    private CameraFollow cam;
    
    // How long the game should be in seconds
    public float GameTimeLimit;

    private Canvas EndDialog;

    public DisplayTimeRemaining timer;

    // Use this for initialization
    void Start () {
        // Setup a lookup dict
        AllShips = new Dictionary<Guid, Ship>();

        // We don't know the player. They'll reveal themselves
        PlayerShip = null;
        
        // Find the camera
        cam = Camera.main.gameObject.GetComponent<CameraFollow>();

        EndDialog = GameObject.FindGameObjectWithTag("GameEnd").GetComponent<Canvas>();

    }
	
	// Update is called once per frame
	void Update () {
        
	}

    // LateUpdate is called once per frame, after each frame
    void LateUpdate()
    {
        // If we know the camera
        if(cam != null)
        {
            // And we know the player
            if (PlayerShip != null)
            {
                // Tell the camera to follow the player
                cam.Focus = this.PlayerShip.transform;
            }
        }


        if(timer.TimeRemaining() < 0)
        {
            EndGame();

        }

        if(PlayerShip == null)
        {
            Invoke("EndGame", 0.1f);
        }
        else
        {
            CancelInvoke("EndGame");
        }
        

        // Since each ship updates us with their status during Update(),
        // we have the up-to-date status of every ship during LateUpdate()
    }

    private void EndGame()
    {
        if(PlayerShip != null)
        {
            PlayerShip.DestroyMe();
        }
        
        EndDialog.enabled = true;
    }

    /// <summary>
    /// Gets the ship that is being controlled by the Player, to the best knowledge of the radar.
    /// </summary>
    /// <returns>The ship that is being controlled by the Player</returns>
    public Ship GetPlayerShip()
    {
        return this.PlayerShip;
    }

    /// <summary>
    /// Gets a list of all ships in the game, regardless of owner.
    /// </summary>
    /// <returns>An array of references to all currently in game ships</returns>
    public Ship[] GetAllShips()
    {
        // Create an array
        Ship[] returnShips = new Ship[AllShips.Count];

        // Copy references to it so mutating the array doesn't harm the radar.
        AllShips.Values.CopyTo(returnShips, 0);
        return returnShips;
    }

    /// <summary>
    /// Finds a ship, based on it's Guid. Returns null if no 
    /// such ship exists.
    /// </summary>
    /// <param name="target">The ID of the ship to find</param>
    /// <returns>A reference to this ship, or null if no such ship exists</returns>
    public Ship GetShipById(Guid target)
    {
        if(this.AllShips != null && this.AllShips.ContainsKey(target))
        {
            return this.AllShips[target];
        }
        else
        {
            return null;
        }
    } 

    /// <summary>
    /// Updates the radar with the latest information from a ship.
    /// </summary>
    /// <param name="ship">The ship sending the update</param>
    /// <param name="isAI">Whether this ship is being controlled by an AI</param>
    public void UpdateShipInfo(Ship ship, bool isAI)
    {
        // If it's not an AI, it's a player. Update our local
        // variable for quick lookups.
        if(!isAI)
        {
            this.PlayerShip = ship;
        }

        // If we don't already know about this ship, 
        // Add a reference to it. The reference will never 
        // change, so we don't need to update it 
        // if we already have it.
        if (!AllShips.ContainsKey(ship.Id)) 
            AllShips.Add(ship.Id, ship);
    }
    
    /// <summary>
    /// Informs the radar that a given ship has been destroyed
    /// </summary>
    /// <param name="ship">The ship that has been destroyed</param>
    public void KillShip(Ship ship)
    {
        // If we knew about it in the first place
        if(AllShips.ContainsKey(ship.Id))
        {
            // Delete it from the active ships
            AllShips.Remove(ship.Id);
        }
    }
}

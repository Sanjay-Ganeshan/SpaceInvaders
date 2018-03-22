using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Enumerates all possible Projectiles in the game
/// </summary>
public enum ProjectileType
{
    /// <summary>
    /// A basic bullet that does damage on hit
    /// </summary>
    BASIC_BULLET,
    /// <summary>
    /// A charge-up massively powerful penetrating bullet
    /// </summary>
    RAILGUN,
    /// <summary>
    /// Individually, each is weaker than a basic bullet,
    /// but with a very high fire rate
    /// </summary>
    MACHINE_GUN,
    /// <summary>
    /// This does moderate damage, and reverses direction, potentially doubling damage.
    /// </summary>
    BOOMERANG,

    /// <summary>
    /// Proximity mines that detonate for massive damage
    /// </summary>
    MINE,

    /// <summary>
    /// A Laser fires a continuous, instantaneous beam, but deals minimal damage.
    /// </summary>
    LASER,

    /// <summary>
    /// Fires 4 powerful shots, equally spaced and 
    /// </summary>
    SHOTGUN,

    /// <summary>
    /// Tethers onto another ship, potentially boarding it.
    /// </summary>
    TETHER,

    /// <summary>
    /// A boarding capsule transfers a player to another ship.
    /// </summary>
    BOARDING_CAPSULE
}

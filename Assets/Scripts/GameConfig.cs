using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// A set of configuration options shared across the game
/// </summary>
public static class GameConfig
{
    /// <summary>
    /// Whether or not to play music
    /// </summary>
    public static bool MusicEnabled = true;

    /// <summary>
    /// Whether or not to play sound effects
    /// </summary>
    public static bool SFXEnabled = false;

    public static bool IsPaused = false;

    public const int PRIMARY_TURRET_NUMBER = 0;
    public const int SECONDARY_TURRET_NUMBER = 1;
}

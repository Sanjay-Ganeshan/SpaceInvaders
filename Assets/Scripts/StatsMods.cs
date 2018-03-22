using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Modifiers for a ship. Controllers carry different mods.
/// </summary>
[Serializable]
public class StatsMods  {
    public float EnergyMultiplier;
    public float SpeedMultiplier;
    public float ReloadMultiplier;
    public float BulletMagnitudeMultiplier;
    public StatsMods()
    {
        this.EnergyMultiplier = 1.0f;
        this.SpeedMultiplier = 1.0f;
        this.ReloadMultiplier = 1.0f;
        this.BulletMagnitudeMultiplier = 1.0f;
    }
    public StatsMods(float energyMultiplier, float speedMultiplier, float reloadMultiplier, float bulletMagnitudeMultiplier)
    {
        this.EnergyMultiplier = energyMultiplier;
        this.SpeedMultiplier = speedMultiplier;
        this.ReloadMultiplier = reloadMultiplier;
        this.BulletMagnitudeMultiplier = bulletMagnitudeMultiplier;
    }
    public void CopyFrom(StatsMods other)
    {
        this.EnergyMultiplier = other.EnergyMultiplier;
        this.SpeedMultiplier = other.SpeedMultiplier;
        this.ReloadMultiplier = other.ReloadMultiplier;
        this.BulletMagnitudeMultiplier = other.BulletMagnitudeMultiplier;
    }
}

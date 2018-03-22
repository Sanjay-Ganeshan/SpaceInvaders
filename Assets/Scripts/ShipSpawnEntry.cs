using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


[Serializable]
public class ShipSpawnEntry
{
    public ShipType ShipType;
    public Ship ShipPrefab;
    public StatsMods Mods;
    public float Probability;
}

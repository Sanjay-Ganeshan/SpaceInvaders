using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {

    public ShipSpawnEntry[] spawns;

    public float SpawnPeriod;
    public int NumberToSpawn;
    public bool SpawnsActive;

    Radar GlobalRadar;

    public float MinSpawnDistance;
    public float MaxSpawnDistance;

    private float TimeSinceSpawn;

    Dictionary<ShipType, ShipSpawnEntry> lookup;


	// Use this for initialization
	void Start () {
        //InvokeRepeating("Spawn", 5f, SpawnPeriod);
        GlobalRadar = GameObject.FindGameObjectWithTag("Radar").GetComponent<Radar>();
        TimeSinceSpawn = 0.0f;
        lookup = new Dictionary<ShipType, ShipSpawnEntry>();
        foreach(ShipSpawnEntry entry in spawns)
        {
            if(!lookup.ContainsKey(entry.ShipType))
            {
                lookup.Add(entry.ShipType, entry);
            }
        }
	}

    void Update ()
    {
        if(SpawnsActive)
        {
            TimeSinceSpawn += Time.deltaTime;
            if(TimeSinceSpawn >= SpawnPeriod)
            {
                SpawnWave();
                TimeSinceSpawn = 0.0f;
            }
        }
        else
        {
            TimeSinceSpawn = 0.0f;
        }
    }

    public void Spawn(ShipType t, Vector2 PlayerOffset)
    {
        ShipSpawnEntry entry = lookup[t];
        if(entry != null)
        {
            
        }
    }
	
	// Update is called once per frame
	void SpawnWave () {
        //int luck = Random.Range(0, 10);
        if(GlobalRadar == null)
        {
            GlobalRadar = GameObject.FindGameObjectWithTag("Radar").GetComponent<Radar>();
        }

        for (int spawnNum = 0; spawnNum < NumberToSpawn; spawnNum++)
        {
            float luck = Random.Range(0.0f, 100.0f);
            foreach (ShipSpawnEntry possibleSpawn in spawns)
            {
                if (luck <= possibleSpawn.Probability)
                {
                    Vector3 positionToSpawn = Random.insideUnitCircle.normalized * (Random.Range(MinSpawnDistance, MaxSpawnDistance));
                    float rotationToSpawn = Random.Range(-179.0f, 179.0f);
                    Ship newShip = null;
                    if (GlobalRadar.GetPlayerShip() != null)
                    {
                        newShip = Instantiate(possibleSpawn.ShipPrefab.gameObject, positionToSpawn + GlobalRadar.GetPlayerShip().transform.position, Quaternion.AngleAxis(rotationToSpawn, Vector3.forward)).GetComponent<Ship>();
                    }
                    if (newShip != null)
                    {
                        newShip.ChangeController(true);
                        newShip.Mods.CopyFrom(possibleSpawn.Mods);
                    }
                    break;
                }
                else
                {
                    luck -= possibleSpawn.Probability;
                }
            }
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleHealth : MonoBehaviour {

    private Radar GlobalRadar;

    private Slider HealthBar;

	// Use this for initialization
	void Start () {

        // Find the radar and get a reference to it.
        GlobalRadar = GameObject.FindGameObjectWithTag("Radar").GetComponent<Radar>();

        HealthBar = GetComponentInChildren<Slider>();

    }
	
	// Update is called once per frame
	void Update () {
        if (GlobalRadar.GetPlayerShip() != null)
        {
            float m_eng = GlobalRadar.GetPlayerShip().MaxEnergy;
            float eng = GlobalRadar.GetPlayerShip().Energy;

            HealthBar.value = eng / m_eng;
        }
        else
        {
            HealthBar.value = 0.0f;
        }
	}
}

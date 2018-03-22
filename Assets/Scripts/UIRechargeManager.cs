using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRechargeManager : MonoBehaviour {

    public Image bulletIndicator;
    public Image capsulIndicator;

    private Radar radar;

	// Use this for initialization
	void Start () {
		radar = GameObject.FindGameObjectWithTag("Radar").GetComponent<Radar>();
    }
	
	// Update is called once per frame
	void Update () {
        if (radar.GetPlayerShip() != null)
        {
            float g = radar.GetPlayerShip().GetTurretReadyPercentage(GameConfig.PRIMARY_TURRET_NUMBER);

            float c = radar.GetPlayerShip().GetTurretReadyPercentage(GameConfig.SECONDARY_TURRET_NUMBER);

            bulletIndicator.fillAmount = g;
            capsulIndicator.fillAmount = c;
        }
        
       
    }
}

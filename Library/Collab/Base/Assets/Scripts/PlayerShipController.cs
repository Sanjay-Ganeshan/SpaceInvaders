using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipController : MonoBehaviour {

    const string INPUT_LATERAL = "Horizontal";
    const string INPUT_THRUST = "Vertical";
    const string INPUT_FIRE = "Fire1";

    Spaceship ControlledShip;

	// Use this for initialization
	void Start () {
        ControlledShip = GetComponent<Spaceship>();
	}
	
	// Update is called once per frame
	void Update () {
        ControlledShip.applyLateral(Input.GetAxis(INPUT_LATERAL));
        ControlledShip.applyThrust(Input.GetAxis(INPUT_THRUST));
        //Vector2 target = Camera.main
        Vector3 mouseTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 target = new Vector2(mouseTarget.x, mouseTarget.y);
        if(Input.GetButtonDown(INPUT_FIRE))
        {
            ControlledShip.Fire();
        }
    }
}

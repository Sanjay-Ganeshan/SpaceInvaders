using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipController : MonoBehaviour {

    const string INPUT_LATERAL = "Horizontal";
    const string INPUT_THRUST = "Vertical";
    const string INPUT_FIRE = "Fire1";

    Spaceship ControlledShip;
    Turret shipTurret;

	// Use this for initialization
	void Start () {
        ControlledShip = GetComponent<Spaceship>();
        shipTurret = GetComponent<Turret>();
	}
	
	// Update is called once per frame
	void Update () {

        ControlledShip.applyRotation(Input.GetAxis(INPUT_LATERAL));
        ControlledShip.applyForwardSpeed(Input.GetAxis(INPUT_THRUST));

        //Vector2 target = Camera.main

        var mouse = Input.mousePosition;
        var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
        var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        shipTurret.transform.rotation = Quaternion.Euler(0, 0, angle);

        if (Input.GetButtonDown(INPUT_FIRE))
        {
            ControlledShip.Fire();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    public Bullet ammoPrefab;
    public Transform outlet;

    private Turret shipTurret;

	// Use this for initialization
	void Start () {
        shipTurret = GetComponent<Turret>();
	}
	
	// Update is called once per frame
	void Update () {
        outlet = shipTurret.transform;
	}

    public void Fire()
    {
        Bullet newBullet = GameObject.Instantiate(ammoPrefab.gameObject, outlet.transform.position, outlet.transform.rotation, null).GetComponent<Bullet>();
        newBullet.GetComponent<Rigidbody2D>().velocity = newBullet.transform.up * newBullet.InitialBulletSpeed;
    }
}

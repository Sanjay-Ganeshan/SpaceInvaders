using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    public Bullet ammoPrefab;
    public Transform outlet;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Fire()
    {
        Bullet newBullet = GameObject.Instantiate(ammoPrefab.gameObject, outlet.transform.position, outlet.transform.rotation, null).GetComponent<Bullet>();
        newBullet.GetComponent<Rigidbody2D>().velocity = newBullet.transform.right * newBullet.InitialBulletSpeed;
    }
}

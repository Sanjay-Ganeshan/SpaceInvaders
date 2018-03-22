using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour {

    public float maximumThrust;

    private float lateralSteer = 0.0f;
    private float forwardSteer = 0.0f;


    public Gun gun;

    private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        gun = GetComponent<Gun>();
	}
	
	// Update is called once per frame
	void Update () {
        this.rb.velocity = new Vector2(this.lateralSteer * this.maximumThrust * Time.deltaTime, this.forwardSteer * this.maximumThrust * Time.deltaTime);
        Vector3 newRot = new Vector3(0, 0, Mathf.Rad2Deg * Mathf.Atan2(this.rb.velocity.y, this.rb.velocity.x));
        if (this.rb.velocity.magnitude > 0)
        {
            this.transform.rotation = Quaternion.Euler(newRot);
        }
        //Vector2 input = new Vector2(this.lateralSteer * this.maximumLateralThrust, this.forwardSteer * this.maximumForwardThrust);
        //this.rb.AddForce(this.forwardSteer * this.maximumForwardThrust * this.transform.up * Time.deltaTime);
        //this.rb.AddTorque(-1 * this.lateralSteer * this.maximumLateralThrust * Time.deltaTime);
	}

    public void applyLateral(float magnitude)
    {
        this.lateralSteer = Mathf.Clamp(magnitude, -1.0f, 1.0f);
    }

    public void applyThrust(float magnitude)
    {
        this.forwardSteer = Mathf.Clamp(magnitude, -1.0f, 1.0f);
    }

    public void Fire()
    {
        if (this.gun != null)
        {
            this.gun.Fire();
        }
    }
}

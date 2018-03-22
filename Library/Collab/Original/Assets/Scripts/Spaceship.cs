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

    public void Target(Vector2 target)
    {
        /*
        Vector3 local = gun.transform.InverseTransformPoint(target);
        Vector2 xy = new Vector2(local.x, local.y);
        float desiredAngle = Mathf.Rad2Deg * Mathf.Atan2(xy.y, xy.x);
        gun.transform.rotation = Quaternion.Euler(0, 0, desiredAngle);
        */
        Vector3 target3 = new Vector3(target.x, target.y, 0);
        Quaternion newRot = Quaternion.LookRotation(target3 - gun.transform.position, Vector3.back);
        Debug.Log(newRot.eulerAngles);
        gun.transform.rotation = Quaternion.Euler(0,0,newRot.eulerAngles.z);

    }

    public void Fire()
    {
        if (this.gun != null)
        {
            this.gun.Fire();
        }
    }
}

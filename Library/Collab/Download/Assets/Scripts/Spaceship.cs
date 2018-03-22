using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour {

    public float MAX_FORWARD_SPEED = 5f;
    public float MAX_TURN_SPEED = 180f;

    private float turnInput = 0.0f;
    private float forwardInput = 0.0f;

    public Gun gun;
    private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        gun = GetComponent<Gun>();
	}
	
	// Update is called once per frame
	void Update () {

        // Caluclate new rotation
        Quaternion rot = transform.rotation;

        float z = rot.eulerAngles.z;

        z -= turnInput * MAX_TURN_SPEED * Time.deltaTime;

        rot = Quaternion.Euler(0, 0, z);

        transform.rotation = rot;

        Debug.Log("Command " + turnInput + " | output " + z);

        // Valculate new position
        Vector3 del_pos = new Vector3(forwardInput * MAX_FORWARD_SPEED * Time.deltaTime, 0, 0);

        transform.position += rot * del_pos;


        /*this.rb.velocity = new Vector2(this.lateralSteer * this.maximumThrust * Time.deltaTime, this.forwardSteer * this.maximumThrust * Time.deltaTime);

        this.rb.transform
        
        Vector3 newRot = new Vector3(0, 0, Mathf.Rad2Deg * Mathf.Atan2(this.rb.velocity.y, this.rb.velocity.x));
        if (this.rb.velocity.magnitude > 0)
        {
            this.transform.rotation = Quaternion.Euler(newRot);
        }*/
        //Vector2 input = new Vector2(this.lateralSteer * this.maximumLateralThrust, this.forwardSteer * this.maximumForwardThrust);
        //this.rb.AddForce(this.forwardSteer * this.maximumForwardThrust * this.transform.up * Time.deltaTime);
        //this.rb.AddTorque(-1 * this.lateralSteer * this.maximumLateralThrust * Time.deltaTime);
	}

    public void applyRotation(float magnitude)
    {
        this.turnInput = Mathf.Clamp(magnitude, -1.0f, 1.0f);
    }

    public void applyForwardSpeed(float magnitude)
    {
        this.forwardInput = Mathf.Clamp(magnitude, -1.0f, 1.0f);
    }

    public void Fire()
    {
        if (this.gun != null)
        {
            this.gun.Fire();
        }
    }
}

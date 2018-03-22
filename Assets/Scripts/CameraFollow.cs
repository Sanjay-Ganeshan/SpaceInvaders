using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script allows the Camera to continually follow a focal point (center)
/// </summary>
public class CameraFollow : MonoBehaviour {

    /// <summary>
    /// The transform to center on the screen
    /// </summary>
    public Transform Focus;

	// Use this for initialization
	void Start () {
		
	}
	
    void Update ()
    {

    }

	// LateUpdate is called once per frame, after Update
	void LateUpdate () {
        // If we have something to focus on
		if(Focus != null)
        {
            // Move the camera to be in the same 2D position, shifted back by it's 
            // original viewing distance.
            this.transform.position = new Vector3(Focus.position.x, Focus.position.y, this.transform.position.z);
        }
	}
}

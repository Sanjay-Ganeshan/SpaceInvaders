using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapFollow : MonoBehaviour {

    CameraFollow MainFollow;
	// Use this for initialization
	void Start () {
        MainFollow = Camera.main.GetComponent<CameraFollow>();
	}

    /// <summary>
    /// The transform to center on the screen
    /// </summary>
    public Transform Focus;

    void Update()
    {

    }

    // LateUpdate is called once per frame, after Update
    void LateUpdate()
    {
        Focus = MainFollow.Focus;
        // If we have something to focus on
        if (Focus != null)
        {
            // Move the camera to be in the same 2D position, shifted back by it's 
            // original viewing distance.
            this.transform.position = new Vector3(Focus.position.x, Focus.position.y, this.transform.position.z);
        }
    }
}

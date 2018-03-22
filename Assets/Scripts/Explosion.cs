using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public float deathTime = 1.0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartTimer()
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (!GameConfig.SFXEnabled && audio != null)
        {
            audio.mute = true;
        }
        Destroy(this.gameObject, deathTime);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTimeRemaining : MonoBehaviour {

    Text display;

    Radar r;

    public float startTime = -1.0f;

	// Use this for initialization
	void Start () {
        display = GetComponent<Text>();
        r = GameObject.FindGameObjectWithTag("Radar").GetComponent<Radar>();
	}
	
	// Update is called once per frame
	void Update () {
        if (startTime > 0)
        {
            int t = (int)(r.GameTimeLimit - (Time.time - startTime));
            display.text = (t > 0 ? t : 0).ToString();
        }
	}

    internal int TimeRemaining()
    {
        int t = (int)(r.GameTimeLimit - (Time.time - startTime));
        return t;
    }
}

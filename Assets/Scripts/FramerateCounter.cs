using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FramerateCounter : MonoBehaviour {

    public Text outText;

	// Use this for initialization
	void Start () {
		if(outText == null)
        {
            outText = GetComponent<Text>();
        }
	}
	
	// Update is called once per frame
	void Update () {
        outText.text = string.Format("FPS: {0}", (int) (1.0f / Time.deltaTime));
	}
}

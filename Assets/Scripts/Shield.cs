using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    const float FlickerTime = 0.2f;

    private float EndTime = 0.0f;

    public SpriteRenderer ShieldRenderer;

	// Use this for initialization
	void Start () {
        this.ShieldRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > EndTime)
        {
            this.gameObject.SetActive(false);
        }
	}

    public void Activate (float shieldStrength)
    {
        if(this.ShieldRenderer == null)
        {
            this.ShieldRenderer = GetComponent<SpriteRenderer>();
        }
        Color newColor = new Color(1.0f, 1.0f, 1.0f, Mathf.Clamp(shieldStrength, 0.0f, 1.0f));
        this.ShieldRenderer.color = newColor;
        this.EndTime = Time.time + FlickerTime;
        this.gameObject.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour {
    public Color AIColor;
    public Color PlayerColor;

    public SpriteRenderer indicator;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void ColorFor(bool isAI)
    {
        if (isAI)
        {
            indicator.color = AIColor;
        }
        else
        {
            indicator.color = PlayerColor;
        }
    }
}

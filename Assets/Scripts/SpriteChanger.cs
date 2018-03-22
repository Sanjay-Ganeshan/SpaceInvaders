using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChanger : MonoBehaviour {
    public Sprite AISprite;
    public Sprite PlayerSprite;

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
            indicator.sprite = AISprite;
        }
        else
        {
            indicator.sprite = PlayerSprite;
        }
    }
}

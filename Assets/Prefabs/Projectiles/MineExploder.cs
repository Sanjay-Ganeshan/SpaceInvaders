using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineExploder : MonoBehaviour {

    public float TimeToArm = 0.2f;

    private float Lifetime;
	// Use this for initialization
	void Start () {
        Lifetime = 0;
	}
	
	// Update is called once per frame
	void Update () {
        Lifetime += Time.deltaTime;
	}

    void OnTriggerEnter2D(Collider2D other) {
        if(Lifetime < TimeToArm)
        {
            return;
        }
        Projectile otherProjectile = other.GetComponent<Projectile>();  
        if ( otherProjectile != null && otherProjectile.PType != ProjectileType.MINE)
        {
            //Explode();
            otherProjectile.DestroyMe();
            this.SendMessageUpwards("ExplodeMine");
            
        }
    }
}

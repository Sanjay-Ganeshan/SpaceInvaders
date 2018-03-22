using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class LaserBeam: Projectile
{
    const string LASER_LAYER = "Ship";

    const int HITS_TO_CHECK = 3;

    public float MaxRange = 10.0f;

    

    public LineRenderer BeamRenderer;

    private LayerMask mask;
    private ContactFilter2D filter;
    private RaycastHit2D[] hits = new RaycastHit2D[HITS_TO_CHECK];

    void Start()
    {
        CommonStart();
        mask = LayerMask.GetMask(LASER_LAYER);
        filter = new ContactFilter2D();
        filter.NoFilter();
        filter.SetLayerMask(mask);
    }

    void Update()
    {
        CommonUpdate();
        ComputeHits();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Do nothing, it's a sustain
    }

    void ComputeHits()
    {
        //Debug.Log("computing hits");
        int numTargets = Physics2D.Raycast(this.transform.position.ToVector2(), this.transform.right.ToVector2(), filter, hits);
        BeamRenderer.SetPositions(new Vector3[] {this.transform.position, this.transform.position});
        for(int i = 0; i < Math.Min(numTargets, hits.Length); i++)
        {
            //Debug.Log("Analyzing hit " + i);
            RaycastHit2D hit = hits[i];
            if(hit.collider != null)
            {
                //Debug.Log("Hit is not null " + hit.collider);
                Ship s = hit.collider.gameObject.GetComponent<Ship>();
                if(s != null && s != FiredBy)
                {
                    // We hit a ship x3, of course
                    BeamRenderer.SetPositions(new Vector3[] { this.transform.position, s.transform.position });
                    DoHit(s);
                    break;
                }   
            }
        }
    }

    void DoHit(Ship s)
    {
        s.DamageMe(this.Damage * Time.deltaTime * FiredBy.Mods.BulletMagnitudeMultiplier, true);
    }



}

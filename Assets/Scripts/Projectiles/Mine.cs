using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class Mine: Projectile
{
    public Explosion ExplosionSprite;
    public int MaxTargets = 10;
    public float BulletTriggerRadius;
    public SpriteChanger TeamColor;



    void Start()
    {
        CommonStart();
        InvokeRepeating("Recolor", 5f, 5f);
        
    }

    void Update()
    {
        CommonUpdate();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        CommonCollision(other);
    }

    public override void Fire(Ship from)
    {
        base.Fire(from);
        Recolor();
    }

    void Recolor()
    {
        TeamColor.ColorFor(this.FiredBy.IsControlledByAI());
    }

    protected override void OnHit(Ship hit)
    {
        ExplodeMine();
    }

    void ExplodeMine()
    {
        Collider2D c = this.GetComponent<Collider2D>();
        Collider2D[] collisions = new Collider2D[MaxTargets];
        int numCollisions = c.OverlapCollider(new ContactFilter2D().NoFilter(), collisions);
        Ship temp;
        for (int i = 0; i < Math.Min(numCollisions, collisions.Length); i++)
        {
            temp = collisions[i].GetComponent<Ship>();
            if (temp != null)// && temp.IsControlledByAI() != this.FiredBy.IsControlledByAI())
            {
                temp.DamageMe(this.Damage);
            }
        }
        if (this.ExplosionSprite != null)
        {
            this.ExplosionSprite.transform.parent = null;
            this.ExplosionSprite.StartTimer();
            this.ExplosionSprite.gameObject.SetActive(true);
        }
        this.DestroyMe();
    }
}

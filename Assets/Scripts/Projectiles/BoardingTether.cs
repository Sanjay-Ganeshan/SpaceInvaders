using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class BoardingTether: Projectile
{
    /// <summary>
    /// The speed at which the boarding shot will move
    /// </summary>
    public float Speed = 30.0f;

    public float TetherNeeded = 3.0f;

    private float TetherTime;

    public float TetherDistance = 100.0f;

    public LineRenderer TetherLine;

    public SpriteRenderer[] renderersToDisable;

    private Ship TetherTarget;

    private bool hasHitTarget;

    // Use this for initialization
    void Start()
    {
        // Projectile initialization
        CommonStart();

        // Start the shot's velocity
        DoStart();
    }


    void Update()
    {
        // Projectile update.
        CommonUpdate();
        UpdateTether();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Projectile Collision defaults
        CommonCollision(other);
    }

    /// <summary>
    /// Initializes the SimpleShot after being fired
    /// </summary>
    private void DoStart()
    {
        // Just make it go forward at it's start speed.
        this.rb.velocity = this.transform.right.normalized * Speed;
        hasHitTarget = false;
    }

    /// <summary>
    /// Triggered when this projectile hits another. Just damages it.
    /// </summary>
    /// <param name="hit">The ship that was hit</param>
    protected override void OnHit(Ship hit)
    {
        if (this.TetherTarget == null)
        {
            this.TetherTarget = hit;
            //this.TetherLine.enabled = true;
            foreach(SpriteRenderer renderer in this.renderersToDisable)
            {
                renderer.enabled = false;
            }
            rb.velocity = Vector2.zero;
            hasHitTarget = true;
        }
    }

    private void UpdateTether()
    {
        try
        {
            if (this.TetherTarget != null)
            {
                if ((TetherTarget.transform.position - FiredBy.transform.position).magnitude < TetherDistance)
                {
                    //TetherLine
                    this.TetherTime += Time.deltaTime;
                    this.TetherLine.SetPositions(new Vector3[] { FiredBy.transform.position, TetherTarget.transform.position });
                    if (this.TetherTime >= this.TetherNeeded)
                    {
                        Debug.Log("Boarding " + TetherTarget);
                        Board(TetherTarget);
                    }
                }
                else
                {
                    BreakTether();
                }
            }
            else
            {
                if ((this.transform.position - FiredBy.transform.position).magnitude > TetherDistance || hasHitTarget)
                {
                    DestroyMe();
                }
                else
                {
                    this.TetherLine.SetPositions(new Vector3[] { FiredBy.transform.position, this.transform.position });
                }
            }
        }
        catch(MissingReferenceException mre)
        {
            BreakTether();
        }
    }

    private void BreakTether()
    {
        this.DestroyMe();
    }

    protected void Board(Ship hit)
    {
        // Destroy the firing ship - there should only be 1 player.
        this.FiredBy.DestroyMe();

        // Set the hit ship to be player controlled.
        hit.ChangeController(isAI: false);

        hit.Mods.CopyFrom(FiredBy.Mods);

        // Stop it from piercing another target before it is destroyed.
        rb.velocity = Vector2.zero;

        // Lastly, destroy the shot
        DestroyMe();
    }
}


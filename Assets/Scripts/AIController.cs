using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An AIController is a form of Ship Controller that will control
/// the ship as an AI to destroy the player.
/// </summary>
public class AIController : ShipController
{

    float trackingDistance = 130f;
    float battleDistance = 60f;
    float fleeDistance = 10f;

    // Use this for initialization
    void Start()
    {
        // Controller initialization
        CommonStart();
    }

    // Update is called once per frame
    void Update()
    {
        // Controller update
        CommonUpdate();

        if (MyShip != null)
        {

            // Try to follow the player
            FollowPlayer();

            // Target and Fire the player
            TargetAndFirePlayer();
        }
    }

    private void FollowPlayer()
    {
        try
        {
            float d = Vector2.SqrMagnitude(MyShip.transform.position - MyShip.GlobalRadar.GetPlayerShip().transform.position);

            // EDIT: Removed trackign distance
            if (d > battleDistance)
            {
                if (IsInvoking("SelectStrafeVector"))
                {
                    CancelInvoke("SelectStrafeVector");
                }
                MyShip.MoveTowards(Common.ToVector2(MyShip.GlobalRadar.GetPlayerShip().transform.position));

            }
            else if (d < battleDistance && d > fleeDistance)
            {
                if (!IsInvoking("SelectStrafeVector"))
                {
                    Invoke("SelectStrafeVector", Random.Range(0.1f, 1.0f));
                }

            }
            else if (d < fleeDistance)
            {
                if (IsInvoking("SelectStrafeVector"))
                {
                    CancelInvoke("SelectStrafeVector");
                }

                FleePlayer();
            }
            else
            {
                if (IsInvoking("SelectStrafeVector"))
                {
                    CancelInvoke("SelectStrafeVector");
                }
                MyShip.Move(new Vector2(0, 0));
            }
        }
        catch (MissingReferenceException mre)
        {

        }
    }

    private void SelectStrafeVector()
    {
        Vector2 t;

        Vector2 s = Common.ToVector2(Random.onUnitSphere) * battleDistance / 2;

        Vector2 p = MyShip.transform.position;

        t = p + s;

        MyShip.MoveTowards(t);

    }

    private void FleePlayer()
    {
        Vector2 p = MyShip.GlobalRadar.GetPlayerShip().transform.position;

        Vector2 v = Common.ToVector2(MyShip.transform.position) - p;

        v = v.normalized;

        MyShip.Move(v);

    }

    private void TargetAndFirePlayer()
    {
        // Fire at some random direction near the player ship
        if (MyShip.GlobalRadar.GetPlayerShip() != null)
        {
            Vector2 p = Common.ToVector2(MyShip.GlobalRadar.GetPlayerShip().transform.position);
            p.x = p.x + Random.Range(-1f, 1f);
            p.y = p.y + Random.Range(-1f, 1f);

            // Fire
            MyShip.Target(p);

            float d = Vector2.SqrMagnitude(MyShip.transform.position - MyShip.GlobalRadar.GetPlayerShip().transform.position);

            if (d < battleDistance + 5f)
            {
                MyShip.FireTurret(0);
            }
        }
    }

    /// <summary>
    /// Returns that this is an AI
    /// </summary>
    /// <returns>Whether the Controller is an AI</returns>
    public override bool isAI()
    {
        // This is an AI controller...so, it's an AI.
        return true;
    }
}

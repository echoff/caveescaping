using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceTrigger : TriggerBase
{
    public float damping = 0;
    private float preDamping;

    public override void Enter(GameObject other)
    {
        base.Enter(other);
        PlayerMovement pm = other.GetComponent<PlayerMovement>();
        if (pm != null)
        {
            preDamping = pm.groundDamping;
            pm.groundDamping = damping;
        }
    }

    public override void Exit(GameObject other)
    {
        base.Exit(other);
        PlayerMovement pm = other.GetComponent<PlayerMovement>();
        if (pm != null)
        {
            pm.groundDamping = preDamping;
        }
    }
}

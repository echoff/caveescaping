using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMConfinerTrigger : TriggerBase
{
    public override void Exit(GameObject other)
    {
        base.Exit(other);
        other.transform.position = Vector3.zero;
    }
}

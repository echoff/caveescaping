using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Enter(GameObject other)
    {
        Debug.Log(other.name+ " Enter.");
    }

    public virtual void Exit(GameObject other)
    {
        Debug.Log(other.name + " Exit.");
    }
}

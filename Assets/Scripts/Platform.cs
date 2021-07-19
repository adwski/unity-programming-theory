using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MovingPlatform
{
    [SerializeField] Vector3 relDestination;

    public void Activate()
    {
        StartMove(transform.position + relDestination, 5f, 5f);
    }

    public void Deactivate()
    {
        StopMove();
    }
}

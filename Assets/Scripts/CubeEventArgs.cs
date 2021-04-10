using System;
using UnityEngine;

public class CubeEventArgs : EventArgs
{
    public CubeEventArgs(ActorType targetObjType, Transform transform, bool blocked)
    {
        this.targetObjType = targetObjType;
        this.transform = transform;
        this.blocked = blocked;
    }

    public ActorType targetObjType;
    public Transform transform;
    public bool blocked;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereColl : Colliders
{
    private void Awake()
    {
        VirtualStart();
    }
    private void Start()
    {
        VirtualStart();
    }

    public override void VirtualStart()
    {
        base.VirtualStart();
        colltype = CharEnumTypes.eCollType.SphereColl;
        Mycollider = GetComponent<SphereCollider>();
    }

    public SphereCollider GetCollider()
    {
        return Mycollider as SphereCollider;
    }

    public override void SetRadious(float radius)
    {
        SphereCollider col = Mycollider as SphereCollider;
        col.radius = radius;
        
    }

    
}

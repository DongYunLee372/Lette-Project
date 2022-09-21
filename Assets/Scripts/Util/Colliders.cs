using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colliders : MonoBehaviour
{
    public CharEnumTypes.eCollType colltype;
    public string targetTag;
    public LayerMask targetLayer;
    public delegate void CollFunction(Collider other);
    public CollFunction _collFunction;
    public Collider Mycollider;

    public void SetCollitionFunction(CollFunction _function)
    {
        _collFunction = _function;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_collFunction!=null)
            _collFunction(other);
    }

    public void SetActive(bool t)
    {
        this.gameObject.SetActive(t);
    }

    public virtual void SetRadious(float radius)
    {
    }

    public virtual void SetSize(Vector3 size)
    {
    }

    public virtual void VirtualStart()
    {
        Start();
    }

    public void Start()
    {
        _collFunction = null;
        targetLayer = -1;
        targetTag = null;


    }

}

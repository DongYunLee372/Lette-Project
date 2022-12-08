using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colliders : MonoBehaviour
{
    public CharEnumTypes.eCollType colltype;
    public string targetTag;
    public LayerMask targetLayer;
    public delegate void CollFunction(Collider other);

    public CollFunction _EnterFunction;
    public CollFunction _OuterFunction;
    public CollFunction _StayFunction;

    public Collider Mycollider;

    public virtual void SetCollitionFunction(CollFunction _enterfunction, CollFunction _outerfunction, CollFunction _stayfunction)
    {
        _EnterFunction = _enterfunction;
        _OuterFunction = _outerfunction;
        _StayFunction = _stayfunction;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.gameObject.tag == (targetTag))
            _EnterFunction?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject.tag == (targetTag))
            _OuterFunction?.Invoke(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.gameObject.tag == (targetTag))
            _StayFunction?.Invoke(other);        
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

    public void SetParent(Transform _parent)
    {
        gameObject.transform.parent = _parent;
    }
    public virtual void VirtualStart()
    {
        //_EnterFunction = null;
        //_OuterFunction = null;
        //_StayFunction = null;
        targetLayer = -1;
        targetTag = null;
        //Start();
    }


}

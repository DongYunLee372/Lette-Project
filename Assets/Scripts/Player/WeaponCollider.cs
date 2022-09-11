using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    string tagname;

    public delegate void CollFunction(Collider other);
    CollFunction _collFunction;

    public void SetCollitionFunction(CollFunction _function)
    {
        _collFunction = _function;
    }


    private void OnTriggerEnter(Collider other)
    {
        _collFunction(other);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInteractive : MonoBehaviour
{

    public abstract bool P_IsInteractive { get; set; }
    public abstract int P_Instance { get; protected set; }
    public abstract EnumScp.InteractiveIndex P_interactive { get; protected set; }
    public abstract void Init();
    public abstract void Oninteractive();
    public virtual void Awake()
    {
        Init();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
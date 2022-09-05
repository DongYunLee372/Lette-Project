using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInteractive : MonoBehaviour
{
    [SerializeField]
    EnumScp.InteractiveIndex interactive;

    public EnumScp.InteractiveIndex P_interactive { get { return interactive; } set { interactive = value; } }
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

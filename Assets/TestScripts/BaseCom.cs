using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCom : MonoBehaviour
{
    [SerializeField]
    EnumScp.ComponentList comlist;

    public EnumScp.ComponentList P_comlist { get { return comlist; } set { comlist = value; } }

    public abstract void InitCom();


    public virtual void Awake()
    {
        InitCom();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

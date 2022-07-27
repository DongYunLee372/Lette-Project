using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseComponent : MonoBehaviour
{
    [SerializeField]
    EnumTypes.eComponentTypes comtype;

    public EnumTypes.eComponentTypes p_comtype
    {
        get
        {
            return comtype;
        }
        set
        {
            comtype = value;
        }
    }
    public abstract void InitComtype();

    public virtual void Init()
    {

    }


    public virtual void Awake()
    {
        InitComtype();
    }

    public virtual void InitSetting()
    {
        
    }


    public virtual void Update()
    {

    }

    //public abstract BaseComponent GetComponent();
    //public abstract void SetComponent();

    //public abstract void InitComponent();
}

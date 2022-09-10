using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseComponent : MonoBehaviour
{
    [SerializeField]
    CharEnumTypes.eComponentTypes comtype;

    public CharEnumTypes.eComponentTypes p_comtype
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
        InitComtype();
    }


    public virtual void Awake()
    {
        InitComtype();
    }

    public virtual void InitSetting()
    {
        
    }
}

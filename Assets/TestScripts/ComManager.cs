using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComManager : MonoBehaviour
{
    [SerializeField]
    BaseCom[] comlist = new BaseCom[(int)(EnumScp.ComponentList.Max)];
    void Start()
    {
        BaseCom[] tempcom = GetComponentsInChildren<BaseCom>();

        foreach(BaseCom a in tempcom)
        {
            comlist[(int)a.P_comlist] = a;
        }
    }

    
    void Update()
    {
        
    }
}

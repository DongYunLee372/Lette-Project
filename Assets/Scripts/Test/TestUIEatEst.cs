using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUIEatEst : MonoBehaviour
{
   

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("F1 ");
            InvenTory.Instance.UseItem(EnumScp.Key.F1 ,1);
        }
    }
}

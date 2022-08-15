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
       if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            UIManager.Instance.Prefabsload("Hpbar", UIManager.CANVAS_NUM.player_cavas);
        }
    }
}

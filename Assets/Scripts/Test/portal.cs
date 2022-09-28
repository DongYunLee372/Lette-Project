using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            GameData_Load.Instance.ChangeScene(Scenes_Stage.Stage2);
        }
    }



}

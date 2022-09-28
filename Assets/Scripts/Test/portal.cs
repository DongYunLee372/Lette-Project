using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            GameMG.Instance.ChangeScene(Scenes_Stage.Stage1);
        }
    }



}

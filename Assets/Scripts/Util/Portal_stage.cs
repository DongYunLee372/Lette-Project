using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal_stage : MonoBehaviour
{
    // Start is called before the first frame update
    bool flag = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            if(!flag)
            {
                GameData_Load.Instance.ChangeScene(Scenes_Stage.Stage1);
                flag = true;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTest : MonoBehaviour
{
   
    void Start()
    {
       GameObject temp=  GameMG.Instance.Resource.Instantiate<GameObject>("Skeleton_Warrior");

        GameMG.Instance.Resource.Destroy<GameObject>(temp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

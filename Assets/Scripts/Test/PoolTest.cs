using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTest : MonoBehaviour
{
   
    void Start()
    {

        GameObject temp4 = GameMG.Instance.Resource.Instantiate<GameObject>("WeaponCol");

        GameObject temp1 =  GameMG.Instance.Resource.Instantiate<GameObject>("Skeleton_Warrior");

        //  GameMG.Instance.Resource.Destroy<GameObject>(temp1);
    
       // GameObject temp2 = GameMG.Instance.Resource.Instantiate<Object.Colliders>("SphereColl");
     //   GameObject temp3 =  GameMG.Instance.Resource.Instantiate<Colliders>("BoxColl");



        StartCoroutine(temp());
    }

    IEnumerator temp()
    {
        int count = 10;
        while(count>=0)
        {
            yield return new WaitForSeconds(1f);
            GameObject temp = GameMG.Instance.Resource.Instantiate<GameObject>("Skeleton_Warrior");
            count--;
        }
       

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

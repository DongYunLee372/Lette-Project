using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTestScript : MonoBehaviour
{

    public GameObject pos;
    public GameObject pos1;
  


    // Start is called before the first frame update
    void Start()
    {
        GameObject postest = new GameObject();
        GameObject postest2 = new GameObject();

        postest.transform.position = new Vector3(0, 0, 0);
        postest2.transform.position = new Vector3(10, 0, 0);

        GameMG.Instance.Resource.Instantiate("Terrain", postest.transform);

        GameMG.Instance.Resource.Instantiate("PlayerCharacter", postest2.transform);

        // CharacterCreate.Instance.CreateMonster()
    }

    public void cleic()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

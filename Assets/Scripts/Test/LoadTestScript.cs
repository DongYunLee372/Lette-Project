using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTestScript : MonoBehaviour
{
    GameObject MapPos;
    GameObject PlayerInitPos;
    // Start is called before the first frame update
    void Start()
    {

         MapPos = new GameObject();
        PlayerInitPos = new GameObject();
        MapPos.transform.position = new Vector3(0, 0, 0);
        PlayerInitPos.transform.position = new Vector3(10, 0, 0);

        //생성
        GameMG.Instance.Resource.Instantiate("Terrain", MapPos.transform);
        GameMG.Instance.Resource.Instantiate("PlayerCharacter", PlayerInitPos.transform);

        CharacterCreate.Instance.CreateMonster(0, PlayerInitPos.transform);
    }

    public void cleic()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

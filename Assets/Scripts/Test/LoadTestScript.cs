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

        //생성 -어드레서블X 실행잘됌
        GameMG.Instance.Resource.Instantiate("Terrain", MapPos.transform);
        GameMG.Instance.Resource.Instantiate("PlayerCharacter", PlayerInitPos.transform);

        //생성 - 어드레서블 오류뜸
        // GameMG.Instance.Resource.Instantiate("Terrain", MapPos.transform);
        // GameMG.Instance.Resource.Instantiate("susu", PlayerInitPos.transform);

        CharacterCreate.Instance.CreateMonster(0, PlayerInitPos.transform);
    }

    //1. uiHP바 오류 ->메인 카메라 오류같음, 캐릭터 찾아야댐
    //2. 어드레서블 연결
    //3. Create호출함수 name호출? 찾기 만들기

    // Update is called once per frame
    void Update()
    {
        
    }
}

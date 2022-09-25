using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData_Load : Singleton<GameData_Load>
{

   
    void Start()
    {
       
        AddressablesLoadManager.Instance.OnSceneAction("Roomtest");

        //GameMG.Instance.startGame("Roomtest");
        TestPos_and_Load();
    }



    void TestPos_and_Load()  //기획자 인스펙터 창에서 수정한 값으로 생성하게 
    {
        var tempDataSave = TestMainLoad.Instance.AssetLoad_("Assets/GameData/TestGameData.asset");
            //AssetDatabase.LoadAssetAtPath<GameSaveData>("Assets/GameData/TestGameData.asset");

        foreach (var s in tempDataSave.SaveDatas)
        {
            Debug.Log("보는중 : " + s.prefabsName);

            if(s.prefabsName=="Boss")
            {
                BoosInit(s.prefabsName,s.Position);
            }
            else
            {
                AddressablesLoadManager.Instance.SingleLoad_Instantiate<GameObject>(s.prefabsName, s.Position);

            }
            Debug.Log("보는중 : " + s.Position);
        }

    }

    void BoosInit(string name,Vector3 pos)
    {
        GameObject abc = new GameObject();
        abc.transform.position = pos;

        AddressablesLoadManager.Instance.SingleAsset_Load<GameObject>(name);
        var boss = AddressablesLoadManager.Instance.FindLoadAsset<GameObject>(name);
      //  boss.GetComponent<Battle_Character>().Stat_Initialize(data, mon_Normal_Atk_Group, bossNomalSkills, monsterSkillInformation, target);

        StartCoroutine(CharacterCreate.Instance.CreateBossMonster_(EnumScp.MonsterIndex.mon_06_01, abc.transform, name));

     
    }

    void Update()
    {
        
    }


}

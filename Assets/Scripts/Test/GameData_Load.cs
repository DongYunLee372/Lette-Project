using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class GameData_Load :Singleton<GameData_Load>
{
    List<string> str=new List<string>();
    public SkyboxManager skyboxMG;
   

    void Start()
    {

     



    //    GameMG.Instance.startGame("Roomtest");

      //  AddressablesLoadManager.Instance.OnSceneAction("Roomtest");

        //GameMG.Instance.startGame("Roomtest");
     //   TestPos_and_Load();
      


    }



   public void TestPos_and_Load(Action action=null)  //기획자 인스펙터 창에서 수정한 값으로 생성하게 
    {



        str.Add("Hpbar");
        str.Add("FriendPanel");
        str.Add("Inven");
        str.Add("Boss_HP");
        str.Add("StartUI");
        str.Add("OptionSetting");

        AddressablesLoadManager.Instance.MultiAsset_Load<GameObject>(str);

        AddressablesLoadManager.Instance.SingleAsset_Load<GameSaveData>("TestGameData");

        var tempDataSave = AddressablesLoadManager.Instance.FindLoadAsset<GameSaveData>("TestGameData");
        // GameMG.Instance.startGame("Roomtest");

        var find = AddressablesLoadManager.Instance.Find_InstantiateObj<GameObject>("PlayerCharacter");
        find.SetActive(false);
        //  TestMainLoad.Instance.
        //    var tempDataSave = UnityEditor.AssetDatabase.LoadAssetAtPath<GameSaveData>("Assets/GameData/TestGameData.asset");
        StartCoroutine(CheckLoadScene(tempDataSave));
        AddressablesLoadManager.Instance.OnSceneAction("Roomtest");

        // var tempDataSave = TestMainLoad.Instance.AssetLoad_("Assets/GameData/TestGameData.asset");

        //  var tempDataSave = TestMainLoad.Instance.AssetLoad_("Assets/GameData/TestGameData.asset");
        //AssetDatabase.LoadAssetAtPath<GameSaveData>("Assets/GameData/TestGameData.asset");
       
        //foreach (var s in tempDataSave.SaveDatas)
        //{
        //    Debug.Log("보는중 : " + s.prefabsName);

        //    if(s!=null)
        //    {
        //        if (s.prefabsName == "Boss")
        //        {
        //            //GameObject abc = new GameObject();
        //            //abc.transform.position = s.Position;

        //            Debug.Log("dd");
        //            // StartCoroutine(CharacterCreate.Instance.CreateBossMonster_(EnumScp.MonsterIndex.mon_06_01, abc.transform, name));

        //            BoosInit(s.prefabsName, s.Position);
        //        }
        //        if(s.prefabsName== "PlayerCharacter")
        //        {
        //            var find = AddressablesLoadManager.Instance.FindLoadAsset<GameObject>("PlayerCharacter");
        //            if (find!=null)
        //            {
        //                Debug.Log("캐릭터 위치이동");
        //                find.transform.position = s.Position;
        //            }
        //            else
        //            {
        //                Debug.Log("캐릭터 생성");
        //                AddressablesLoadManager.Instance.SingleLoad_Instantiate<GameObject>(s.prefabsName, s.Position);
        //            }
        //        }
        //        else
        //        {
        //            AddressablesLoadManager.Instance.SingleLoad_Instantiate<GameObject>(s.prefabsName, s.Position);

        //        }
        //        Debug.Log("보는중 : " + s.Position);
        //    }

         
        //}

        if(action!=null)
        {
        action();
        }

    }
    
    void PlayerPos()
    {

        var tempDataSave = AddressablesLoadManager.Instance.FindLoadAsset<GameSaveData>("TestGameData");

        foreach (var a in tempDataSave.SaveDatas)
        {
            if(a.prefabsName== "PlayerCharacter")
            {
                var find = AddressablesLoadManager.Instance.FindLoadAsset<GameObject>("PlayerCharacter");
                if (find != null)
                {
                    Debug.Log("캐릭터 위치이동");
                  
                    find.transform.position = a.Position;
                }
            }
        }
    }

  

    IEnumerator Load_Boss()
    {

        AddressablesLoadManager.Instance.OnUnloadedAction("BoatScene");
        yield return new WaitForSeconds(2f);
        TestPos_and_Load();
        yield return new WaitForSeconds(2.5f);
        PlayerPos();
         var find=AddressablesLoadManager.Instance.Find_InstantiateObj<GameObject>("PlayerCharacter");
        find.SetActive(true);
        var boss= AddressablesLoadManager.Instance.Find_InstantiateObj<GameObject>("Boss");
        boss.SetActive(true);



    }

    public void DataLoad(GameSaveData tempDataSave)
    {
        foreach (var s in tempDataSave.SaveDatas)
        {
            Debug.Log("보는중 : " + s.prefabsName);

            if (s != null)
            {
                if (s.prefabsName == "Boss")
                {
                    //GameObject abc = new GameObject();
                    //abc.transform.position = s.Position;

                    Debug.Log("dd");
                    // StartCoroutine(CharacterCreate.Instance.CreateBossMonster_(EnumScp.MonsterIndex.mon_06_01, abc.transform, name));
                    GameObject abc = new GameObject();
                    abc.transform.position = s.Position;

                    StartCoroutine(CharacterCreate.Instance.CreateBossMonster_S(EnumScp.MonsterIndex.mon_06_01, abc.transform, s.prefabsName));
                }
                else if (s.prefabsName == "PlayerCharacter")
                {
                    var find = AddressablesLoadManager.Instance.Find_InstantiateObj<GameObject>("PlayerCharacter");
                    if (find != null)
                    {
                        Debug.Log("캐릭터 위치이동");
                        find.transform.position = s.Position;
                    }
                    else
                    {
                        Debug.Log("캐릭터 생성");
                        AddressablesLoadManager.Instance.SingleLoad_Instantiate<GameObject>(s.prefabsName, s.Position);
                    }
                }
                else
                {
                    Debug.Log("보스 캐릭터아닌 뭔가를 생성하러옴");
                    AddressablesLoadManager.Instance.SingleLoad_Instantiate<GameObject>(s.prefabsName, s.Position);

                }
                Debug.Log("보는중 : " + s.Position);
            }


        }
    }

    public void ChangeScene(Scenes_Stage num)
    {
        switch (num)
        {
            case Scenes_Stage.Stage1:
                GameMG.Instance.scenes_Stage = Scenes_Stage.Stage1;
                BoatScene_Data_Load();
                skyboxMG.SkyBox_Setting("BoatScene");
                break;

            case Scenes_Stage.Stage2:
                GameMG.Instance.scenes_Stage = Scenes_Stage.Stage2;
                StartCoroutine(Load_Boss());
                skyboxMG.SkyBox_Setting("Roomtest");
                break;


            case Scenes_Stage.Boss:

                break;
        }
    }

    public void BoatScene_Data_Load()
    {

        AddressablesLoadManager.Instance.SingleAsset_Load<GameSaveData>("BoatData");

        var tempDataSave = AddressablesLoadManager.Instance.FindLoadAsset<GameSaveData>("BoatData");

        if (tempDataSave == null)
        {
            Debug.Log("데이터 널");
        }
        foreach (var a in tempDataSave.SaveDatas)
        {
            Debug.Log("가져온데이터 이름" + a.prefabsName);
            Debug.Log("가져온데이터 위치" + a.Position);

        }

        // AddressablesLoadManager.Instance.action1= DataLoad;

        StartCoroutine(CheckLoadScene(tempDataSave));
        AddressablesLoadManager.Instance.OnSceneAction("BoatScene");

    }

    IEnumerator CheckLoadScene(GameSaveData tempsave)
    {
        while(true)
        {
            if(AddressablesLoadManager.Instance.SceneLoadCheck==true)
            {
                Debug.Log("gpg?");
                DataLoad(tempsave);
                Debug.Log("gpg2?");
                AddressablesLoadManager.Instance.SceneLoadCheck = false;
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(0.3f);
            }
        }
    }

    public void BoatScene()
    {

    }

    void Update()
    {
        
    }


}

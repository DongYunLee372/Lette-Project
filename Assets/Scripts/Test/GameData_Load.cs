using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.UI;
using TMPro;
using Global_Variable;
using Canvas_Enum;
public class GameData_Load : Singleton<GameData_Load>
{
    int MonsterDeadCount = 3;
    List<string> str = new List<string>();
    public SkyboxManager skyboxMG;
    public GameObject Canvas_;
    public GameObject Im;
    GameLoadingData gameLoadingData;
    int gameLoadingCount = 0;

    GameObject LoadingImgae;  //이미지
    GameObject LoadingText_Ins;  //텍스트
    List<LoadingData> loadingDatas = new List<LoadingData>();
    bool ImageCheck = false;

    public List<GameObject> tempcheckList = new List<GameObject>(10);


    int MonsterCount = 0;
    void Start()
    {





        //    GameMG.Instance.startGame("Roomtest");

        //  AddressablesLoadManager.Instance.OnSceneAction("Roomtest");

        //GameMG.Instance.startGame("Roomtest");
        //   TestPos_and_Load();



    }

    class LoadingData
    {
        public string scripts;
        public string ImageName;
    }


    public void TestPos_and_Load(bool restart=false)  //기획자 인스펙터 창에서 수정한 값으로 생성하게 
    {



        str.Add("Hpbar");
        str.Add("FriendPanel");
        str.Add("Inven");
        str.Add("Boss_HP");
        str.Add("StartUI");
        str.Add("OptionSetting");

        AddressablesLoadManager.Instance.MultiAsset_Load<GameObject>(str);
        GameSaveData tempDataSave;
        if (!restart)
        {
            AddressablesLoadManager.Instance.SingleAsset_Load<GameSaveData>("TestGameData");
            tempDataSave = AddressablesLoadManager.Instance.FindLoadAsset<GameSaveData>("TestGameData");
        }
        else
        {
            AddressablesLoadManager.Instance.SingleAsset_Load<GameSaveData>("BossRestartGameData");
            tempDataSave = AddressablesLoadManager.Instance.FindLoadAsset<GameSaveData>("BossRestartGameData");
        }
        // GameMG.Instance.startGame("Roomtest");
        if(tempDataSave==null)
        {
            Debug.LogError("없어!!!!!!!!");
        }
        var find = AddressablesLoadManager.Instance.Find_InstantiateObj<GameObject>("PlayerCharacter");
        if (find != null)
        {
            find.SetActive(false);

        }
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


        
    }

    void PlayerPos(bool restart)
    {

        GameSaveData tempDataSave;
        if(restart)
        {
          
            AddressablesLoadManager.Instance.SingleAsset_Load<GameSaveData>("BossRestartGameData");
            tempDataSave = AddressablesLoadManager.Instance.FindLoadAsset<GameSaveData>("BossRestartGameData"); 
        }
        else
        {
           
            tempDataSave = AddressablesLoadManager.Instance.FindLoadAsset<GameSaveData>("TestGameData");
        }
        if(tempDataSave==null)
        {
            Debug.LogError("없어!!!!");
            return;
        }

        foreach (var a in tempDataSave.SaveDatas)
        {
            if (a.prefabsName == "PlayerCharacter")
            {
                var find = AddressablesLoadManager.Instance.FindLoadAsset<GameObject>("PlayerCharacter");
                if (find != null)
                {
                   
                    find.transform.position = a.Position;
                }
            }
        }
    }



    IEnumerator Load_Boss(bool restart=false)
    {

        //  yield return new WaitForSeconds(2f);
        TestPos_and_Load(restart);
        yield return new WaitForSeconds(2.5f);
        //PlayerPos(restart);

        //잠시 주석 테스트
        //var find = AddressablesLoadManager.Instance.Find_InstantiateObj<GameObject>("PlayerCharacter");
        //find.SetActive(true);

        //var boss = AddressablesLoadManager.Instance.Find_InstantiateObj<GameObject>("Boss");
        //boss.SetActive(true);

        //SoundManager.Instance.bgmSource.GetComponent<AudioSource>().PlayOneShot(SoundManager.Instance.Bgm[1]);
        //SoundManager.Instance.bgmSource.GetComponent<AudioSource>().loop = true;

    }

    public void DataLoad(GameSaveData tempDataSave)
    {
        foreach (var s in tempDataSave.SaveDatas)
        {
         
            if (s != null)
            {
                if (s.prefabsName == "Boss")
                {
                    //GameObject abc = new GameObject();
                    //abc.transform.position = s.Position;

                
                    // StartCoroutine(CharacterCreate.Instance.CreateBossMonster_(EnumScp.MonsterIndex.mon_06_01, abc.transform, name));
                    GameObject abc = new GameObject();
                    abc.transform.position = s.Position;
                    StartCoroutine( CharacterCreate.Instance.CreateBossMonster_S(EnumScp.MonsterIndex.mon_06_01, abc.transform, s.prefabsName));
                    Destroy(abc);
                }
                else if (s.prefabsName == "PlayerCharacter")
                {
                    var find = AddressablesLoadManager.Instance.Find_InstantiateObj<GameObject>("PlayerCharacter");
                    if (find != null)
                    {
                      
                        find.transform.position = s.Position;
                    }
                    else
                    {
                      

                        AddressablesLoadManager.Instance.SingleLoad_Instantiate<GameObject>(s.prefabsName, s.Position);
                       
                    }
                    // AddressablesLoadManager.Instance.SingleAsset_Load<GameObject>("Inven");
                    //UIManager.Instance.Prefabsload("Inven", CANVAS_NUM.player_cavas);
                }
                else if (s.prefabsName == "Skeleton")
                {
                    GameObject Monster = new GameObject();
                    Monster.transform.position = s.Position;
                
                    MonsterCount++;
                    StartCoroutine( CharacterCreate.Instance.CreateMonster_S(EnumScp.MonsterIndex.mon_01_01, Monster.transform, s.prefabsName));
                    //생성한 포지션 삭제
                    Destroy(Monster);
                }
                else
                {
                 
                    AddressablesLoadManager.Instance.SingleLoad_Instantiate<GameObject>(s.prefabsName, s.Position);

                    //}
                }
             
            }


        }
    }

    public void ChangeScene(Scenes_Stage num)
    {
        switch (num)
        {
            case Scenes_Stage.Stage1:
                Canvas_.SetActive(true);
                if (GameMG.Instance.scenes_Stage != Scenes_Stage.Loding)
                {
                    unloadBoatScene(Scenes_Stage.Stage1);
                    //skyboxMG.SkyBox_Setting("BoatScene");
                    //GameMG.Instance.scenes_Stage = Scenes_Stage.Stage1;
                    // break;
                }
                //StartCoroutine(CheckInputKey());
                ImageCheck = true;
                GameMG.Instance.scenes_Stage = Scenes_Stage.Stage1;
                LoadingImageShow(Scenes_Stage.Stage1);
                // BoatScene_Data_Load();
                skyboxMG.SkyBox_Setting("BoatScene");

                //체크
                break;

            case Scenes_Stage.Stage2:
                SoundManager.Instance.bgmSource.GetComponent<AudioSource>().Stop();
                UIManager.Instance.Hide("Boss_HP");
                AddressablesLoadManager.Instance.OnUnloadedAction("BoatScene");  //언로드
                                                                                 // EndunLoadBoatScene();
                UnloadMonster_restart();
                GameMG.Instance.scenes_Stage = Scenes_Stage.Stage2;
                var charatcter = AddressablesLoadManager.Instance.Find_InstantiateObj<GameObject>("PlayerCharacter");
                if (charatcter != null)
                {
                    charatcter.SetActive(false);
                }
                ImageCheck = true;
                LoadingImageShow(Scenes_Stage.Stage2);
                skyboxMG.SkyBox_Setting("Roomtest");
   
                //AddressablesLoadManager.Instance.OnUnloadedAction("BoatScene");

                break;

            case Scenes_Stage.BossEnd:
                SoundManager.Instance.bgmSource.GetComponent<AudioSource>().Stop();

                charatcter = AddressablesLoadManager.Instance.Find_InstantiateObj<GameObject>("PlayerCharacter");
                if (charatcter != null)
                {
                    AddressablesLoadManager.Instance.Delete_Object<GameObject>(charatcter);
                    //charatcter.SetActive(false);
                }
                ImageCheck = true;
                GameMG.Instance.scenes_Stage = Scenes_Stage.BossEnd;
                LoadingImageShow(Scenes_Stage.BossEnd);
                break;

            case Scenes_Stage.GameMenuEnd:
                //UIManager.Instance.Canvasoff(CANVAS_NUM.player_cavas);
                //UIManager.Instance.Canvasoff(CANVAS_NUM.enemy_canvas);
                AllanloadScene();
                break;

            case Scenes_Stage.restart_Loading:
                {
                    switch (GameMG.Instance.scenes_Stage)
                    {
                        case Scenes_Stage.Stage1:   //스테이지 1에서 죽었을때
                                                    //ImageCheck = true;
                           
                            EndunLoadBoatScene();
                            StartCoroutine(restartLoading_());

                            //LoadingImgae.SetActive(true);

                            //unloadBoatScene(Scenes_Stage.restart_Loading);
                            break;

                        case Scenes_Stage.Stage2:  //스테이지 2에서 죽었을 때
                            //ImageCheck = true;
                            //  unloadBoss_Scene();
                            SoundManager.Instance.bgmSource.GetComponent<AudioSource>().Stop();
                            EndunLoadBossScene();
                            StartCoroutine(restartLoading_());

                            break;
                    }
                }

                break;
        }
    }

   public void TestBossLoad()
    {
        StartCoroutine( Load_Boss());
    }

    //void check()
    //{
    //    Debug.Log("로딩재시작");
    //    EndunLoadBoatScene();
    //    StartCoroutine(restartLoading_());
    //}

    IEnumerator restartLoading_()
    {
    
        AddressablesLoadManager.Instance.SingleAsset_Load<GameObject>("LoadingImage");
        AddressablesLoadManager.Instance.SingleAsset_Load<GameObject>("LoadingText");
        GameObject Loading = AddressablesLoadManager.Instance.FindLoadAsset<GameObject>("LoadingImage");
        GameObject LoadingText = AddressablesLoadManager.Instance.FindLoadAsset<GameObject>("LoadingText");
        if (LoadingImgae == null)
        {
            LoadingImgae = Instantiate(Loading, new Vector3(0f, 0f, 0f), Quaternion.identity, GameMG.Instance.Canvas.transform);
        }
        if (LoadingText_Ins == null)
        {
            LoadingText_Ins = Instantiate(LoadingText, new Vector3(0f, 0f, 0f), Quaternion.identity, GameMG.Instance.Canvas.transform);
        }
        RectTransform rect = LoadingImgae.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(1920, 1080);

        RectTransform parentPos = GameMG.Instance.Canvas.GetComponent<RectTransform>();
        LoadingImgae.transform.position = parentPos.position;
        LoadingText_Ins.transform.position = new Vector3(parentPos.position.x + 27, parentPos.position.y - 232, parentPos.position.z);
        //Loading.transform.position= GameMG.Instance.Canvas.GetComponent<RectTransform>().position;

        AddressablesLoadManager.Instance.SingleAsset_Load<GameLoadingData>("LoadingRestart");
        gameLoadingData = AddressablesLoadManager.Instance.FindLoadAsset<GameLoadingData>("LoadingRestart");
        if(gameLoadingData==null)
        {
            Debug.Log("게임데이터null");
        }
        LoadingData_ListIN(out List<string> loadkey);
         AddressablesLoadManager.Instance.MultiAsset_Load<Sprite>(loadkey);
      
        LoadingImgae.GetComponent<Image>().sprite = AddressablesLoadManager.Instance.FindLoadAsset<Sprite>(loadingDatas[0].ImageName);
        LoadingText_Ins.GetComponent<TextMeshProUGUI>().text = loadingDatas[0].scripts;

        yield return new WaitForSeconds(3f);

        Destroy(LoadingImgae);
        Destroy(LoadingText_Ins);

        //  LoadingText_Ins.GetComponent<TextMeshProUGUI>().text = "";
        // LoadingImgae.SetActive(false);
        //  StartCoroutine(AddressablesLoadManager.Instance.Delete_Object<Sprite>(loadkey));
        AddressablesLoadManager.Instance.Delete_Object<Sprite>(loadingDatas[0].ImageName);

        switch (GameMG.Instance.scenes_Stage)
        {
            case Scenes_Stage.Stage1:
                BoatScene_Data_Load();
                break;

            case Scenes_Stage.Stage2:
                StartCoroutine(Load_Boss(true));
                UIManager.Instance.Hide("Boss_HP");
                break;

            case Scenes_Stage.BossEnd:
                StartCoroutine(onLoadingScene());
                break;
        }
        //로딩
        yield break;
    }

    void AllanloadScene()  //게임 종료
    {
        switch (GameMG.Instance.scenes_Stage)
        {
            case Scenes_Stage.Stage1:   //스테이지 1
                EndunLoadBoatScene();
                break;

            case Scenes_Stage.Stage2:  //스테이지 2
                EndunLoadBossScene();
                break;
        }
        SoundManager.Instance.bgmSource.GetComponent<AudioSource>().clip = (SoundManager.Instance.Bgm[0]);
        SoundManager.Instance.bgmSource.Play();
    }

    public void MonsterDead(GameObject delete_monster)
    {
      
        if (delete_monster.layer==9)
        { 
            return;
        }


        StartCoroutine(DeadMonster_delete(delete_monster));


        // StartCoroutine()
        //if(MonsterCount<=0)
        //{
        //    UnloadMonster();
        //}
    }
    

    //안개 걷히기 스테이지 1 클리어 했을 때
    IEnumerator SceneChange()
    {
        Fog.Instance.OffFog();
        yield return new WaitForSeconds(2f);
        SkyboxManager.Instance.SkyBox_Change("FS003_Day_Cubemap");
        yield return new WaitForSeconds(7f);
        GameData_Load.Instance.ChangeScene(Scenes_Stage.Stage2);
    }

    IEnumerator DeadMonster_delete(GameObject delete)
    {
        yield return new WaitForSeconds(3f);
        
        for (int i = 0; i < MonsterDeadCount; i++)
        {
            var temp = AddressablesLoadManager.Instance.Find_InstantiateObj<GameObject>(delete);
         

            if (temp == delete)
            {
          
                AddressablesLoadManager.Instance.Delete_Object<GameObject>(temp);  //몬스터 삭제

          
                MonsterCount--;

                if (MonsterCount == 0)
                {
                    StartCoroutine(SceneChange());
                }
                break;
            }
        }
    }

    void UnloadMonster()
    {
        //삭제 전 

        for (int i = 0; i < MonsterDeadCount; i++)
        {
            var temp1 = AddressablesLoadManager.Instance.Find_InstantiateObj<GameObject>("Skeleton");
            AddressablesLoadManager.Instance.Delete_Object<GameObject>(temp1);  //몬스터 삭제. 
        }
    }

    void UnloadMonster_restart()
    {
      
        for (int i = 0; i < MonsterDeadCount; i++)
        {
            //Debug.Log("스켈삭제");
            var temp1 = AddressablesLoadManager.Instance.Find_InstantiateObj<GameObject>("Skeleton");
            // Debug.Log("스켈레톤 삭제 시점" + temp1.name);
            if (temp1 != null)
            {
               
                AddressablesLoadManager.Instance.Delete_Object<GameObject>(temp1);  //몬스터 삭제. 
            }
        }

        MonsterCount = 0;
    }

    //보트 씬 내리기
    void EndunLoadBoatScene()
    {
      
        AddressablesLoadManager.Instance.OnUnloadedAction("BoatScene");  //언로드
        var temp = AddressablesLoadManager.Instance.Find_InstantiateObj<GameObject>("PlayerCharacter");
        AddressablesLoadManager.Instance.Delete_Object<GameObject>(temp);  //캐릭터 삭제. 

        UIManager.Instance.RemoveAll();
        //몬스터 추가되면 삭제
        // UIManager.Instance.CanvaschildRemove(CANVAS_NUM.player_cavas);
        //UIManager.Instance.CanvaschildRemove(CANVAS_NUM.enemy_canvas);
        UnloadMonster_restart();
    }
    //보스 내리기
    void EndunLoadBossScene()
    {
        AddressablesLoadManager.Instance.OnUnloadedAction("Roomtest");  //언로드
        var temp = AddressablesLoadManager.Instance.Find_InstantiateObj<GameObject>("PlayerCharacter");
        AddressablesLoadManager.Instance.Delete_Object<GameObject>(temp);  //캐릭터 삭제. 
        var temp1 = AddressablesLoadManager.Instance.Find_InstantiateObj<GameObject>("Boss");
        AddressablesLoadManager.Instance.Delete_Object<GameObject>(temp1);  //캐릭터 삭제. 
        UIManager.Instance.RemoveAll();

        //   UIManager.Instance.CanvaschildRemove(CANVAS_NUM.player_cavas);
        //UIManager.Instance.CanvaschildRemove(CANVAS_NUM.enemy_canvas);

    }

    void unloadBoatScene(Scenes_Stage scenes_Stage)  //보트
    {
        AddressablesLoadManager.Instance.OnUnloadedAction("BoatScene");  //언로드
        var temp = AddressablesLoadManager.Instance.Find_InstantiateObj<GameObject>("PlayerCharacter");
        AddressablesLoadManager.Instance.Delete_Object<GameObject>(temp);  //캐릭터 삭제. 
        UIManager.Instance.RemoveAll();

        //  UIManager.Instance.CanvaschildRemove(CANVAS_NUM.player_cavas);
        //UIManager.Instance.CanvaschildRemove(CANVAS_NUM.enemy_canvas);
        //몬스터 추가되면 삭제
        UnloadMonster_restart();

        LoadingImageShow(scenes_Stage);  //로딩 이미지
    }

    void unloadBoss_Scene()  //보스
    {
        AddressablesLoadManager.Instance.OnUnloadedAction("Roomtest");  //언로드
        var temp = AddressablesLoadManager.Instance.Find_InstantiateObj<GameObject>("PlayerCharacter");
        AddressablesLoadManager.Instance.Delete_Object<GameObject>(temp);  //캐릭터 삭제. 
                                                                           //  UIManager.Instance.CanvaschildRemove(CANVAS_NUM.player_cavas);
                                                                           // UIManager.Instance.CanvaschildRemove(CANVAS_NUM.enemy_canvas);
        UIManager.Instance.RemoveAll();

        var temp1 = AddressablesLoadManager.Instance.Find_InstantiateObj<GameObject>("Boss");
        AddressablesLoadManager.Instance.Delete_Object<GameObject>(temp1);  //캐릭터 삭제. 

        LoadingImageShow(Scenes_Stage.Stage2);  //로딩이미지

    }


   


    //로딩 이미지 교체
    public void LoadingImageShow(Scenes_Stage scenes_Stage)
    {
        Canvas_.GetComponent<TestOnoff>().ShowImage(true);
        // LoadingImgae.SetActive(true);



     
        AddressablesLoadManager.Instance.SingleAsset_Load<GameObject>("LoadingImage");
        AddressablesLoadManager.Instance.SingleAsset_Load<GameObject>("LoadingText");
        GameObject Loading = AddressablesLoadManager.Instance.FindLoadAsset<GameObject>("LoadingImage");
        GameObject LoadingText = AddressablesLoadManager.Instance.FindLoadAsset<GameObject>("LoadingText");


        if (LoadingImgae == null)
        {
            // LoadingImgae = new GameObject();
            LoadingImgae = Instantiate(Loading, new Vector3(0f, 0f, 0f), Quaternion.identity, GameMG.Instance.Canvas.transform);
        }
        if (LoadingText_Ins == null)
        {
            LoadingText_Ins = Instantiate(LoadingText, new Vector3(0f, 0f, 0f), Quaternion.identity, GameMG.Instance.Canvas.transform);
        }

        RectTransform parentPos = GameMG.Instance.Canvas.GetComponent<RectTransform>();
        LoadingImgae.transform.position = parentPos.position;
        LoadingText_Ins.transform.position = parentPos.position;

        Color color = LoadingImgae.GetComponent<Image>().color;
        color.a = 0;
        LoadingImgae.GetComponent<Image>().color = color;

   

        LoadingText_Ins.GetComponent<TextMeshProUGUI>().text = "";
        gameLoadingData = new GameLoadingData();
        LoadingText_Ins.transform.position = new Vector3(parentPos.position.x, parentPos.position.y - 250, parentPos.position.z);
        LoadingImgae.transform.position = new Vector3(parentPos.position.x, parentPos.position.y + 100, parentPos.position.z);
        RectTransform rect = LoadingImgae.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(900, 500);
        switch (scenes_Stage)
        {
            case Scenes_Stage.Stage1: //스테이지 1
                AddressablesLoadManager.Instance.SingleAsset_Load<GameLoadingData>("LoadingData_Start");
                gameLoadingData = AddressablesLoadManager.Instance.FindLoadAsset<GameLoadingData>("LoadingData_Start");
                if (gameLoadingData == null)
                {
                    Debug.Log("확인차null");

                }
             
                break;

            case Scenes_Stage.Stage2:  //스테이지 2

                AddressablesLoadManager.Instance.SingleAsset_Load<GameLoadingData>("LoadingData_Boss");
                gameLoadingData = AddressablesLoadManager.Instance.FindLoadAsset<GameLoadingData>("LoadingData_Boss");
                if (gameLoadingData == null)
                {
                    Debug.Log("확인차null");

                }
                
                break;

            case Scenes_Stage.BossEnd:  //보스 잡고 완료했을 때

                AddressablesLoadManager.Instance.SingleAsset_Load<GameLoadingData>("LoadingData_Ending");
                gameLoadingData = AddressablesLoadManager.Instance.FindLoadAsset<GameLoadingData>("LoadingData_Ending");
                if (gameLoadingData == null)
                {
                    Debug.Log("확인차null");

                }
            
                break;

            case Scenes_Stage.restart_Loading:
                AddressablesLoadManager.Instance.SingleAsset_Load<GameLoadingData>("LoadingRestart");
                gameLoadingData = AddressablesLoadManager.Instance.FindLoadAsset<GameLoadingData>("LoadingRestart");
                if (gameLoadingData == null)
                {
                    Debug.Log("확인차null");

                }
              
                break;
        }


        List<string> loadKeyList = new List<string>();

        LoadingData_ListIN(out List<string> loadkey);
        AddressablesLoadManager.Instance.MultiAsset_Load<Sprite>(loadkey);

        if (loadingDatas[0].ImageName != null)
        {
            Color color2 = LoadingImgae.GetComponent<Image>().color;
            color2.a = 1;
            LoadingImgae.GetComponent<Image>().color = color2;
            LoadingImgae.GetComponent<Image>().sprite = AddressablesLoadManager.Instance.FindLoadAsset<Sprite>(loadingDatas[0].ImageName);
        }
        if (loadingDatas[0].scripts != null)
        {
            if (loadingDatas[0].ImageName == null)
            {
                LoadingText_Ins.transform.position = parentPos.transform.position;
            }
            LoadingText_Ins.GetComponent<TextMeshProUGUI>().text = loadingDatas[0].scripts;
        }
        gameLoadingCount = 1;
        // loadKeyList.Add("Loading_Door1");
        //  loadKeyList.Add("LoadingDoor2");
        //loadKeyList.Add("Loading_Treasure");
        // StartCoroutine( LoadImageChange(loadKeyList, LoadingImgae, scenes_Stage));
        //StartCoroutine(LoadImageChange(loadkey, LoadingImgae, scenes_Stage));

    }

    void LoadingData_ListIN(out List<string> loadkey)
    {
        loadkey = new List<string>();
        loadingDatas.Clear();

        foreach (var a in gameLoadingData.LoadingData)
        {
            if (a.LoadImageNameList.Count != 0 && a.imgae_Name != "")
            {
                string tempstring = "";
                foreach (var sc in a.LoadImageNameList)  //대사
                {
                    tempstring += sc;
                    tempstring = tempstring + "\n";

                }
                LoadingData temp = new LoadingData();
                temp.scripts = tempstring;
                temp.ImageName = a.imgae_Name;
                loadingDatas.Add(temp);
            }
            else
            {
                if (a.LoadImageNameList.Count != 0)
                {
                    foreach (var sc in a.LoadImageNameList)  //대사
                    {
                        LoadingData temp = new LoadingData();
                        temp.scripts = sc;
                        loadingDatas.Add(temp);
                    }
                }

                if (a.imgae_Name != "")  //이미지 이름
                {
                    LoadingData temp = new LoadingData();
                    temp.ImageName = a.imgae_Name;
                    loadingDatas.Add(temp);
                }
            }

        }

        foreach (var i in loadingDatas)
        {
            if (i.ImageName != null)
            {
                loadkey.Add(i.ImageName);
             
            }
    
        }

    }

    public void ImageClick()
    {
    

        LoadingData[] temp = loadingDatas.ToArray();

        List<string> deleteImageLisg = new List<string>();

      

        if (gameLoadingCount > temp.Length || !ImageCheck)
        {
        
            return;
        }

        else if (gameLoadingCount < temp.Length)
        {
            if (temp[gameLoadingCount].ImageName != null)
            {
                Color color = LoadingImgae.GetComponent<Image>().color;
                color.a = 1;
                LoadingImgae.GetComponent<Image>().color = color;
                //   LoadingImgae.GetComponent<Image>().color = new Color(255, 255, 255, 1);
                if (temp[gameLoadingCount].scripts == null)
                {
                    LoadingText_Ins.GetComponent<TextMeshProUGUI>().text = "";
                }

          

                LoadingImgae.GetComponent<Image>().sprite = AddressablesLoadManager.Instance.FindLoadAsset<Sprite>(temp[gameLoadingCount].ImageName);

          

                deleteImageLisg.Add(temp[gameLoadingCount].ImageName);
                // Debug.Log("data출력이미지 " + temp[gameLoadingCount].ImageName);
            }
            if (temp[gameLoadingCount].scripts != null)
            {
                if (temp[gameLoadingCount].ImageName == null)
                {
                    Color color = LoadingImgae.GetComponent<Image>().color;
                    color.a = 0;
                    LoadingImgae.GetComponent<Image>().color = color;
                    LoadingText_Ins.transform.position = GameMG.Instance.Canvas.GetComponent<RectTransform>().position;
                }
                else
                {
                    RectTransform parentPos = GameMG.Instance.Canvas.GetComponent<RectTransform>();
                    LoadingText_Ins.transform.position = new Vector3(parentPos.position.x, parentPos.position.y - 250, parentPos.position.z);
                }
                // LoadingImgae.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                LoadingText_Ins.GetComponent<TextMeshProUGUI>().text = temp[gameLoadingCount].scripts;
              
            }
            //  gameLoadingCount++;

            //foreach (var i in loadingDatas)
            //{
            //    if (i.ImageName != null)
            //    {

            //       // loadkey.Add(i.ImageName);
            //    }
            //    if (i.scripts != null)
            //    {

            //    }
            //}

        }
        else
        {

            Destroy(LoadingImgae);
            Destroy(LoadingText_Ins);
            // LoadingText_Ins.GetComponent<TextMeshProUGUI>().text = "";
            // LoadingImgae.SetActive(false);
            StartCoroutine(AddressablesLoadManager.Instance.Delete_Object<Sprite>(deleteImageLisg));

            switch (GameMG.Instance.scenes_Stage)
            {
                case Scenes_Stage.Stage1:
                    BoatScene_Data_Load();
                    break;

                case Scenes_Stage.Stage2:
                    StartCoroutine(Load_Boss());
                    UIManager.Instance.Hide("Boss_HP");
                    break;

                case Scenes_Stage.BossEnd:
                    StartCoroutine(onLoadingScene());
                    GameMG.Instance.scenes_Stage = Scenes_Stage.Stage1;
                    break;
            }
            //로딩

        
            gameLoadingCount = 0;
            ImageCheck = false;
            UIManager.Instance.Canvason(CANVAS_NUM.player_cavas);
            UIManager.Instance.Canvason(CANVAS_NUM.enemy_canvas);
            return;
        }

        gameLoadingCount++;


    }


    IEnumerator onLoadingScene()
    {
        yield return new WaitForSeconds(2f);
        UIManager.Instance.RemoveAll();
        Canvas_.GetComponent<TestOnoff>().ShowImage(false);
        UIManager.Instance.Prefabsload("StartUI", CANVAS_NUM.start_canvas);
        var temp1 = AddressablesLoadManager.Instance.Find_InstantiateObj<GameObject>("Boss");
        if (temp1 != null)
        {
            AddressablesLoadManager.Instance.Delete_Object<GameObject>(temp1);  //캐릭터 삭제
        }
        var temp2 = AddressablesLoadManager.Instance.Find_InstantiateObj<GameObject>("PlayerCharacter");
        if (temp2 != null)
        {
            AddressablesLoadManager.Instance.Delete_Object<GameObject>(temp1);  //캐릭터 삭제
        }


    }

    //이미지 보여주고 씬 교체
    //IEnumerator LoadImageChange(List<string> keylist,GameObject LoadingImgae, Scenes_Stage scenes_Stage)
    //{

    //    LoadingImgae.GetComponent<Image>().color = new Color(255, 255, 255, 1);

    //    AddressablesLoadManager.Instance.MultiAsset_Load<Sprite>(keylist);

    //    foreach(string s in keylist)
    //    {
    //        LoadingImgae.GetComponent<Image>().sprite = AddressablesLoadManager.Instance.FindLoadAsset<Sprite>(s);
    //        Debug.Log("나도 확인" + s);
    //        yield return new WaitForSeconds(1f);
    //    }

    //  // GameObject delete= AddressablesLoadManager.Instance.FindLoadAsset<GameObject>("LoadingImage");
    //    //AddressablesLoadManager.Instance.Delete_Object<GameObject>(delete);
    //    Destroy(LoadingImgae);
    //    StartCoroutine(AddressablesLoadManager.Instance.Delete_Object<Sprite>(keylist));

    //    switch (scenes_Stage)
    //    {
    //        case Scenes_Stage.Stage1:
    //            BoatScene_Data_Load();
    //            break;

    //        case Scenes_Stage.Stage2:
    //            StartCoroutine(Load_Boss());
    //            break;
    //    }

    //}
    public void BoatScene_Data_Load()
    {

        AddressablesLoadManager.Instance.SingleAsset_Load<GameSaveData>("BoatData");

        var tempDataSave = AddressablesLoadManager.Instance.FindLoadAsset<GameSaveData>("BoatData");

        if (tempDataSave == null)
        {
            Debug.Log("데이터 널");
        }
       
        // AddressablesLoadManager.Instance.action1= DataLoad;

        StartCoroutine(CheckLoadScene(tempDataSave));
        AddressablesLoadManager.Instance.OnSceneAction("BoatScene");

    }

    IEnumerator CheckLoadScene(GameSaveData tempsave)
    {
        while (true)
        {
         

            if (AddressablesLoadManager.Instance.SceneLoadCheck == true)
            {
          
                DataLoad(tempsave);
                  AddressablesLoadManager.Instance.SceneLoadCheck = false;
                yield return new WaitForSeconds(1f);

                switch (GameMG.Instance.scenes_Stage)
                {
                    case Scenes_Stage.Stage1:
                        //checkList();

                        //SoundManager.Instance.bgmSource.GetComponent<AudioSource>().PlayOneShot(SoundManager.Instance.Bgm[2]);
                        //SoundManager.Instance.bgmSource.GetComponent<AudioSource>().loop = true;
                        SoundManager.Instance.bgmSource.GetComponent<AudioSource>().clip = (SoundManager.Instance.Bgm[2]);
                        SoundManager.Instance.bgmSource.Play();
                        break;
                    case Scenes_Stage.Stage2:
                        // checkList();
                        var find = AddressablesLoadManager.Instance.Find_InstantiateObj<GameObject>("PlayerCharacter");
                        find.SetActive(true);

                        var boss = AddressablesLoadManager.Instance.Find_InstantiateObj<GameObject>("Boss");
                        boss.SetActive(true);

                        break;
                }

                Canvas_.GetComponent<TestOnoff>().ShowImage(false);

                //GameMG.Instance.Loading_screen(false);

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

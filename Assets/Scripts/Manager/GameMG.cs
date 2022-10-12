using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes_Stage
{
    Loding=0,
    Stage1,
    Stage2,
    Stage3,
    Boss,
    Menu,
    Stop
};

public class GameMG : Singleton<GameMG>
{
    public Scenes_Stage scenes_Stage = Scenes_Stage.Loding;

    public GameObject Canvas;
    public GameObject PlayerCanvas;
    public GameObject EnemyCanvas;


    public float PlayTime;  //플레이 시간 저장
    private float time_start;
    private float time_current;
    private float time_Max = 5f;
    private bool isEnded;
    private Vector3 Init_PlayerPos = new Vector3(0, 0, 0);

     ObjectManager _objManager= new ObjectManager();
     ResourceManager _resourceManager= new ResourceManager();
    public List<GameObject> tempObj_Manager = new List<GameObject>();
    public GameData_Load gameData_Load=null;

    public ObjectManager ObjManager { get { return Instance._objManager; } }  //이거 아녀???
    public ResourceManager Resource { get { return Instance._resourceManager; } }

    //플레이 시간
    //일시정지할때 부정확
    private void Check_Timer()
    {
        time_current = Time.time - time_start;
        if (time_current < time_Max)
        {
            // text_Timer.text = $"{time_current:N2}";
            Debug.Log(time_current);
        }
        else if (!isEnded)
        {
            End_Timer();
        }

    }

    private void End_Timer()
    {
        Debug.Log("End");
        time_current = time_Max;
        // text_Timer.text = $"{time_current:N2}";
        isEnded = true;
    }


    private void Reset_Timer()
    {
        time_start = Time.time;
        time_current = 0;
        //  text_Timer.text = $"{time_current:N2}";
        isEnded = false;
        Debug.Log("Start");
    }

    //게임 시작
    public void startGame(string SceneName)//스테이지값)  //플레이어 스탯 고정...
    {

       // string s = SceneName;
        //어드레서블 생성 후 

        AddressablesLoadManager.Instance.OnSceneAction(SceneName);

      //  StartCoroutine(LoadMyAsyncScene(SceneName));

       // GameObject PlayerInitPos = new GameObject();
      //  PlayerInitPos.transform.position = Init_PlayerPos;

      
       // GameMG.Instance.Resource.Instantiate("PlayerCharacter", PlayerInitPos.transform);

        // 게임 저장데이터

        //게임필드
        //캐릭터 생성
        // UI매니저 호출
    }

    public void startGame(string SceneName, Vector3 PlayerPos)
    {

        Debug.Log(SceneManager.GetActiveScene().name + " 씬 이름 ");
        GameObject PlayerInitPos = new GameObject();
        PlayerInitPos.transform.position = PlayerPos;

        foreach (var obj in tempObj_Manager)
        {
            if (obj.name == "PlayerCharacter")
            {
                obj.transform.parent.transform.position = PlayerPos;
            }
        }
        //GameMG.Instance.Resource.Instantiate("PlayerCharacter", PlayerInitPos.transform);
    }

    //public IEnumerator startGame(string SceneName, Vector3 PlayerPos)
    //{
    //    yield return StartCoroutine(LoadMyAsyncScene(SceneName));


    //    foreach (var obj in tempObj_Manager)
    //    {
    //        if (obj.name == "PlayerCharacter")
    //        {
    //            obj.transform.parent.transform.position = PlayerPos;
    //        }
    //    }

    //}

    //씬 불러오기 -임시 어드레서블 적용 x
    IEnumerator LoadMyAsyncScene(string SceneName)
    {
        // AsyncOperation을 통해 Scene Load 정도를 알 수 있다.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName);

        // Scene을 불러오는 것이 완료되면, AsyncOperation은 isDone 상태가 된다.
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Debug.Log("끝");
    }




    //로딩 씬 만들기
    //로딩중 

    //게임종료 저장

    //몬스터 캐릭터 로드

    //몬스터랑 캐릭터 만드는 규칙
    //스탯종류가 어떤지 역할 
    //캐릭터 
    //스탯 클래스 
    //스킬 데이터 계산

    public void Damage_calculator()
    {
        //데미지= (가해)공격력 - (피해)방어력 
    }

    void Update()
    {
        //타이머인데 잠시 디버그 할라고 꺼둠

        //if (isEnded)
        //    return;

        //Check_Timer();

       //임시로 씬 불러오기 로딩씬에서 사용(테스트)
            if (Input.GetKeyDown(KeyCode.F2))
            {
            //startGame("Lo");  //어드레서블 x

            // ScenesLoadMG.Instance.loadSubSceneFn("Load_test");
            AddressablesLoader.OnSceneAction("TestScenes");  //씬 로드 어드레서블
           // AddressablesLoader.OnSceneAction("Load_test");  //씬 로드 어드레서블
                                                            
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            AddressablesLoader.OnUnloadedAction("TestScenes");  //씬 언로드  어드레서블 적용

        }


    }

    void GameSceneChange()
    {
        AddressablesLoadManager.Instance.OnUnloadedAction("Roomtest");
        AddressablesLoadManager.Instance.Delete_Object<GameObject>("PlayerCharacter");
        AddressablesLoadManager.Instance.Delete_Object<GameObject>("Boss");

    }

    //바깥에서 사용
    public void Loading_screen(bool show)
    {
        Canvas.SetActive(show);

        PlayerCanvas.SetActive(!show);
        EnemyCanvas.SetActive(!show);

        //여기 다른 UI도 넣어.. 조절 가능하게..
    }

    IEnumerator temp()
    {
        GameData_Load.Instance.TestPos_and_Load();

        yield return new WaitForSeconds(7f);
        GameSceneChange();
        yield return new WaitForSeconds(10f);
        GameData_Load.Instance.TestPos_and_Load();


    }

   

    void Start()
    {
        //처음 시작할때 캔버스 끄게..
        //PlayerCanvas.SetActive(false);
        //EnemyCanvas.SetActive(false);


        // StartCoroutine(temp());  

       // GameData_Load.Instance.ChangeScene(Scenes_Stage.Stage1);

       // AddressablesLoadManager.Instance.OnSceneAction("BoatScene");


       
    }

}

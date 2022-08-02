using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class ScenesLoadMG : Singleton<ScenesLoadMG>
{

    public enum Scenes_State  //씬 상태에 따라 어드레서블을 언로드, 로드 하기 위해 명시함.
    {
         Load=0,  //로딩중일때
        UnLoad,  //언로드 중일때
        Wait //로드,언로드 안하고 있을 때
    }

    public enum Load_Result
    {
        Complete_Load=0,
        Fail_Load,
        Complete_Unload,
        Fail_Unload
    }

    public enum Load_UnLoad_Scenes
    {

        Loading_Scenes=-1,  //로딩중인 씬 (흰색화면에 로딩 중...?)
        Loby_Scenes,  //게임 대기 화면 (아마도 로비 화면..? 게임 일시정지나 이럴때 뜨는..?걸수도..?)
        GameStart,  //게임 시작 (초반 게임 시작하기 전 씬) (아마도 게임 시작 하기 전에 뜨는 씬...?)
        Stage1,  //스테이지 1
        Stage2,  //스테이지 2
        Stage3,  //스테이지 3
        BossStage,  //보스 스테이지
        Menu_Scenes  //혹시 메뉴화면 있을수도 있으니 메뉴 화면   (기타 볼륨이나 이런거 선택하는 메뉴..? 근데 이걸 씬으로 할지는,,,?)
    }




}

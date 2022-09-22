using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;


public class TestAddressablesLoader : Singleton<TestAddressablesLoader>
{
  
    public static List<string> Load_String_List = new List<string>();
    public static int ListCount = 0;

    public static List<UnityEngine.Object> CreateObjectList = new List<UnityEngine.Object>();  //Instantiate로 생성된 오브젝트들 관리 =>어드레서블 에셋이 아니라 에셋으로 복사 된 오브젝트를 말함.
    public static List<UnityEngine.Object> InstList = new List<UnityEngine.Object>();  //바로 생성된 오브젝트 리스트에 로드 자산 관리 시키기 , 핸들 x
    public static List<UnityEngine.Object> AssetList = new List<UnityEngine.Object>();  //로드된 자산 관리 시키기 ,핸들 x

    public static List<AsyncOperationHandle<UnityEngine.Object>> handleList = new List<AsyncOperationHandle<UnityEngine.Object>>();  //핸들 저장해서 언로드 관리 시키기.
  //  public static List<AsyncOperationHandle<IList<UnityEngine.Object>>> handleIList = new List<AsyncOperationHandle<IList<UnityEngine.Object>>>();  //핸들 저장


    //test 끝나면 헤더파일로 이동시키기
    string Inst_String = "(Clone)";  //InstList 찾을때 필요
    string Load_String= " (UnityEngine.GameObject)";  //List찾을때

    //Addressables.Release();
    //원본지켜...
    ////label가져와서 바로 생성 시키기, 멀티 ,동기
    //public async Task InitAssets_label<T>(string label)
    // where T : UnityEngine.Object

    //{
    //    ErrorCode error=ErrorCode.None;

    //    if (!LoadCheck(label,out error))
    //    {
    //        Debug.Log("에러"+error);
    //        return;
    //    }

    //    Load_String_List.Add(label);  //로드되는 label

    //    Debug.Log("생성전" + label);


    //    var locations = await Addressables.LoadResourceLocationsAsync(label).Task;
    //    Debug.Log("생성가ㅣ져옴" + label);


    //    foreach (var location in locations)
    //    {
    //        var temp = await Addressables.InstantiateAsync(location).Task;
    //        InstList.Add(temp as T);  //List에 저장 (생성된 오브젝트)
    //        Load_String_List.Add(temp.ToString());  //로드되는 오브젝트 이름
    //        Debug.Log("생성" + temp);
    //    }

    //    return;

    //}

    //label가져와서 바로 생성 시키기, 멀티 ,동기
    public async Task InitAssets_label<T>(string label)
     where T : UnityEngine.Object

    {
        ErrorCode error = ErrorCode.None;

        if (!LoadCheck(label, out error))
        {
            Debug.Log("에러" + error);
            return;
        }

        Load_String_List.Add(label);  //로드되는 label

        Debug.Log("생성전" + label);



             var locations = await Addressables.LoadResourceLocationsAsync(label).Task;
            Debug.Log("생성가ㅣ져옴" + label);


            foreach (var location in locations)
            {
                var temp = await Addressables.InstantiateAsync(location).Task;
                InstList.Add(temp as T);  //List에 저장 (생성된 오브젝트)
                Load_String_List.Add(temp.ToString());  //로드되는 오브젝트 이름
                Debug.Log("생성" + temp);
            }
     
       

        return;

    }

    //객체 바로 생성 /동기  확인 완
    public async Task<T> InitAssets_Instantiate<T>(string name)
     where T : UnityEngine.Object
    {

        ErrorCode error = ErrorCode.None;

        if (!LoadCheck(name, out error))
        {
            Debug.Log("에러" + error);
            return null;
        }

        Load_String_List.Add(name); //로드되는 에셋 이름

        var temp = await Addressables.InstantiateAsync(name).Task;

        InstList.Add(temp as T);

        foreach (var t in InstList)
        {
            Debug.Log("List 리스트 들어감" + t);
        }

        return temp as T;
    }

    //비동기 로드  확인 완  //단일 로드 (객체 생성아님
    public async Task InitAssets_name_<T>(string object_name)
     where T : UnityEngine.Object

    {
        ErrorCode error = ErrorCode.None;

        if (!LoadCheck(object_name, out error))
        {
            Debug.Log("에러" + error);
            return;
        }
        Load_String_List.Add(object_name); //로드되는 에셋 이름

        Debug.Log("시작" + object_name);

        //Task te = Addressables.LoadAssetAsync<T>(object_name).Task;

        var temp = await Addressables.LoadAssetAsync<T>(object_name).Task;
        Debug.Log("가져옴" + temp);

      
        AssetList.Add(temp as T);
         
        Debug.Log("저장기다림");

        foreach (var t in AssetList)
        {
            Debug.Log("요소 출력: " + t);
        }

        return;

    }


    //동기 로드 멀티
    public void InitAssets_name_sync<T>(List<string> keyList)
   where T : UnityEngine.Object

    {
        ErrorCode error = ErrorCode.None;

        foreach(var t in keyList)
        {
            if (!LoadCheck(t, out error))
            {
                Debug.Log("에러" + error);
                return;
            }
            else
            {
                Load_String_List.Add(t); //로드되는 에셋 이름
                Debug.Log("시작" + t);
            }

            var temp = Addressables.LoadAssetAsync<T>(t);
            T go = temp.WaitForCompletion();

            Debug.Log("가져옴" + go);
            AssetList.Add(go);

        }
  
        foreach (var t in AssetList)
        {
            Debug.Log("요소 출력: " + t);
        }
        return;
    }


    //동기 로드 단일
    public void InitAssets_name_sync<T>(string object_name)
   where T : UnityEngine.Object

    {
        ErrorCode error = ErrorCode.None;

        if (!LoadCheck(object_name, out error))
        {
            Debug.Log("에러" + error);
            return;
        }
        Load_String_List.Add(object_name); //로드되는 에셋 이름

        Debug.Log("시작" + object_name);

        //Task te = Addressables.LoadAssetAsync<T>(object_name).Task;

        var temp = Addressables.LoadAssetAsync<T>(object_name);
        T go = temp.WaitForCompletion();

        Debug.Log("가져옴" + temp);
        AssetList.Add(go);

        foreach (var t in AssetList)
        {
            Debug.Log("요소 출력: " + t);
        }
        return;
    }
    //원본
    //동기 로드  확인 완  //단일 로드 (객체 생성아님
    //public async Task InitAssets_name_<T>(string object_name)
    // where T : UnityEngine.Object

    //{
    //    ErrorCode error = ErrorCode.None;

    //    if (!LoadCheck(object_name, out error))
    //    {
    //        Debug.Log("에러" + error);
    //        return;
    //    }
    //    Load_String_List.Add(object_name); //로드되는 에셋 이름

    //    Debug.Log("시작" + object_name);

    //    var temp = await Addressables.LoadAssetAsync<T>(object_name).Task;
    //    Debug.Log("가져옴" + object_name);

    //    AssetList.Add(temp);
    //    Debug.Log("저장" + temp);


    //    foreach (var t in AssetList)
    //    {
    //        Debug.Log("요소 출력: " + t);
    //    }

    //    return;

    //}

    //멀티 로드,리스트
    public async Task MultiLoadAsset<T>(List<string> keyList)
     where T : UnityEngine.Object

    {
        ErrorCode error = ErrorCode.None;

        foreach (var s in keyList)
        {
            if (!LoadCheck(s, out error))
            {
                Debug.Log("에러" + error);
                return;
            }
            else
            {
                Load_String_List.Add(s); //로드되는 에셋 이름

            }

            var temp = await Addressables.LoadAssetAsync<T>(s).Task;
            Debug.Log("가져옴" + temp.name);

            AssetList.Add(temp);
            Debug.Log("저장" + temp);

        }

        foreach (var t in AssetList)
        {
            Debug.Log("요소 출력: " + t);
        }

        return;

    }


    
    //하나 로드 할때, 비동기면 false, 동기면 true  생성까지 되는거 아님.
    public async void Single_Load_Task_Test<T>(string label, bool sync, Action<T> Complete = null)
     where T : UnityEngine.Object
    {
        if (!sync)
        {

            Debug.Log("시작하러 옴");
            InitAssets_name_sync<T>(label);
             //InitAssets_name_<T>(label).Wait();
             // ta.Wait();
            Debug.Log("대기 끝");

            // Task task = Task.Run(InitAssets_name_<T>(label));


            T findobj = FindLoadAsset<T>(label);
            if (Complete != null)
            {
                Complete(findobj); //동기 작업 하고싶은 내용 (밖에서 호출하고 await안쓰면 그냥 넘어감.

            }
            // Task.WaitAll();
        }
        else
        {
             await InitAssets_name_<T>(label);
            T findobj = FindLoadAsset<T>(label);
            if (Complete != null)
            {
                Complete(findobj); //동기 작업 하고싶은 내용 (밖에서 호출하고 await안쓰면 그냥 넘어감.

            }
        }

        return;
    }


    //하나 로드 할때, 비동기면 false, 동기면 true  생성까지 되는거 아님.
    public async void Single_Load_Task_Test<T>(List<string> keyList, bool sync, Action<T> Complete = null)
     where T : UnityEngine.Object
    {
        if (!sync)
        {

            Debug.Log("시작하러 옴");
            InitAssets_name_sync<T>(keyList);
          
            Debug.Log("대기 끝");

        }
        else
        {
            //await InitAssets_name_<T>(keyList);
           // T findobj = FindLoadAsset<T>(label);
            if (Complete != null)
            {
            //    Complete(findobj); //동기 작업 하고싶은 내용 (밖에서 호출하고 await안쓰면 그냥 넘어감.

            }
        }

        return;
    }

    //await안써도 되는거
    //하나 로드 할때, 비동기면 false, 동기면 true
    public async void Single_Load<T>(string label, bool sync,Action<T> Complete=null)
     where T : UnityEngine.Object

    {
        if(!sync)
        {
            await InitAssets_name_<T>(label);
            T findobj = FindLoadAsset<T>(label);
          
            if(Complete!=null)
            {
                Complete(findobj); //동기 작업 하고싶은 내용 (밖에서 호출하고 await안쓰면 그냥 넘어감.

            }
        }
        else
        {
            InitAssets_name_<T>(label);
            T findobj = FindLoadAsset<T>(label);
            if (Complete != null)
            {
                Complete(findobj); //동기 작업 하고싶은 내용 (밖에서 호출하고 await안쓰면 그냥 넘어감.

            }
        }
    }

    //하나 로드 할때  (Gameobject생성)
    public async void Single_Instantiate<T>(string key, bool sync, Action<T> action = null)
     where T : UnityEngine.Object

    {
        if(!sync)
        {
            await InitAssets_Instantiate<T>(key);
            T findobj = FindLoadAsset<T>(key);
            action(findobj);  //동기 작업 하고싶은 내용
        }
        else
        {
            InitAssets_Instantiate<T>(key);
            T findobj = FindLoadAsset<T>(key);
            action(findobj);   //비동기로 이루어짐 (비동기면 굳이 여기로 안넘기고 밖에서 호출해도 됌)
        }
    }

    //여러개 로드 할때
    public async void Multi_List_Load<T>(List<string> keyList,bool sync, Action action =null)
     where T : UnityEngine.Object
    {

        if (!sync)
        {
            await MultiLoadAsset<T>(keyList);
            action();  //동기 작업 하고싶은 내용
        }
        else
        {
            MultiLoadAsset<T>(keyList);
            action();  //동기 작업 하고싶은 내용
        }
    }

    //여러개 로드 label값 (Gameobject생성)
    public async void Multi_Lable_Instantiate<T>(string label, bool sync, Action action = null)
     where T : UnityEngine.Object
    {
        if(!sync)
        {
            await InitAssets_label<T>(label);
            action();  //동기 작업 하고싶은 내용
        }
        else
        {
            InitAssets_label<T>(label);
            action();  //동기 작업 하고싶은 내용
        }
    }


    //비동기 로드 한개,  함수에 로드 결과 가져가기
    public IEnumerator AsyncLoad_single<T>(string key, Action<T> Complete = null)
        where T : UnityEngine.Object
    {

        ErrorCode error = ErrorCode.None;

        if (!LoadCheck(key, out error))
        {
            Debug.Log("에러" + error);
            yield break;
        }

        Debug.Log("load");
        Load_String_List.Add(key);  //로드되는 label
        // AsyncOperationHandle<T> goHandle = Addressables.LoadAssetAsync<T>(key);
        AsyncOperationHandle<UnityEngine.Object> goHandle = Addressables.LoadAssetAsync<UnityEngine.Object>(key);
        //goHandle.Completed += Complete;

        yield return goHandle;
        if (goHandle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log(goHandle.Result.name);
            if(Complete!=null)
            {
                Complete(goHandle.Result as T);
            }
            Debug.Log(goHandle.Result + "가져옴");
            //T gameObject = temp.Result;
            handleList.Add(goHandle);
        }
    }

    //찾기
    public T FindLoadAsset<T>(string key)
     where T : UnityEngine.Object

    {

        Debug.Log("FindLoadAsset찾으러 들어옴");

        T findAsset = default;

        foreach (var t in InstList)
        {
            Debug.Log("InstList : " + t);

            if (key+Inst_String+ Load_String == t.ToString())
            {
                Debug.Log("InstList 발견" + t);
                findAsset = t as T;
                return findAsset;
            }
        }

        foreach (var t in AssetList)
        {
            Debug.Log("List : " + t.name);

            if (key == t.name)
            {
                Debug.Log("List 발견" + t);
                findAsset = t as T;
                return findAsset;
            }
        }

        foreach (var t in handleList)
        {
            Debug.Log("handleList : " + t);

            if (t.Result.name==key)
            {
                Debug.Log("handleList 발견"+t.Result.name);

                //핸들말고 결과만
                findAsset = t.Result as T;
                return findAsset;

            }
        }

        findAsset = Find_InstantiateObj<T>(key);
        if(findAsset!=null)
        {
            return findAsset;
        }

        //foreach (var t in handleIList)
        //{
        //  foreach(var e in t.Result)
        //    {
        //        Debug.Log("handleIList : " + e);

        //        if (e.name==key)
        //        {
        //            Debug.Log("handleIList 발견"+e.name);

        //            findAsset = e as T;
        //            return findAsset;

        //        }
        //    }
        //}
        return findAsset;
    }

    //삭제
    public bool Delete_Object<T>(T delete)
     where T : UnityEngine.Object

    {
        //OnRelease();

        Debug.Log("Delete_Object : " );

        //바로 생성된 객체들  확인 완
        if (InstList.Contains(delete))
        {
            Debug.Log("삭제 InstList : " + delete);

            Addressables.ReleaseInstance(delete as GameObject);
            //Addressables.Release(delete);
            InstList.Remove(delete);
            return true;
        }
        //로드 확인 완
        if (AssetList.Contains(delete))
        {
            Debug.Log("삭제 List : " + delete);


            //Addressables.ReleaseInstance(delete as GameObject);
            Addressables.Release(delete);
            AssetList.Remove(delete);
            return true;
        }

        if (CreateObjectList.Contains(delete))
        {
            Debug.Log("삭제 CreateObjectList : " + delete);
            Destroy(delete);
            return true;
        }

        AsyncOperationHandle<UnityEngine.Object> temp = new AsyncOperationHandle<UnityEngine.Object>();
        bool result = false;

        foreach (var t in handleList)
        {
            Debug.Log("handleList : " + t);

            if (t.Result== delete)
            {
                Debug.Log("handleList 삭제" + t.Result.name);

                Addressables.Release(delete);
               
                temp = t ;
                result= true;

            }
        }

        if(result)
        {
            handleList.Remove(temp);
            return result;
        }
        //이거 요소 다 빠지면 해제 해줘야됌  OnRelease
        //foreach (var t in handleIList)
        //{
        //    foreach (var e in t.Result)
        //    {
        //        Debug.Log("handleIList : " + e);

        //        if (e == delete)
        //        {
        //            Debug.Log("handleIList 발견" + e.name);
        //            Addressables.ReleaseInstance(delete as GameObject);

        //            t.Result.Remove(e);
        //           // OnRelease();  이거 안댕
        //            return true;

        //        }
        //    }
        //}
        Debug.Log("찾진 못했고 그냥 Destroy");
        Destroy(delete);
        return false;

    }

    public T Find_InstantiateObj<T>(string key)
        where T : UnityEngine.Object
    {
        T findobj = default;
        foreach(var t in CreateObjectList)
        {
            if(key+Inst_String==t.name)
            {
                Debug.Log(name + "CreateObjectList안에서 발견");
                findobj = t as T;
                return findobj;
            }
        }

        return findobj;
    }

    //IList 다 빠지면 핸들 해제 해줘야 함 
    public void OnRelease()
    {
        Debug.Log("OnRelease실행");

        //foreach (var t in handleIList)
        //{
        //   if(t.Result.Count==0)
        //    {

        //        Addressables.Release(t );
        //        Debug.Log("핸들 삭제됌");

        //    }
        //    handleIList.Remove(t);

        //    return;
        //}
    }

    //잠시 핸들 확인하려고 하는거 (요소 다 빠지는지 확인)
    public void tempListCheck()
    {

        Debug.Log("AssetList조회");
        foreach (var t in AssetList)
        {
                Debug.Log("AssetList : " + t);
        }

        Debug.Log("InstList");
        foreach (var t in InstList)
        {
            Debug.Log("InsList : " + t);
        }

        Debug.Log("CreateObjectList");
        foreach (var t in CreateObjectList)
        {
            Debug.Log("CreateObjectList : " + t);
        }

        Debug.Log("handleList");
        foreach (var t in handleList)
        {
            Debug.Log("handleList : " + t);
        }
        
    }

    //로드 되었는지 체크
    public bool LoadCheck(string key,out ErrorCode error)
    {
        bool result = true;

       if(Load_String_List.Contains(key))
        {
            result = false;
            error = ErrorCode.Assets_Already_Loaded;
        }
       else
        {
            error = ErrorCode.None;
        }

        return result;
    }

    //ture같이 들어오면 삭제 수행  =>삭제 할때마다 해줘야 함 (고민좀)
    public bool LoadCheck(string key,bool remove)
    {
        if(remove)
        {
            if (Load_String_List.Contains(key))
            {
                Load_String_List.Remove(key);
                return true;
            }
            else
            {
                return false;
            }
        }

        //기본적으로 string일치하는거 있으면 false (이미 로드 or 로드요청 된거 있으면)
        bool result = true;

        if (Load_String_List.Contains(key))
        {
            result = false;
        }

        return result;
    }


    public IEnumerator Load_Name<T>(string name, Transform parent)
        where T : UnityEngine.Object
    {

        Debug.Log("로드" + name);
        yield return StartCoroutine(AsyncLoad_single<T>(name));

        var findobj = FindLoadAsset<T>(name);
        CreateObjectList.Add(Instantiate(findobj, parent.position, Quaternion.identity));

    }

    //델리게이트
    //public IEnumerator LoadGameObjectAndMaterial<T>(string name, Action<AsyncOperationHandle<T>> Complete)
    // where T : UnityEngine.Object

    //{
    //    Debug.Log("LoadGameObjectAndMaterial호출");



    //    if (Load_String_List.Contains(name))
    //    {
    //        Debug.Log("이미 로드된 파일입니다. 동일한 이름의 소스 이미 로드 요청되어있음.");
    //        yield return null;
    //    }
    //    else
    //    {
    //        Load_String_List.Add(name);
    //    }
    //    //같은거 로드 못하게 예외 처리 
    //    //못찾으면 로드,찾으면 리턴,
    //    GameObject findGameobj = AddressablesController.Instance.find_Asset_in_list(name);
    //    if (findGameobj != null)
    //    {
    //        Debug.Log("이미 로드된 파일입니다.");
    //        yield return null;
    //    }
    //    else
    //    {
    //        //Load a GameObject
    //        AsyncOperationHandle<T> goHandle = Addressables.LoadAssetAsync<T>(name);
    //        goHandle.Completed += Complete;
    //        yield return goHandle;
    //        if (goHandle.Status == AsyncOperationStatus.Succeeded)
    //        {
    //            T gameObject = goHandle.Result;
             
    //            Debug.Log(gameObject.name + "로드");
    //        }

    //    }
    //    ////Load a Material
    //    //AsyncOperationHandle<IList<IResourceLocation>> locationHandle = Addressables.LoadResourceLocationsAsync("materialKey");
    //    //yield return locationHandle;
    //    //AsyncOperationHandle<Material> matHandle = Addressables.LoadAssetAsync<Material>(locationHandle.Result[0]);
    //    //yield return matHandle;
    //    //if (matHandle.Status == AsyncOperationStatus.Succeeded)
    //    //{
    //    //	Material mat = matHandle.Result;
    //    //	//etc...
    //    //}

    //    //Use this only when the objects are no longer needed
    //    //Addressables.Release(goHandle);
    //    //Addressables.Release(matHandle);
    //}




    static SceneInstance m_LoadedScene;

    public static void OnSceneAction(string SceneName)
    {
        if (m_LoadedScene.Scene.name == null)
        {
            Addressables.LoadSceneAsync(SceneName, LoadSceneMode.Additive).Completed += OnSceneLoaded;
        }
        else
        {
            //Addressables.UnloadSceneAsync(m_LoadedScene).Completed += OnSceneUnloaded;
            Debug.Log("로드 실패");
        }
    }

    public static void OnUnloadedAction(string SceneName)
    {
        if (m_LoadedScene.Scene.name != null)
        {
            Addressables.UnloadSceneAsync(m_LoadedScene).Completed += OnSceneUnloaded;
        }
        else
        {
            Debug.Log("언로드 실패");
        }
    }

    public static void OnSceneUnloaded(AsyncOperationHandle<SceneInstance> obj)
    {
        switch (obj.Status)
        {
            case AsyncOperationStatus.Succeeded:
                m_LoadedScene = new SceneInstance();
                break;
            case AsyncOperationStatus.Failed:
                Debug.LogError("씬 언로드 실패: " /*+ addSceneReference.AssetGUID*/);
                break;
            default:
                break;
        }
    }

    public static void OnSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
    {
        switch (obj.Status)
        {
            case AsyncOperationStatus.Succeeded:
                m_LoadedScene = obj.Result;
                break;
            case AsyncOperationStatus.Failed:
                Debug.LogError("씬 로드 실패: " /*+ addSceneReference.AssetGUID*/);
                break;
            default:
                break;
        }
    }



}

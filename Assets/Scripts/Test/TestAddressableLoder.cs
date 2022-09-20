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

    public static List<UnityEngine.Object> InstList = new List<UnityEngine.Object>();  //바로 생성된 오브젝트 리스트에 로드 자산 관리 시키기 , 핸들 x
    public static List<UnityEngine.Object> AssetList = new List<UnityEngine.Object>();  //로드된 자산 관리 시키기 ,핸들 x

    public static List<AsyncOperationHandle<UnityEngine.Object>> handleList = new List<AsyncOperationHandle<UnityEngine.Object>>();  //핸들 저장해서 언로드 관리 시키기.
    public static List<AsyncOperationHandle<IList<UnityEngine.Object>>> handleIList = new List<AsyncOperationHandle<IList<UnityEngine.Object>>>();  //핸들 저장

    public static Dictionary<string, AsyncOperationHandle<UnityEngine.Object>> handleList_Dic = new Dictionary<string, AsyncOperationHandle<UnityEngine.Object>>();  //key값을 통해 오브젝트, 핸들을 관리  핸들1
    public static Dictionary<string, AsyncOperationHandle<IList<UnityEngine.Object>>> HandleIList_Dic = new Dictionary<string, AsyncOperationHandle<IList<UnityEngine.Object>>>();   //key값을 통해 오브젝트, 핸들을 관리 , IList


    //test 끝나면 헤더파일로 이동시키기
    string Inst_String = "(Clone)";  //InstList 찾을때 필요
    string Load_String= " (UnityEngine.GameObject)";  //List찾을때

    //Addressables.Release();
    //label가져와서 바로 생성 시키기, 멀티 ,동기
    public async Task InitAssets_label<T>(string label)
     where T : UnityEngine.Object

    {
        ErrorCode error=ErrorCode.None;

        if (!LoadCheck(label,out error))
        {
            Debug.Log("에러"+error);
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
            Debug.Log("생성" + label);
        }
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

    //동기 로드  확인 완  //단일 로드 (객체 생성아님
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

        var temp = await Addressables.LoadAssetAsync<T>(object_name).Task;
        Debug.Log("가져옴" + object_name);

        AssetList.Add(temp);
        Debug.Log("저장" + temp);


        foreach (var t in AssetList)
        {
            Debug.Log("요소 출력: " + t);
        }

    }

    //그냥 저거 단일을 계속 중첩하면 그게 멀티로드지 안그래?

    //찾기
    public T FindLoadAsset<T>(string key)
     where T : UnityEngine.Object

    {


        Debug.Log("FindLoadAsset찾으러 들어옴");

        T findAsset = default;

        foreach (var t in InstList)
        {
            Debug.Log("InstList : " + t.name);

            if (key+Inst_String == t.name)
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

                findAsset = t as T;
                return findAsset;

            }
        }

        foreach (var t in handleIList)
        {
          foreach(var e in t.Result)
            {
                Debug.Log("handleIList : " + e);

                if (e.name==key)
                {
                    Debug.Log("handleIList 발견"+e.name);

                    findAsset = e as T;
                    return findAsset;

                }
            }
        }
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


            Addressables.ReleaseInstance(delete as GameObject);
            AssetList.Remove(delete);
            return true;
        }

        AsyncOperationHandle<UnityEngine.Object> temp = new AsyncOperationHandle<UnityEngine.Object>();
        bool result = false;

        foreach (var t in handleList)
        {
            Debug.Log("handleList : " + t);

            if (t.Result== delete)
            {
                Debug.Log("handleList 발견" + t.Result.name);

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
        foreach (var t in handleIList)
        {
            foreach (var e in t.Result)
            {
                Debug.Log("handleIList : " + e);

                if (e == delete)
                {
                    Debug.Log("handleIList 발견" + e.name);
                    Addressables.ReleaseInstance(delete as GameObject);

                    t.Result.Remove(e);
                   // OnRelease();  이거 안댕
                    return true;

                }
            }
        }
        Debug.Log("찾진 못했고 그냥 Destroy");
        Destroy(delete);
        return false;

    }

    //IList 다 빠지면 핸들 해제 해줘야 함 
    public void OnRelease()
    {
        Debug.Log("OnRelease실행");

        foreach (var t in handleIList)
        {
           if(t.Result.Count==0)
            {

                Addressables.Release(t );
                Debug.Log("핸들 삭제됌");

            }
            handleIList.Remove(t);

            return;
        }
    }

    //잠시 핸들 확인하려고 하는거 (요소 다 빠지는지 확인)
    public void tem()
    {
        foreach (var t in handleIList)
        {
            foreach (var e in t.Result)
            {
                Debug.Log("handleIList : " + e);
            }
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

    //객체 불러오기 (1개
    public IEnumerator LoadGameObjectAndMaterial<T>(string name)
     where T : UnityEngine.Object


    {
        Debug.Log("LoadGameObjectAndMaterial호출");


        if (Load_String_List.Contains(name))
        {
            Debug.Log("이미 로드된 파일입니다. 동일한 이름의 소스 이미 로드 요청되어있음.");
            yield return null;
        }
        else
        {
            Load_String_List.Add(name);
        }
        //같은거 로드 못하게 예외 처리 
        //못찾으면 로드,찾으면 리턴,
        GameObject findGameobj = AddressablesController.Instance.find_Asset_in_list(name);
        if (findGameobj != null)
        {
            Debug.Log("이미 로드된 파일입니다.");
            yield return null;
        }
        else
        {
            //Load a GameObject
            AsyncOperationHandle<T> goHandle = Addressables.LoadAssetAsync<T>(name);
            yield return goHandle;
            if (goHandle.Status == AsyncOperationStatus.Succeeded)
            {
                T gameObject = goHandle.Result;
                //   tempobj.Add(gameObject);
                InstList.Add(gameObject);
                // ListCount = tempobj.Count;
                Debug.Log(gameObject.name + "로드");

                //foreach (var obj in tempobj)
                //{
                //    //	c++;
                //    Debug.Log(obj.name + "tempobj 리스트확인");
                //}
                foreach (var obj in InstList)
                {
                    //	c++;
                    Debug.Log(obj + "List 리스트확인");
                }
                //etc...
            }

        }
        ////Load a Material
        //AsyncOperationHandle<IList<IResourceLocation>> locationHandle = Addressables.LoadResourceLocationsAsync("materialKey");
        //yield return locationHandle;
        //AsyncOperationHandle<Material> matHandle = Addressables.LoadAssetAsync<Material>(locationHandle.Result[0]);
        //yield return matHandle;
        //if (matHandle.Status == AsyncOperationStatus.Succeeded)
        //{
        //	Material mat = matHandle.Result;
        //	//etc...
        //}

        //Use this only when the objects are no longer needed
        //Addressables.Release(goHandle);
        //Addressables.Release(matHandle);
    }

    //델리게이트
    public IEnumerator LoadGameObjectAndMaterial<T>(string name, Action<AsyncOperationHandle<T>> Complete)
     where T : UnityEngine.Object

    {
        Debug.Log("LoadGameObjectAndMaterial호출");


        if (Load_String_List.Contains(name))
        {
            Debug.Log("이미 로드된 파일입니다. 동일한 이름의 소스 이미 로드 요청되어있음.");
            yield return null;
        }
        else
        {
            Load_String_List.Add(name);
        }
        //같은거 로드 못하게 예외 처리 
        //못찾으면 로드,찾으면 리턴,
        GameObject findGameobj = AddressablesController.Instance.find_Asset_in_list(name);
        if (findGameobj != null)
        {
            Debug.Log("이미 로드된 파일입니다.");
            yield return null;
        }
        else
        {
            //Load a GameObject
            AsyncOperationHandle<T> goHandle = Addressables.LoadAssetAsync<T>(name);
            goHandle.Completed += Complete;
            yield return goHandle;
            if (goHandle.Status == AsyncOperationStatus.Succeeded)
            {
                T gameObject = goHandle.Result;
             
                Debug.Log(gameObject.name + "로드");
            }

        }
        ////Load a Material
        //AsyncOperationHandle<IList<IResourceLocation>> locationHandle = Addressables.LoadResourceLocationsAsync("materialKey");
        //yield return locationHandle;
        //AsyncOperationHandle<Material> matHandle = Addressables.LoadAssetAsync<Material>(locationHandle.Result[0]);
        //yield return matHandle;
        //if (matHandle.Status == AsyncOperationStatus.Succeeded)
        //{
        //	Material mat = matHandle.Result;
        //	//etc...
        //}

        //Use this only when the objects are no longer needed
        //Addressables.Release(goHandle);
        //Addressables.Release(matHandle);
    }

    //키 연관 리스트 저장, label로 다수로딩 비동기 /객체 생성 아니고 로딩만. 확인
    public IEnumerator LoadAndStoreResult<T>(string Key)
     where T : UnityEngine.Object
    {
        ErrorCode error;

        if (!LoadCheck(Key, out error))
        {
            Debug.Log("에러" + error);
            yield return null;
        }

        Load_String_List.Add(Key); //로드할 에셋 이름 저장

        List<UnityEngine.Object> associationDoesNotMatter = new List<UnityEngine.Object>();

        AsyncOperationHandle<IList<UnityEngine.Object>> handle =
            Addressables.LoadAssetsAsync<UnityEngine.Object>(Key, obj =>
            {
                associationDoesNotMatter.Add(obj);
                //잠시  object저장 되는지 확인

                //List.Add(obj);
                Debug.Log(obj);
            });
        yield return handle;
        //handleList.Add(handle);
        handleIList.Add(handle);

        HandleIList_Dic.Add(Key, handle);

        foreach (var t in InstList)
        {
            Debug.Log(t.ToString());
        }
    }


    //델리게이트 실행 (Key에 label) ,다수로딩/ 비동기 +  함수 연결  확인 / 해제 생각
    public IEnumerator LoadAndStoreResult<T>(string Key, Action<AsyncOperationHandle<IList<UnityEngine.Object>>> Complete=null)
        where T : UnityEngine.Object
    {

        ErrorCode error;

        if (!LoadCheck(Key, out error))
        {
            Debug.Log("에러" + error);
            yield return null;
        }

        Load_String_List.Add(Key); //로드할 에셋 이름 저장

        List<UnityEngine.Object> associationDoesNotMatter = new List<UnityEngine.Object>();

        AsyncOperationHandle<IList<UnityEngine.Object>> handle =
            Addressables.LoadAssetsAsync<UnityEngine.Object>(Key, obj => associationDoesNotMatter.Add(obj));
        handle.Completed += Complete;
        yield return handle;
        // handleIObjectList.Add();

         handleIList.Add(handle);
        //List.Add(handle);

        HandleIList_Dic.Add(Key, handle);

    }


 
    //리스트로 로드 비동기
    //MergeMode.Union외에 다른거는 오류
    public IEnumerator Load_Key_List<T>(List<string> KeyList)
        where T : UnityEngine.Object

    {

        ErrorCode error;

        
        //AsyncOperationHandle<IList<GameObject>> loadWithMultipleKeys =
        //Addressables.LoadAssetsAsync<GameObject>(new List<object>() { "susu", "Susu_" },
        //    obj =>
        //    {
        //        //Gets called for every loaded asset
        //        Debug.Log(obj.name);
        //    });
        //yield return loadWithMultipleKeys;
        //IList<GameObject> multipleKeyResult1 = loadWithMultipleKeys.Result;

        foreach (var key in KeyList)
        {
            if (!LoadCheck(key, out error))
            {
                Debug.Log("에러" + error);
                yield return null;
            }
            Load_String_List.Add(key); //로드할 에셋 이름 저장
        }


        AsyncOperationHandle<IList<T>> intersectionWithMultipleKeys =
           Addressables.LoadAssetsAsync<T>(KeyList,
               obj =>
               {
                   //Gets called for every loaded asset
                  // AssetList.Add(obj);
                   Debug.Log(obj);
               }, Addressables.MergeMode.Union);
        yield return intersectionWithMultipleKeys;

        //저장..ㅠㅠ
        // handleIList.Add(intersectionWithMultipleKeys );

        IList<T> multipleKeyResult2 = intersectionWithMultipleKeys.Result;

        foreach(var t in intersectionWithMultipleKeys.Result)
        {
            Debug.Log("intersectionWithMultipleKeys"+t.name);
        }

        foreach (var t in AssetList)
        {
            Debug.Log("AssetList"+t.name);
        }

    }

    //리스트로 로드 비동기+함수
    public IEnumerator Load_Key_List<T>(List<string> KeyList, Action<AsyncOperationHandle<IList<T>>> action)
        where T : UnityEngine.Object

    {
        ErrorCode error;

        foreach (var key in KeyList)
        {
            if (!LoadCheck(key, out error))
            {
                Debug.Log("에러" + error);
                yield return null;
            }

            Load_String_List.Add(key); //로드할 에셋 이름 저장
        }

        AsyncOperationHandle<IList<T>> intersectionWithMultipleKeys =
           Addressables.LoadAssetsAsync<T>(KeyList,
               obj =>
               {
                 
               }, Addressables.MergeMode.Union);

        intersectionWithMultipleKeys.Completed += action;

        yield return intersectionWithMultipleKeys;

        //저장..ㅠㅠ
       // handleIList.Add(intersectionWithMultipleKeys);

        IList<T> multipleKeyResult2 = intersectionWithMultipleKeys.Result;

        foreach (var t in intersectionWithMultipleKeys.Result)
        {
            Debug.Log("intersectionWithMultipleKeys" + t.name);
        }

        foreach (var t in AssetList)
        {
            Debug.Log("AssetList" + t.name);
        }

    }



    //생각좀..ㅠ
    public async Task Load_Key_List_Sync(List<string> KeyList)
    {

      
    }


    //키 연관 저장, label사용 다수 로딩  ,비동기라 ( 비동기 label 있으니까 보류
    public IEnumerator LoadAndAssociateResultWithKey<T>(string Key)
    {

        Load_String_List.Add(Key); //로드할 에셋 이름 저장

        Debug.Log("시작");
        AsyncOperationHandle<IList<IResourceLocation>> locations = Addressables.LoadResourceLocationsAsync(Key);
        yield return locations;
        Debug.Log("locations리턴");

        Dictionary<string, T> associationDoesMatter = new Dictionary<string, T>();

        foreach (IResourceLocation location in locations.Result)
        {
            Debug.Log("foreach시작");

            AsyncOperationHandle<T> handle =
                Addressables.LoadAssetAsync<T>(location);
            handle.Completed += obj =>
            {
                associationDoesMatter.Add(location.PrimaryKey, obj.Result);

                if (associationDoesMatter.ContainsKey(location.PrimaryKey))
                {
                    if (associationDoesMatter.TryGetValue(location.PrimaryKey, out T game))
                    {
                        Debug.Log("primarykey : " + location.PrimaryKey);

                        Debug.Log("objname : " + game);
                    }

                }
            };
            yield return handle;
        }
    }

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

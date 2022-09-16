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

public enum ErrorCode
{
    None=-1,
    LoadSuccess=0,  //성공
    Load_Fail=1,  //로딩 실패
    LoadObjectName_Duplication,  //이름 요청한 이름
    Assets_Already_Loaded,  //이미 로딩 되어있는 경우
    Instantiate_Fail ,  //생성 실패
    Delete_Fail ,//삭제 실패
    Unload_Fail //언로드 실패
}

public enum SaveListName
{
    Name_Save_List =0 ,  //이름만 저장하는 리스트
    Asset_Save_List,  //에셋 로드 저장시키는 리스트
    Assete_Handle_Save_List ,  //핸들 저장시키는 리스트
    Instantiate_Object_Save_List //생성된 오브젝트 저장시키는 리스트
}

public static class AddressablesLoader
{
    public static List<GameObject> tempobj = new List<GameObject>();
    public static List<string> Load_String_List = new List<string>();
    public static int ListCount = 0;

    public static List<object> List = new List<object>();  //하나의 리스트에 로드 자산 관리 시키기
    public static List<AsyncOperationHandle<GameObject>> handleList = new List<AsyncOperationHandle<GameObject>>();  //핸들 저장해서 언로드 관리 시키기.
    public static List<AsyncOperationHandle<IList<GameObject>>> handleIList = new List<AsyncOperationHandle<IList<GameObject>>>();
    public static List<GameObject> Instantiate_Obj_List = new List<GameObject>();  //instantiateAsync를 통해 생성된 오브젝트 관리




    //Addressables.Release();
    //label가져와서 바로 생성 시키기
    public static async Task InitAssets_label<T>(string label, List<T> createdObjs)
        where T : UnityEngine.Object
    {
        Debug.Log("생성전" + label);


        var locations = await Addressables.LoadResourceLocationsAsync(label).Task;
        Debug.Log("생성가ㅣ져옴" + label);


        foreach (var location in locations)
        {
            createdObjs.Add(await Addressables.InstantiateAsync(location).Task as T);
            Debug.Log("생성" + label);
        }
    }

    //로드 없이 바로 생성만 
    public static async Task<T> InitAssets_Instantiate<T>(string name, List<T> createdObjs)
       where T : UnityEngine.Object
    {

        var temp = await Addressables.InstantiateAsync(name).Task;

        createdObjs.Add(temp as T);
       
        foreach(var t in createdObjs)
        {
            Debug.Log("리스트 들어감"+t.name);
        }

        return temp as T;

        //var locations = await Addressables.LoadResourceLocationsAsync(name).Task;
        //Debug.Log("생성가ㅣ져옴" + name);


        //foreach (var location in locations)
        //{
        //    createdObjs.Add(await Addressables.InstantiateAsync(location).Task as T);
        //    Debug.Log("생성" + name);
        //}
    }


    public static void Destroy_Obj(GameObject delete_Obj)
    {
        if(delete_Obj!=null)
        {
            Debug.Log("객체 메모리 삭제 시도");

            if (!Addressables.ReleaseInstance(delete_Obj))
            {
                Debug.Log("객체 메모리 삭제");
                Addressables.ReleaseInstance(delete_Obj);
                //Instantiate_Obj_List.Remove(delete_Obj);
                Debug.Log("리스트 메모리 삭제");
            }
        }
        
    }

    public static void tempCheckList_delete<T>(T delete_Obj)
         where T : UnityEngine.Object
    {
       
            Debug.Log("객체 메모리 삭제 시도 tempCheckList_delete");

          object[] tempArr = List.ToArray();

        //for(int i =0; i < tempArr.Length; i++)
        //{
        //    if(tempArr[i]==delete_Obj as object)
        //    {
        //        Addressables.Release(tempArr[i]);
        //        List.Remove(tempArr[i]);
        //    }
        //}
        AsyncOperationHandle<IList<GameObject>> tempsaveT=new AsyncOperationHandle<IList<GameObject>>();

        foreach(var t in handleIList)
        {
            foreach (var s in t.Result)
            {
                if(s as T == delete_Obj)
                {
                    Debug.Log("삭제하려고왔는뎅 handleIList");
                    tempsaveT = t;
                    List.Remove(t);
                    Addressables.Release(t);
                  
                    Debug.Log("삭제 완료");
                }
            }
        }

        //IList핸들 리스트에서 삭제
        handleIList.Remove(tempsaveT);
        Debug.Log("일단 삭제해씀");


        foreach (var t in handleIList)
        {
            Debug.Log("삭제 됐나 조회중 : "+ t);

            foreach (var s in t.Result)
            {
                Debug.Log(s.name + "삭제됐나 조회중");
            }
        }

        foreach (var t in List)
        {
            //object에 저장할때
            if(t==delete_Obj as object)
            {
                Debug.Log("삭제하려고왔는뎅");
              
                Addressables.Release(t);
                List.Remove(t);
            }

            Debug.Log("List 조회 : " + t);
        }

    }

    public static object Find_Asset_In_AllList<T>(T FindObj)
    {
        object findAsset=null;

        Debug.Log("Find_Asset_In_AllList 진입");

        foreach(var t in List)
        {
            if( t==FindObj as object)
            {
                findAsset = t;
                Debug.Log("findAsset찾음 in List");

                return findAsset;
            }
        }

        //이 밑에 다른 리스트도 돌리기
       foreach(var t in Instantiate_Obj_List)
        {
            if (t == FindObj as GameObject)
            {
                findAsset = t;
                Debug.Log("findAsset찾음 in Instantiate_Obj_List");

                return findAsset;
            }
        }

        Debug.Log("아무것도 못찾음");

        return findAsset;
    }

    public static object Find_Asset_In_AllList(string FindObj)
    {
        object findAsset = null;

        Debug.Log("Find_Asset_In_AllList 진입");

        foreach (var t in List)
        {
            Debug.Log("List 요소 조회 : " + t);
            if (t.ToString() == FindObj)
            {
                findAsset = t;
                Debug.Log("findAsset찾음 in List");

                return findAsset;
            }
        }

        //이 밑에 다른 리스트도 돌리기
        foreach (var t in Instantiate_Obj_List)
        {
            Debug.Log("Instantiate_Obj_List 요소 조회 : " + t);


            if (t.name == FindObj)
            {
                findAsset = t;
                Debug.Log("findAsset찾음 in Instantiate_Obj_List");

                return findAsset;
            }
        }

        foreach(var t in handleList)
        {
            Debug.Log("handleList 요소 조회 : " + t.Result.name);

            if (t.Result.name==FindObj)
            {
                findAsset = t;
                Debug.Log("findAsset찾음 in handleList");

                return findAsset;
            }
        }
        
        foreach(var t in handleIList)
        {

            foreach (var s in t.Result)
            {
                Debug.Log("handleIList 요소 조회 : " + s.name);

                if (s.name == FindObj)
                {
                    findAsset = s;
                    Debug.Log("findAsset찾음 in handleIList");

                    return findAsset;
                }
            }
        }

        Debug.Log("아무것도 못찾음");

        return findAsset;
    }

    //동기 로드
    public static async Task InitAssets_name_<T>(string object_name)
                where T : UnityEngine.Object
    {
        //AsyncOperationHandle<GameObject> operationHandle=
        // Addressables.LoadAssetAsync<GameObject>(object_name);
        Debug.Log("시작" + object_name);

        var temp = await Addressables.LoadAssetAsync<GameObject>(object_name).Task;        
        Debug.Log("가져옴" + object_name);

        tempobj.Add(temp);
      
        foreach(var t in tempobj)
        {
            Debug.Log("요소 출력: "+t.name);
        }

    

        // yield return operationHandle;

        //createdObjs.Add(operationHandle.Result as T
    }


    //이름으로 생성
    //Addressables.ReleaseInstance();
    public static async Task InitAssets_name<T>(string object_name, List<T> createdObjs)
        where T : UnityEngine.Object
    {
        //AsyncOperationHandle<GameObject> operationHandle=
        // Addressables.LoadAssetAsync<GameObject>(object_name);

        Addressables.LoadAssetAsync<GameObject>(object_name).Completed += ObjectLoadDone;

        // yield return operationHandle;

        //createdObjs.Add(operationHandle.Result as T);

    }
    //이름으로 생성
    //리스트 필요없이 메모리 할당만 할 때
    //Addressables.ReleaseInstance();
    public static async Task InitAssets_name(string object_name)
    {
        //AsyncOperationHandle<GameObject> operationHandle=
        // Addressables.LoadAssetAsync<GameObject>(object_name);

        Addressables.LoadAssetAsync<GameObject>(object_name).Completed += ObjectLoadDone;

        // yield return operationHandle;

        //createdObjs.Add(operationHandle.Result as T
    }

    public static async Task InitAssets_name<T>(string object_name, Action<AsyncOperationHandle<T>> Complete)
    {
        //AsyncOperationHandle<GameObject> operationHandle=
        // Addressables.LoadAssetAsync<GameObject>(object_name);

        Addressables.LoadAssetAsync<T>(object_name).Completed += Complete;

        // yield return operationHandle;

        //createdObjs.Add(operationHandle.Result as T
    }

 

    //   public static void InitAssets_name(string object_name)
    //{

    //	Addressables.LoadAssetAsync<GameObject>(object_name).Completed += ObjectLoadDone;

    //}

    //static IEnumerator LoadGameObjectAndMaterial(string name)
    //{
    //	//Load a GameObject
    //	AsyncOperationHandle<GameObject> goHandle = Addressables.LoadAssetAsync<GameObject>(name);
    //	yield return goHandle;
    //	if (goHandle.Status == AsyncOperationStatus.Succeeded)
    //	{
    //		GameObject obj = goHandle.Result;
    //		tempobj.Add(obj);
    //		//etc...
    //	}



    //	////Load a Material
    //	//AsyncOperationHandle<IList<IResourceLocation>> locationHandle = Addressables.LoadResourceLocationsAsync("materialKey");
    //	//yield return locationHandle;
    //	//AsyncOperationHandle<Material> matHandle = Addressables.LoadAssetAsync<Material>(locationHandle.Result[0]);
    //	//yield return matHandle;
    //	//if (matHandle.Status == AsyncOperationStatus.Succeeded)
    //	//{
    //	//	Material mat = matHandle.Result;
    //	//	//etc...
    //	//}

    //	//Use this only when the objects are no longer needed
    //	Addressables.Release(goHandle);
    //	//Addressables.Release(matHandle);
    //}

    //객체 불러오기 (1개
    public static IEnumerator LoadGameObjectAndMaterial(string name)
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
        GameObject findGameobj= AddressablesController.Instance.find_Asset_in_list(name);
        if(findGameobj!=null)
        {
            Debug.Log("이미 로드된 파일입니다.");
            yield return null;
        }
        else
        {
            //Load a GameObject
            AsyncOperationHandle<GameObject> goHandle = Addressables.LoadAssetAsync<GameObject>(name);
            yield return goHandle;
            if (goHandle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject gameObject = goHandle.Result;
                tempobj.Add(gameObject);
                ListCount = tempobj.Count;
                Debug.Log(gameObject.name + "로드");

                foreach (var obj in tempobj)
                {
                    //	c++;
                    Debug.Log(obj.name + "리스트확인");
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
    public static IEnumerator LoadGameObjectAndMaterial(string name, Action<AsyncOperationHandle<GameObject>> Complete)
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
            AsyncOperationHandle<GameObject> goHandle = Addressables.LoadAssetAsync<GameObject>(name);
            goHandle .Completed += Complete;
            yield return goHandle;
            if (goHandle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject gameObject = goHandle.Result;
                tempobj.Add(gameObject);
                ListCount = tempobj.Count;
                Debug.Log(gameObject.name + "로드");

                foreach (var obj in tempobj)
                {
                    //	c++;
                    Debug.Log(obj.name + "리스트확인");
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

    //키 연관 리스트 저장, label로 다수로딩 코루틴 
    public static IEnumerator LoadAndStoreResult(string Key)
    {
        List<GameObject> associationDoesNotMatter = new List<GameObject>();

        AsyncOperationHandle<IList<GameObject>> handle =
            Addressables.LoadAssetsAsync<GameObject>(Key, obj =>
            {
                associationDoesNotMatter.Add(obj);
                //잠시  object저장 되는지 확인
               
                //List.Add(obj);
                Debug.Log(obj.name);
            });
        yield return handle;
        //handleList.Add(handle);
        handleIList.Add(handle);
        List.Add(handle);
    }

 
    public static void tempCheckList()
    {
        foreach(var temp in List)
        {
            Debug.Log("tempCheckList : " + temp);
        }
    }

    //델리게이트 실행 (Key에 label)
    public static IEnumerator LoadAndStoreResult<T>(string Key, Action<AsyncOperationHandle<IList<T>>> Complete)
        where T : UnityEngine.Object
    {
        List<T> associationDoesNotMatter = new List<T>();

        AsyncOperationHandle<IList<T>> handle =
            Addressables.LoadAssetsAsync<T>(Key, obj => associationDoesNotMatter.Add(obj));
        handle.Completed += Complete;
        yield return handle;
    }

    //키 연관 저장, label사용 다수 로딩
    public static IEnumerator LoadAndAssociateResultWithKey(string Key)
    {
        Debug.Log("시작");
        AsyncOperationHandle<IList<IResourceLocation>> locations = Addressables.LoadResourceLocationsAsync(Key);
        yield return locations;
        Debug.Log("locations리턴");

        Dictionary<string, GameObject> associationDoesMatter = new Dictionary<string, GameObject>();

        foreach (IResourceLocation location in locations.Result)
        {
            Debug.Log("foreach시작");

            AsyncOperationHandle<GameObject> handle =
                Addressables.LoadAssetAsync<GameObject>(location);
            handle.Completed += obj =>
            {
                associationDoesMatter.Add(location.PrimaryKey, obj.Result);

                if (associationDoesMatter.ContainsKey(location.PrimaryKey))
                {
                    if (associationDoesMatter.TryGetValue(location.PrimaryKey, out GameObject game))
                    {
                        Debug.Log("primarykey : " + location.PrimaryKey);

                        Debug.Log("objname : "+game.name);
                    }

                }
            };
            yield return handle;
        }
    }

    //리스트로 로드
    //MergeMode.Union외에 다른거는 오류
    public static IEnumerator Load_Key_List(List<object> KeyList)
    {
        //AsyncOperationHandle<IList<GameObject>> loadWithMultipleKeys =
        //Addressables.LoadAssetsAsync<GameObject>(new List<object>() { "susu", "Susu_" },
        //    obj =>
        //    {
        //        //Gets called for every loaded asset
        //        Debug.Log(obj.name);
        //    });
        //yield return loadWithMultipleKeys;
        //IList<GameObject> multipleKeyResult1 = loadWithMultipleKeys.Result;

        AsyncOperationHandle<IList<GameObject>> intersectionWithMultipleKeys =
           Addressables.LoadAssetsAsync<GameObject>(KeyList,
               obj =>
               {
                //Gets called for every loaded asset
                Debug.Log(obj.name);
               }, Addressables.MergeMode.Union);
        yield return intersectionWithMultipleKeys;
        IList<GameObject> multipleKeyResult2 = intersectionWithMultipleKeys.Result;

    }


    //리스트 저장
    //저장시키고자 하는 리스트 이름 입력
    public static bool Save_List<T>(SaveListName saveListName, T saveAsset)
    {

        switch(saveListName)
        {
            case SaveListName.Name_Save_List:
                Load_String_List.Add(saveAsset as string);

                break;

            case SaveListName.Assete_Handle_Save_List:
                List.Add(saveAsset);   //핸들 저장? 
                break;

            case SaveListName.Instantiate_Object_Save_List:
                Instantiate_Obj_List.Add(saveAsset as GameObject);
                break;

            case SaveListName.Asset_Save_List:

                break;
        }

        return false;
    }


    public static ErrorCode Return_ErrorCode()
    {
        ErrorCode errorCode = ErrorCode.None;




        return errorCode;
    }

    //public static IEnumerator LoadGameObjectAndMaterial(<string name)
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
    //        AsyncOperationHandle<GameObject> goHandle = Addressables.LoadAssetAsync<GameObject>(name);
    //        yield return goHandle;
    //        if (goHandle.Status == AsyncOperationStatus.Succeeded)
    //        {
    //            GameObject gameObject = goHandle.Result;
    //            tempobj.Add(gameObject);
    //            ListCount = tempobj.Count;
    //            Debug.Log(gameObject.name + "로드");

    //            foreach (var obj in tempobj)
    //            {
    //                //	c++;
    //                Debug.Log(obj.name + "리스트확인");
    //            }
    //            //etc...
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


    private static void ObjectLoadDone(AsyncOperationHandle<GameObject> obj)
    {
        GameObject gameObject = obj.Result;
        tempobj.Add(gameObject);

        Debug.Log(obj.Result.name + "어드레서블로드");

    }

    public static GameObject returnAssets(string object_name)
    {
        GameObject tempobj = null;

        Addressables.LoadAssetAsync<GameObject>(object_name).Completed += (handle) =>
         {
             tempobj = handle.Result;
             Debug.Log(tempobj.name + "에셋리턴");
         // return tempobj;
     };

        if (tempobj != null)
        {
            return tempobj;
        }
        Debug.Log("비어있음");
        return tempobj;
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

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

public static class AddressablesLoader
{
    public static List<GameObject> tempobj = new List<GameObject>();
    public static List<string> Load_String_List = new List<string>();
    public static int ListCount = 0;

    public static List<object> List = new List<object>();  //하나의 리스트에 로드 자산 관리 시키기
    public static List<AsyncOperationHandle<GameObject>> handleList = new List<AsyncOperationHandle<GameObject>>();  //핸들 저장해서 언로드 관리 시키기.


    //Addressables.Release();
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

    public static async Task InitAssets_name_<T>(string object_name, Action<AsyncOperationHandle<T>> Complete)
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
                Debug.Log(obj.name);
            });
        yield return handle;
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

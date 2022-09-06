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
    //레이블로 생성
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

    private static void OnSceneUnloaded(AsyncOperationHandle<SceneInstance> obj)
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

    private static void OnSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
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

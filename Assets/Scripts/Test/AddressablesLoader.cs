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
    public static List<AsyncOperationHandle<IList<GameObject>>> handleIList = new List<AsyncOperationHandle<IList<GameObject>>>();  //핸들 저장
    public static List<AsyncOperationHandle<IList<object>>> handleIObjectList = new List<AsyncOperationHandle<IList<object>>>();  //핸들 저장

    public static List<GameObject> Instantiate_Obj_List = new List<GameObject>();  //instantiateAsync를 통해 생성된 오브젝트 관리

    public static Dictionary<string, List<object>> AssetList = new Dictionary<string, List<object>>();  //key값을 통해 오브젝트, 핸들을 관리
    public static Dictionary<string, List<AsyncOperationHandle<IList<object>>>> HandleDicList =new Dictionary<string, List<AsyncOperationHandle<IList<object>>>>();   //key값을 통해 오브젝트, 핸들을 관리




    //Addressables.Release();
    //label가져와서 바로 생성 시키기, 리스트 저장 (label,저장할 리스트)
    public static async Task InitAssets_label<T>(string label, List<T> createdObjs)
        where T : UnityEngine.Object
    {


        var locations = await Addressables.LoadResourceLocationsAsync(label).Task;


        foreach (var location in locations)
        {
            var temp = await Addressables.InstantiateAsync(location).Task as T;
             createdObjs.Add(temp);
             List.Add(temp);  //List에 저장 (생성된 오브젝트)

        }
    }

    //로드 없이 바로 생성만 /동기
    public static async Task<T> InitAssets_Instantiate<T>(string name, List<T> createdObjs)
       where T : UnityEngine.Object
    {

        var temp = await Addressables.InstantiateAsync(name).Task;

        createdObjs.Add(temp as T);

        List.Add(temp);

        foreach (var t in createdObjs)
        {
        }


        foreach (var t in List)
        {
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

            if (!Addressables.ReleaseInstance(delete_Obj))
            {
                Addressables.ReleaseInstance(delete_Obj);
                //Instantiate_Obj_List.Remove(delete_Obj);
            }
        }
        
    }

    public static bool Delete_Object(GameObject deleteObj)
    {
        bool check = false;






        return check;
    }

    //삭제할때는 핸들만, 리턴 받을때는 List만


    public static void tempCheckList_delete(GameObject delete_Obj)
    {


        object[] tempArr = List.ToArray();

        //for(int i =0; i < tempArr.Length; i++)
        //{
        //    if(tempArr[i]==delete_Obj as object)
        //    {
        //        Addressables.Release(tempArr[i]);
        //        List.Remove(tempArr[i]);
        //    }
        //}


        //리스트 .......오류...
        if (AssetList.ContainsKey(delete_Obj.name))
        {

            var s = AssetList[delete_Obj.name];
           // Addressables.Release(s[0]);  //오류
           s.Remove(s[0]);

            foreach (var tt in AssetList[delete_Obj.name])
            {


               // Addressables.Release(tt);
             

                AssetList[delete_Obj.name].Remove(tt);
                break;
            }
        }

        AsyncOperationHandle<IList<GameObject>> tempsaveT = new AsyncOperationHandle<IList<GameObject>>();

        foreach (var t in handleIList)
        {
            foreach (var s in t.Result)
            {
                if (s == delete_Obj)
                {
                    tempsaveT = t;
                    List.Remove(t);
                    Addressables.Release(t);

                    return;
                }
            }
        }

        GameObject tempdeleteSave=new GameObject();

        foreach (var t in handleIList)
        {

            foreach (var s in t.Result)
            {
            }
        }

        foreach (var t in List)
        {
            //object에 저장할때
            if (t == delete_Obj as object)
            {

                Addressables.ReleaseInstance((GameObject)t);
                 tempdeleteSave = (GameObject)t;

                // GameObject.Destroy(delete_Obj);
                break;

            }

        }

        //IList핸들 리스트에서 삭제
        List.Remove(tempdeleteSave);


        foreach (var t in List)
        {
        }


        handleIList.Remove(tempsaveT);


    }

    public static void tempCheckList_delete<T>(T delete_Obj)
         where T : UnityEngine.Object
    {
       

          object[] tempArr = List.ToArray();

        //for(int i =0; i < tempArr.Length; i++)
        //{
        //    if(tempArr[i]==delete_Obj as object)
        //    {
        //        Addressables.Release(tempArr[i]);
        //        List.Remove(tempArr[i]);
        //    }
        //}

       if(AssetList.ContainsKey(delete_Obj.ToString()))
        {
         
        }

        AsyncOperationHandle<IList<GameObject>> tempsaveT=new AsyncOperationHandle<IList<GameObject>>();

        foreach(var t in handleIList)
        {
            foreach (var s in t.Result)
            {
                if(s as T == delete_Obj)
                {
                    tempsaveT = t;
                    List.Remove(t);
                    Addressables.Release(t);

                    return;
                }
            }
        }


        foreach (var t in handleIList)
        {

            foreach (var s in t.Result)
            {
            }
        }

        foreach (var t in List)
        {
            //object에 저장할때
            if(t==delete_Obj as object)
            {
              
                Addressables.Release(t);
                List.Remove(t);
                // GameObject.Destroy(delete_Obj);
                break;

            }

        }

        //IList핸들 리스트에서 삭제

       handleIList.Remove(tempsaveT);


    }

    public static object Find_Asset_In_AllList<T>(T FindObj)
    {
        object findAsset=null;


        //if(List.Contains(FindObj))
        //{
        //    findAsset = List.Find(FindObj);
        //    Debug.Log("findAsset찾음 in List");

        //    return findAsset;
        //}


        foreach(var t in List)
        {
            if( t==FindObj as object)
            {
                findAsset = t;

                return findAsset;
            }
        }

        //이 밑에 다른 리스트도 돌리기
       foreach(var t in Instantiate_Obj_List)
        {
            if (t == FindObj as GameObject)
            {
                findAsset = t;

                return findAsset;
            }
        }


        return findAsset;
    }

    //로딩 된 거 찾기
    public static object Find_Asset_In_AllList(string FindObj)
    {
        object findAsset = null;
        string List_name = " (UnityEngine.GameObject)";  //string형이라 뒤에 이거 붙여야 ..


        foreach (var t in List)
        {
            if (t.ToString() == FindObj+ List_name)
            {
                findAsset = t;

                return findAsset;
            }
            if(t.ToString()==FindObj)
            {
                findAsset = t;

                return findAsset;
            }
        }


        if(AssetList.ContainsKey(FindObj))
        {
            foreach(var t in AssetList[FindObj])
            {
                findAsset = t as GameObject;
                GameObject tempobj = UnityEngine.Object.Instantiate((GameObject)findAsset, new Vector3(0f, 0f, 0f), Quaternion.identity);
                return tempobj;
            }
        }

        //이 밑에 다른 리스트도 돌리기
        foreach (var t in Instantiate_Obj_List)
        {


            if (t.name == FindObj)
            {
                findAsset = t;

                return findAsset;
            }
        }

        foreach(var t in handleList)
        {

            if (t.Result.name==FindObj)
            {
                findAsset = t;

                return findAsset;
            }
        }
        
        foreach(var t in handleIList)
        {

            foreach (var s in t.Result)
            {

                if (s.name == FindObj)
                {
                    findAsset = s;

                    return findAsset;
                }
            }
        }


        return findAsset;
    }

    //동기 로드
    public static async Task InitAssets_name_<T>(string object_name)
                where T : UnityEngine.Object
    {
        //AsyncOperationHandle<T> operationHandle=
        //await Addressables.LoadAssetAsync<T>(object_name).Task;

        var temp = await Addressables.LoadAssetAsync<T>(object_name).Task;        

        List.Add(temp);

        foreach(var t in List)
        {
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


        if (Load_String_List.Contains(name))
        {
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
                List.Add(gameObject);
               // ListCount = tempobj.Count;

                //foreach (var obj in tempobj)
                //{
                //    //	c++;
                //    Debug.Log(obj.name + "tempobj 리스트확인");
                //}
                foreach (var obj in List)
                {
                    //	c++;
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
    public static IEnumerator LoadGameObjectAndMaterial<T>(string name, Action<AsyncOperationHandle<T>> Complete)
    {


        if (Load_String_List.Contains(name))
        {
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
            yield return null;
        }
        else
        {
            //Load a GameObject
            AsyncOperationHandle<T> goHandle = Addressables.LoadAssetAsync<T>(name);
            goHandle .Completed += Complete;
            yield return goHandle;
            if (goHandle.Status == AsyncOperationStatus.Succeeded)
            {
                T gameObject = goHandle.Result;
                tempobj.Add(gameObject as GameObject);
                List.Add(gameObject);
                ListCount = tempobj.Count;

                foreach (var obj in tempobj)
                {
                    //	c++;
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
               
                List.Add(obj);
            });
        yield return handle;
        //handleList.Add(handle);
        handleIList.Add(handle);
        //List.Add(handle);

        foreach(var t in List)
        {
        }
    }

 
    public static void tempCheckList()
    {
        foreach(var temp in List)
        {
        }
    }

    //델리게이트 실행 (Key에 label)
    public static IEnumerator LoadAndStoreResult<T>(string Key, Action<AsyncOperationHandle<IList<T>>> Complete)
        where T : UnityEngine.Object
    {
        List<T> associationDoesNotMatter = new List<T>();

        AsyncOperationHandle<IList<T>> handle =
            Addressables.LoadAssetsAsync<T>(Key, obj => List.Add(obj));
        handle.Completed += Complete;
        yield return handle;
       // handleIObjectList.Add();
       // handleIList.Add(handle);
      // Debug.Log()
        //List.Add(handle.Result);

         List<object> temp = new List<object>();
        temp.Add(handle);
        AssetList.Add(Key, temp);

    }

    //키 연관 저장, label사용 다수 로딩
    public static IEnumerator LoadAndAssociateResultWithKey(string Key)
    {
        AsyncOperationHandle<IList<IResourceLocation>> locations = Addressables.LoadResourceLocationsAsync(Key);
        yield return locations;

        Dictionary<string, GameObject> associationDoesMatter = new Dictionary<string, GameObject>();

        foreach (IResourceLocation location in locations.Result)
        {

            AsyncOperationHandle<GameObject> handle =
                Addressables.LoadAssetAsync<GameObject>(location);
            handle.Completed += obj =>
            {
                associationDoesMatter.Add(location.PrimaryKey, obj.Result);

                if (associationDoesMatter.ContainsKey(location.PrimaryKey))
                {
                    if (associationDoesMatter.TryGetValue(location.PrimaryKey, out GameObject game))
                    {

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


    }

    public static GameObject returnAssets(string object_name)
    {
        GameObject tempobj = null;

        Addressables.LoadAssetAsync<GameObject>(object_name).Completed += (handle) =>
         {
             tempobj = handle.Result;
         // return tempobj;
     };

        if (tempobj != null)
        {
            return tempobj;
        }
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
                break;
            default:
                break;
        }
    }



}

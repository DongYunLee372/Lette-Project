using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Game_Object_save
{
    public GameObject value;
}

public class ResourceManager : MonoBehaviour
{
    public T Load<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');
            if (index >= 0)
                name = name.Substring(index + 1);

            GameObject go = GameMG.Instance.ObjManager.GetOriginal<GameObject>(name);
            if (go != null)
                return go as T;
        }
        return Resources.Load<T>(path);
    }

    //오브젝트 풀링 어드레서블
    public IEnumerator Instantiate_(string path, Transform parent = null)
    {
        GameObject original = AddressablesController.Instance.find_Asset_in_list(path);
    
        if (original == null)
        {
           
            Debug.Log("없어서 로드하려는중...");
          //  StartCoroutine(AddressablesController.Instance.check_List_routine(path, original));
            StaticCoroutine.DoCoroutine(AddressablesController.Instance.check_List_routine(path, parent));
        }

        yield  break;

        //if (original == null)
        //{
        //    Debug.Log($"failed to load prefab : {path}");
        //    yield return null;
        //}

        //if (original.GetComponent<Poolable>() != null)
        //{
        //    Debug.Log("gameobject리턴");

        //    yield return GameMG.Instance.ObjManager.Pop(original, parent).gameObject;
        //}

        //Debug.Log("그 외?");
        //GameObject go = Object.Instantiate(original, parent);
        //go.name = original.name;
        //yield return go;

    }

    //    public GameObject Instantiate(string path, Transform parent = null)
    //{
    //    //일단 네임으로 호출해보고 리스트에 있으면 반환

    //    GameObject original = AddressablesController.Instance.find_Asset_in_list(path);

    //    //만약 불러왔는데 없으면 새로 로드
    //    if (original == null)
    //    {
    //        //일단 메모리 불러옴 (이름으로)
    //        Debug.Log("없어서 로드하려는중...");
    //       // StaticCoroutine.DoCoroutine(AddressablesLoader.LoadGameObjectAndMaterial(path));
    //        //StartCoroutine(AddressablesLoader.LoadGameObjectAndMaterial(path));
    //        Debug.Log("없어서 로드중...");

    //        //리스트 추가 대기(1초)하다가 추가 되면 리스트에서 찾아봄
    //       // StaticCoroutine.DoCoroutine(AddressablesController.Instance.check_List_routine(path, original));
    //        // StartCoroutine(AddressablesController.Instance.check_List_routine());

    //        Debug.Log("코루틴 탈출하고 코드 실행...");

    //        Debug.Log("Loder_ListCount" + AddressablesController.Instance.Loder_ListCount);
    //        Debug.Log("tempobj" + AddressablesLoader.tempobj.Count);
    //        if (AddressablesController.Instance.Loder_ListCount != AddressablesLoader.tempobj.Count)
    //        {
    //            AddressablesController.Instance.Loder_ListCount = AddressablesLoader.tempobj.Count;
    //            Debug.Log("list수는" + AddressablesLoader.tempobj.Count);
    //           // AddressablesController.Instance.check_List("susu");
    //            AddressablesController.Instance.load_Comp = true;
    //        }

    //        //if (AddressablesController.Instance.load_Comp)
    //        //{
    //        //    Debug.Log("load_Comp");

    //        //    original = AddressablesController.Instance.find_Asset_in_list(name);
    //        //    Debug.Log("load_Comp완료" + original.name);
    //        //    Debug.Log("찾은 거" + original.name);
    //        //    AddressablesController.Instance.load_Comp = false;
    //        //}

    //    }

    //    if (original == null)
    //    {
    //        Debug.Log($"failed to load prefab : {path}");
    //        return null;
    //    }

    //    if (original.GetComponent<Poolable>() != null)
    //    {
    //        Debug.Log("gameobject리턴");

    //        return GameMG.Instance.ObjManager.Pop(original, parent).gameObject;
    //    }

    //    Debug.Log("그 외?");
    //    GameObject go = Object.Instantiate(original, parent);
    //    go.name = original.name;
    //    return go;

    //}

    // 어드레서블로 바꿔야댐
    public T Instantiate<T>(string path, Transform parent = null)
       where T : UnityEngine.Object

    {
        AddressablesLoadManager.Instance.SingleAsset_Load<T>(path);
       T original = AddressablesLoadManager.Instance.FindLoadAsset<T>(path);

        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }


      //  if (original.GetComponent<Poolable>() != null)
        //{
       var t= GameMG.Instance.ObjManager.Pop<T>(original, parent) as T;
        Debug.Log("bb"+t);
        return GameMG.Instance.ObjManager.Pop<T>(original, parent) as T;
        //}

       // GameObject go = Object.Instantiate(original, parent) as T;
      //  go.name = original.name;
       // return go;
    }

    //public T Instantiate<T>(string path, Transform parent = null)
    //where T : UnityEngine.Object
    //{
    //    AddressablesLoadManager.Instance.SingleAsset_Load<T>(path);
    //    T = AddressablesLoadManager.Instance.FindLoadAsset<T>(path);

    //    if (original == null)
    //    {
    //        Debug.Log($"Failed to load prefab : {path}");
    //        return null;
    //    }


    //    return GameMG.Instance.ObjManager.Pop(original, parent).gameObject;

    //    T go = Object.Instantiate(original, parent);
    //    go.name = original.name;
    //    return go;
    //}




    // 어드레서블로 바꿔야댐 원본
    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");

        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        if (original.GetComponent<Poolable>() != null)
        {
            return GameMG.Instance.ObjManager.Pop(original, parent).gameObject;
        }

        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        Poolable poolable = go.GetComponent<Poolable>();
        if (poolable != null)
        {
            GameMG.Instance.ObjManager.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }
}

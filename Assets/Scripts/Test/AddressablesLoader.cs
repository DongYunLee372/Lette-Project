using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public static class AddressablesLoader
{
    public static List<GameObject> tempobj = new List<GameObject>();

    //���̺�� ����
    //Addressables.Release();
    public static async Task InitAssets_label<T>(string label, List<T> createdObjs)
        where T : Object
    {
        Debug.Log("������" + label);


        var locations = await Addressables.LoadResourceLocationsAsync(label).Task;
        Debug.Log("������������" + label);


        foreach (var location in locations)
        {
            createdObjs.Add(await Addressables.InstantiateAsync(location).Task as T);
            Debug.Log("����" + label);
        }
    }

    //�̸����� ����
    //Addressables.ReleaseInstance();
    public static async Task InitAssets_name<T>(string object_name, List<T> createdObjs)
        where T : Object
    {
        //AsyncOperationHandle<GameObject> operationHandle=
        // Addressables.LoadAssetAsync<GameObject>(object_name);

        Addressables.LoadAssetAsync<GameObject>(object_name).Completed += ObjectLoadDone;

        // yield return operationHandle;

        //createdObjs.Add(operationHandle.Result as T);

    }
    //�̸����� ����
    //����Ʈ �ʿ���� �޸� �Ҵ縸 �� ��
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

    public static IEnumerator LoadGameObjectAndMaterial(string name)
    {
        //Load a GameObject
        AsyncOperationHandle<GameObject> goHandle = Addressables.LoadAssetAsync<GameObject>(name);
        yield return goHandle;
        if (goHandle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject gameObject = goHandle.Result;
            tempobj.Add(gameObject);
            Debug.Log(gameObject.name + "�ε�");

            foreach (var obj in tempobj)
            {
                //	c++;
                Debug.Log(obj.name + "����ƮȮ��");
            }
            //etc...
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

    private static void ObjectLoadDone(AsyncOperationHandle<GameObject> obj)
    {
        GameObject gameObject = obj.Result;
        tempobj.Add(gameObject);

        Debug.Log(obj.Result.name + "��巹����ε�");

    }

    public static GameObject returnAssets(string object_name)
    {
        GameObject tempobj = null;

        Addressables.LoadAssetAsync<GameObject>(object_name).Completed += (handle) =>
         {
             tempobj = handle.Result;
             Debug.Log(tempobj.name + "���¸���");
         // return tempobj;
     };

        if (tempobj != null)
        {
            return tempobj;
        }
        Debug.Log("�������");
        return tempobj;
    }




    //public static GameObject LoadAsset_name(string object_name )
    //   {


    //	return 
    //   }


}

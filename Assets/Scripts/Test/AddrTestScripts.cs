using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class AddrTestScripts : MonoBehaviour
{

    SceneInstance m_LoadedScene;

    public GameObject pos;

    public delegate void Complete_delegate(AsyncOperationHandle<GameObject> comp);
    public Action<AsyncOperationHandle<GameObject>> Complete_Aciton;

    public GameObject tempHPbar=null;
    // Start is called before the first frame update
    void Start()
    {
        pos = new GameObject();
        pos.transform.position = new Vector3(0f, 0f, 0f);

        //예외처리 테스트
        //StartCoroutine(AddressablesLoader.LoadGameObjectAndMaterial("Hpbar"));
        //StartCoroutine(AddressablesLoader.LoadGameObjectAndMaterial("Hpbar"));

        //델리게이트 테스트

        // StartCoroutine(AddressablesLoader.LoadGameObjectAndMaterial("Hpbar", handle => Debug.Log("델리게이트"+handle.Result.name)));

        //  StartCoroutine(AddressablesLoader.LoadGameObjectAndMaterial("Demo"));

        // AddressablesLoader.InitAssets_name<SceneInstance>("Demo", handle => m_LoadedScene = handle.Result);

       // AddressablesLoader.InitAssets_name<GameObject>("Susu_", handle => tempHPbar = handle.Result);

       // StartCoroutine(AddressablesController.Instance.Load_Name("Susu_", pos.transform));

         Loder();
    }

    public async Task Loder()
    {
        await AddressablesLoader.InitAssets_name<GameObject>("Susu_", handle => tempHPbar = handle.Result);
        Debug.Log("다녀왔음"+ tempHPbar);

        Instantiate(tempHPbar, new Vector3(0, 0, 0), Quaternion.identity);
        Debug.Log("생성함" + pos.name);

      
    }

    IEnumerator temp()
    {
        yield return StartCoroutine(AddressablesLoader.LoadGameObjectAndMaterial("Hpbar", handle => tempHPbar = handle.Result));
        Debug.Log("다녀왔음" + tempHPbar);
    }

    


    // Update is called once per frame
    void Update()
    {
        
    }
}

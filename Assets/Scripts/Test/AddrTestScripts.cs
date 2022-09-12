using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class AddrTestScripts : MonoBehaviour
{

    SceneInstance m_LoadedScene;

    public GameObject tempOBJ_;
    public GameObject pos;

    public delegate void Complete_delegate(AsyncOperationHandle<GameObject> comp);
    public Action<AsyncOperationHandle<GameObject>> Complete_Aciton;

    public GameObject tempHPbar;
    // Start is called before the first frame update
    void Start()
    {
        pos = new GameObject();
       // tempHPbar = new GameObject();
        pos.transform.position = new Vector3(0f, 0f, 0f);

        //예외처리 테스트
        //StartCoroutine(AddressablesLoader.LoadGameObjectAndMaterial("Hpbar"));
        //StartCoroutine(AddressablesLoader.LoadGameObjectAndMaterial("Hpbar"));

        //델리게이트 테스트

        //StartCoroutine(AddressablesLoader.LoadGameObjectAndMaterial("Susu_", handle => Debug.Log("델리게이트"+handle.Result.name)));
        //StartCoroutine( temp());

        //  StartCoroutine(AddressablesLoader.LoadGameObjectAndMaterial("Demo"));

        // AddressablesLoader.InitAssets_name<SceneInstance>("Demo", handle => m_LoadedScene = handle.Result);

        // AddressablesLoader.InitAssets_name<GameObject>("Susu_", handle => tempHPbar = handle.Result);

        // StartCoroutine(AddressablesController.Instance.Load_Name("Susu_", pos.transform));

         Loder();

        //label로 다수 로딩
        //StartCoroutine(AddressablesLoader.LoadAndStoreResult("Monster"));

        //  StartCoroutine(AddressablesLoader.LoadAndAssociateResultWithKey("Monster"));

        //리스트로 다수 로딩
       //StartCoroutine(AddressablesLoader.Load_Key_List(new List<object>() { "susu", "Susu_" }));


        //TaskRun();
        //TaskFromResult();

    }

     public async void Loder()
    {

        //await AddressablesLoader.InitAssets_name_<GameObject>("susu", handle => tempHPbar = handle.Result);

        Debug.Log("다녀왔음"+ tempHPbar);

        Instantiate(tempHPbar, pos.transform);
        Debug.Log("생성함"+pos.name );

      
    }

    //델리게이트 예시
    IEnumerator temp()
    {
        //yield return StartCoroutine(AddressablesLoader.LoadGameObjectAndMaterial("Hpbar", handle => tempHPbar = handle.Result));
        //Debug.Log("다녀왔음" + tempHPbar);

        yield return StartCoroutine(AddressablesLoader.LoadGameObjectAndMaterial("Susu_", handle => tempOBJ_ = handle.Result));
        Debug.Log("다녀왔음" + tempOBJ_);
        Instantiate(tempOBJ_, pos.transform);
    }


    async void TaskRun()
    {
        var task = Task.Run(() => TaskRunMethod(3));
        int count = await task;
        Debug.Log("Count : " + task.Result); // 출력 : Count : 3  
    }

    private int TaskRunMethod(int limit)
    {
        int count = 0;
        for (int i = 0; i < limit; i++)
        {
            ++count;
            Thread.Sleep(1000);
        }

        return count;
    }

    async void TaskFromResult()
    {
        int sum = await Task.FromResult(Add(1, 2));
        Debug.Log(sum+"sum"); // 출력 : 3  
    }

    private int Add(int a, int b)
    {
        return a + b;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}

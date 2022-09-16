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
    public GameObject te;
    async void Start()
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

        //델리게이트 동기 로드 테스트
        //await Loder("susu",tempAction);

        //te = await AddressablesLoader.InitAssets_Instantiate<GameObject>("susu", AddressablesLoader.Instantiate_Obj_List);
        //Debug.Log("끝나야되는데?");


        StartCoroutine(tempCheck());




        //label로 다수 로딩
        //StartCoroutine(AddressablesLoader.LoadAndStoreResult("Monster"));

        //  StartCoroutine(AddressablesLoader.LoadAndAssociateResultWithKey("Monster"));

        //리스트로 다수 로딩
        //StartCoroutine(AddressablesLoader.Load_Key_List(new List<object>() { "susu", "Susu_" }));


        //TaskRun();
        //TaskFromResult();

    }

    IEnumerator tempCheck()
    {
        yield return StartCoroutine(AddressablesLoader.LoadAndStoreResult("susu"));
        AddressablesLoader.tempCheckList();

        yield return new WaitForSeconds(2f);
        Debug.Log("2초 지남");
       object tempObj= AddressablesLoader.Find_Asset_In_AllList("susu");
        Instantiate(tempObj as GameObject, new Vector3(0f, 0f, 0f), Quaternion.identity);

        yield return new WaitForSeconds(2f);
        Debug.Log("2초 지남");


        if (tempObj!=null)
        {
            Debug.Log("삭제하러 들어옴");
            AddressablesLoader.tempCheckList_delete<UnityEngine.Object>((UnityEngine.Object)tempObj);
        }
        Debug.Log("끝남");

    }

    //동기 로드 후 델리게이트 사용
    public async Task Loder(string obj_name,Action action)
    {

        await AddressablesLoader.InitAssets_name_<GameObject>(obj_name);

       Debug.Log("다녀왔음");

        action();
      //  Instantiate(tempHPbar, pos.transform);
      //Debug.Log("생성함"+pos.name );
    }

    void tempAction()
    {
        Debug.Log("다녀왔음 다음에 실행되면 되겠다.");
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

        if(Input.GetKeyDown(KeyCode.A))
        {
            AddressablesLoader.Destroy_Obj(te);
        }
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddrTestScripts : MonoBehaviour
{

    public delegate void Complete_delegate(AsyncOperationHandle<GameObject> comp);
    public Action<AsyncOperationHandle<GameObject>> Complete_Aciton;

    public GameObject tempHPbar=null;
    // Start is called before the first frame update
    void Start()
    {
        //예외처리 테스트
        //StartCoroutine(AddressablesLoader.LoadGameObjectAndMaterial("Hpbar"));
        //StartCoroutine(AddressablesLoader.LoadGameObjectAndMaterial("Hpbar"));

        //델리게이트 테스트

        StartCoroutine(AddressablesLoader.LoadGameObjectAndMaterial("Hpbar", handle => Debug.Log("델리게이트"+handle.Result.name)));

      //  StartCoroutine(AddressablesLoader.LoadGameObjectAndMaterial("Demo"));

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

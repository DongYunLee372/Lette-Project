using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class LoadAddressableScene : MonoBehaviour
{

    [SerializeField] private AssetReference _scene;
    private AsyncOperationHandle<SceneInstance> handle;
    public GameObject camera;
    public DownloadProgress downloadProgressScript;
    public GameObject uiGameObject;
    long Downloadsize = 0;

    public GameObject PlayerInitPos;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayerInitPos = new GameObject();
        PlayerInitPos.transform.position = new Vector3(10, 0, 0);

        //BOSSROOM();
      // StartCoroutine(AddressablesController.Instance.Load_Name("PlayerCharacter", PlayerInitPos.transform));
        //AddressablesLoader.OnSceneAction("Demo");  //씬 로드 어드레서블

        StartCoroutine(BOSSROOM());

        // StartCoroutine(DownloadScene());
        // StartCoroutine(ScenesLoadMG.Instance.BossRoomScene());
    }
    IEnumerator BOSSROOM()
    {
        //v필요한 거 다 로드 시킨다음에
        yield return new WaitForSeconds(3);

        yield return StartCoroutine(AddressablesController.Instance.Load_Name("PlayerCharacter", PlayerInitPos.transform));

        //씬을 로드하고
       AddressablesLoader.OnSceneAction("Demo");  //씬 로드 어드레서블

       //연출같은거 필요하면 하고, 캔버스 ,카메라 비활성화

        yield return null;
    }


    IEnumerator DownloadScene()
    {
        var downloadScene = Addressables.LoadSceneAsync(_scene, LoadSceneMode.Additive);
        downloadScene.Completed += SceneDownloadComplete;
        Debug.LogError("Starting scene download");
        while (!downloadScene.IsDone)
        {
            var status = downloadScene.GetDownloadStatus();
            float progress = status.Percent;
            Debug.Log("progress" + progress);
            downloadProgressScript.downloadProgressInput =(int)(progress * 100);
            Debug.Log("downloadProgressScript"+downloadProgressScript.downloadProgressInput);

             yield return null;
        }

        Debug.LogError("Download Complete, starting next scene");
        downloadProgressScript.downloadProgressInput = 100;

    }

    private void SceneDownloadComplete(AsyncOperationHandle<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance> _handle)
    {

        if (_handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.LogError(_handle.Result.Scene.name + "successfully loaded");
            camera.SetActive(false);
            uiGameObject.SetActive(false);
            handle = _handle;

         

            StartCoroutine(UnloadScene());
        }
    }

    private IEnumerator UnloadScene()
    {
        yield return new WaitForSeconds(10);
        Addressables.UnloadSceneAsync(handle, true).Completed += op =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                camera.SetActive(true);
                uiGameObject.SetActive(true);
                Debug.LogError("Successfully unloaded the scene");
            }

        };
        yield return new WaitForSeconds(5);
        StartCoroutine(DownloadScene());
    }


    //IEnumerator DownloadScene()
    //{
    //    var downloadScene = Addressables.LoadSceneAsync(_scene, LoadSceneMode.Additive);
    //    downloadScene.Completed += SceneDownloadComplete;
    //    Debug.LogError("Starting scene download");
    //    while (!downloadScene.IsDone)
    //    {
    //        var status = downloadScene.GetDownloadStatus();
    //        float progress = status.Percent;
    //        downloadProgressScript.downloadProgressInput = (int)(progress * 100);
    //        yield return null;
    //    }

    //    if (downloadScene.IsDone)
    //    {
    //        yield return new WaitForSeconds(3);
    //    }

    //    Debug.LogError("Download Complete, starting next scene");
    //    downloadProgressScript.downloadProgressInput = 100;

    //}

    //private void SceneDownloadComplete(AsyncOperationHandle<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance> _handle)
    //{

    //    if (_handle.Status==AsyncOperationStatus.Succeeded)
    //    {
    //        Debug.LogError(_handle.Result.Scene.name + "successfully loaded");
    //        camera.SetActive(false);
    //        uiGameObject.SetActive(false);
    //        handle = _handle;

    //        StartCoroutine(UnloadScene());
    //    }
    //}

    //private IEnumerator UnloadScene()
    //{
    //    yield return new WaitForSeconds(10);
    //    Addressables.UnloadSceneAsync(handle, true).Completed += op =>
    //       {
    //           if (op.Status == AsyncOperationStatus.Succeeded)
    //           {
    //               camera.SetActive(true);
    //               uiGameObject.SetActive(true);
    //               Debug.LogError("Successfully unloaded the scene");
    //           }

    //       };
    //    yield return new WaitForSeconds(5);
    //    StartCoroutine(DownloadScene());
    //}


}

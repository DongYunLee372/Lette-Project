using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadAddressableScene : MonoBehaviour
{

    [SerializeField] private AssetReference _scene;
    private AsyncOperationHandle<SceneInstance> handle;
    public GameObject camera;
    public DownloadProgress downloadProgressScript;
    public GameObject uiGameObject;
    public GameObject PlayerCanvas;
    public GameObject EnemyCanvas;

    float Downloadsize = 0;

    public Slider slider;  //로딩 슬라이더 바 (임시,,?)
    public float slider_show;  //다운로드 얼마나 됐는지


    public GameObject PlayerInitPos;
    public GameObject BossPos;
    public GameObject MapPos;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        BossPos = new GameObject();
        BossPos.transform.position = new Vector3(2.5f, -3.29f, 80.0f);  //보스위치임.
        PlayerInitPos = new GameObject();
        PlayerInitPos.transform.position = new Vector3(1.21f, -4.572893f, 56.09f);  //캐릭터위치.
        MapPos = new GameObject();
        MapPos.transform.position = new Vector3(0f, -10f, 0f);  //맵.

       PlayerCanvas.SetActive(false);
        EnemyCanvas.SetActive(false);

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

        yield return StartCoroutine(AddressablesLoader.LoadGameObjectAndMaterial("Hpbar"));
        slider_show = 0.1f;
        yield return StartCoroutine(AddressablesLoader.LoadGameObjectAndMaterial("FriendPanel"));
        slider_show = 0.2f;
        yield return StartCoroutine(AddressablesLoader.LoadGameObjectAndMaterial("Inven"));
        slider_show = 0.3f;
        yield return StartCoroutine(AddressablesLoader.LoadGameObjectAndMaterial("Bosshpbar"));
        slider_show = 0.5f;
        //yield return StartCoroutine(AddressablesController.Instance.Load_Name("Boss", PlayerInitPos.transform));


      // yield return StartCoroutine(AddressablesController.Instance.Load_Name("Long_Hall", MapPos.transform));


        slider_show = 0.7f;

        //   yield return StartCoroutine(AddressablesController.Instance.Load_Name("PlayerCharacter", PlayerInitPos.transform));
        //씬을 로드하고
         AddressablesLoader.OnSceneAction("Demo");  //씬 로드 어드레서블

        //연출같은거 필요하면 하고, 캔버스 ,카메라 비활성화

       // yield return new WaitForSeconds(3);
        slider_show = 1f;
        yield return StartCoroutine(AddressablesController.Instance.Load_Name("PlayerCharacter", PlayerInitPos.transform));
        yield return StartCoroutine(CharacterCreate.Instance.CreateBossMonster_(EnumScp.MonsterIndex.mon_06_01, BossPos.transform));

        yield return new WaitForSeconds(1);

        //SceneManager.LoadScene("Demo");

        PlayerCanvas.SetActive(true);
        EnemyCanvas.SetActive(true);

        camera.SetActive(false);
        uiGameObject.SetActive(false);

        yield return null;
    }

    private void Update()
    {
        if(Downloadsize!= slider_show)
        {
            slider.value = slider_show;
            Downloadsize = slider_show;
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInfo
{
    public GameObject obj;
    public int index;
    public string path;
    public bool active = false;
    public bool Instance = false;
}
public class UIManager : Singleton<UIManager>
{
    private UIInfo uiinfo = new UIInfo();

    public List<UIInfo> info = new List<UIInfo>();
    public List<GameObject> canvas;


    public enum CANVAS_NUM
    {
        player_cavas = 0,
        enemy_canvas,
        sdf
    }

    public GameObject Prefabsload(string name, CANVAS_NUM x , Transform a = null)
    {
        bool same = false;
        for (int i = 0; i < info.Count; i++)
        {
            //Debug.Log(info[i].path);
            if (info[i].path == name && info[i].path != "Hpbar")
            {
                same = true;
            }
        }
        if (same)
        {
            Debug.Log("이미있습니다");
        }
        else
        {
            UIInfo tmp = new UIInfo();
            GameObject obj = AddressablesController.Instance.find_Asset_in_list(name);
            tmp.obj = Instantiate(obj, canvas[(int)x].transform);
            tmp.obj.transform.SetParent(canvas[(int)x].transform);
            tmp.path = name;
            tmp.obj.name = name;
            tmp.active = true;
            info.Add(tmp);
            info[info.Count - 1].index = info.Count - 1;
            return info[info.Count-1].obj;
        }
            return null;
    }
    // (예 아니오 팝업 ) 쇼메세지 .
    // 쇼메세지에서 인수를 받아서 콜백을한다 . 
    // 마우스커서 컨트롤 
    public void Show(string path)
    {
        for (int i = 0; i < info.Count; i++)
        {
            if (info[i].path == path)
            {
                Debug.Log(info[i].path);
                info[i].obj.SetActive(true);
                info[i].active = true;
            }
        }
    }
    public void Hide(string path)
    {
        for (int i = 0; i < info.Count; i++)
        {
            if (info[i].path == path)
            {
                Debug.Log(info[i].path);
                info[i].obj.SetActive(false);
                info[i].active = false;
            }
        }
    }
    public void Remove(string path)
    {
        for (int i = 0; i < info.Count; i++)
        {
            if (info[i].path == path && info[i].active == true)
            {
                Debug.Log(info.Count);
                Destroy(info[i].obj);
                info.Remove(info[i]);
                i = 0;
                continue;
            }
        }
    }
    public void Remove(GameObject obj)
    {
        for (int i = 0; i < info.Count; i++)
        {
            if (info[i].obj == obj && info[i].active == true)
            {
                Debug.Log(info.Count);
                Destroy(info[i].obj);
                info.Remove(info[i]);
                i = 0;
                continue;
            }
        }
    }
    public void Canvasoff(CANVAS_NUM num)
    {
        canvas[(int)num].SetActive(false);
    }
    public void Canvason(CANVAS_NUM num)
    {
        canvas[(int)num].SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AddressablesLoader.LoadGameObjectAndMaterial("Hpbar"));
        StartCoroutine(AddressablesLoader.LoadGameObjectAndMaterial("FriendPanel"));
    }

    // Update is called once per frame
    void Update()
    {

    }
}

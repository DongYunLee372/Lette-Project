using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Canvas_Enum;
using Global_Variable;
using UnityEngine.UI;
public static class KeySetting 
{
    public static Dictionary<KeyAction, KeyCode> keys = new Dictionary<KeyAction, KeyCode>(); 

}
public class OnclickButton : MonoBehaviour
{
    
    [SerializeField]
    private string undo_uiname;
    [SerializeField]
    private string curr_uiname;

    public ButtonText buttontext;
    public List<Text> text;
    public int compltesettingcount = 0;
    [SerializeField]
    private KeyCode[] defaultkeys = new KeyCode[7];//{ KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D , KeyCode.Space , KeyCode.Mouse0 , KeyCode.Mouse1 }; //처음키 설정 .
    static KeyCode[] s_defautkeys = new KeyCode[7];
    [SerializeField]
    bool KeymapingCheck = false;
    [SerializeField]
    bool buttoncheck = false;
    private int key = -1;
    private void Awake()
    {
        //for(int i=0; i< (int)KeyAction.KEYCOUNT; i++)
        //{
        //    s_defautkeys[i] = defaultkeys[i]; //전역 변수 사용
        //}
        //KeymapingCheck = false;
        //Save_Optiondata loadData = SaveSystem.Load("save_001"); //캐릭터 아이디로 변경.
        //if (loadData == null)                                      //캐릭터가 없다면 디폴트셋팅.  
        //{
        //    KeymapingCheck = false;
        //    for (int i = 0; i < (int)KeyAction.KEYCOUNT; i++)
        //    {
        //        Debug.Log(i);
        //        KeySetting.keys.Add((KeyAction)i, defaultkeys[i]);
        //    }
        //}
        //else
        //{
        //    KeySetting.keys.Add((KeyAction.UP), loadData.up);
        //    KeySetting.keys.Add((KeyAction.DOWN), loadData.down);
        //    KeySetting.keys.Add((KeyAction.LEFT), loadData.left);
        //    KeySetting.keys.Add((KeyAction.RIGHT), loadData.right);
        //    KeySetting.keys.Add((KeyAction.ROOL), loadData.roll);
        //    KeySetting.keys.Add((KeyAction.ATTACK), loadData.attack);
        //    KeySetting.keys.Add((KeyAction.DEFENSE), loadData.defens);
        //    Debug.Log("로드하기");
        //}
    }
    public void DefaultSetting()
    {
        for (int i = 0; i < (int)KeyAction.KEYCOUNT; i++)
        {
            KeySetting.keys[(KeyAction)i] = s_defautkeys[i];      
        }
    }
    private void Update()
    {
     
        Updatetexts();
    }
    public void OkButton()
    {
       
        UIManager.Instance.Show(UIname.StartUI);
        UIManager.Instance.Hide(UIname.MainOption);
        
    }
    public void Settingcountup()
    {
        compltesettingcount++;
    }
    // Start is called before the first frame update
    public void b_Gamestart() //게임시작 
    {
        GameObject canvas = UIManager.Instance.Canvasreturn(CANVAS_NUM.start_canvas);
        canvas.GetComponent<MainOption>().GameStart = true;
        canvas.GetComponent<MainOption>().ShowOption = false;
        canvas.GetComponent<MainOption>().mainoption = false;
        UIManager.Instance.Hide(UIname.StartUI);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameData_Load.Instance.ChangeScene(Scenes_Stage.Stage1);
        SoundManager.Instance.bgmSource.Stop();

        UIManager.Instance.Prefabsload("Minimap", CANVAS_NUM.player_cavas);
       // GameData_Load.Instance.ChangeScene(Scenes_Stage.GameMenuEnd);
    }

    public void b_Restart() //재시작 
    {
        GameObject canvas = UIManager.Instance.Canvasreturn(CANVAS_NUM.start_canvas);
        canvas.GetComponent<MainOption>().GameStart = true;
        canvas.GetComponent<MainOption>().ShowOption = false;
        canvas.GetComponent<MainOption>().mainoption = false;
        GameData_Load.Instance.ChangeScene(Scenes_Stage.restart_Loading);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void b_Exitgame() //종료 
    {
        // GameData_Load.Instance.ChangeScene(Scenes_Stage.GameMenuEnd);
        Application.Quit();
    }

    public void b_Reverse()
    {
        GameObject canvas = UIManager.Instance.Canvasreturn(CANVAS_NUM.start_canvas);
        canvas.GetComponent<MainOption>().ReverseMouse = !canvas.GetComponent<MainOption>().ReverseMouse;
    }
    public void b_Autoevade()
    {
        GameObject canvas = UIManager.Instance.Canvasreturn(CANVAS_NUM.start_canvas);
        canvas.GetComponent<MainOption>().AutoeVade = !canvas.GetComponent<MainOption>().AutoeVade;
    }
    public void b_Lookon()
    {
        GameObject canvas = UIManager.Instance.Canvasreturn(CANVAS_NUM.start_canvas);
        canvas.GetComponent<MainOption>().LooKon = !canvas.GetComponent<MainOption>().LooKon;
    }
    public void Optionokbutton() //밖에있는 종료버튼 
    {
        GameObject canvas = UIManager.Instance.Canvasreturn(CANVAS_NUM.start_canvas);
        GameData_Load.Instance.ChangeScene(Scenes_Stage.GameMenuEnd);
        UIManager.Instance.RemoveAll();
        UIManager.Instance.Remove(UIname.StartUI);
        canvas.GetComponent<MainOption>().StartCoroutine("UI");

        //UIManager.Instance.Show(UIname.StartUI);
        //UIManager.Instance.Hide(UIname.MainOption);
        //UIManager.Instance.Hide(UIname.IngameOption);
        //SoundManager.Instance.bgmSource.GetComponent<AudioSource>().PlayOneShot(SoundManager.Instance.Bgm[0]);
 
       
        canvas.GetComponent<MainOption>().mainoption = true;

        //Save_Optiondata character = new Save_Optiondata(KeySetting.keys[(KeyAction)0], KeySetting.keys[(KeyAction)1], KeySetting.keys[(KeyAction)2], KeySetting.keys[(KeyAction)3],
        //KeySetting.keys[(KeyAction)4], KeySetting.keys[(KeyAction)5], KeySetting.keys[(KeyAction)6]);
        //SaveSystem.Save(character, "save_001");
        //buttoncheck = false;

        //실행창 종료
       // Application.Quit();
    }
    public void Option()
    {
        if (UIManager.Instance.Findobjbool(UIname.MainOption))   //옵션버튼 눌렀을때 옵션UI가 HIDE상태라면 
        {       
            UIManager.Instance.Show(UIname.MainOption);          //옵션버튼을 SHOW한다 
        }
        else    //오브젝트가 존재하지않으면 
        {
            UIManager.Instance.Prefabsload(UIname.MainOption, CANVAS_NUM.start_canvas);  //추가한다. 
        }
        UIManager.Instance.Hide(UIname.StartUI); //메인 UI는 HIDE시킨다 
        GameObject canvas = UIManager.Instance.Canvasreturn(CANVAS_NUM.start_canvas);
        canvas.GetComponent<MainOption>().GameStart = false;
        canvas.GetComponent<MainOption>().mainoption = true;
        buttoncheck = true;
    }
    
    public void Keysetting(int num)
    {
        key = num;
        KeymapingCheck = true;
    }
    private void OnGUI() 
    {
        if (!KeymapingCheck)
            return;
        Event keyEvent = Event.current;    
       
        if(keyEvent.isKey) // 키보드가 눌렸을경우에만 실행 
        {
            CheckKey(keyEvent.keyCode);      
            KeySetting.keys[(KeyAction)key] = keyEvent.keyCode;
            buttontext.Updatetexts();
            KeymapingCheck = false;
            key = -1;        
        }

        else if (keyEvent.isMouse) //마우스가 눌렸을경우에만 실행 
        {
            
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                CheckKey(KeyCode.Mouse0);
                KeySetting.keys[(KeyAction)key] = KeyCode.Mouse0;
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                CheckKey(KeyCode.Mouse1);
                KeySetting.keys[(KeyAction)key] = KeyCode.Mouse1;
            }
            buttontext.Updatetexts();
            KeymapingCheck = false;
            key = -1;
        }
    }

    private void CheckKey(KeyCode p_event)
    {
        for (int i = 0; i < (int)KeyAction.KEYCOUNT; i++)
        {
            if (p_event == KeySetting.keys[(KeyAction)i])
            {
                KeySetting.keys[(KeyAction)i] = KeyCode.None;
                break;
            }
        }
    }

    public void Updatetexts()
    {
        for (int i = 0; i < text.Count; i++)
        {
            if (KeySetting.keys[(KeyAction)i].ToString() == "None")
                text[i].text = "";
            else
                text[i].text = KeySetting.keys[(KeyAction)i].ToString();
        }
    }
}

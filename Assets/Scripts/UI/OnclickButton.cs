using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Canvas_Enum;
using Global_Variable;

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
    public int compltesettingcount = 0;
    [SerializeField]
    private KeyCode[] defaultkeys = new KeyCode[(int)KeyAction.KEYCOUNT];// { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D , KeyCode.Space , KeyCode.Mouse0 , KeyCode.Mouse1 }; //처음키 설정 .
    [SerializeField]
    bool KeymapingCheck = false;
    private int key = -1;
    private void Awake()
    {
        KeymapingCheck = false;
        for (int i =0; i<(int)KeyAction.KEYCOUNT; i++)
        {
            Debug.Log(i);
            KeySetting.keys.Add((KeyAction)i, defaultkeys[i]);
            
        }
    }
    public void Settingcountup()
    {
        compltesettingcount++;
    }
    // Start is called before the first frame update
    public void Gamestart()
    {
        Debug.Log("게임시작");
    }

    public void Restart()
    {

    }

    public void Exitgame()
    {

    }
    public void Optionokbutton()
    {
        UIManager.Instance.Show(UIname.StartUI);
        UIManager.Instance.Hide(UIname.OptionSetting);
    }
    public void Option()
    {
        if (UIManager.Instance.Findobjbool(UIname.OptionSetting))   //옵션버튼 눌렀을때 옵션UI가 HIDE상태라면 
        {       
            UIManager.Instance.Show(UIname.OptionSetting);          //옵션버튼을 SHOW한다 
        }
        else    //오브젝트가 존재하지않으면 
        {
            UIManager.Instance.Prefabsload(UIname.OptionSetting, CANVAS_NUM.start_canvas);  //추가한다. 
        }
        UIManager.Instance.Hide(UIname.StartUI); //메인 UI는 HIDE시킨다 
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
}

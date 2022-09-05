using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Canvas_Enum;

public static class KeySetting { public static Dictionary<KeyAction, KeyCode> keys = new Dictionary<KeyAction, KeyCode>(); }
public class OnclickButton : MonoBehaviour
{
    public ButtonText buttontext;
    public int compltesettingcount = 0;
    [SerializeField]
    private KeyCode[] defaultkeys = new KeyCode[] { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D }; //처음키 설정 .
    [SerializeField]
    bool KeymapingCheck = false;
    private int key = -1;
    private void Awake()
    {
        KeymapingCheck = false;
        for (int i =0; i<(int)KeyAction.KEYCOUNT; i++)
        {
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

    public void Option()
    {
        UIManager.Instance.Prefabsload("OptionSetting", CANVAS_NUM.start_canvas);
        UIManager.Instance.Hide("StartUI");
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
            KeySetting.keys[(KeyAction)key] = keyEvent.keyCode;
            buttontext.Updatetexts();
            KeymapingCheck = false;
            key = -1;
        }
    }

}

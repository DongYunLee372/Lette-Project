using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Canvas_Enum;
public class UITest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //UIManager.Instance.Canvasoff(CANVAS_NUM.enemy_canvas);
        //UIManager.Instance.Canvasoff(CANVAS_NUM.start_canvas);
        //UIManager.Instance.Canvasoff(CANVAS_NUM.player_cavas);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        { 
            UIManager.Instance.Prefabsload("StartUI", CANVAS_NUM.start_canvas);
            //UIManager.Instance.Prefabsload("OptionSetting", CANVAS_NUM.start_canvas);
        }

        if(Input.GetKey( KeySetting.keys[KeyAction.UP] ))
        {
            Debug.Log("위");
        }
        if (Input.GetKey(KeySetting.keys[KeyAction.DOWN]))
        {
            Debug.Log("아래");
        }
        if (Input.GetKey(KeySetting.keys[KeyAction.LEFT]))
        {
            Debug.Log("왼쪽");
        }
        if (Input.GetKey(KeySetting.keys[KeyAction.RIGHT]))
        {
            Debug.Log("오른쪽");
        }
        if (Input.GetKey(KeySetting.keys[KeyAction.ROOL]))
        {
            Debug.Log("구르기");
        }
        if (Input.GetKey(KeySetting.keys[KeyAction.ATTACK]))
        {
            Debug.Log("공격");
        }
        if (Input.GetKey(KeySetting.keys[KeyAction.DEFENSE]))
        {
            Debug.Log("방어");
        }
    }
}

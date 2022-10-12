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
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {     
            UIManager.Instance.Prefabsload("StartUI", CANVAS_NUM.start_canvas);
           // UIManager.Instance.Prefabsload("OptionSetting", CANVAS_NUM.enemy_canvas);
            
          //  CharacterCreate.Instantiate.
        }
  

       
    }
}

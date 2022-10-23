using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Canvas_Enum;
using UnityEngine.EventSystems;

public class UITest : MonoBehaviour
{
    public Transform transf;
    // Start is called before the first frame update
    void Start()
    {
        //transf.position = new Vector3(0.01f, 2.14f, -9f);
        //UIManager.Instance.Canvasoff(CANVAS_NUM.enemy_canvas);
        //UIManager.Instance.Canvasoff(CANVAS_NUM.start_canvas);
        //UIManager.Instance.Canvasoff(CANVAS_NUM.player_cavas);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
          
            //UIManager.Instance.Prefabsload("StartUI", CANVAS_NUM.start_canvas);

            StartCoroutine(CharacterCreate.Instance.CreateMonster_(EnumScp.MonsterIndex.mon_01_01, transf));
            Debug.Log(transf);
            // UIManager.Instance.Prefabsload("OptionSetting", CANVAS_NUM.enemy_canvas);

            //  CharacterCreate.Instantiate.
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {

            //UIManager.Instance.Prefabsload("StartUI", CANVAS_NUM.start_canvas);

            StartCoroutine(CharacterCreate.Instance.CreateBossMonster_(EnumScp.MonsterIndex.mon_01_01, transf));
            Debug.Log(transf);
            // UIManager.Instance.Prefabsload("OptionSetting", CANVAS_NUM.enemy_canvas);

            //  CharacterCreate.Instantiate.
        }


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform spawn;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            CameraManager.Instance.SetPlayerCamera(GameMG.Instance.Resource.Instantiate("PlayerCharacter", null));
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
          //  GameObject a=UIManager.Instance.Findobj("Bosshpbar");

           // a.GetComponent<Bosshpbar>().HitDamage(100);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

             UIManager.Instance.Prefabsload("Bosshpbar", UIManager.CANVAS_NUM.player_cavas);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("??");

            StartCoroutine(CharacterCreate.Instance.CreateMonster_(EnumScp.MonsterIndex.mon_01_01, spawn));
            //GameObject a=   UIManager.Instance.Prefabsload("Bosshpbar", UIManager.CANVAS_NUM.player_cavas);
        }
    }
}

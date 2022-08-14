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
        if (Input.GetKeyDown(KeyCode.F1))
        {
            UIManager.Instance.Prefabsload("Fire Demon-Yellow", UIManager.CANVAS_NUM.ex_skill);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            UIManager.Instance.Prefabsload("Test1", UIManager.CANVAS_NUM.ex_skill);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            UIManager.Instance.Prefabsload("Test2", UIManager.CANVAS_NUM.ex_skill);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            CameraManager.Instance.SetPlayerCamera(GameMG.Instance.Resource.Instantiate("PlayerCharacter", null));
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UIManager.Instance.Show("Fire Demon-Yellow");

            UIManager.Instance.Show("Test2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

            GameObject hpBar = UIManager.Instance.Prefabsload("FriendPanel", UIManager.CANVAS_NUM.ex_skill);
            // UIManager.Instance.Remove("Test2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("??");

            StartCoroutine(CharacterCreate.Instance.CreateMonster_(EnumScp.MonsterIndex.mon_01_01, spawn));
        }
    }
}

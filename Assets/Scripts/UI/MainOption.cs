using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainOption : MonoBehaviour
{
    public float Backgroundsound;
    public float Effectsound;
    public float Mousesensetive;
    public float Lightcontroll;

    public bool Reversemouse;
    public bool Autoevade;
    public bool Lookon;

    public bool GameStart;
    public bool ShowOption;
    public bool mainoption;
    private void Update()
    {
        if (mainoption)
            return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Ingame();
        }
      
    }

    public void Ingame()
    {
        if(ShowOption)
        {
            UIManager.Instance.Hide("IngameOption");
            ShowOption = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            return;
        }
        if(GameStart)
        {
            
            if (UIManager.Instance.Findobjbool("IngameOption"))
            {
                UIManager.Instance.Show("IngameOption");
                ShowOption = true;
            }
            else
            {
                UIManager.Instance.Prefabsload("IngameOption", Canvas_Enum.CANVAS_NUM.start_canvas);
                ShowOption = true;
            }
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    private void Start()
    {
       StartCoroutine("UI");
    }
    IEnumerator UI()
    {
        yield return new WaitForSeconds(2f);
        UIManager.Instance.Prefabsload("StartUI", Canvas_Enum.CANVAS_NUM.start_canvas); 
    }
}

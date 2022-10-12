using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
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
            return;
        }
        if(GameStart)
        {
            
            if (UIManager.Instance.Findobjbool("IngameOption"))
            {
                UIManager.Instance.Show("IngameOption");
            }
            else
            {
                UIManager.Instance.Prefabsload("IngameOption", Canvas_Enum.CANVAS_NUM.start_canvas);
            }

                ShowOption = true;
        }

        
    }
}

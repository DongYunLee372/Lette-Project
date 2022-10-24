using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Text : MonoBehaviour
{

    private GameObject Mycanvas;
    public Text text;
    public int myindex;
    // Start is called before the first frame update
    void Start()
    {
        Mycanvas = UIManager.Instance.Canvasreturn(Canvas_Enum.CANVAS_NUM.start_canvas);
    }

    // Update is called once per frame
    void Update()
    {
       if(myindex==0)
        {
           if( Mycanvas.GetComponent<MainOption>()._reversemouse )
            {
                text.text = "켜기";
            }
           else
            {
                text.text = "끄기";
            }
        }
        if (myindex == 1)
        {
            if (Mycanvas.GetComponent<MainOption>()._autoevad)
            {
                text.text = "켜기";
            }
            else
            {
                text.text = "끄기";
            }
        }
        if (myindex == 2)
        {
            if (Mycanvas.GetComponent<MainOption>()._lookon)
            {
                text.text = "켜기";
            }
            else
            {
                text.text = "끄기";
            }
        }
    }
}

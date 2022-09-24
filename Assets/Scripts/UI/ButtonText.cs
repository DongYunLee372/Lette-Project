using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Canvas_Enum;

public class ButtonText : MonoBehaviour
{
    public GameObject Paraentscanvas;
    public List<Text> texts;
    // Start is called before the first frame update
    void Start()
    {
        Paraentscanvas = UIManager.Instance.Canvasreturn(CANVAS_NUM.start_canvas);
        Paraentscanvas.GetComponent<OnclickButton>().buttontext= this.gameObject.GetComponent<ButtonText>();
        Paraentscanvas.GetComponent<OnclickButton>().text = texts;
        Updatetexts();
    }

    public void Updatetexts()
    {
        for (int i = 0; i < texts.Count; i++)
        {
            if (KeySetting.keys[(KeyAction)i].ToString() == "None")
                texts[i].text = "";
            else
                texts[i].text = KeySetting.keys[(KeyAction)i].ToString();
        }
    }
}

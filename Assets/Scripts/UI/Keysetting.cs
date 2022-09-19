using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Canvas_Enum;
public class Keysetting : MonoBehaviour
{
    public Button mybutton;
    private int count;
    public GameObject Paraentobj; 
    // Start is called before the first frame update
    void Start()
    {
        GameObject paranetobj = UIManager.Instance.Canvasreturn(CANVAS_NUM.start_canvas);
        count = paranetobj.GetComponent<OnclickButton>().compltesettingcount;
        mybutton.onClick.AddListener(delegate { paranetobj.GetComponent<OnclickButton>().Keysetting(count); });
        paranetobj.GetComponent<OnclickButton>().Settingcountup();
        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_HP_3 : MonoBehaviour
{
    public Rect pos;
    public GameObject my;
    public Image Boss_HP_2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 temp = new Vector3();
        temp = my.transform.position;
        temp.x = Boss_HP_2.fillAmount * 800 -400 + 960;
        my.transform.position = temp;
    }
}

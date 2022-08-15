using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bosshpbar : MonoBehaviour
{

    public Image Bosshp;
    public Text t_Bosshp;
    public Text t_Bossname;
    public float Maxhp;
    public float Curhp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Bosshp.fillAmount = 0;
    }
}

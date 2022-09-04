using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Canvas_Enum;
public class Bosshpbar : MonoBehaviour
{

    public Image Bosshp;
    public Text t_Bosshp;
    public Text t_Bossname;
    public float Maxhp;
    public float Curhp;

    public Bosshpbar myhpbar;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetHpbar(float p_hp,string bossname)
    {
        t_Bossname.text = bossname;
        t_Bosshp.text = "HP " + p_hp.ToString() + "/" + p_hp.ToString();
        Maxhp = p_hp;
        Curhp = p_hp;
       UIManager.Instance.Prefabsload("Bosshpbar", CANVAS_NUM.enemy_canvas);


        

    }

    public void HitDamage(float curhp)
    {

        Curhp = curhp;
        t_Bosshp.text = "HP " + Curhp.ToString() + "/" + Maxhp.ToString();
        Bosshp.fillAmount = Curhp / Maxhp;
    }
}

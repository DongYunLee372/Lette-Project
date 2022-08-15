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

    public Bosshpbar myhpbar;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetHpbar(float p_hp)
    {
        t_Bosshp.text = "HP " + p_hp.ToString() + "/" + p_hp.ToString();
        Maxhp = p_hp;
        Curhp = p_hp;
       UIManager.Instance.Prefabsload("Bosshpbar", UIManager.CANVAS_NUM.player_cavas);


        

    }

    public void HitDamage(float curhp)
    {

        Curhp = curhp;
        t_Bosshp.text = "HP " + Curhp.ToString() + "/" + Maxhp.ToString();
        Bosshp.fillAmount = Curhp / Maxhp;
    }
}

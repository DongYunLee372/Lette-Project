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

    }

    public void SetHpbar(float p_hp)
    {
        t_Bosshp.text = "HP " + p_hp.ToString() + "/" + p_hp.ToString();
        GameObject hpBar = UIManager.Instance.Prefabsload("Bosshpbar", UIManager.CANVAS_NUM.player_cavas);
        var _hpbar = hpBar.GetComponent<Bosshpbar>();
        _hpbar.Maxhp = p_hp;
        _hpbar.Curhp = p_hp;
        
        Bosshp.fillAmount = _hpbar.Maxhp / _hpbar.Maxhp;

    }

    public void HitDamage(float curhp)
    {
        Curhp = curhp;
        t_Bosshp.text = "HP " + Curhp.ToString() + "/" + Maxhp.ToString();
        Bosshp.fillAmount = Curhp / Maxhp;
    }
}

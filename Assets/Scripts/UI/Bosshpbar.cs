using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Canvas_Enum;
public class Bosshpbar : MonoBehaviour
{
    [SerializeField]
    private GameObject Myobj;
    public Image Bosshp;
    public Text t_Bossname;
    public float Maxhp;
    public float Curhp;
    public Bosshpbar myhpbar;
    public Battle_Character battle_Character;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        Bosshp.fillAmount = battle_Character.cur_HP / Maxhp;
        if (Bosshp.fillAmount <= 0)
        {
            UIManager.Instance.Remove(Myobj);
            Debug.Log("나죽음");
        }
    }

    public Bosshpbar SetHpbar(float p_hp,string bossname,Battle_Character p_battle)
    {

       GameObject hpbar= UIManager.Instance.Prefabsload("Boss_HP", CANVAS_NUM.enemy_canvas);
       
        var _hpbar = hpbar.GetComponent<Bosshpbar>();
        //  hpBar.transform.SetParent(enemyHpBarCanvas.transform);
        _hpbar.Myobj = hpbar;
        _hpbar.Maxhp = p_hp;
        _hpbar.Curhp = p_hp;
        _hpbar.battle_Character = p_battle;
        var _test = hpbar.GetComponent<Image>();
        //_hpbar.Bosshp = _test;
        myhpbar = _hpbar;

        return myhpbar;
    }

    public void HitDamage(float curhp)
    {

        Curhp = curhp;
        Bosshp.fillAmount = Curhp / Maxhp;
    }
}

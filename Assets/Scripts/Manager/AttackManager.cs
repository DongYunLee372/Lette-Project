using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : BaseComponent
{    
    public void AttackMana(AnimationController animator, string AniName, float time)
    {
        animator.Play(AniName, time);
    }    
    public void CreateEffect(GameObject effect , Transform EffectPosRot , float destroyTime , float damage)
    {
        GameObject effectobj;

        effectobj = GameObject.Instantiate(effect);
        effectobj.transform.position = EffectPosRot.position;
        effectobj.transform.rotation = EffectPosRot.rotation;
        effectobj.transform.parent = this.transform;

        effectobj.GetComponent<ColliderEventDamage>().DamageSetting(damage);
        StartCoroutine(Cor_TimeCounter(destroyTime , effectobj));
               
    }
    IEnumerator Cor_TimeCounter(float time , GameObject obj)
    {
        float starttime = Time.time;

        while (true)
        {
            if ((Time.time - starttime) >= time)
            {
                Destroy(obj);
                yield break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    
    public override void InitComtype()
    {
        p_comtype = EnumTypes.eComponentTypes.AttackCom;
    }

    void Start()
    {
       
    }


    void Update()
    {

    }

//    foreach (var item in ComboEffectDic)
//        {
//            if (item.Key == Id)
//            {
//                RemoveDic(item.Value, Id);
//}
//        }
}

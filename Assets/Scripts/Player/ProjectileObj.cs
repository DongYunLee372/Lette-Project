using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileObj : MonoBehaviour
{
    //데미지 판정 주기
    public float DamageTime;

    public string MonsterTag;

    public SphereColl coll;

    public void InitSetting(float _damageTime, string _monsterTag, SphereColl _coll)
    {

    }

}

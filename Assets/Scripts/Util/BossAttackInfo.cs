using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BossAttack Data", menuName = "Scriptable Object/BossAttackInfo", order = int.MaxValue)]

public class BossAttackInfo : ScriptableObject
{
    //skill_ID skill_Type  skill_Targetyp skill_Range skill_dmg skill_MP    skill_Cool skill_atkTime   skill_continueTime skill_AtkCount  skill_DiffObj skill_ThrowObj

    [SerializeField]
    private string skill_Name_En; //몬스터 영어명
    public string P_skill_Name_En { get { return skill_Name_En; } set { skill_Name_En = value; } }

    [SerializeField]
    private string skill_Name_Kor;
    public string P_skill_Name_Kor { get { return skill_Name_Kor; } set { skill_Name_Kor = value; } }

    [SerializeField]
    private string mon_Index; 
    public string P_mon_Index { get { return mon_Index; } set { mon_Index = value; } }

    [SerializeField]
    private string skill_ID; 
    public string P_skill_ID { get { return skill_ID; } set { skill_ID = value; } }

    [SerializeField]
    private int skill_Type; 
    public int P_skill_Type { get { return skill_Type; } set { skill_Type = value; } }

    [SerializeField]
    private int skill_Targetyp; 
    public int P_skill_Targetyp { get { return skill_Targetyp; } set { skill_Targetyp = value; } }

    [SerializeField]
    private int skill_Range; 
    public int P_skill_Range { get { return skill_Range; } set { skill_Range = value; } }

    [SerializeField]
    private int skill_dmg; 
    public int P_skill_dmg { get { return skill_dmg; } set { skill_dmg = value; } }

    [SerializeField]
    private int skill_MP;
    public int P_skill_MP { get { return skill_MP; } set { skill_MP = value; } }

    [SerializeField]
    private int skill_Cool;
    public int P_skill_Cool { get { return skill_Cool; } set { skill_Cool = value; } }

    [SerializeField]
    private int skill_atkTime;
    public int P_skill_atkTime { get { return skill_atkTime; } set { skill_atkTime = value; } }

    [SerializeField]
    private int skill_continueTime;
    public int P_skill_continueTime { get { return skill_continueTime; } set { skill_continueTime = value; } }

    [SerializeField]
    private int skill_AtkCount;
    public int P_skill_AtkCount { get { return skill_AtkCount; } set { skill_AtkCount = value; } }

    [SerializeField]
    private int skill_DiffObj;
    public int P_skill_DiffObj { get { return skill_DiffObj; } set { skill_DiffObj = value; } }

    [SerializeField]
    private int skill_ThrowObj;
    public int P_skill_ThrowObj { get { return skill_ThrowObj; } set { skill_ThrowObj = value; } }
}

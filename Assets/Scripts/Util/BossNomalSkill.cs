using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BossNomal Data", menuName = "Scriptable Object/BossNomalAttack Data", order = int.MaxValue)]

public class BossNomalSkill : ScriptableObject
    
{
    [SerializeField]
    private string skill_ID; //몬스터 영어명
    public string P_skill_ID { get { return skill_ID; } set { skill_ID = value; } }

    [SerializeField]
    private string skill_Name_Kor; 
    public string P_skill_Name_Kor { get { return skill_Name_Kor; } set { skill_Name_Kor = value; } }

    [SerializeField]
    private string mon_Index; //몬스터 영어명
    public string P_mon_Index { get { return mon_Index; } set { mon_Index = value; } }

    [SerializeField]
    private string skill_Name_En; //몬스터 영어명
    public string P_skill_Name_En { get { return skill_Name_En; } set { skill_Name_En = value; } }

    [SerializeField]
    private int skill_Type; //몬스터 영어명
    public int P_skill_Type { get { return skill_Type; } set { skill_Type = value; } }

    [SerializeField]
    private int skill_Targetyp; //몬스터 영어명
    public int P_skill_Targetyp { get { return skill_Targetyp; } set { skill_Targetyp = value; } }

    [SerializeField]
    private string Group_Index; //몬스터 영어명
    public string P_Group_Index { get { return Group_Index; } set { Group_Index = value; } }

    [SerializeField]
    private string Group_Motion_Count; //몬스터 영어명
    public string P_Group_Motion_Count { get { return Group_Motion_Count; } set { Group_Motion_Count = value; } }

    
    [SerializeField]
    private int skill_Range; //몬스터 영어명
    public int P_skill_Range { get { return skill_Range; } set { skill_Range = value; } }


    [SerializeField]
    private int skill_dmg; //몬스터 영어명
    public int P_skill_dmg { get { return skill_dmg; } set { skill_dmg = value; } }

    [SerializeField]
    private int skill_MP; //몬스터 영어명
    public int P_skill_MP { get { return skill_MP; } set { skill_MP = value; } }
    

    [SerializeField]
    private int skill_Groggy; //몬스터 영어명
    public int P_skill_Groggy { get { return skill_Groggy; } set { skill_Groggy = value; } }

    [SerializeField]
    private int skill_atkTime; //몬스터 영어명
    public int P_skill_atkTime { get { return skill_atkTime; } set { skill_atkTime = value; } }

    [SerializeField]
    private int skill_continueTime; //몬스터 영어명
    public int P_skill_continueTime { get { return skill_continueTime; } set { skill_continueTime = value; } }

    [SerializeField]
    private int skill_AtkCount; //몬스터 영어명
    public int P_skill_AtkCount { get { return skill_AtkCount; } set { skill_AtkCount = value; } }

    [SerializeField]
    private int skill_DiffObj;
    public int P_skill_DiffObj { get { return skill_DiffObj; } set { skill_DiffObj = value; } }

    [SerializeField]
    private int skill_ThrowObj;
    public int P_skill_ThrowObj { get { return skill_ThrowObj; } set { skill_ThrowObj = value; } }
}

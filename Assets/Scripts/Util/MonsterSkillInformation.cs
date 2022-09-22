using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterSkill Data", menuName = "Scriptable Object/MonsterSkill Data", order = int.MaxValue)]
public class MonsterSkillInformation : Data
{
    [SerializeField]
    private string skill_Name_En; //몬스터 영어명
    public string P_skill_Name_En { get { return skill_Name_En; } set { skill_Name_En = value; } }
   
    [SerializeField]
    private string mon_Index; //몬스터 번호
    public string P_mon_Index { get { return mon_Index; } set { mon_Index = value; } }
    [SerializeField]
    private string skill_Name_Kor; //몬스터 한글명
    public string P_skill_Name_Kor { get { return skill_Name_Kor; } set { skill_Name_Kor = value; } }
    
    [SerializeField]
    private int skill_ID; //몬스터 아이디
    public int P_skill_ID { get { return skill_ID; } set { skill_ID = value; } }
    [SerializeField]
    private int skill_Type; //몬스터 타입
    public int P_skill_Type { get { return skill_Type; } set { skill_Type = value; } }
    [SerializeField]
    private int skill_Targetyp; //몬스터 타겟 타입
    public int P_skill_Targetyp { get { return skill_Targetyp; } set { skill_Targetyp = value; } }

    [SerializeField]
    private int skill_Range; //몬스터 스킬 사용 범위
    public int P_skill_Range { get { return skill_Range;} set { skill_Range = value; } }

    [SerializeField]
    private int skill_Dmg; //몬스터 데미지
    public int P_skill_dmg { get { return skill_Dmg; } set { skill_Dmg = value; } }

    [SerializeField]
    private int skill_MP; //몬스터 스킬 마나
    public int P_skill_MP { get { return skill_MP; } set { skill_MP = value; } }

    [SerializeField]
    private int skill_Cool; //몬스터 스킬 쿨타임
    public int P_skill_Cool { get { return skill_Cool; } set { skill_Cool = value; } }

    [SerializeField]
    private float skill_atkTime; //몬스터 공격 판정시간
    public float P_skill_atkTime { get { return skill_atkTime; } set { skill_atkTime = value; } }

    [SerializeField]
    private int skill_continueTime; //몬스터 스킬 지속시간
    public int P_skill_continueTime { get { return skill_continueTime; } set { skill_continueTime = value; } }

    [SerializeField]
    private int skill_AtkCount; //몬스터 스킬 피격횟수
    public int P_skill_AtkCount { get { return skill_AtkCount; } set { skill_AtkCount = value; } }

    [SerializeField]
    private int skill_DiffObj; //몬스터 스킬 다른 객체 생성여부
    public int P_skill_DiffObj { get { return skill_DiffObj; } set { skill_DiffObj = value; } }

    [SerializeField]
    private int skill_ThrowObj; //몬스터 스킬 투사체 생성여부
    public int P_skill_ThrowObj { get { return skill_ThrowObj; } set { skill_ThrowObj = value; } }

   

}

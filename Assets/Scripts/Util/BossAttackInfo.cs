using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BossAttack Data", menuName = "Scriptable Object/BossAttackInfo", order = int.MaxValue)]

public class BossAttackInfo : MonoBehaviour
{
    [SerializeField]
    private string skill_Name_En; //몬스터 영어명
    public string P_skill_Name_En { get { return skill_Name_En; } set { skill_Name_En = value; } }

    [SerializeField]
    private string skill_Name_Kor;
    public string P_skill_Name_Kor { get { return skill_Name_Kor; } set { skill_Name_Kor = value; } }

    [SerializeField]
    private string mon_Index; //몬스터 영어명
    public string P_mon_Index { get { return mon_Index; } set { mon_Index = value; } }

    [SerializeField]
    private string Group_Index; //몬스터 영어명
    public string P_Group_Index { get { return Group_Index; } set { Group_Index = value; } }

    [SerializeField]
    private int Atk_Skill1; //몬스터 영어명
    public int P_Atk_Skill1 { get { return Atk_Skill1; } set { Atk_Skill1 = value; } }

    [SerializeField]
    private int Atk_Skill2; //몬스터 영어명
    public int P_Atk_Skill2 { get { return Atk_Skill2; } set { Atk_Skill2 = value; } }

    [SerializeField]
    private int Atk_Skill3; //몬스터 영어명
    public int P_Atk_Skill3 { get { return Atk_Skill3; } set { Atk_Skill3 = value; } }

    [SerializeField]
    private int skill_Range; //몬스터 영어명
    public int P_skill_Range { get { return skill_Range; } set { skill_Range = value; } }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Monster Data", menuName = "Scriptable Object/Monster Data", order = int.MaxValue)]
public class MonsterInformation : Data
{
    [SerializeField]
    private string mon_nameEng; //몬스터 영어명
    public string P_mon_nameeng { get { return mon_nameEng; } set { mon_nameEng = value; } }

    [SerializeField]
    private int Number;
    public int P_Number { get { return Number; } set { Number = value; } }
    [SerializeField]
    private string mon_Index; //몬스터 번호
    public string P_mon_Index { get { return mon_Index; } set { mon_Index = value; } }
    [SerializeField]
    private string mon_nameKor; //몬스터 한국명
    public string P_mon_nameKor { get { return mon_nameKor; } set { mon_nameKor = value; } }

    
    [SerializeField]
    private int mon_Default; //몬스터 등급
    public int P_mon_Default { get { return mon_Default; } set { mon_Default = value; } }

    [SerializeField]
    private int mon_Type; //몬스터 타입
    public int P_mon_Type { get { return mon_Type; } set { mon_Type = value; } }
    [SerializeField]
    private int mon_Position; //몬스터 위치
    public int P_mon_Position { get { return mon_Position; } set { mon_Position = value; } }

    [SerializeField]
    private int mon_MaxHP; //몬스터 체력
    public int P_mon_MaxHP { get { return mon_MaxHP; } set { mon_MaxHP = value; } }

    [SerializeField]
    private int mon_Atk; //몬스터 공격력
    public int P_mon_Atk { get { return mon_Atk; } set { mon_Atk = value; } }

    [SerializeField]
    private int mon_Def; //몬스터 방어력
    public int P_mon_Def { get { return mon_Def; } set { mon_Def = value; } }

    [SerializeField]
    private int mon_moveSpeed; //몬스터 스피드
    public int P_mon_moveSpeed { get { return mon_moveSpeed; } set { mon_moveSpeed = value; } }

    [SerializeField]
    private int mon_Balance; //몬스터 균형게이지
    public int P_mon_Balance { get { return mon_Balance; } set { mon_Balance = value; } }


    //mon_CloseAtk mon_FarAtk  mon_SpecialAtk mon_regenMP dieDelay drop_Reward

    [SerializeField]
    private int mon_MaxMp; //몬스터 최대마나
    public int P_mon_MaxMp { get { return mon_MaxMp; } set { mon_MaxMp = value; } }

    [SerializeField]
    private int mon_CloseAtk; //몬스터 마나 회복량
    public int P_mon_CloseAtk { get { return mon_CloseAtk; } set { mon_CloseAtk = value; } }

    [SerializeField]
    private int mon_FarAtk; 
    public int P_mon_FarAtk { get { return mon_FarAtk; } set { mon_FarAtk = value; } }

    [SerializeField]
    private string mon_SpecialAtk; //몬스터 마나 회복량
    public string P_mon_SpecialAtk { get { return mon_SpecialAtk; } set { mon_SpecialAtk = value; } }

    [SerializeField]
    private int mon_regenMP; //몬스터 마나 회복량
    public int P_mon_regenMP { get { return mon_regenMP; } set { mon_regenMP = value; } }


    [SerializeField]
    private int dieDelay; //몬스터 사망딜레이
    public int P_dieDelay { get { return dieDelay; } set { dieDelay = value; } }

    [SerializeField]
    private int drop_Reward; //몬스터 보상
    public int P_drop_Reward{ get { return drop_Reward; } set { drop_Reward = value; } }

    

    

   
}

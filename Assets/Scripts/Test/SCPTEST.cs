using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCPTEST : MonoBehaviour
{

    //테스트용 몬스터 스크립트 

    public MonsterInformation data;
    public MonsterSkillInformation data2;
    public CharacterInformation pdata;
    public DataLoad_Save TestDataLoad;
    public MonsterTargetInformation target;

    

    void Start()
    {
        
        
        //data = DataLoad_Save.Instance.Get_MonsterDB(EnumScp.MonsterIndex.mon_06_01);
        //pdata = DataLoad_Save.Instance.Get_PlayerDB(EnumScp.PlayerDBIndex.Level1);
        //data2 = DataLoad_Save.Instance.Get_MonsterSkillDB(EnumScp.MonsterSkill.mon_05_01_3);
        //target = DataLoad_Save.Instance.Get_MonsterTargetDB(EnumScp.MonsterTarget.ID33330212);


        Debug.Log("아라라" + data2.P_skill_Name_En);
        Debug.Log("아라라" + target.P_mon_Range);
        //Debug.Log("오라라" + data.P_mon_nameeng);
        //Debug.Log(pdata.P_player_Atk1);
        //Debug.Log(StaticClass.Add);
        //Debug.Log(StaticClass.ADD);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

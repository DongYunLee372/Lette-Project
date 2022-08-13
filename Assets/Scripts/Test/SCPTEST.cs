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


    

    void Start()
    {
        
        data = ScriptableObject.CreateInstance<MonsterInformation>();
        data = DataLoad_Save.Instance.Get_MonsterDB(EnumScp.MonsterIndex.mon_06_01);
        pdata = DataLoad_Save.Instance.Get_PlayerDB(EnumScp.PlayerDBIndex.Level1);
        data2 = DataLoad_Save.Instance.Get_MonsterSkillDB(EnumScp.MonsterSkill.mon_06_07);



        Debug.Log(data2.P_skill_Name_En);
        Debug.Log(pdata.P_player_HP);
        //Debug.Log(StaticClass.Add);
        //Debug.Log(StaticClass.ADD);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

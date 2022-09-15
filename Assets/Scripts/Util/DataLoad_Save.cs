using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//데이터 적용 자동화 (자동 업데이트)
// object 수정 데이터 박싱 언박싱 문제

public class DataLoad_Save : MySingleton<DataLoad_Save>
{
    Dictionary<string, MonsterInformation> MonsterDB_List;
    Dictionary<string, CharacterInformation> PlayerDB_List;
    Dictionary<string, MonsterSkillInformation> MonsterSkillDB_List;
    Dictionary<string, MonsterTargetInformation> MonsterTargetDB_List;
    Dictionary<string, BossAttackInfo> BossAttackInfoDB_List;
    Dictionary<string, BossNomalSkill> BossNomalSkillDB_List;



    private void Awake()
    {       
        LoadFile.Read<MonsterInformation>(out MonsterDB_List);
        LoadFile.Read<CharacterInformation>(out PlayerDB_List);
        LoadFile.Read<MonsterSkillInformation>(out MonsterSkillDB_List);
        LoadFile.Read<MonsterTargetInformation>(out MonsterTargetDB_List);
        LoadFile.Read<BossAttackInfo>(out BossAttackInfoDB_List);
        LoadFile.Read<BossNomalSkill>(out BossNomalSkillDB_List);
    }
    public BossAttackInfo Get_BossAttackDB(string index)
    {
        BossAttackInfo testData = ScriptableObject.CreateInstance<BossAttackInfo>();
        testData = BossAttackInfoDB_List[index];
        return testData;

    }
    public BossNomalSkill Get_BossSkillDB(string index)
    {
        BossNomalSkill testData = ScriptableObject.CreateInstance<BossNomalSkill>();
        testData = BossNomalSkillDB_List[index];
        return testData;

    }
    public MonsterInformation Get_MonsterDB(string index)
    {
        MonsterInformation testData = ScriptableObject.CreateInstance<MonsterInformation>();
        testData = MonsterDB_List[index];
        return testData;
        
    }
    public CharacterInformation Get_PlayerDB(string index)
    {
        CharacterInformation testData = ScriptableObject.CreateInstance<CharacterInformation>();
        testData = PlayerDB_List[index];
        return testData;
    }
    public MonsterSkillInformation Get_MonsterSkillDB(string index)
    {
        MonsterSkillInformation testData = ScriptableObject.CreateInstance<MonsterSkillInformation>();
        testData = MonsterSkillDB_List[index];
        return testData;
    }
    public MonsterTargetInformation Get_MonsterTargetDB(string index)
    {
        MonsterTargetInformation testData = ScriptableObject.CreateInstance<MonsterTargetInformation>();
        testData = MonsterTargetDB_List[index];
        return testData;
    }
    

    

    
    
}

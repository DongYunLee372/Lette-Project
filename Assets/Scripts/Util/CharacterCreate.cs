using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreate : Singleton<CharacterCreate>
{

    //  public DataLoad_Save TestDataLoad;



    void Start()
    {
        DataLoad_Save.Instance.Init();

    }

    // Update is called once per frame
    void Update()
    {

    }


    //어드레서블 수정
    //public void CreateMonster_(EnumScp.MonsterIndex p_index, Transform trans)
    //{
    //    MonsterInformation data = ScriptableObject.CreateInstance<MonsterInformation>();
    //    data = DataLoad_Save.Instance.Get_MonsterDB(p_index);

    //    GameObject a = Resources.Load<GameObject>(StaticClass.Prefabs + "Skeleton_Knight");
    //    a.GetComponent<Battle_Character>().Stat_Initialize(data);

    //    GameObject b = Instantiate(a, trans);
    //    b.GetComponent<Battle_Character>().MyHpbar = b.GetComponent<Battle_Character>().MyHpbar.SetHpBar(data.P_mon_MaxHP, b.transform);
    //}

    public IEnumerator CreateMonster_(EnumScp.MonsterIndex p_index, Transform trans,string name = "Skeleton_Knight")
    {
        MonsterInformation data = ScriptableObject.CreateInstance<MonsterInformation>();
        data = DataLoad_Save.Instance.Get_MonsterDB(p_index);

       // string tempName = "Skeleton_Knight";

        //로드
        yield return StartCoroutine(AddressablesLoader.LoadGameObjectAndMaterial(name));

        GameObject temp = AddressablesController.Instance.find_Asset_in_list(name);
      //  temp.GetComponent<Battle_Character>().Stat_Initialize(data);

        GameObject b = Instantiate(temp, trans);
        b.GetComponent<Battle_Character>().MyHpbar = b.GetComponent<Battle_Character>().MyHpbar.SetHpBar(data.P_mon_MaxHP, b.transform);

        yield return null;

    }
    public IEnumerator CreateBossMonster_(EnumScp.MonsterIndex p_index, Transform trans, string name = "Boss")
    {
        MonsterInformation data = ScriptableObject.CreateInstance<MonsterInformation>();
        List<MonsterSkillInformation> skill = new List<MonsterSkillInformation>();
        MonsterTargetInformation target = ScriptableObject.CreateInstance<MonsterTargetInformation>();

        skill.Add(DataLoad_Save.Instance.Get_MonsterSkillDB(EnumScp.MonsterSkill.mon_06_01));
        skill.Add(DataLoad_Save.Instance.Get_MonsterSkillDB(EnumScp.MonsterSkill.mon_06_02));
        skill.Add(DataLoad_Save.Instance.Get_MonsterSkillDB(EnumScp.MonsterSkill.mon_06_04));
        skill.Add(DataLoad_Save.Instance.Get_MonsterSkillDB(EnumScp.MonsterSkill.mon_06_06));
        skill.Add(DataLoad_Save.Instance.Get_MonsterSkillDB(EnumScp.MonsterSkill.mon_06_08));
        skill.Add(DataLoad_Save.Instance.Get_MonsterSkillDB(EnumScp.MonsterSkill.mon_06_09));
        skill.Add(DataLoad_Save.Instance.Get_MonsterSkillDB(EnumScp.MonsterSkill.mon_06_10));
        target = DataLoad_Save.Instance.Get_MonsterTargetDB(EnumScp.MonsterTarget.ID33330212);
        data = DataLoad_Save.Instance.Get_MonsterDB(p_index);

        // string tempName = "Skeleton_Knight";

        //로드
        yield return StartCoroutine(AddressablesLoader.LoadGameObjectAndMaterial(name));

        GameObject temp = AddressablesController.Instance.find_Asset_in_list(name);
        temp.GetComponent<Battle_Character>().Stat_Initialize(data,skill,target);

        GameObject b = Instantiate(temp, trans);
    

        yield return null;

    }
    //IEnumerator setting()
    //{
    //    //find_Asset_in_list
    //}

}

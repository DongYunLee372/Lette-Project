using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterCreate : Singleton<CharacterCreate>
{

    //  public DataLoad_Save TestDataLoad;

    public GameObject hpBar;
    void Start()
    {
        //DataLoad_Save.Instance.Init();

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
    //    data = DataLoad_Save.Instance.Get_MonsterDB(p_index);

       // string tempName = "Skeleton_Knight";

        //로드
        yield return StartCoroutine(AddressablesLoader.LoadGameObjectAndMaterial(name));

        GameObject temp = AddressablesController.Instance.find_Asset_in_list(name);
      //  temp.GetComponent<Battle_Character>().Stat_Initialize(data);
        
        GameObject Monster = Instantiate(temp, trans);


        hpBar.GetComponent<EnemyHpbar>().SetHpBar(data.P_mon_MaxHP, Monster.transform, Monster);

        //  obj_hp.GetComponent<EnemyHpbar>().battle_Character = b.GetComponent<Battle_Character>();
        //  obj_hp.GetComponent<EnemyHpbar>().MyHpbar = obj_hp.GetComponent<EnemyHpbar>().SetHpBar(data.P_mon_MaxHP, b.transform);

        //   b.GetComponent<Battle_Character>().MyHpbar = b.GetComponent<Battle_Character>().MyHpbar.SetHpBar(data.P_mon_MaxHP, b.transform);

        yield return null;

    }
    public IEnumerator CreateBossMonster_(EnumScp.MonsterIndex p_index, Transform trans, string name = "Boss")
    {
        MonsterInformation data = ScriptableObject.CreateInstance<MonsterInformation>();
        MonsterTargetInformation target = ScriptableObject.CreateInstance<MonsterTargetInformation>();

        List<BossNomalSkill> bossNomalSkills = new List<BossNomalSkill>();
        List<Mon_Normal_Atk_Group> mon_Normal_Atk_Group = new List<Mon_Normal_Atk_Group>();
        MonsterSkillInformation monsterSkillInformation = ScriptableObject.CreateInstance<MonsterSkillInformation>();

        bossNomalSkills.Add(DataLoad_Save.Instance.Get_BossSkillDB(Global_Variable.Boss.BOSS_USwing));
        bossNomalSkills.Add(DataLoad_Save.Instance.Get_BossSkillDB(Global_Variable.Boss.BOSS_SSwing));
        bossNomalSkills.Add(DataLoad_Save.Instance.Get_BossSkillDB(Global_Variable.Boss.LRush));
        bossNomalSkills.Add(DataLoad_Save.Instance.Get_BossSkillDB(Global_Variable.Boss.Sting));
        bossNomalSkills.Add(DataLoad_Save.Instance.Get_BossSkillDB(Global_Variable.Boss.DiagonalSwing));
        bossNomalSkills.Add(DataLoad_Save.Instance.Get_BossSkillDB(Global_Variable.Boss.LRush_and_USwing));

        monsterSkillInformation=(DataLoad_Save.Instance.Get_MonsterSkillDB(Global_Variable.Boss.Rush_Atk));

        mon_Normal_Atk_Group.Add(DataLoad_Save.Instance.Get_Mon_Normal_Atk_GroupDB(Global_Variable.Boss.First_Atk));
        mon_Normal_Atk_Group.Add(DataLoad_Save.Instance.Get_Mon_Normal_Atk_GroupDB(Global_Variable.Boss.Second_Atk));
        mon_Normal_Atk_Group.Add(DataLoad_Save.Instance.Get_Mon_Normal_Atk_GroupDB(Global_Variable.Boss.Third_Atk));


        target = DataLoad_Save.Instance.Get_MonsterTargetDB(Global_Variable.CharVar.one33330211);
        data = DataLoad_Save.Instance.Get_MonsterDB(Global_Variable.CharVar.Arthur);

        // string tempName = "Skeleton_Knight";

        //로드
        yield return StartCoroutine(AddressablesLoader.LoadGameObjectAndMaterial(name));

        GameObject temp = AddressablesController.Instance.find_Asset_in_list(name);
        temp.GetComponent<Battle_Character>().Stat_Initialize(data,mon_Normal_Atk_Group,bossNomalSkills, monsterSkillInformation,target);

        GameObject b = Instantiate(temp, trans);
       b.GetComponent<Battle_Character>().bosshpbar.SetHpbar(data.P_mon_MaxHP,data.P_mon_nameKor);


        yield return null;

    }
    //IEnumerator setting()
    //{
    //    //find_Asset_in_list
    //}

}

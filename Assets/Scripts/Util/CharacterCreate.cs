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
        List<BossAttackInfo> mon_Attack_Info = new List<BossAttackInfo>();

        bossNomalSkills.Add(DataLoad_Save.Instance.Get_BossSkillDB(Global_Variable.Boss.USwing));
        bossNomalSkills.Add(DataLoad_Save.Instance.Get_BossSkillDB(Global_Variable.Boss.SSwing));
        bossNomalSkills.Add(DataLoad_Save.Instance.Get_BossSkillDB(Global_Variable.Boss.LRush));
        bossNomalSkills.Add(DataLoad_Save.Instance.Get_BossSkillDB(Global_Variable.Boss.Sting));
        bossNomalSkills.Add(DataLoad_Save.Instance.Get_BossSkillDB(Global_Variable.Boss.DiagonalSwing));
        bossNomalSkills.Add(DataLoad_Save.Instance.Get_BossSkillDB(Global_Variable.Boss.LRush_and_USwing));
        mon_Attack_Info.Add(DataLoad_Save.Instance.Get_BossAttackDB(Global_Variable.Boss.USwing));
        mon_Attack_Info.Add(DataLoad_Save.Instance.Get_BossAttackDB(Global_Variable.Boss.SSwing));
        mon_Attack_Info.Add(DataLoad_Save.Instance.Get_BossAttackDB(Global_Variable.Boss.Guided_Magic_Bullet));

        target = DataLoad_Save.Instance.Get_MonsterTargetDB(Global_Variable.CharVar.one33330211);
        data = DataLoad_Save.Instance.Get_MonsterDB(Global_Variable.CharVar.Arthur);

        // string tempName = "Skeleton_Knight";

        //로드
        yield return StartCoroutine(AddressablesLoader.LoadGameObjectAndMaterial(name));

        GameObject temp = AddressablesController.Instance.find_Asset_in_list(name);
        temp.GetComponent<Battle_Character>().Stat_Initialize(data,mon_Attack_Info,bossNomalSkills,target);

        GameObject b = Instantiate(temp, trans);
       b.GetComponent<Battle_Character>().bosshpbar.SetHpbar(data.P_mon_MaxHP,data.P_mon_nameKor);


        yield return null;

    }
    //IEnumerator setting()
    //{
    //    //find_Asset_in_list
    //}

}

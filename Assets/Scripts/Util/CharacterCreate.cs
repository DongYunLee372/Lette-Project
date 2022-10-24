using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterCreate : Singleton<CharacterCreate>
{

    //  public DataLoad_Save TestDataLoad;

    public GameObject hpBar;
    public GameObject bosshpbar;
    public int enemycount=0;
    public int deadcount=0;
    public List<GameObject> l_enemy;
    public GameObject obj_boss;
    void Awake()
    {
        //DataLoad_Save.Instance.Init();
        enemycount = 0;
        deadcount = 0;
    }
   
    // Update is called once per frame

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

    public IEnumerator CreateMonster_(EnumScp.MonsterIndex p_index, Transform trans,string name = "Skeleton")
    {
        MonsterInformation data = ScriptableObject.CreateInstance<MonsterInformation>();
        MonsterTargetInformation target = ScriptableObject.CreateInstance<MonsterTargetInformation>();
        //data = DataLoad_Save.Instance.Get_MonsterDB(p_index);
        data =DataLoad_Save.Instance.Get_MonsterDB(Global_Variable.CharVar.Skeleton);
        enemycount++;

        List<BossNomalSkill> NomalSkills = new List<BossNomalSkill>();
        List<Mon_Normal_Atk_Group> mon_Normal_Atk_Group = new List<Mon_Normal_Atk_Group>();     

        NomalSkills.Add(DataLoad_Save.Instance.Get_BossSkillDB(Global_Variable.Boss.SSwing));
        mon_Normal_Atk_Group.Add(DataLoad_Save.Instance.Get_Mon_Normal_Atk_GroupDB(Global_Variable.Boss.First_Atk));
        target = DataLoad_Save.Instance.Get_MonsterTargetDB(Global_Variable.CharVar.one33330211);

        //로드
        yield return StartCoroutine(AddressablesLoader.LoadGameObjectAndMaterial(name));

        GameObject temp = AddressablesController.Instance.find_Asset_in_list(name);
        temp.GetComponent<Battle_Character>().Stat_Initialize(data, mon_Normal_Atk_Group, NomalSkills,target);
        GameObject Monster = Instantiate(temp, trans);
     
        hpBar.GetComponent<EnemyHpbar>().SetHpBar(data.P_mon_MaxHP, Monster.transform, Monster.GetComponent<Battle_Character>());
        l_enemy.Add(Monster);
      
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
        bosshpbar.GetComponent<Bosshpbar>().SetHpbar(data.P_mon_MaxHP,data.P_mon_nameKor,b.GetComponent<Battle_Character>());
        obj_boss = b;
        yield return null;

    }

    public IEnumerator CreateBossMonster_S(EnumScp.MonsterIndex p_index, Transform trans, string name = "Boss")
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

        monsterSkillInformation = (DataLoad_Save.Instance.Get_MonsterSkillDB(Global_Variable.Boss.Rush_Atk));

        mon_Normal_Atk_Group.Add(DataLoad_Save.Instance.Get_Mon_Normal_Atk_GroupDB(Global_Variable.Boss.First_Atk));
        mon_Normal_Atk_Group.Add(DataLoad_Save.Instance.Get_Mon_Normal_Atk_GroupDB(Global_Variable.Boss.Second_Atk));
        mon_Normal_Atk_Group.Add(DataLoad_Save.Instance.Get_Mon_Normal_Atk_GroupDB(Global_Variable.Boss.Third_Atk));


        target = DataLoad_Save.Instance.Get_MonsterTargetDB(Global_Variable.CharVar.one33330211);
        data = DataLoad_Save.Instance.Get_MonsterDB(Global_Variable.CharVar.Arthur);

        // string tempName = "Skeleton_Knight";

        //로드
        AddressablesLoadManager.Instance.SingleAsset_Load<GameObject>(name);
        GameObject temp = AddressablesLoadManager.Instance.FindLoadAsset<GameObject>(name);
                        //AddressablesLoadManager.Instance.Instantiate_LoadObject<GameObject>(name);
     //   yield return StartCoroutine(AddressablesLoadManager.Instance.AsyncLoad_single<GameObject>(name));

    //    GameObject temp = AddressablesLoadManager.Instance.FindLoadAsset<GameObject>(name);
        temp.GetComponent<Battle_Character>().Stat_Initialize(data, mon_Normal_Atk_Group, bossNomalSkills, monsterSkillInformation, target);

        GameObject b = AddressablesLoadManager.Instance.Instantiate_LoadObject<GameObject>(name);
        b.transform.position = trans.position;
        //GameObject b = Instantiate(temp, trans);
        bosshpbar.GetComponent<Bosshpbar>().SetHpbar(data.P_mon_MaxHP, data.P_mon_nameKor, b.GetComponent<Battle_Character>());

        b.SetActive(false);
        Debug.Log(data.P_mon_nameKor);
        Debug.Log(data.P_mon_MaxHP);

        yield return null;

    }

    public IEnumerator CreateMonster_S(EnumScp.MonsterIndex p_index, Transform trans, string name = "Skeleton")
    {
        MonsterInformation data = ScriptableObject.CreateInstance<MonsterInformation>();
        MonsterTargetInformation target = ScriptableObject.CreateInstance<MonsterTargetInformation>();
        //data = DataLoad_Save.Instance.Get_MonsterDB(p_index);
        data = DataLoad_Save.Instance.Get_MonsterDB(Global_Variable.CharVar.Skeleton);


        List<BossNomalSkill> NomalSkills = new List<BossNomalSkill>();
        List<Mon_Normal_Atk_Group> mon_Normal_Atk_Group = new List<Mon_Normal_Atk_Group>();

        NomalSkills.Add(DataLoad_Save.Instance.Get_BossSkillDB(Global_Variable.Boss.SSwing));
        mon_Normal_Atk_Group.Add(DataLoad_Save.Instance.Get_Mon_Normal_Atk_GroupDB(Global_Variable.Boss.First_Atk));
        target = DataLoad_Save.Instance.Get_MonsterTargetDB(Global_Variable.CharVar.one33330211);

        //로드
        // yield return StartCoroutine(AddressablesLoader.LoadGameObjectAndMaterial(name));
        AddressablesLoadManager.Instance.SingleAsset_Load<GameObject>(name);
        GameObject temp = AddressablesLoadManager.Instance.FindLoadAsset<GameObject>(name);

       // GameObject temp = AddressablesController.Instance.find_Asset_in_list(name);
        temp.GetComponent<Battle_Character>().Stat_Initialize(data, mon_Normal_Atk_Group, NomalSkills, target);
        GameObject Monster = AddressablesLoadManager.Instance.Instantiate_LoadObject<GameObject>(name);//Instantiate(temp, trans);

        hpBar.GetComponent<EnemyHpbar>().SetHpBar(data.P_mon_MaxHP, Monster.transform, Monster.GetComponent<Battle_Character>());
        l_enemy.Add(Monster);
        enemycount++;
        yield return null;

    }
    public void Deadcount()
    {
        deadcount++;
        if (deadcount == enemycount)
        {
            return ;
        }
    }
    //IEnumerator setting()
    //{
    //    //find_Asset_in_list
    //}

}

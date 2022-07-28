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

    public void CreateMonster(EnumScp.MonsterIndex p_index, Transform trans)
    {
        MonsterInformation data = ScriptableObject.CreateInstance<MonsterInformation>();
        data = DataLoad_Save.Instance.Get_MonsterDB(p_index);

        GameObject a = Resources.Load<GameObject>(StaticClass.Prefabs + "Skeleton_Knight");
        a.GetComponent<Battle_Character>().Stat_Initialize(data);

        GameObject b = Instantiate(a, trans);
        b.GetComponent<Battle_Character>().MyHpbar=b.GetComponent<Battle_Character>().MyHpbar.SetHpBar(data.P_mon_MaxHP, b.transform);



    }

}

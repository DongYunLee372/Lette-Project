using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestData : MonoBehaviour
{
    Dictionary<string, MonsterInformation> MonsterDB_List;
    void Start()
    {              
        TestLoadFile.Read<MonsterInformation>(out MonsterDB_List);

        

    }

    private void Awake()
    {
        TestLoadFile.Read<MonsterInformation>(out MonsterDB_List);
    }
    
}

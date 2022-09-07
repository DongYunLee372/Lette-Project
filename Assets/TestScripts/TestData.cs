using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestData : MonoBehaviour
{
    Dictionary<string, MonsterInformation> MonsterDB_List = new Dictionary<string, MonsterInformation>();
    void Start()
    {              
        TestLoadFile.Read<MonsterInformation>(out MonsterDB_List);

        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

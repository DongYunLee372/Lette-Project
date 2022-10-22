using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;
using System;
using System.Reflection;
using System.Text;
using UnityEditor;

public class TestDataSCP : MySingleton<LoadFile>
{
    //-9298
    public float abc = 0;
    public float qqq = 3f;
    public GameObject eee;

    private void Start()
    {
        var a= -9298;
        //Debug.Log(eee.GetInstanceID());
        //eee = (GameObject)a;
    }

    //Dictionary<string, TestDataSCP> BossNomalSkillDB_List;
    [MenuItem("CONTEXT/Rigidbody/TestDataSCP")]
    public static void Save(MenuCommand command)
    {
        
       




    }
    public static void abc2()
    {
        Debug.Log("d");
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            LoadFile.Save<TestDataSCP>(this);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            //LoadFile.Read<TestDataSCP>(out BossNomalSkillDB_List);
        }
    }
}

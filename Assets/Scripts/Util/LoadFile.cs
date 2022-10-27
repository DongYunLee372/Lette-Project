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

public class LoadFile : MySingleton<LoadFile>
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };
    static string path = "Assets/Resources/CSV/";
    //D:\TeamProjectHades\Hadas-Fiesta\Hadas-Fiesta\Assets\Resources\SaveFile
    public static void Read<T>(out Dictionary<string, T> Dic2) /*where T : abc*/
    {

        FieldInfo[] Fieldlist = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance| BindingFlags.Public);

        TextAsset data = Resources.Load("CSV/" + typeof(T).ToString()) as TextAsset;
        //Debug.Log(data.name);
        
        var lines = Regex.Split(data.text, LINE_SPLIT_RE);
        if (lines.Length <= 1)
        {
            Dic2 = null;
            return; //list;
        }

        Dic2 = new Dictionary<string, T>(lines.Length);

        var header = Regex.Split(lines[0], SPLIT_RE);
        //var datatype = Regex.Split(lines[1], SPLIT_RE);
        //Debug.Log(lines[0]);
        //Debug.Log(lines[1]);

        string Key;
        for (var i = 1; i < lines.Length; i++)
        {
            object information_T = Activator.CreateInstance(typeof(T));

            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            Key = values[0];
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;

                Type type = Fieldlist[j].FieldType;
                //Debug.Log(type);
                Fieldlist[j].SetValue(information_T, Convert.ChangeType(value, type));
            }

            Dic2.Add(Key, (T)information_T);
        }


    }

   
    public static void Save<T>(T m_class)
    {
        
        FieldInfo[] Fieldlist = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
        string allPath = path + m_class.GetType().Name + ".csv";      
        string delimiter = ",";
        List<int> a = new List<int>();
        StringBuilder sb = new StringBuilder();
        Type type;
        //FileStream file = new FileStream(allPath, FileMode.Append, FileAccess.Write); //생성(이미 존재한다면 생성된 파일을 사용) , FileMode.Append로 데이터 추가시 덮어쓰지않고 아래로 추가되어서 데이터 삽입
        //StreamWriter outStream = new StreamWriter(file, Encoding.UTF8);

        StreamWriter outStream = System.IO.File.CreateText(allPath); //지정된 경로로 가서 filename에 맞는 파일 생성 이미 존재한다면 덮어쓰기하여 새롭게 생성 

        for (int i = 0; i < Fieldlist.Length; i++)
        {
            type = Fieldlist[i].FieldType;
            Debug.Log(type);
            sb.AppendFormat("{0}{1}" ,Fieldlist[i].Name , delimiter); //필드에 저장된 값 가로로 Save 
            //Debug.Log(Fieldlist[i].GetValue(m_class));
        }
        outStream.WriteLine(sb);
        sb.Clear();
        for (int i = 0; i < Fieldlist.Length; i++)
        {
            sb.AppendFormat("{0}{1}",Fieldlist[i].GetValue(m_class).ToString(), delimiter);
            //Debug.Log(Fieldlist[i].GetValue(m_class));
        }
        


        outStream.WriteLine(sb); //StreamWriter에 저장된 값들 csv에 쓰기
        outStream.Close();
        

        
        
    }
}

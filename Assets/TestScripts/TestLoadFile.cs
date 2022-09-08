using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;
using System;
using System.Reflection;

// 파일 변경시 에디터에서 감지 , 에디터 이벤트 키워드 
// 이미지 등록시 메모리에 2의 배수로 올라감 (작은 크기의 이미지에도 )

public class TestLoadFile : MonoBehaviour
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static void Read<T>(out Dictionary<string, T> Dic2) /*where T : abc*/
    {
                     
        FieldInfo[] Fieldlist = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        
        TextAsset data = Resources.Load("CSV/" + typeof(T).ToString()) as TextAsset;
        var lines = Regex.Split(data.text, LINE_SPLIT_RE);
        if (lines.Length <= 2)
        {
            Dic2 = null;
            return; //list;
        }

        Dic2 = new Dictionary<string, T>(lines.Length);

        var header = Regex.Split(lines[0], SPLIT_RE);
        var datatype = Regex.Split(lines[1], SPLIT_RE);
        Debug.Log(lines[0]);
        Debug.Log(lines[1]);
        for (var i = 2; i < lines.Length; i++)
        {
            object information_T = Activator.CreateInstance(typeof(T));

            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;

                Type type = Fieldlist[j].FieldType;               
                Fieldlist[j].SetValue(information_T, Convert.ChangeType(value, type));               
            }
            
            Dic2.Add(i.ToString(), (T)information_T);
        }
        
       
    }
}

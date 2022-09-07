using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;
using System;
using System.Reflection;

public class TestLoadFile : MonoBehaviour
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static void Read<T>(out Dictionary<string, T> Dic2)
    {
        Dic2 = new Dictionary<string, T>();               
        FieldInfo[] Fieldlist = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

        //foreach (FieldInfo a in Fieldlist)
        //{
        //    Debug.Log(a.Name + " " + a.GetValue(information_T));
        //}

        //typeof(T).ToString();

        var list = new List<Dictionary<string, object>>();
        TextAsset data = Resources.Load("CSV/" + typeof(T).ToString()) as TextAsset;
        var lines = Regex.Split(data.text, LINE_SPLIT_RE);
        if (lines.Length <= 2)
        {
            Dic2 = null;
            return; //list;
        }

        var header = Regex.Split(lines[0], SPLIT_RE);
        var datatype = Regex.Split(lines[1], SPLIT_RE);
        Debug.Log(lines[0]);
        Debug.Log(lines[1]);
        for (var i = 2; i < lines.Length; i++)
        {
            object information_T = Activator.CreateInstance(typeof(T));

            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            //var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;

                if (datatype[j] == "string")
                    Fieldlist[j].SetValue(information_T, value);
                if (datatype[j] == "int")
                    Fieldlist[j].SetValue(information_T, Convert.ToInt32(value));
                //if (datatype[j] == "string")
                    //Fieldlist[j].SetValue(information_T, value);

                //int n;
                //float f;
                //if (int.TryParse(value, out n))
                //{
                //    finalvalue = n;
                //}
                //else if (float.TryParse(value, out f))
                //{
                //    finalvalue = f;
                //}
                //entry[header[j]] = finalvalue;
            }
            //list.Add(entry);
            Dic2.Add(i.ToString(), (T)information_T);
        }
        //return list;
       
    }
}

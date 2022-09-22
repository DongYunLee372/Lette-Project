using Canvas_Enum;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
[System.Serializable]
public class Save_Optiondata
{
    //public Save_Optiondata(string p_up,string p_down,string p_left,string p_rigth,string p_roll,string p_attack,string p_defense)
    //{
    //    up = p_up;
    //    down = p_down;
    //    left = p_left;
    //    right = p_rigth;
    //    roll = p_roll;
    //    attack = p_attack;
    //    defens = p_defense;
    //}
    public Save_Optiondata(KeyCode p_up, KeyCode p_down, KeyCode p_left, KeyCode p_rigth, KeyCode p_roll, KeyCode p_attack, KeyCode p_defense)
    {
        up = p_up;
        down = p_down;
        left = p_left;
        right = p_rigth;
        roll = p_roll;
        attack = p_attack;
        defens = p_defense;
    }
    //public string up;
    //public string down;
    //public string left;
    //public string right;
    //public string roll;
    //public string attack;
    //public string defens;

    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
    public KeyCode roll;
    public KeyCode attack;
    public KeyCode defens;
 
}
public static class SaveSystem
{
    private static string SavePath => Application.persistentDataPath + "/saves/";

    public static void Save(Save_Optiondata saveData, string saveFileName)
    {
        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }

        string saveJson = JsonUtility.ToJson(saveData);

        string saveFilePath = SavePath + saveFileName + ".json";
        File.WriteAllText(saveFilePath, saveJson);
        Debug.Log("Save Success: " + saveFilePath);
    }

    public static Save_Optiondata Load(string saveFileName)
    {
        string saveFilePath = SavePath + saveFileName + ".json";

        if (!File.Exists(saveFilePath))
        {
            Debug.LogError("No such saveFile exists");
            return null;
        }

        string saveFile = File.ReadAllText(saveFilePath);
        Save_Optiondata saveData = JsonUtility.FromJson<Save_Optiondata>(saveFile);
        return saveData;
    }
}
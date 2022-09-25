using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class TestMainLoad : MonoBehaviour
{

    public List<Load_And_SaveData> a = new List<Load_And_SaveData>();

    public List<string> Prefapsname = new List<string>();
    public List<Vector3> Position = new List<Vector3>();

    public List<Load_And_SaveData> load_And_SaveDatas = new List<Load_And_SaveData>();

    string path= "Assets/GameData/";
    string testSaveDataName = "TestData";
    string type = ".asset";
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F2))
        {
            //var DataSave = ScriptableObject.CreateInstance<GameSaveData>();
            //AssetDatabase.CreateAsset(DataSave, "Assets/GameData/TestGameData.asset");
            var tempDataSave = AssetDatabase.LoadAssetAtPath<GameSaveData>("Assets/GameData/TestGameData.asset");

            for (int i=0; i<Prefapsname.Count; i++)
            {
                string tempstring = path + testSaveDataName + i + type;

                var tempData = AssetDatabase.LoadAssetAtPath<Load_And_SaveData>(tempstring);

                if (tempData != null)
                {
                    Debug.Log("null아님");
                    string[] te = AssetDatabase.FindAssets(tempstring);
                    Debug.Log("찾는결과 : "+te);
                    var data = AssetDatabase.LoadAssetAtPath<Load_And_SaveData>(tempstring);
                    Debug.Log(tempData+"원래있던거?");
                    data.prefabsName = Prefapsname[i].ToString();
                    Debug.Log(data.prefabsName + i);
                    data.Position = Position[i];
                    Debug.Log(data.Position + "" + i);
                    EditorUtility.SetDirty(data);
                }
                else
                {
                    Debug.Log("null임");

                    var data = ScriptableObject.CreateInstance<Load_And_SaveData>();

                    data.prefabsName = Prefapsname[i].ToString();
                    Debug.Log(data.prefabsName + i);
                    data.Position = Position[i];
                    Debug.Log(data.Position + "" + i);
                    AssetDatabase.CreateAsset(data, "Assets/GameData/TestData" + i + ".asset");
                    var saveData = AssetDatabase.LoadAssetAtPath<Load_And_SaveData>("Assets/GameData/TestData" + i + ".asset");
                    tempDataSave.SaveDatas.Add(saveData);
                }

                
                //Debug.
            }
            // AssetDatabase.CreateAsset(DataSave, "Assets/GameData/TestGameData.asset");
            EditorUtility.SetDirty(tempDataSave);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }


}

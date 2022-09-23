using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSaveData", menuName = "Scp/GameSave")]
public class GameSaveData : ScriptableObject
{
    public List<Load_And_SaveData> SaveDatas;
   
}

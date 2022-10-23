using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameLoadData", menuName = "Scp/GameLoadData_")]
public class GameLoadingData : ScriptableObject
{
    public List<LoadImageData> LoadingData;
}

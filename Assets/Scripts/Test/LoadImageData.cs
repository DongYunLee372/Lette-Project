using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LoadImage", menuName = "Scp/LoadImageData")]
public class LoadImageData : ScriptableObject
{
    public  string imgae_SceneName;
    public List<string> LoadImageNameList;
}

public enum Imgae_SceneName
{
    GameTitle_ = 0,  //게임 시작에서 뜨는 이미지
    GameStartLoading=1,  //게임 시작할때 로딩에서 뜨는 이미지
    GameReStartLoading,  //게임 재시작 할때 뜨는 이미지
    GameEndLoading,  //게임 죽었을 때 뜨는 이미지
}
  
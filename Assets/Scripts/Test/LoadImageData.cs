using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LoadImage", menuName = "Scp/LoadImageData")]
public class LoadImageData : ScriptableObject
{
    public  string imgae_Name;  //이미지
    public List<string> LoadImageNameList; 
    public int index;
    public Imgae_SceneName SceneName;
}

public enum Imgae_SceneName
{
    GameTitle_ = 0,  //게임 시작에서 뜨는 이미지
    GameStartLoading=1,  //게임 시작할때 로딩에서 뜨는 이미지
    BoatSceneLoading,  //배씬 이동할때 뜨는 이미지
    BossSceneLoading,  //보스씬 이동
    GameReStartLoading,  //게임 재시작 할때 뜨는 이미지
    GameEndLoading,  //게임 죽었을 때 뜨는 이미지
}
  
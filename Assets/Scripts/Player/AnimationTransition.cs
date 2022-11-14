using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationTransition", menuName = "Scriptable Object/AnimationTransition", order = int.MaxValue)]
public class AnimationTransition : ScriptableObject
{
    [Header("start")]
    public AnimationClip Clip_1;
    [Header("next")]
    public AnimationClip Clip_2;

    [Header("클립 1에서 클립2로 넘어가는 시점 0~1값"),Range(0.0f,1.0f)]
    public float exitTime;//클립 1에서 클립2로 넘어가는 시점 0~1값

    [Header("블렌딩 되는 시간")]
    public float normalizedTransitionDuration;//블렌딩 되는 기간

    [Header("두번째 클립이 실행되는 시점 0이면 처음부터 시작 1이면 끝"), Range(0.0f, 1.0f)]
    public float normalizedTimeOffset;//두번째 클립이 실행되는 시점 0이면 처음부터 시작 1이면 끝
}

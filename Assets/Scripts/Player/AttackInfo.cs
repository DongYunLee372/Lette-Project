using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackInfo
{
    [Tooltip("공격번호")]
    [SerializeField]
    public int attackNum;

    [Tooltip("공격이름")]
    [SerializeField]
    public string attackName;

    [Tooltip("해당 공격의 타입을 설정한다 (노말, 광역, 투사체, 타겟팅)")]
    [SerializeField]
    public string attackType;

    //해당 매니메이션 클립 이후 공격컴포넌트에서 이름을 이용해 실제 클립을 받아오는것이 필요
    [Tooltip("해당 공격의 애니메이션 클립 이름")]
    [SerializeField]
    public string aniclipName;

    //해당 매니메이션 클립 이후 공격컴포넌트에서 이름을 이용해 실제 클립을 받아오는것이 필요
    [Tooltip("해당 공격의 복귀 애니메이션 클립 이름")]
    [SerializeField]
    public string endAniclipName;

    //공격 중 움직일 거리
    [Tooltip(" 복귀 애니메이션 움직임을 시작할 시간")]
    [SerializeField]
    public float endmovestarttime;

    //공격 중 움직일 거리
    [Tooltip(" 복귀 애니메이션 움직일 거리")]
    [SerializeField]
    public float endmovedis;

    //움직일 시간
    [Tooltip(" 복귀 애니메이션 움직일 시간")]
    [SerializeField]
    public float endmovetime;

    //애니메이션 배속
    [Tooltip("해당 공격의 애니메이션 재생 속도")]
    [SerializeField]
    [Range(0.0f, 10.0f)]
    public float animationPlaySpeed;

    [Tooltip("해당 공격의 복귀 애니메이션 클립 이름")]
    [SerializeField]
    [Range(0.0f, 10.0f)]
    public float endanimationPlaySpeed;

    [Tooltip("선딜")]
    [SerializeField]
    [Range(0.0f, 10.0f)]
    public float startDelay;

    //후딜레이
    [Tooltip("후딜")]
    [Range(0.0f, 10.0f)]
    [SerializeField]
    public float recoveryDelay;

    [Tooltip("다음 동작으로 넘어갈 수 있는 시간")]
    [SerializeField]
    public float bufferdInputTime_Start;

    //다음동작으로 넘어가기 위한 시간
    //해당동작이 끝나고 해당 시간 안에 Attack()함수가 호출되어야지 다음동작으로 넘어간다.
    [Tooltip("연속동작이 있을때 다음 동작으로 들어가기 위한 입력 시간")]
    [SerializeField]
    public float BufferdInputTime_End;

    //데미지
    [Tooltip("공격 데미지")]
    [SerializeField]
    public float damage;

    [Tooltip("공격시 줄어들 스테미나 게이지")]
    [SerializeField]
    public float StaminaGaugeDown;

    //공격 이펙트
    [Tooltip("공격 이펙트")]
    [SerializeField]
    public string EffectName;

    //이펙트 생성 타이밍
    [Tooltip("공격 이펙트 생성 타이밍")]
    [SerializeField]
    public float EffectStartTime;

    //공격 이펙트의 위치
    [Tooltip("공격 이펙트 생성 위치")]
    [SerializeField]
    public Transform effectPosRot;

    [Tooltip("공격 이펙트 파괴 시간")]
    [SerializeField]
    public float EffectDestroyTime;

    //공격 중 움직일 거리
    [Tooltip("공격할때 움직임을 시작할 시간")]
    [SerializeField]
    public float movestarttime;

    //공격 중 움직일 거리
    [Tooltip("공격할때 움직일 거리")]
    [SerializeField]
    public float movedis;

    //움직일 시간
    [Tooltip("공격할때 움직일 시간")]
    [SerializeField]
    public float movetime;

    [Tooltip("투사체가 있는 공격일때 투사체의 게임 오브젝트")]
    [SerializeField]
    public string ProjectileObjName;

    [Tooltip("타겟팅공격일때 타겟오브젝트")]
    [SerializeField]
    public string TargetObjName;

    [Tooltip("공격 끝 지점")]
    [SerializeField]
    public float AttackEndTime;

    ///////////////////////////////////////////////////////////////////////////////////////

    private AnimationClip aniClip;

    public AnimationClip AniClip
    {
        get
        {
            if (aniClip == null)
                Resources.Load("AnimationClips/PlayableCharacter/" + aniclipName);
            return aniClip;
        }
    }

    //private Vector3 EffectPosRot

    private GameObject effectObj;

    public GameObject EffectObj
    {
        get
        {
            if (effectObj == null)
                Resources.Load("Prefabs/PlayerEffects/" + EffectName);
            return effectObj;
        }
    }

    private GameObject projectileObj;

    public GameObject ProjectileObj
    {
        get
        {
            if (projectileObj == null)
                Resources.Load("Prefabs/PlayerEffects/" + ProjectileObjName);
            return projectileObj;
        }
    }

    //private Transform EffectPosRot;

    //public GameObject PEffectPosRot
    //{
    //    get
    //    {
    //        if (projectileObj == null)
    //            Resources.Load("Prefabs/PlayerEffects/" + ProjectileObjName);
    //        return projectileObj;
    //    }
    //}

}


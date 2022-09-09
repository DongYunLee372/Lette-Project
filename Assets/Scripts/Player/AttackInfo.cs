using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackInfo
{
    [Tooltip("공격이름")]
    [SerializeField]
    private string attackName;

    [Tooltip("해당 공격의 타입을 설정한다 (노말, 광역, 투사체, 타겟팅)")]
    [SerializeField]
    private string AttackType;

    //[Tooltip("공격번호")]
    //public int AttackNum;

    //해당 매니메이션 클립 이후 공격컴포넌트에서 이름을 이용해 실제 클립을 받아오는것이 필요
    [Tooltip("해당 공격의 애니메이션 클립 이름")]
    [SerializeField]
    private string aniclipName;

    //애니메이션 배속
    [Tooltip("해당 공격의 애니메이션 재생 속도")]
    [SerializeField]
    [Range(0.0f, 10.0f)]
    private float animationPlaySpeed;

    [Tooltip("선딜")]
    [SerializeField]
    [Range(0.0f, 10.0f)]
    private float StartDelay;

    //후딜레이
    [Tooltip("후딜")]
    [Range(0.0f, 10.0f)]
    [SerializeField]
    private float RecoveryDelay;

    [Tooltip("다음 동작으로 넘어갈 수 있는 시간")]
    [SerializeField]
    private float BufferdInputTime_Start;

    //다음동작으로 넘어가기 위한 시간
    //해당동작이 끝나고 해당 시간 안에 Attack()함수가 호출되어야지 다음동작으로 넘어간다.
    [Tooltip("연속동작이 있을때 다음 동작으로 들어가기 위한 입력 시간")]
    [SerializeField]
    private float BufferdInputTime_End;

    //데미지
    [Tooltip("공격 데미지")]
    [SerializeField]
    private float damage;

    [Tooltip("공격시 줄어들 스테미나 게이지")]
    [SerializeField]
    private float StaminaGaugeDown;

    //공격 이펙트
    [Tooltip("공격 이펙트")]
    [SerializeField]
    private string EffectName;

    //이펙트 생성 타이밍
    [Tooltip("공격 이펙트 생성 타이밍")]
    [SerializeField]
    private float EffectStartTime;

    //공격 이펙트의 위치
    [Tooltip("공격 이펙트 생성 위치")]
    [SerializeField]
    private string EffectPosRot;

    [Tooltip("공격 이펙트 파괴 시간")]
    [SerializeField]
    private float EffectDestroyTime;

    //공격 중 움직일 거리
    [Tooltip("공격할때 움직일 거리")]
    [SerializeField]
    private float movedis;

    //움직일 시간
    [Tooltip("공격할때 움직일 시간")]
    [SerializeField]
    private float movetime;

    [Tooltip("투사체가 있는 공격일때 투사체의 게임 오브젝트")]
    [SerializeField]
    private string ProjectileObjName;

    [Tooltip("타겟팅공격일때 타겟오브젝트")]
    [SerializeField]
    private string TargetObjName;

    [Tooltip("다음 연결동작 이름")]
    [SerializeField]
    private string NextAttackName;
}


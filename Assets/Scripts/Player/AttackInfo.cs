using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Playable_Character
{
    [System.Serializable]
    public class AttackInfo
    {
        [Tooltip("해당 공격의 타입을 설정한다 (노말, 광역, 투사체, 타겟팅)")]
        public CharEnumTypes.eAttackType AttackType;

        [Tooltip("공격이름")]
        public int AttackName;

        [Tooltip("공격번호")]
        public int AttackNum;

        //애니메이션 배속
        [Tooltip("해당 공격의 애니메이션 재생 속도")]
        [Range(0.0f, 10.0f)]
        public float animationPlaySpeed;

        //해당 매니메이션 클립
        [Tooltip("해당 공격의 애니메이션 클립")]
        public AnimationClip aniclip;

        [Tooltip("선딜")]
        [Range(0.0f, 10.0f)]
        public float StartDelay;

        //후딜레이
        [Tooltip("후딜")]
        [Range(0.0f, 10.0f)]
        public float RecoveryDelay;

        //다음동작으로 넘어가기 위한 시간
        //해당동작이 끝나고 해당 시간 안에 Attack()함수가 호출되어야지 다음동작으로 넘어간다.
        [Tooltip("연속동작이 있을때 다음 동작으로 들어가기 위한 입력 시간")]
        public float NextMovementTimeVal;

        //데미지
        [Tooltip("공격 데미지")]
        public float damage;

        //이펙트 생성 타이밍
        [Tooltip("공격 이펙트 생성 타이밍")]
        public float EffectStartTime;

        //공격 이펙트
        [Tooltip("공격 이펙트")]
        public GameObject Effect;

        //공격 이펙트의 위치
        [Tooltip("공격 이펙트 생성 위치")]
        public Transform EffectPosRot;

        [Tooltip("공격 이펙트 파괴 시간")]
        public float EffectDestroyTime;

        //공격 중 움직일 거리
        [Tooltip("공격할때 움직일 거리")]
        public float movedis;

        //움직일 시간
        [Tooltip("공격할때 움직일 시간")]
        public float movetime;

        [Tooltip("투사체가 있는 공격일때 투사체의 게임 오브젝트")]
        public GameObject ProjectileObj;

        [Tooltip("타겟팅공격일때 타겟오브젝트")]
        public GameObject TargetObj;

        [Tooltip("다음 연결동작 이름")]
        public string NextAttackName;
    }
}


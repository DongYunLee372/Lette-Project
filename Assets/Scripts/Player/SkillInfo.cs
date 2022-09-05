using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Playable_Character
{
    [System.Serializable]
    public class SkillInfo
    {
        [Tooltip("해당 공격의 타입을 설정한다 (노말, 광역, 투사체, 타겟팅)")]
        public CharEnumTypes.eAttackType AttackType;

        [Tooltip("스킬이름")]
        public string SkillName;

        [Tooltip("스킬번호")]
        public int SkillNum;

        //스킬 애니메이션
        public AnimationClip aniclip;

        //스킬 애니메이션 재생속도
        public float animationPlaySpeed;

        [Tooltip("선딜")]
        [Range(0.0f, 10.0f)]
        public float StartDelay;

        //후딜레이
        [Tooltip("후딜")]
        [Range(0.0f, 10.0f)]
        public float RecoveryDelay;

        //연결동작이 있을시 사용
        public float NextMovementTimeVal;

        //데미지
        public float damage;

        //이펙트
        public GameObject Effect;

        //이펙트 생성 시간
        public float EffectStartTime;

        //이펙트 생성 위치
        public Transform EffectPosRot;

        //움직일 거리
        public float Movedis;

        //움직일 시간
        public float MoveTime;
    }
}


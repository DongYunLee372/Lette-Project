using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharEnumTypes
{
    public enum eAttackType
    {
        Normal,//일반
        AreaOfEffect,//장판, 광역
        Projectile,//투사체
        Targeting,//타겟팅
    }

    public enum eComponentTypes
    {
        InputCom,
        MoveCom,
        AnimatorCom,
        GuardCom,
        AttackCom,
        comMax
    }
    public enum eInputEvents
    {

    }

    public enum eAnimationState
    {
        Idle,
        Attack,
        Skill,
        Die,
        Stun,
        Damage,
        Move,
        AniStateMax
    }

    public enum eAniMove
    {
        Run,
        Dash,
        MoveMax
    }


    public enum eAniIdle
    {
        Idle01,
        Idle02,
        Idle03,
        IdleMax
    }

    public enum eAniSkill
    {
        Skill01,
        Skill02,
        Skill03,
        Skill04,
        SkillMax
    }

    public enum eAniAttack
    {
        Attack01,
        Attack02,
        Attack03,
        AttackMax
    }



    //public enum eAnimationClips
    //{
    //    Idle01,
    //    Idle02,
    //    Idle03,
    //    Dash,
    //    Die,
    //    Attack01,
    //    Attack010,
    //    Attack02,
    //    Damage,
    //    Run,
    //    Skill01,
    //    Skill02,
    //    Skill03,
    //    Skill04,
    //    Stun,
    //    AnimationMax
    //}
}

namespace Enemy_Enum
{
    public enum Enemy_Grade
    {
        Normal = 1, // 일반 몬스터 
        General,  // 정예 몬스터
        Boss // 보스 몬스터
    }

    public enum Enemy_Type
    {
        Preemptive = 1, // 선공형
        Non_Preemptive,  // 비선공형
    }

    public enum Enemy_Attack_Type
    {
        Normal_Attack,
        Skill_Attack,
    }

    public enum Enemy_Attack_Logic
    {
        Melee_Attack = 0,
        Long_Attack,
        Skill_Using,
        Skill_Wait,
        Attack_Logic_Amount,
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy_Enum;

public class State_Attack : State
{
    [SerializeField]
    private Enemy_Attack_Logic judge_logic; // 리턴받는 공격방식

    public override bool Judge(out State _State, Battle_Character b_c)
    {
        if (b_c.isAttack_Run)
        {
            judge_logic = Enemy_Attack_Logic.Skill_Wait;
            _State = this;
            return true;
        }

        //b_c.Attack_Melee_Range 가 스킬 사용범위 변수로 바뀌어야함.
        if ((Vector3.Distance(b_c.transform.position,
            b_c.cur_Target.transform.position) <= b_c.now_Skill_Info.P_skill_Range) && b_c.mon_Info.P_dieDelay >= b_c.now_Skill_Info.P_skill_MP)
        {
            judge_logic = Enemy_Attack_Logic.Skill_Using;
            _State = this;
            return true;
        }

        //if ((Vector3.Distance(b_c.transform.position,
        //    b_c.cur_Target.transform.position) <= b_c.Attack_Melee_Range)) // 사정 거리 내에 있다면 
        //{
        //    judge_logic = Enemy_Attack_Logic.Melee_Attack;
        //    _State = this;
        //    return true;
        //}

        //if (b_c.attack_Logic[(int)Enemy_Attack_Logic.Long_Attack] == true && (Vector3.Distance(b_c.transform.position,
        //    b_c.cur_Target.transform.position) <= b_c.Attack_Long_Range)) // 원거리 공격방식이 존재
        //{
        //    judge_logic = Enemy_Attack_Logic.Long_Attack;
        //    _State = this;
        //    return true;
        //}

        b_c.real_AI.pre_State = this;
        _State = this; // Trans_List[0];
        return false;
    }

    public override void Run(Battle_Character b_c)
    {
        switch (judge_logic)
        {
            case Enemy_Attack_Logic.Skill_Wait:
                // 기다리기
                break;
            case Enemy_Attack_Logic.Melee_Attack:
                // 근접 공격이라면 배틀캐릭터 스크립트 내 공격 판정범위 활성화
                b_c.attack_Type = Enemy_Attack_Type.Normal_Attack;
                b_c.attack_Collider.SetActive(true);
                b_c.animator.Play("Melee Attack");
                b_c.isAttack_Run = true;
                break;
            case Enemy_Attack_Logic.Long_Attack:
                // 원거리라면 원거리 발사체 발사
                b_c.attack_Type = Enemy_Attack_Type.Normal_Attack;
                break;
            case Enemy_Attack_Logic.Skill_Using:
                // 이번에 사용할 순서의 스킬을 사용.
                b_c.attack_Type = Enemy_Attack_Type.Skill_Attack;
                b_c.skill_handler.Skill_Run(b_c, b_c.now_Skill_Info);
                b_c.Skill_Rand();
                break;
        }
    }


}

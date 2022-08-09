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
        //  b_c.Attack_Melee_Range 가 스킬 사용범위 변수로 바뀌어야함.
        if ((Vector3.Distance(b_c.transform.position,
            b_c.cur_Target.transform.position) <= b_c.Attack_Melee_Range) && b_c.char_Info.P_player_MP >= b_c.need_Mana)
        {
            judge_logic = Enemy_Attack_Logic.Skill_Using;
            _State = this;
            return true;
        }

        if ((Vector3.Distance(b_c.transform.position,
            b_c.cur_Target.transform.position) <= b_c.Attack_Melee_Range)) // 사정 거리 내에 있다면 
        {
            judge_logic = Enemy_Attack_Logic.Melee_Attack;
            _State = this;
            return true;
        }

        if (b_c.attack_Logic[(int)Enemy_Attack_Logic.Long_Attack] == true && (Vector3.Distance(b_c.transform.position,
            b_c.cur_Target.transform.position) <= b_c.Attack_Long_Range)) // 원거리 공격방식이 존재
        {
            judge_logic = Enemy_Attack_Logic.Long_Attack;
            _State = this;
            return true;
        }

        _State = Trans_List[0];
        return false;
    }

    public override void Run(Battle_Character b_c)
    {
        switch (judge_logic)
        {
            case Enemy_Attack_Logic.Melee_Attack:
                // 근접 공격이라면 배틀캐릭터 스크립트 내 공격 판정범위 활성화
                break;
            case Enemy_Attack_Logic.Long_Attack:
                // 원거리라면 원거리 발사체 발사
                break;
            case Enemy_Attack_Logic.Skill_Using:
                // 이번에 사용할 순서의 스킬을 사용.
                b_c.skill_handler.Skill_Run(/*이번에 사용할 스킬데이터를 넣어줌*/);
                break;
        }

    }
}

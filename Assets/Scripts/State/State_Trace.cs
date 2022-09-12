using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy_Enum;

public class State_Trace : State
{
    public override bool Judge(out State _State, Battle_Character b_c)
    {
        // 아래 부분들이 다 필요없고 최종 사거리만 계산해서 최종 사거리안에 진입했다면 Attack 스테이트로 돌리면 될듯
        // 변수 추가 받은 후 수정


        if (Vector3.Distance(b_c.transform.position,
            b_c.cur_Target.transform.position)
            <= b_c.mon_Info.P_mon_ShortRange) // 타겟을 공격할 수 있는 사거리 내 진입했다면
        {
            _State = Trans_List[0];
            b_c.real_AI.pre_State = this;
            return false;
        }

        if ((Vector3.Distance(b_c.transform.position,
            b_c.cur_Target.transform.position) <= b_c.now_Skill_Info.P_skill_Range)
            && b_c.mon_Info.P_mon_haveMP >= b_c.now_Skill_Info.P_skill_MP)
        {
            _State = Trans_List[0];
            b_c.real_AI.pre_State = this;
            return false;
        }

        if (b_c.now_Skill_Info.P_skill_Range == 0
            && b_c.mon_Info.P_mon_haveMP >= b_c.now_Skill_Info.P_skill_MP)
        {
            _State = Trans_List[0];
            b_c.real_AI.pre_State = this;
            return false;
        }

        if (b_c.attack_Logic[(int)Enemy_Attack_Logic.Long_Attack] == true && (Vector3.Distance(b_c.transform.position,
            b_c.cur_Target.transform.position) <= b_c.mon_Info.P_mon_LongRange)) // 원거리 공격방식이 존재
        {
            _State = Trans_List[0];
            b_c.real_AI.pre_State = this;
            return true;
        }

        _State = this;
        return true;
    }

    public override void Run(Battle_Character b_c)
    {
        b_c.animator.Play("Walk");
        b_c.real_AI.navMesh.SetDestination(b_c.cur_Target.transform.position);
    }
}

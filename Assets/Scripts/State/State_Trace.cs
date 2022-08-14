using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Trace : State
{
    public override bool Judge(out State _State, Battle_Character b_c)
    {
        //if (Vector3.Distance(b_c.transform.position,
        //    b_c.cur_Target.transform.position)
        //    <= b_c.Attackable_Range) // 타겟을 공격할 수 있는 사거리 내 진입했다면
        //{
        //    _State = Trans_List[0];
        //    b_c.real_AI.pre_State = this;
        //    return false;
        //}

        _State = this;
        return true;
    }

    public override void Run(Battle_Character b_c)
    {
        b_c.animator.Play("Walk");
        b_c.real_AI.navMesh.SetDestination(b_c.cur_Target.transform.position);
    }
}

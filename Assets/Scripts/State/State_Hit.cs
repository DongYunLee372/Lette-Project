using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Hit : State
{
    public override bool Judge(out State _State, Battle_Character b_c)
    {
        if (b_c.isHit) // 방금 맞았다면
        {
            b_c.real_AI.pre_State = b_c.real_AI.now_State;
            _State = this;
            return true;
        }

        if (b_c.real_AI.pre_State == this)
            _State = b_c.real_AI.now_State;
        else
            _State = null;

        return false;
    }

    public override void Run(Battle_Character b_c)
    {
        // 피격시 처리.
        b_c.isHit = false;

        b_c.real_AI.now_State = b_c.real_AI.pre_State;
        b_c.real_AI.pre_State = this;
    }
}

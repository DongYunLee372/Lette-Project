using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Die : State
{
    public override bool Judge(out State _State, Battle_Character b_c)
    {
        //if (b_c.char_Info.P_player_HP <= 0) // 사망
        //{
        //    _State = this;
        //    return true;
        //}

        _State = null;
        return false;
    }

    public override void Run(Battle_Character b_c)
    {
        // 사망 애니메이션 등 사망 시 부활하고 소환하는 등등의 효과(스킬)을 호출. 
        // 스킬 구조 구현 시 추가해줘야함.

        b_c.real_AI.now_State = Trans_List[0];
    }
}

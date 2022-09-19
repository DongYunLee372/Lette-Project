using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Hit : State
{
    public override bool Judge(out State _State, Battle_Character battle_character)
    {
        if (battle_character.isHit) // 방금 맞았다면
        {
            battle_character.real_AI.pre_State = battle_character.real_AI.now_State;
            _State = this;
            return true;
        }

        if (battle_character.real_AI.pre_State == this)
            _State = battle_character.real_AI.now_State;
        else
            _State = null;

        return false;
    }

    public override void Run(Battle_Character battle_character)
    {
        // 피격시 처리.
        battle_character.isHit = false;

        battle_character.real_AI.now_State = battle_character.real_AI.pre_State;
        battle_character.real_AI.pre_State = this;
    }
}

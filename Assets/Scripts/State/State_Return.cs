using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Return : State
{
    public override bool Judge(out State _State, Battle_Character battle_character)
    {
        if (battle_character.isReturn)
        {
            if ((Vector3.Distance(battle_character.transform.position, battle_character.return_Pos) <= 0.5f))
            {
                _State = Trans_List[0];
                battle_character.real_AI.pre_State = this;
                return false;
            }

            _State = this;
            return true;
        }

        _State = null;
        return false;
    }

    public override void Run(Battle_Character battle_character)
    {
        battle_character.animator.Play("Walk"); // 애니메이션 시스템을 통해 애니메이션 재생
        battle_character.real_AI.navMesh.SetDestination(battle_character.return_Pos);
    }
}

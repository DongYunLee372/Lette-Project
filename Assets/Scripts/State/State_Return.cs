using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Return : State
{
    public override bool Judge(out State _State, Battle_Character b_c)
    {
        if (b_c.isReturn)
        {
            if ((Vector3.Distance(b_c.transform.position, b_c.return_Pos) <= 0.5f))
            {
                _State = Trans_List[0];
                b_c.real_AI.pre_State = this;
                return false;
            }

            _State = this;
            return true;
        }

        _State = null;
        return false;
    }

    public override void Run(Battle_Character b_c)
    {
        b_c.animator.Play("Walk");
        b_c.real_AI.navMesh.SetDestination(b_c.return_Pos);
    }
}

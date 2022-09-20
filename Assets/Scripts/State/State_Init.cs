using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Init : State
{
    public override bool Judge(out State _State, Battle_Character battle_character)
    {
        _State = Trans_List[0]; // 0번은 init에서 전이될 스테이트
        return false;
    }

    public override void Run(Battle_Character battle_character)
    {

    }

    public List<State> Init_State_Initialize()
    {
        List<State> state_List = new List<State>();

        for (int i = 1; i < Trans_List.Count; i++)
        {
            state_List.Add(Trans_List[i]); // 0번 뒤로 1번부터는 선행 조건 행동(State)들을 넣어놓고 AI에게 넘겨줌.
        }

        return state_List;
    }
}

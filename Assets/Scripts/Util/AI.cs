using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class AI
{
    public Battle_Character battle_character;

    public State Init_State; // 초기화 할때 사용할 스테이트(State_Init)를 등록

    public State pre_State;
    public State now_State;
    public List<State> pre_State_List;
    public NavMeshAgent navMesh;

    public bool isPause = false; // 정지

    public void AI_Init(Battle_Character battle_character)
    {
        this.battle_character = battle_character;

        navMesh = battle_character.GetComponent<NavMeshAgent>();

        // now_State = b_c.init_state << 이런식으로 배틀 캐릭터에서 어드레서블로 불러온 State_Init 을 넣어주면 자연스럽게 연결된 스테이트들도 같이 붙는다.
        // 그리고 Init을 통해 해당 AI에 필요한
        // pre_State_List를 넣어줌.
        // pre_State_List = new List<State>();
        pre_State_List = Init_State.GetComponent<State_Init>().Init_State_Initialize();

        pre_State = now_State;
    }

    public void AI_Update()
    {
        if (isPause) // BattleCharacter에서 AI를 정지시켰다면 정지
            return;

        foreach (var st in pre_State_List)
        {
            State temp_State = now_State;

            if (st.Judge(out now_State, battle_character))
            {
                st.Run(battle_character);
                return;
            }
            else // 위의 판단에서 반환값으로 null 받았을경우에 다시 상태를 넣어주기 위함.
            {
                now_State = temp_State;
            }
        }

        if (now_State.Judge(out now_State, battle_character))
        {
            if (!now_State.first_Start)
            {
                now_State.State_Initialize(battle_character);
                now_State.first_Start = true;
            }

            now_State.Run(battle_character);
        }
    }
}

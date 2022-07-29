using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class AI
{
    public Battle_Character b_c;

    public State now_State;
    public List<State> pre_State_List;
    public NavMeshAgent navMesh;

    public bool isPause = false; // 정지

    public void AI_Init(Battle_Character b_c)
    {
        this.b_c = b_c;

        navMesh = b_c.GetComponent<NavMeshAgent>();

        // now_State = b_c.init_state << 이런식으로 배틀 캐릭터에서 어드레서블로 불러온 State_Init 을 넣어주면 자연스럽게 연결된 스테이트들도 같이 붙는다.
        // 그리고 Init을 통해 해당 AI에 필요한
        // pre_State_List를 넣어줌.
        pre_State_List = new List<State>();
        pre_State_List = now_State.State_Initialize();
    }

    public void AI_Update()
    {
        if (isPause) // BattleCharacter에서 AI를 정지시켰다면 정지
            return;

        foreach (var st in pre_State_List)
        {
            State temp_State = now_State;

            if (st.Judge(out now_State, b_c))
            {
                st.Run(b_c);
                return;
            }
            else // 위의 판단에서 반환값으로 null 받았을경우에 다시 상태를 넣어주기 위함.
            {
                now_State = temp_State;
            }
        }

        if (now_State.Judge(out now_State, b_c))
        {
            now_State.Run(b_c);
        }
    }
}

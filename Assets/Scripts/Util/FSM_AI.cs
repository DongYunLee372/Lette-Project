using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State // ������Ʈ 
{
    Init,
    Patrol_Enter,
    Patrol,
    Patrol_Exit,
    Trace_Enter,
    Trace,
    Trace_Exit,
    Attack_Enter,
    Attack,
    Attack_Exit,
    Die_Enter,
    Die,
    Die_Exit,
    Return,
    Wait,
}

[System.Serializable]
public class FSM_AI
{
    public State now_State;
    public Battle_Character battle_Character;

    public void AI_Initialize(Battle_Character bc) // AI �ʱ�ȭ �Լ�. �� �Լ��� ȣ���ؼ� �ʱ�ȭ�� �������.
    {
        now_State = State.Init;
        battle_Character = bc;
    }

    public State AI_Update() // �� ������Ʈ �Լ��� ȣ���ؼ� ���� State�� ���� �Ǵ� �� �Ǵ� ���(����)�� return ����.
    {
        switch (now_State)
        {
            case State.Init:
                now_State = State.Patrol_Enter;
                break;
            case State.Patrol_Enter:
                now_State = State.Patrol;
                break;
            case State.Patrol:
                Collider[] cols = Physics.OverlapSphere(battle_Character.transform.position, 10f);
                //, 1 << 8); // ��Ʈ �����ڷ� 8��° ���̾�

                if (cols.Length > 0)
                {
                    for (int i = 0; i < cols.Length; i++)
                    {
                        if (cols[i].tag == "Player")
                        {
                            battle_Character.cur_Target = cols[i].gameObject;
                            now_State = State.Trace;
                        }
                    }
                }
                break;
            case State.Trace:
                if (Vector3.Distance(battle_Character.transform.position, battle_Character.cur_Target.transform.position) <= battle_Character.Attack_Range) // Ÿ�ٿ� ��Ҵٸ�
                {
                    now_State = State.Attack; // ���� ���·� ����
                }
                break;
            case State.Attack:
                if (!(Vector3.Distance(battle_Character.transform.position, battle_Character.cur_Target.transform.position) <= battle_Character.Attack_Range)) // ���� �Ÿ� ���� �ִٸ� 
                {
                    now_State = State.Trace;
                }
                break;
            case State.Die_Enter:
                now_State = State.Die;
                break;
            case State.Die:
                now_State = State.Wait;
                break;
            case State.Return:
                if ((Vector3.Distance(battle_Character.transform.position, battle_Character.destination_Pos) <= 0.5f)) // ���� �Ÿ� ���� �ִٸ� 
                {
                    now_State = State.Init;
                }
                break;
        }

        // any state 
        if (battle_Character.Cur_HP <= 0 && now_State != State.Wait && now_State != State.Die)
        {
            now_State = State.Die_Enter;
        }

        return now_State;
    }

    public void Return_Set()
    {
        now_State = State.Return;
    }
}

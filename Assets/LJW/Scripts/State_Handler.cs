using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State_Handler : MonoBehaviour
{
    public State state;

    [SerializeField]
    protected Battle_Character battle_Character;

    public abstract void State_Handler_Update();

    public void State_Handler_Initialize(Battle_Character b_c)
    {
        battle_Character = b_c;

        StartCoroutine(Mana_Regen());
    }

    protected abstract void Patrol_Enter_Process();

    protected abstract void Patrol_Process();

    protected abstract void Patrol_Exit_Process();

    protected abstract void Trace_Process();

    protected abstract void Attack_Process();

    protected IEnumerator patrol_Think_Coroutine()  // ���� ������ �����ϴ� �ڷ�ƾ
    {
        yield return new WaitForSeconds(1f);

        int randX = Random.Range(-10, 10);
        int randZ = Random.Range(-10, 10);

        battle_Character.destination_Pos = new Vector3(battle_Character.return_Pos.x + randX, battle_Character.return_Pos.y, battle_Character.return_Pos.z + randZ);

        battle_Character.patrol_Start = false;
    }

    protected void Destination_Move(Vector3 in_destination_Pos)
    {
        battle_Character.transform.position = Vector3.MoveTowards(battle_Character.transform.position,
                                                                 in_destination_Pos,
                                                                 Time.deltaTime * 5f);

        if (Vector3.Distance(battle_Character.transform.position, in_destination_Pos) <= 0.5f)
        {
            //if (cur_State == 4)
            //    anim.SetBool("isReturn", false);
            //else
            //  anim.SetBool("isWalk", false);
        }
        else
        {
            //if (cur_State == 4)
            //    anim.SetBool("isReturn", true);
            //else
            //    anim.SetBool("isWalk", true);

            battle_Character.transform.LookAt(in_destination_Pos);
        }
    }

    protected virtual IEnumerator Mana_Regen() // ���� ��� �Լ�. virtual �̹Ƿ� ���Ϳ� ���� ���� ȹ�淮 �ٸ��� �� ���� ����. ������ ���ͺ� ���� ȹ�淮 �𸣴ϱ� ����
    {
        yield return new WaitForSeconds(1f);

        battle_Character.Mana += 5; // ���� ���� ������� �����ָ� �ɵ�.

        StartCoroutine(Mana_Regen());
    }

    protected void Enemy_Skill_Rand()
    {
        battle_Character.next_Skill = Random.Range(1, 3); // ��ų ������ �����͸� �޾ƿͼ� ���� ��ų �� ��ŭ �߿� �������� ������
        battle_Character.Mana = 0;
    }
}

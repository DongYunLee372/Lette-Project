using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner : Enemy
{
   
    void Start()
    {
        parent_Init();
    }

    protected override void Enemy_FSM()
    {
        switch (cur_State)
        {
            case 1:
                Enemy_Patrol();
                break;
            case 2:
                Enemy_Trace();
                break;
            case 3:
                Enemy_Attack();
                break;
            case 4:
                Enemy_Return();
                break;
        }
    }

    protected override void Enemy_Attack()
    {
        if (Mana >= need_Mana)
        {
            switch (next_Skill)
            {
                case 1: // 1�� ��ų
                    break;
                case 2: // 2�� ��ų
                    break;
                    // ��ų�� ���� ����
            }
        }
        else // �⺻ ����
        {
            if (Vector3.Distance(transform.position, cur_Target.transform.position) <= Attack_Range) // ���� �Ÿ� ���� �ִٸ� 
            {
                anim.SetBool("isWalk", false);
                anim.SetTrigger("isAttack");
            }
            else // ���� �Ÿ� �ܿ� �ִٸ�
            {
                cur_State = 2; // ���� state�� ����
            }
        }
    }

    void Update()
    {
        Enemy_FSM();
    }
}

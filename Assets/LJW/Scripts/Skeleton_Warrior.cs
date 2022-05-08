using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Warrior : Enemy
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

    void Update()
    {
        Enemy_FSM();
    }

    protected override void Enemy_Attack()
    {
        if(Mana >= need_Mana)
        {
            switch (next_Skill)
            {
                // ��ų�� ���� ����
            }
        }
        else // �⺻ ����
        {
            if(Vector3.Distance(transform.position, cur_Target.transform.position) <= Attack_Range) // ���� �Ÿ� ���� �ִٸ� 
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
}

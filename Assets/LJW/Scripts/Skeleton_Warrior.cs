using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Warrior : Enemy
{
    void Start()
    {
        parent_Init();
        Enemy_Skill_Rand();

        StartCoroutine(Mana_Regen());
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

        if (Input.GetKeyDown(KeyCode.F3))
            Enemy_Die();
    }

    protected override void Enemy_Attack()
    {
        if (Mana >= need_Mana)
        {
            switch (next_Skill)
            {
                case 1: // 1�� ��ų
                    Skeleton_Warrior_Skill_1();
                    break;
                case 2: // 2�� ��ų
                    Skeleton_Warrior_Skill_2();
                    break;
                    // ��ų�� ���� ����
            }
            Enemy_Skill_Rand();
        }
        else // �⺻ ����
        {
            if (Vector3.Distance(transform.position, cur_Target.transform.position) <= Attack_Range) // ���� �Ÿ� ���� �ִٸ� 
            {
                anim.SetBool("isWalk", false);
                anim.SetBool("isAttack", true);
            }
            else // ���� �Ÿ� �ܿ� �ִٸ�
            {
                cur_State = 2; // ���� state�� ����
                anim.SetBool("isAttack", false);
            }
        }
    }

    protected override void Enemy_Skill_Rand()
    {
        next_Skill = Random.Range(1, 3);
        Mana = 0;
    }

    protected override void Enemy_Die()
    {
        enemy_isDie = true;
        cur_State = -1;

        AnimatorControllerParameter[] parameters = anim.parameters;

        foreach (var parameter in parameters)
        {
            anim.SetBool(parameter.name, false);
        }
        anim.SetBool("isDie", true);

        StartCoroutine(Skeleton_Warrior_Revival());
    }

    IEnumerator Skeleton_Warrior_Revival()
    {
        yield return new WaitForSeconds(5f);

        anim.SetBool("isDie", false);

        yield return new WaitForSeconds(2f);
        enemy_isDie = false;
        cur_State = 1;
    }

    void Skeleton_Warrior_Skill_1() // ���̷��� ������ 1����ų ����ġ��
    {
        Debug.Log("����ġ�� �ߵ�");
    }

    void Skeleton_Warrior_Skill_2() // ���̷��� ������ 2����ų ���
    {
        Debug.Log("��� �ߵ�");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Knight : Enemy
{
    [SerializeField]
    private GameObject spawn_Skeleton_Prefab; // ��ȯ�� ���̷��� ���� ������

    // ���и� ��� �̵��ϰ� ���и� ���ø� �ʿ��� �Դ� �������� �����ϱ� ������ �ݶ��̴��� �ϳ� �� ����������.
    void Start()
    {
        parent_Init();
        Enemy_Skill_Rand();

        StartCoroutine(Mana_Regen());
    }

    void Update()
    {
        //
        Enemy_FSM();

        if (Input.GetKeyDown(KeyCode.F1))
            Skeleton_Knight_Skill_3();
    }

    protected override void Enemy_Attack()
    {
        if (Mana >= need_Mana)
        {
            switch (next_Skill)
            {
                case 1: // 1�� ��ų
                    Skeleton_Knight_Skill_1();
                    break;
                case 2: // 2�� ��ų
                    Skeleton_Knight_Skill_2();
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

    protected override void Enemy_Skill_Rand()
    {
        next_Skill = Random.Range(1, 4);
        Mana = 0;
    }

    void Skeleton_Knight_Skill_1() // ���̷��� ����Ʈ 1����ų 
    {
        Debug.Log("��Ÿ �ߵ�");
        // ������ Į�� ���� ġ�� �ִϸ��̼�
    }

    void Skeleton_Knight_Skill_2() // ���̷��� ����Ʈ 2����ų 
    {
        Debug.Log("����ī���� �ߵ�");
        // ������ ���з� ���� �о� �÷�ġ�� �ִϸ��̼�
    }

    void Skeleton_Knight_Skill_3() // ���̷��� ����Ʈ 3����ų ( �нú� ��ų : ü�� 50 % �̸��� �Ǹ� �ߵ� ) 
    {
        Debug.Log("��ȿ �ߵ�");
        // Į�� ���и� 2�� ģ �� ������ ���� ä ��ȿ�� �������� �ִϸ��̼�.

        // ���� 8���� ��ȯ
        // ��ȯ�Ǵ� ������ ���ӿ��� �ö���� �ִϸ��̼� ����
        for (int i = 0; i < 8; i++)
        {
            GameObject spawned_enemy = GameObject.Instantiate(spawn_Skeleton_Prefab);
            spawned_enemy.gameObject.name = i.ToString() + "��°";

            Vector3 v = new Vector3(Mathf.Sin(30.0f * i) * 5.0f, 0, Mathf.Cos(30.0f * i) * 5.0f);

            spawned_enemy.transform.position = transform.position + v;
        }
    }
}

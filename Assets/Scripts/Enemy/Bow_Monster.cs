using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow_Monster : Battle_Character
{
    [SerializeField]
    private float Knife_Range; // ���� ��Ÿ� 

    void Start()
    {
        Initalize();
        ai.AI_Initialize(this);
    }

    void Update()
    {
        state_handler.state = ai.AI_Update();
        state_handler.State_Handler_Update();
    }

    public override void Skill_1() // ���� ���ݴ� 1����ų ���
    {
        Debug.Log("��� �ߵ�");
    }

    public override void Die_Process() // ������ ȣ��Ǵ� �Լ�
    {

    }

    public override void Attack_Process()
    {
        if (Player_Mana >= need_Mana)
        {
            switch (next_Skill)
            {
                case 1: // 1�� ��ų
                    Skill_1();
                    break;
                    // ��ų�� ���� ����
            }
            //Enemy_Skill_Rand(); // ���� ��ų ã��
        }
        else // �⺻ ����
        {
            // �⺻ ���� �ڵ�
            //anim.SetBool("isAttack", true);
            if (Vector3.Distance(transform.position, cur_Target.transform.position) <= Knife_Range) // ���� ��Ÿ� ���� ������ �ִٸ�
            {
                // ��������
            }
            else
            {
                // Ȱ ����
            }
        }
    }
}

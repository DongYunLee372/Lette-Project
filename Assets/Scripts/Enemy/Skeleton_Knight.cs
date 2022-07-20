using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Knight : Battle_Character
{
    [SerializeField]
    private GameObject spawn_Skeleton_Prefab; // ��ȯ�� ���̷��� ���� ������

    [SerializeField]
    private bool isPassive = false; // �нú� ��ų ( ��ų 3�� ) �� �ߵ��Ǿ����� üũ�� bool ����

    // ���и� ��� �̵��ϰ� ���и� ���ø� �ʿ��� �Դ� �������� �����ϱ� ������ �ݶ��̴��� �ϳ� �� ����������.
    void Start()
    {
        Initalize();
        ai.AI_Initialize(this);
    }

    void Update()
    {
        state_handler.state = ai.AI_Update();
        state_handler.State_Handler_Update();

        if (Input.GetKeyDown(KeyCode.Space))
            Damaged(5);
    }

    public override void Skill_1() // ���̷��� ����Ʈ 1����ų 
    {
        Debug.Log("��Ÿ �ߵ�");
        // ������ Į�� ���� ġ�� �ִϸ��̼�
    }

    public override void Skill_2() // ���̷��� ����Ʈ 2����ų 
    {
        Debug.Log("����ī���� �ߵ�");
        // ������ ���з� ���� �о� �÷�ġ�� �ִϸ��̼�
    }

    public override void Skill_3()  // ���̷��� ����Ʈ 3����ų ( �нú� ��ų : ü�� 50 % �̸��� �Ǹ� �ߵ� ) 
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

    public override void Damaged(float damage_Amount)
    {
        base.Damaged(damage_Amount);

        Debug.Log("��������");

        if (Cur_HP <= (Max_HP / 2) && !isPassive)
        {
            isPassive = true;
            Skill_3();
        }
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
                case 2: // 2�� ��ų
                    Skill_2();
                    break;
                    // ��ų�� ���� ����
            }
            //Enemy_Skill_Rand(); // ���� ��ų ã��
        }
        else // �⺻ ����
        {
            // �⺻ ���� �ڵ�
            //anim.SetBool("isAttack", true);
        }
    }
}

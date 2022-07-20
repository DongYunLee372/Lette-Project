using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Battle_Character
{
    public bool isDivide = false; // �ѹ��ۿ� �п����� ���ϹǷ� �п��� �̹� �� ���������� üũ�ϱ� ����.

    public bool OnTree = true; // ���� ���� �ִ���

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, mon_find_Range);
    }

    public override void Skill_1() // ������ 1�� ��ų ( ��� ) 
    {
        Debug.Log("��� �ߵ�");
    }

    public override void Skill_2() // �п� ( ������ ��Ȱ )  
    {
        Debug.Log("�п� �ߵ�");
        // ������ 2���� ��������
        // ������ ������ ũ��� ü�� ���� Slime_Devide_Init �Լ� �������ֱ� 
    }

    public override void Die_Process() // ������ ȣ��Ǵ� �Լ� (��Ȱ ó���ؾ���)
    {
        Skill_2();
    }

    public void Slime_Devide_Init(Slime slime)
    {
        slime.isDivide = true;

        slime.transform.localScale *= 0.5f;

        slime.Max_HP = slime.Max_HP * 0.5f;

        slime.Cur_HP = slime.Max_HP;
    }
}

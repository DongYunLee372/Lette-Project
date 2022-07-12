using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Warrior : Battle_Character
{
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

    //protected override void Enemy_Die()
    //{
    //    enemy_isDie = true;
    //    cur_State = -1;

    //    AnimatorControllerParameter[] parameters = anim.parameters;

    //    foreach (var parameter in parameters)
    //    {
    //        anim.SetBool(parameter.name, false);
    //    }
    //    anim.SetBool("isDie", true);

    //    StartCoroutine(Skeleton_Warrior_Revival());
    //}

    //IEnumerator Skeleton_Warrior_Revival()
    //{
    //    yield return new WaitForSeconds(5f);

    //    anim.SetBool("isDie", false);

    //    yield return new WaitForSeconds(2f);
    //    enemy_isDie = false;
    //    cur_State = 1;
    //}


    public override void Skill_1() // ���̷��� ������ 1����ų ����ġ��
    {
        Debug.Log("����ġ�� �ߵ�");
    }

    public override void Skill_2() // ���̷��� ������ 2����ų ���
    {
        Debug.Log("��� �ߵ�");
    }

    public override void Die_Process() // ������ ȣ��Ǵ� �Լ� (��Ȱ ó���ؾ���)
    {
        StartCoroutine(Skeleton_Warrior_Revival());
    }

    IEnumerator Skeleton_Warrior_Revival()
    {
        // ����ϴ� �ִϸ��̼� ó��

        yield return new WaitForSeconds(5f); // 5�� �ȿ� ������ �ʾҴٸ�

        Debug.Log("��Ȱ��Ȱ");
        // ��Ȱ�ϴ� �ִϸ��̼� ��� �� 
        ai.AI_Initialize(this);

        Cur_HP = 100;
    }
}

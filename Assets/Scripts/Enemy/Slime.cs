using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Battle_Character
{
    public bool isDivide = false; // �ѹ��ۿ� �п����� ���ϹǷ� �п��� �̹� �� ���������� üũ�ϱ� ����.

    public bool OnTree = true; // ���� ���� �ִ���

    public bool isJump = false; // ���� ������ üũ

    void Start()
    {
        Initalize();
        ai.AI_Initialize(this);
    }

    Vector3 dirvec;
    void Update()
    {
        state_handler.state = ai.AI_Update();
        state_handler.State_Handler_Update();

        if (cur_Target != null)
        {
            dirvec = cur_Target.transform.position - transform.position;
            dirvec += new Vector3(0, 8, 0);
        }
        if (Input.GetKeyDown(KeyCode.Space))
            GetComponent<Rigidbody>().AddForce(dirvec * 200f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, mon_find_Range);
    }

    public override void Skill_1() // ������ 1�� ��ų ( ��� ) 
    {
        Debug.Log("��� �ߵ�");

        state_handler.navMesh.enabled = false;

        Vector3 dirvec = cur_Target.transform.position - transform.position;
        dirvec += new Vector3(0, 8, 0);

        GetComponent<Rigidbody>().AddForce(dirvec * 500f);

        ai.now_State = State.Next_Wait;

        Player_Mana = 0;

        StartCoroutine(Skill_Coroutine());
    }

    IEnumerator Skill_Coroutine()
    {
        yield return new WaitForSeconds(2f);

        state_handler.navMesh.enabled = true;
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        ai.now_State = State.Patrol_Enter;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appoint_fireteam : Enemy
{
    public int general_attack_damage;  //�Ϲݰ��� ������

   private float rush_speed=1000f;

    bool isDelay = true;
    float delayTime = 1f;
    float timer = 0f;


    void Start()
    {
        parent_Init();
    }


    void Update()
    {
        Enemy_FSM();

        if (isDelay)
        {
            timer += Time.deltaTime;
            if (timer >= delayTime)
            {
                timer = 0f;
                Mana += 10;
            }
        }
    }

    void general_attack() //�Ϲݰ���
    {
        //�˹�
        //������
        //�˹� ĳ���� ��ġ�� ����
        //������ ����?
        //������ ĳ���� hp���� ����ؼ� ĳ�� ��ġ�� ����

        Debug.Log("�Ϲݰ���");
    }

    private void FixedUpdate()
    {
        Vector3 dir;

        if(cur_State==5)
        {
            dir = (cur_Target.transform.position - transform.position).normalized;
            Debug.Log("�뽬");
            GetComponent<Rigidbody>().AddForce(dir * rush_speed);
        }
      
    }

    IEnumerator attack()
    {
        anim.SetBool("Melee Attack 02", true);
        yield return new WaitForSeconds(2f);
        anim.SetBool("Shockwave Attack", true);
        cur_State = 3;
    }

    void Rush_pierce()  //�������
    {
        //������ ������ ������
        //������ ��?
        //ĳ���� ������ ��ġ�� ��
        anim.SetBool("isAttack", false);
        anim.SetTrigger("isWalk");

        Vector3 dir = (cur_Target.transform.position - transform.position).normalized;

        if (Vector3.Distance(transform.position, cur_Target.transform.position) <= 1.5f)
        {
            anim.SetBool("isWalk", false);
            anim.SetTrigger("isAttack");
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            Debug.Log("���� ����");
            next_Skill = 0;
            cur_State = 6;


            //2��Ÿ
            //anim.SetBool("isAttack", false);
            //anim.SetBool("MeleeAttack02", true);
        }

        Debug.Log("2��Ÿ ����");
        
        StartCoroutine(attack());

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
            case 5:

                if(next_Skill!=0)
                {
                    Rush_pierce();
                }

                break;
            case 6:
               // anim.SetBool("Melee Attack 02", true);
                break;

        }
    }

    protected override void Enemy_Attack()
    {
        if (Mana >= need_Mana)
        {
            if(next_Skill==0)
            {
                next_Skill = Random.Range(1, 3);
            }
            switch (next_Skill)
            {
                case 1: // 1�� ��ų
                    cur_State = 5;
                  //  next_Skill = 0;
                    break;
                case 2: // 2�� ��ų
                    general_attack();
                    next_Skill = 0;

                    break;
                    // ��ų�� ���� ����
            }
            Mana = 0;

        }
        else // �⺻ ����
        {
            if (Vector3.Distance(transform.position, cur_Target.transform.position) <= Attack_Range) // ���� �Ÿ� ���� �ִٸ� 
            {
                anim.SetBool("isWalk", false);
                anim.SetTrigger("isAttack");
                //Attack_Mana();
            }
            else // ���� �Ÿ� �ܿ� �ִٸ�
            {
                cur_State = 2; // ���� state�� ����
            }
        }
    }


   
}

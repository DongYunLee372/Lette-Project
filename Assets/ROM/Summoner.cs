using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner : Enemy
{

    public GameObject SusuPrefabs;
    public GameObject ShootingStarPrefabs;


    void Start()
    {
        parent_Init();
    }

    void Attack_Mana()
    {
        Mana += 5;
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
            next_Skill = Random.Range(1, 3);
            switch (next_Skill)
            {
                case 1: // 1�� ��ų
                    susu_Summons();
                    break;
                case 2: // 2�� ��ų
                    ShootingStar();
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

    void Update()
    {
        Enemy_FSM();
    }


    void susu_Summons()
    {
        Instantiate(SusuPrefabs, new Vector3(transform.position.x, transform.position.y, transform.position.z+20f),Quaternion.identity);
    }

    void ShootingStar()
    {
        for (int i = 1; i < 6; i++)
        {
            Instantiate(ShootingStarPrefabs, new Vector3(transform.position.x+i*5, transform.position.y+20, transform.position.z + 20f), Quaternion.identity);
        }

    }

}

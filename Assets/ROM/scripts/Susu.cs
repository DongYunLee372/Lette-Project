using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Susu : Enemy
{

    public GameObject ShootingStarPrefabs;
    public int c = 0;
    public float rush_length = 2f;
    float rush_speed = 1000f;
    bool return_savepoint=false;
    int skill_coolingTime = 0;  //��ų ��Ÿ�� ����

    bool isDelay=true;
    float delayTime = 1f;  
    float timer = 0f;

    Vector3 savePoint;
    // Start is called before the first frame update
    void Start()
    {
        parent_Init();
    }


    private void Hang()
    {
      

        anim.SetBool("isAttack", false);
        anim.SetTrigger("isWalk");


        Vector3 dir = (cur_Target.transform.position - transform.position).normalized;


        if (Vector3.Distance(transform.position, cur_Target.transform.position) <= 2f)
        {
            anim.SetBool("isWalk", false);
            anim.SetTrigger("isAttack");
           
            next_Skill = 0;
            cur_State = 3;
            return_savepoint = true;
            StartCoroutine(ch());

        }
    }



    IEnumerator ch()
    {
        //���� �� �Ŵ޸���
        //�Ŵ޸��� ��������?
        //�����Ĵ� �ٵ�... ���� ������? �Ŵ޸��ְ�??
        //������ ������ ���ݹ���?

        //������ ���� ��
        //�÷��̾� hp ���� 
        //������ ����?
        Debug.Log("����");
        //�� �ٽ� ����
        yield return new WaitForSeconds(2f);
        cur_State = 5;


        
    }

    private void FixedUpdate()
    {
        if (cur_State == 5)
        {
            Vector3 dir;
            if (return_savepoint)
            {

                 dir = (savePoint - transform.position).normalized;

                Debug.Log("����");
                GetComponent<Rigidbody>().AddForce(dir * rush_speed);
              
                // transform.position = Vector3.MoveTowards(transform.position, savePoint, Time.deltaTime * 10f);

            }
            else
            {
                 dir = (cur_Target.transform.position - transform.position).normalized;
                Debug.Log("�뽬");
                GetComponent<Rigidbody>().AddForce(dir * rush_speed);
              
            }
        }
    }

    private void Meteor()
    {
        if (cur_Target != null)
        {
            GameObject temp = Instantiate(ShootingStarPrefabs, new Vector3(transform.position.x, transform.position.y + 5, transform.position.z), Quaternion.identity);
            temp.GetComponent<Shooting>().eixst = true;
            temp.GetComponent<Shooting>().Shooting_target(cur_Target.transform.position);
        }
    }

    private void Approach_prohibition()
    {
        //���Ȱ� ����?
        //ĳ���� hp (��ų ������?) 
        //��������?
       

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
                
                if (next_Skill != 0)
                {
                    Hang();
                }
                if (Vector3.Distance(transform.position, cur_Target.transform.position) >= Attack_Range)
                {
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                    cur_State = 3;
                    return_savepoint = false;
                }
                break;

        }
    }

    protected override void Enemy_Attack()
    {
        if (Mana >= need_Mana)
        {
            if (next_Skill == 0)
            {
                next_Skill = Random.Range(1, 3);
            }

            switch (next_Skill)
            {
                case 1: // 1�� ��ų
                    Meteor();
                    next_Skill = 0;
                    break;
                case 2: // 2�� ��ų
                  
                    savePoint = transform.position;
                    cur_State = 5;
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

    // Update is called once per frame
    void Update()
    {
        Enemy_FSM();

        if(isDelay)
        {
            timer += Time.deltaTime;
            if(timer>=delayTime)
            {
                timer = 0f;
                Mana += 1;
            }
        }

    }
}

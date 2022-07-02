using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_Character : State_Handler
{
    [SerializeField]
    protected FSM_AI ai = new FSM_AI();

    public GameObject cur_Target;
    [SerializeField]
    protected Vector3 return_Pos; // ������ ��ġ
    protected Vector3 destination_Pos; // Patrol ������
    public float Attack_Range; // ��Ÿ�
    protected bool patrol_Start = false; // Ž�� ����
    protected Animator anim;

    protected void Initalize()
    {
        return_Pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        destination_Pos = transform.position;
        //anim = GetComponent<Animator>();
    }

    protected override void Patrol_Enter_Process()
    {
        patrol_Start = false;
    }

    protected override void Patrol_Process()
    {
        if (Vector3.Distance(transform.position, destination_Pos) == 0f)
        {
            if (!patrol_Start)
            {
                StartCoroutine(patrol_Think_Coroutine());
                patrol_Start = true;
            }
        }
        else
        {
            Destination_Move(destination_Pos);
        }
    }

    protected override void Trace_Process()
    {
        //if (Vector3.Distance(transform.position, cur_Target.transform.position) <= Attack_Range) // Ÿ�ٿ� ��Ҵٸ�
        //{
        //    cur_State = 3; // ���� ���·� ����
        //}
        //else
        //{
        Destination_Move(cur_Target.transform.position);
        //}
    }

    protected override void Attack_Process()
    {

    }

    IEnumerator patrol_Think_Coroutine()  // ���� ������ �����ϴ� �ڷ�ƾ
    {
        yield return new WaitForSeconds(1f);

        int randX = Random.Range(-10, 10);
        int randZ = Random.Range(-10, 10);

        destination_Pos = new Vector3(return_Pos.x + randX, return_Pos.y, return_Pos.z + randZ);

        patrol_Start = false;
    }

    void Destination_Move(Vector3 in_destination_Pos)
    {
        transform.position = Vector3.MoveTowards(transform.position,
                                                                 in_destination_Pos,
                                                                 Time.deltaTime * 5f);

        if (Vector3.Distance(transform.position, in_destination_Pos) <= 0.5f)
        {
            //if (cur_State == 4)
            //    anim.SetBool("isReturn", false);
            //else
            //  anim.SetBool("isWalk", false);
        }
        else
        {
            //if (cur_State == 4)
            //    anim.SetBool("isReturn", true);
            //else
            //    anim.SetBool("isWalk", true);

            transform.LookAt(in_destination_Pos);
        }
    }
}

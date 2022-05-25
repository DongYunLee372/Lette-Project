using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy Info")]
    [SerializeField]
    protected float moveSpeed;
    [SerializeField]
    protected float Attack_Range; // �⺻ ���� ��Ÿ�
    [SerializeField]
    protected int Mana; // ���� ����
    [SerializeField]
    protected int need_Mana; // ��ų ���� �ʿ��� ����
    [Header("Enemy Now State")]
    [SerializeField]
    protected bool is_Target_Set; // Ÿ���� �������� ���ͼ� �������ִٸ�
    [SerializeField]
    protected Vector3 return_Pos; // ������ ��ġ
    [SerializeField]
    protected int cur_State; // ���� ���� 1 : ���� 2 : ���� 3 : ���� 4 : ����
    [SerializeField]
    protected GameObject cur_Target;
    [SerializeField]
    protected int next_Skill;

    protected Animator anim;

    protected void parent_Init()
    {
        return_Pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        destination_Pos = transform.position;
        anim = GetComponent<Animator>();
        cur_State = 1;
    }

    [SerializeField]
    protected Vector3 destination_Pos;
    protected bool patrol_Start = false; // Ž�� ����
    protected void Enemy_Patrol()
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

        Collider[] cols = Physics.OverlapSphere(transform.position, 10f);  //, 1 << 8); // ��Ʈ �����ڷ� 8��° ���̾�

        if (cols.Length > 0)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].tag == "Player")
                {
                    cur_Target = cols[i].gameObject;
                    cur_State = 2; // ���� ���·� ����
                }
            }
        }
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
                                                                 Time.deltaTime * moveSpeed);
       
        if (Vector3.Distance(transform.position, in_destination_Pos) <= 0.5f)
        {
            anim.SetBool("isWalk", false);
        }
        else
        {
            anim.SetBool("isWalk", true); 
            transform.LookAt(in_destination_Pos);
        }
    }

    protected void Enemy_Trace() // ���� �Լ�
    {
        if (Vector3.Distance(transform.position, cur_Target.transform.position) <= Attack_Range) // Ÿ�ٿ� ��Ҵٸ�
        {
            cur_State = 3; // ���� ���·� ����
        }
        else
        {
            Destination_Move(cur_Target.transform.position);
        }
    }

    public void Enemy_Return_Set()
    {
        cur_State = 4;
    }

    protected void Enemy_Return()
    {
        if (Vector3.Distance(transform.position, return_Pos) == 0)
        {
            cur_State = 1; // ���� �Ϸ� �� �ٽ� ��������
            StartCoroutine(patrol_Think_Coroutine());
            patrol_Start = true;
        }
        else
        {
            Destination_Move(return_Pos);
        }
    }

    abstract protected void Enemy_FSM();

    abstract protected void Enemy_Attack(); // �ȿ��� ������ ��á���� ����� ��ų�� �ߵ�.

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 10f);
    }

    void Update()
    {

    }
}

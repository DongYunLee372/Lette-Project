using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private bool is_Target_Set; // Ÿ���� �������� ���ͼ� �������ִٸ�
    [SerializeField]
    private Vector3 return_Pos; // ������ ��ġ
    [SerializeField]
    private int cur_State; // ���� ���� 1 : ���� 2 : ���� 3 : ���� 4 : ����
    [SerializeField]
    private GameObject cur_Target;

    void Start()
    {
        return_Pos = new Vector3(transform.position.x, 0, transform.position.z);
        destination_Pos = transform.position;
    }

    [SerializeField]
    private Vector3 destination_Pos;
    bool patrol_Start = false; // Ž�� ����
    void Enemy_Patrol()
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
            transform.position = Vector3.MoveTowards(transform.position,
                                                     destination_Pos,
                                                     Time.deltaTime * moveSpeed);
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

    void Enemy_Trace() // ���� �Լ�
    {
        if (Vector3.Distance(transform.position, cur_Target.transform.position) == 0) // Ÿ�ٿ� ��Ҵٸ�
        {
            cur_State = 3; // ���� ���·� ����
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position,
                                                                 cur_Target.transform.position,
                                                                 Time.deltaTime * moveSpeed);
        }
    }

    public void Enemy_Return_Set()
    {
        cur_State = 4;
    }

    void Enemy_Return()
    {
        if (Vector3.Distance(transform.position, return_Pos) == 0)
        {
            cur_State = 1; // ���� �Ϸ� �� �ٽ� ��������
            StartCoroutine(patrol_Think_Coroutine());
            patrol_Start = true;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position,
                                                                return_Pos,
                                                                 Time.deltaTime * moveSpeed);
        }
    }

    void Enemy_FSM()
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
                break;
            case 4:
                Enemy_Return();
                break;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 10f);
    }

    void Update()
    {
        Enemy_FSM();
    }
}

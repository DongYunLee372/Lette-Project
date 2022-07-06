using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
ex ) ���Ÿ�, �ٰŸ� ó�� �ٸ� ������Ʈ ���μ����� �����ؾ� �Ѵٸ� 
������Ʈ ó���⸦ ��ӹ��� Ŭ������ �����ؼ�, ������ patrol �� �ʿ���� ai ������ ������Ʈ ó����, �ʿ��� ������ ������Ʈ ó���� ����
�����ؾ��� . �׸��� state_handler �� ����� �ƴ϶� ai ó�� ������ ���� �� �ְ� �ؾ���. 

�ð� �� �������ִ� �͵� ó��.
 */

public class Battle_Character : MonoBehaviour
{
    [SerializeField]
    protected FSM_AI ai = new FSM_AI();

    [SerializeField]
    protected State_Handler state_handler;

    // ������Ʈ ó���⸦ ����. ĳ���Ϳ� ���� �ٸ� ������Ʈ ó���⸦ �޾ƿ�. 
    public GameObject cur_Target;
    [SerializeField]
    public Vector3 return_Pos; // ������ ��ġ
    public Vector3 destination_Pos; // Patrol ������
    public float Attack_Range; // ��Ÿ�
    public bool patrol_Start = false; // Ž�� ����
    public int Mana; // ���� ����
    public int need_Mana; // ��ų ���� �ʿ��� ����
    public int next_Skill;
    public int Max_HP; // �ִ� ü��
    public int Cur_HP; // ���� ü��
    protected Animator anim;

    public void Stat_Initialize(MonsterInformation info)
    {
        //        st = ScriptableObject.CreateInstance<MonsterInformation>();

    }

    protected void Initalize()
    {

        return_Pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        destination_Pos = transform.position;

        // ���⼭ switch �� ������ ���� ������Ʈ ó���� �и�
        state_handler = new General_Monster_State();

        state_handler.State_Handler_Initialize(this);
        //anim = GetComponent<Animator>();
    }

    public virtual void Skill_1()
    {

    }

    public virtual void Skill_2()
    {

    }

    public virtual void Skill_3()
    {

    }
}

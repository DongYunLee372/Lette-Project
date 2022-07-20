using Enemy_Enum;
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
    public FSM_AI ai = new FSM_AI();

    [SerializeField]
    protected State_Handler state_handler;

    // ������Ʈ ó���⸦ ����. ĳ���Ϳ� ���� �ٸ� ������Ʈ ó���⸦ �޾ƿ�. 

    [Header("Common Stats")] // �÷��̾�� ĳ����, ���� ���� ����
    public int index; // �ĺ���
    public string Character_Name; // ���� �Ǵ� �÷��̾� ĳ���� �̸�
    public float Max_HP; // �ִ� ü��
    public float Cur_HP; // ���� ü��
    public float Armor; // ����
    public float balance_gauge; // ���� ������ 
    public float move_Speed; // �̵� �ӵ�
    [Header("Player Character Stats")]
    public float Player_Mana; // ����
    public float Player_Stamina; // ���׹̳�
    public float player_Atk_1; // 1�� ���ݷ�
    public float player_Sta_down_1; // 1�� ���׹̳� �Ҹ�
    public float player_MP_Up_1; // 1�� ��ų������ ������
    public float player_Bal_Down_1; // 1�� ���������� ���ҷ�
    public float player_Atk_2; // 2�� ���ݷ�
    public float player_Sta_down_2; // 2�� ���׹̳� �Ҹ�
    public float player_MP_Up_2; // 2�� ��ų������ ������
    public float player_Bal_Down_2; // 2�� ���������� ���ҷ�
    public float player_Atk_3; // 3�� ���ݷ�
    public float player_Sta_down_3; // 3�� ���׹̳� �Ҹ�
    public float player_MP_Up_3; // 3�� ��ų������ ������
    public float player_Bal_Down_3; // 3�� ���������� ���ҷ�
    [Header("Monster Stats")]
    public Enemy_Grade enemy_Grade; // ���� ���
    public Enemy_Type enemy_Type; // ���� Ÿ��
    public Vector3 return_Pos; // �ʱ� ��ǥ
    public Vector3 destination_Pos; // ���� ��ǥ
    public int die_Delay; // ��� ������
    public int drop_Reward; // ������ ����
    public bool patrol_Start = false; // Ž�� ����
    public float mon_attack_Power; // ���� ���ݷ�
    public float mon_find_Range; // Ž������

    [Header("Etc Stats")]
    public GameObject cur_Target;
    public float Attack_Range; // ��Ÿ�
    public int need_Mana; // ��ų ���� �ʿ��� ����
    public int next_Skill;
    protected Animator anim;

    public void Stat_Initialize(MonsterInformation info) // ���� ���� �� ���� ���� �ʱ�ȭ
    {
        //        st = ScriptableObject.CreateInstance<MonsterInformation>();
        die_Delay = info.P_dieDelay;
        drop_Reward = info.P_drop_Reward;
        mon_attack_Power = info.P_mon_Atk;
        balance_gauge = info.P_mon_Balance;
        Armor = info.P_mon_Def;
        enemy_Grade = (Enemy_Grade)info.P_mon_Default;
        index = int.Parse(info.P_mon_Index);
        Max_HP = info.P_mon_MaxHP;
        move_Speed = info.P_mon_moveSpeed;
        Character_Name = info.P_mon_nameKor;
        enemy_Type = (Enemy_Type)info.P_mon_Type;
    }

    protected void Initalize()
    {

        return_Pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        destination_Pos = transform.position;

        // ���⼭ switch �� ������ ���� ������Ʈ ó���� �и�
        if (Character_Name == "Slime")
            state_handler = gameObject.AddComponent<Slime_State_Handler>();
        else
            state_handler = gameObject.AddComponent<General_Monster_State>();

        state_handler.State_Handler_Initialize(this);
        //anim = GetComponent<Animator>();
    }

    public virtual void Damaged(float damage_Amount) // ���� �޾��� �� ȣ��� �Լ�
    {
        Cur_HP -= (damage_Amount - Armor);
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

    public virtual void Die_Process()
    {

    }

    public virtual void Attack_Process()
    {

    }
}

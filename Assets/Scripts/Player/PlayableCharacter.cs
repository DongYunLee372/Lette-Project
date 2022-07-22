using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�÷��̾�� ĳ������ ������ �����Ѵ�.
//1. ������Ʈ�� ����, ���� ComponentManager�� �ϴ� ���� �״�� ����
//2. �÷��̾� �����͸� �޾ƿͼ� ������ ������Ʈ �鿡�� ���� �ʿ��� �����͵��� �Ѱ��ش�.


public class PlayableCharacter : MonoBehaviour
{
    [Header("================UnityComponent================")]
    public CharacterStateMachine statemachine;


    [Header("================BaseComponent================")]
    public BaseComponent[] components = new BaseComponent[(int)EnumTypes.eComponentTypes.comMax];

    public BaseStatus status;

    /*�̱���*/
    static PlayableCharacter _instance;
    public static PlayableCharacter Instance
    {
        get
        {
            return _instance;
        }
    }



    //public CharacterInformation CharacterDBInfo;

    /*�ʱ�ȭ*/
    private void Awake()
    {
        _instance = this;
    }

    /*�ʱ�ȭ*/
    private void Start()
    {
        //CharacterDBInfo = DataLoad_Save.Instance.Get_PlayerDB(EnumScp.PlayerDBIndex.Level1);
        //Debug.Log($"{CharacterDBInfo.P_player_HP}");

        BaseComponent[] temp = GetComponentsInChildren<BaseComponent>();

        foreach (BaseComponent a in temp)
        {
            components[(int)a.p_comtype] = a;
        }

        status = new BaseStatus();
        status.Init(DataLoad_Save.Instance);
    }

    /*Component ���� �޼ҵ�*/
    public BaseComponent GetMyComponent(EnumTypes.eComponentTypes type)
    {
        return components[(int)type];
    }

    public void InActiveMyComponent(EnumTypes.eComponentTypes type)
    {
        components[(int)type].enabled = false;
    }

    public void ActiveMyComponent(EnumTypes.eComponentTypes type)
    {
        components[(int)type].enabled = true;
    }


    /*�÷��̾� ĳ���� ��ȣ�ۿ� �޼ҵ�*/

    /*�ÿ��̾ ������ �޾�����
      ���� �÷��̾��� ���¿� ���� �˹�, ����˹�, ȸ�� ����� ������ �����Ѵ�.*/
    public void BeAttacked(float damage)
    {
        CharacterStateMachine.eCharacterState state = CharacterStateMachine.Instance.GetState();
        

        //1. ������ ������ �����ϴ� ����(Idle, Move, OutOfControl)
        if (state == CharacterStateMachine.eCharacterState.Idle ||
            state == CharacterStateMachine.eCharacterState.Move ||
            state == CharacterStateMachine.eCharacterState.OutOfControl)
        {
            Damaged(damage);
        }

        //2. ������ 
        //�뷱���������� ����� ���� ������ ���忡 �����ϰ� �뷱�� �������� ���� ��Ų��.
        //�뷱�� �������� ����� ���� ���� ������ ���忡 �����ϰ� �������� �Դ´�.
        else if(state == CharacterStateMachine.eCharacterState.Guard)
        {
            CGuardComponent guardcom = GetMyComponent(EnumTypes.eComponentTypes.MoveCom) as CGuardComponent;

            guardcom.Damaged_Guard(damage);
        }

        //3. ȸ����
        //ĳ���Ͱ� ȸ�����̰� �����ð��϶��� ���� ȸ�ǿ� �����ϰ�
        //ĳ���Ͱ� ȸ���������� �����ð��� �ƴҶ��� ȸ�ǿ� �����ϰ� �������� �Դ´�.
        else if(state == CharacterStateMachine.eCharacterState.Rolling)
        {
            CMoveComponent movecom = GetMyComponent(EnumTypes.eComponentTypes.MoveCom) as CMoveComponent;

            movecom.Damaged_Rolling(damage);
        }

        //4. ������
        else if(state == CharacterStateMachine.eCharacterState.Attack)
        {
            CAttackComponent attackcom = GetMyComponent(EnumTypes.eComponentTypes.AttackCom) as CAttackComponent;
            attackcom.AttackCutOff();
            Damaged(damage);
        }

    }

    
    public void Damaged(float damage)
    {
        CMoveComponent movecom = GetMyComponent(EnumTypes.eComponentTypes.MoveCom) as CMoveComponent;
        //���� ������ = ���� ������ - ���� ���� ��
        float finaldamage = damage - status.Defense;
        status.CurHP -= finaldamage;

        if (finaldamage >= 80)
        {
            movecom.KnockDown();
        }
        else
        {
            movecom.KnockBack();
        }
    }

    public BaseStatus GetCharacterStatus()
    {
        return status;
    }
    
    public void GetExp(int exp)
    {
        status.CurExp += exp;
    }





    private void Update()
    {
        
    }


}

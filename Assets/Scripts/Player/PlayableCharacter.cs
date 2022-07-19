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

    /*�ʱ�ȭ*/
    private void Awake()
    {
        _instance = this;
    }

    /*�ʱ�ȭ*/
    private void Start()
    {
        BaseComponent[] temp = GetComponentsInChildren<BaseComponent>();

        foreach (BaseComponent a in temp)
        {
            components[(int)a.p_comtype] = a;
        }
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
    public void Damaged(float damage)
    {
        CharacterStateMachine.eCharacterState state = CharacterStateMachine.Instance.GetState();
        CMoveComponent movecom = GetMyComponent(EnumTypes.eComponentTypes.MoveCom) as CMoveComponent;

        //1. ������ ������ �����ϴ� ����(Idle, Move, Attack, OutOfControl)
        if (state == CharacterStateMachine.eCharacterState.Idle ||
            state == CharacterStateMachine.eCharacterState.Move ||
            state == CharacterStateMachine.eCharacterState.Attack ||
            state == CharacterStateMachine.eCharacterState.OutOfControl)
        {
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

        //2. ������
        else if(state == CharacterStateMachine.eCharacterState.Guard)
        {

        }

        //3. ȸ����
        else if(state == CharacterStateMachine.eCharacterState.Rolling)
        {

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

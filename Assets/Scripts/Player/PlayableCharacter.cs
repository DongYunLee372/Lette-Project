using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�÷��̾�� ĳ������ ������ �����Ѵ�.
//1. ������Ʈ�� ���� ���� ComponentManager�� �ϴ� ���� �״�� ����
//2. 
public class PlayableCharacter : BaseStatus
{
    //�̱���
    static PlayableCharacter _instance;
    public static PlayableCharacter Instance
    {
        get
        {
            return _instance;
        }
    }

    [Header("================UnityComponent================")]
    public CharacterStateMachine statemachine;


    [Header("================BaseComponent================")]
    public BaseComponent[] components = new BaseComponent[(int)EnumTypes.eComponentTypes.comMax];


    private void Awake()
    {
        _instance = this;

        BaseComponent[] temp = GetComponentsInChildren<BaseComponent>();

        foreach (BaseComponent a in temp)
        {
            components[(int)a.p_comtype] = a;
        }
    }
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



    private void Update()
    {
        
    }


}

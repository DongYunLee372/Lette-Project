using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//jo
//���� ĳ������ ���¸� ����
//
public class CharacterStateMachine : MySingleton<CharacterStateMachine>
{
    public enum eCharacterState
    {
        //Idle,//movecom
        //Walk,//movecom
        //Run,//movecom
        //Attack01,//attackcom ���� �ִϸ��̼� ����
        //Attack02,//attackcom 
        //Attack03,//attackcom
        //Rolling,//movecom
        //Guard,//guardcom ���� �ִϸ��̼� ����
        //GuardStun,//battlesystem ������ ������Ʈ �鿡�� �ִϸ��̼� ����
        //DamagedStun,//movecom ������ ������Ʈ�鿡�� �ִϸ��̼� ����
        //DamagedKnockBack,//movecom ������ ������Ʈ�鿡�� �ִϸ��̼� ����
        //Slip,//movecom ������ ������Ʈ�鿡�� �ִϸ��̼� ����
        //OutOfControl,//move, battelsys, guard 

        Idle,
        Move,
        Attack,
        Rolling,
        Guard,
        OutOfControl,




        StateMax
    }

    [System.Serializable]
    public class AnimationBlendingTimeSet
    {
        public eCharacterState prestate;
        public eCharacterState changestate;
        [Range(0.0f, 5.0f)]
        public float blendtime;
        
    }

    //����� �����̴� ������ ����� �����Ҷ� ���ڰ� ������ �ֵ��� �Ѵ�.

    public List<AnimationBlendingTimeSet> animationBlendingTimeSets = new List<AnimationBlendingTimeSet>();
    //curstate
    public eCharacterState CurState;
    //
    public eCharacterState PreState;

    //���� ��ȭ�� ���� �ִϸ��̼��� �ٲ��ش�.
    public void SetState(eCharacterState state)
    {
        if (CurState != state)
        {
            //Debug.Log($"{state} ����");
            PreState = CurState;
            CurState = state;
        }
    }

    public eCharacterState GetState()
    {
        return CurState;
    }

    public eCharacterState GetPreState()
    {
        return PreState;
    }

    private void Update()
    {
        
    }
}

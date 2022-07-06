using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//jo
//���� ĳ������ ���¸� ����, ���¿� ���� ����(�ִϸ��̼�)���� ���ش�.
//
public class CharacterStateMachine : MonoBehaviour
{
    public enum E_CharacterState
    {
        Idle,//movecom
        Walk,//movecom
        Run,//movecom
        Attack01,//attackcom
        Attack02,//attackcom
        Attack03,//attackcom
        Rolling,//movecom
        Guard,//guardcom
        GuardStun,//battlesystem
        DamagedStun,//movecom
        DamagedKnockBack,//movecom
        Slip,//movecom
        OutOfControl,//move, battelsys, guard
        StateMax
    }

    [System.Serializable]
    public class AnimationBlendingTimeSet
    {
        public E_CharacterState prestate;
        public E_CharacterState changestate;
        [Range(0.0f, 5.0f)]
        public float blendtime;
        
    }

    public List<AnimationBlendingTimeSet> animationBlendingTimeSets = new List<AnimationBlendingTimeSet>();
    //curstate
    public E_CharacterState CurState;
    //
    public E_CharacterState PreState;

    //���� ��ȭ�� ���� �ִϸ��̼��� �ٲ��ش�.
    public void SetState(E_CharacterState state)
    {
        PreState = CurState;
        CurState = state;

        switch(CurState)
        {
            //�޸���->�⺻, ����1Ÿ->�⺻, ����2Ÿ->�⺻, ����3Ÿ->�⺻, �ȱ�->�⺻, ������->�⺻
            case E_CharacterState.Idle:

                break;

            
            case E_CharacterState.Rolling:

                break;
            case E_CharacterState.Guard:

                break;
            case E_CharacterState.Slip:

                break;
            case E_CharacterState.OutOfControl:

                break;
        }



    }

    public E_CharacterState GetState()
    {
        return CurState;
    }

    public E_CharacterState GetPreState()
    {
        return PreState;
    }

    private void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//jo
//���� ĳ������ ���¸� ����, ���¿� ���� ����(�ִϸ��̼�)���� ���ش�.
//
public class CharacterStateMachine : MonoBehaviour
{
    [System.Serializable]
    public class AnimationBlendingTimeSet
    {
        public EnumTypes.eCharacterState prestate;
        public EnumTypes.eCharacterState changestate;
        [Range(0.0f, 5.0f)]
        public float blendtime;
        
    }

    //����� �����̴� ������ ����� �����Ҷ� ���ڰ� ������ �ֵ��� �Ѵ�.

    public List<AnimationBlendingTimeSet> animationBlendingTimeSets = new List<AnimationBlendingTimeSet>();
    //curstate
    public EnumTypes.eCharacterState CurState;
    //
    public EnumTypes.eCharacterState PreState;

    //���� ��ȭ�� ���� �ִϸ��̼��� �ٲ��ش�.
    public void SetState(EnumTypes.eCharacterState state)
    {
        PreState = CurState;
        CurState = state;

        //switch(CurState)
        //{
        //    //�޸���->�⺻, ����1Ÿ->�⺻, ����2Ÿ->�⺻, ����3Ÿ->�⺻, �ȱ�->�⺻, ������->�⺻
        //    case E_CharacterState.Idle:

        //        break;
            
        //    case E_CharacterState.Rolling:

        //        break;
        //    case E_CharacterState.Guard:

        //        break;
        //    case E_CharacterState.Slip:

        //        break;
        //    case E_CharacterState.OutOfControl:

        //        break;
        //}



    }

    public EnumTypes.eCharacterState GetState()
    {
        return CurState;
    }

    public EnumTypes.eCharacterState GetPreState()
    {
        return PreState;
    }

    private void Update()
    {
        
    }
}

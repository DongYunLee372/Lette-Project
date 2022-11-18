using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//move에서 사용하는 변수들
//해당 값이 변경되면 해당되는 state 변경이 필요
//여기에서 판단할 경우들
//1. 현재 움직일 수 있는지 없는지
//2. 현재 달릴 수 있는지 없는지
//3. 회피가 가능한지
//
[System.Serializable]
public class CurState
{
    //현재 상태가 outofstate일때
    
    public bool CheckRollingAble()
    {
        //이미 구르는 중이 아니고 땅에 있어야지 회피 가능
        if(!IsRolling&& IsGrounded)
        {
            return true;
        }
        return false;
    }

    public bool CheckRunAble()
    {

        return false;
    }

    public bool CheckMoveAble()
    {
        //회피중이거나 공격중이 아닐때 움직임 가능
        if (!IsRolling && !IsAttacking)
        {
            return true;
        }
        return false;
    }

    public bool CheckStepAble()
    {
        CMoveComponent movecom = PlayableCharacter.Instance.GetMyComponent(CharEnumTypes.eComponentTypes.MoveCom) as CMoveComponent;
        //
        if (IsFowordBlock)
        {
            return false;
        }
        //
        if (IsStep && CurStepHeight<movecom.moveoption.StepHeight)
        {
            return true;
        }
        return false;
    }


    [SerializeField]
    public bool IsCursorActive = false;
    [SerializeField]
    public bool IsFPP = true;
    [SerializeField]
    private bool isMoving = false;
    [SerializeField]
    public bool IsRunning = false;
    [SerializeField]
    public bool IsGrounded = false;
    [SerializeField]
    public bool IsJumping = false;
    [SerializeField]
    public bool IsFalling = false;
    [SerializeField]
    public bool IsSlip = false;
    [SerializeField]
    public bool IsFowordBlock = false;
    [SerializeField]
    public bool IsOnTheSlop = false;
    [SerializeField]
    private bool isAttacking = false;
    [SerializeField]
    private bool isGuard = false;
    [SerializeField]
    private bool isKnockBack = false;
    [SerializeField]
    private bool isKnockDown = false;

    [SerializeField]
    public bool IsNoDamage = false;

    [SerializeField]
    public bool IsStep = false;

    //[SerializeField]
    //private bool isAttacked = false;
    //[SerializeField]
    //private bool isOutofControl = false;
    [SerializeField]
    private bool isRolling = false;
    [SerializeField]
    public float LastJump;
    [SerializeField]
    public float CurGroundSlopAngle;
    [SerializeField]
    public float CurFowardSlopAngle;
    [SerializeField]
    public Vector3 CurGroundNomal;
    [SerializeField]
    public Vector3 CurGroundCross;
    [SerializeField]
    public Vector3 CurHorVelocity;
    [SerializeField]
    public Vector3 CurVirVelocity;
    [SerializeField]
    public float MoveAccel;
    [SerializeField]
    public float CurStepHeight;
    [SerializeField]
    public Vector3 CurStepPos;

    public bool IsMoving { 
        get
        {
            return isMoving;
        }
        set
        {
            isMoving = value;
            if (isMoving)
                PlayableCharacter.Instance.SetState(PlayableCharacter.States.Move);
            //else
            //    CharacterStateMachine.Instance.SetState(CharacterStateMachine.eCharacterState.Idle);
        }
    }
    
    
    
    
    //public bool IsAttacked { get => isAttacked; set => isAttacked = value; }
    //public bool IsOutofControl { 
    //    get
    //    {
    //        return isOutofControl;
    //    }
    //    set
    //    {
    //        isOutofControl = value;
    //    }
    //}
    public bool IsRolling { 
        get
        {
            return isRolling;
        }
        set
        {
            isRolling = value;
            if (isRolling)
            {
                PlayableCharacter.Instance.SetState(PlayableCharacter.States.Rolling);
            }
            else
            {
                if(PlayableCharacter.Instance.GetState()!= PlayableCharacter.States.OutOfControl)
                    PlayableCharacter.Instance.SetState(PlayableCharacter.States.Idle);
            }
        }
    }

    

    
    public bool IsAttacking { 
        get
        {
            return isAttacking;
        }
        set
        {
            isAttacking = value;
            if (isAttacking)
            {
                PlayableCharacter.Instance.SetState(PlayableCharacter.States.Attack);
            }
            else
            {
                if (PlayableCharacter.Instance.GetState() != PlayableCharacter.States.OutOfControl)
                    PlayableCharacter.Instance.SetState(PlayableCharacter.States.Idle);
            }
        }  
    }
    public bool IsGuard { 
        get
        {
            return isGuard;
        }
        set
        {
            isGuard = value;

            if (isGuard)
            {
                PlayableCharacter.Instance.SetState(PlayableCharacter.States.Guard);
                //Debug.Log("guard들어옴");
            }
            else
            {
                PlayableCharacter.Instance.SetState(PlayableCharacter.States.Idle);
                //Debug.Log("guard나감");
            }
                

        }
    }

    public bool IsKnockBack { 
        get
        {
            return isKnockBack;
        }
        set
        {
            isKnockBack = value;

            if (isKnockBack)
            {
                PlayableCharacter.Instance.SetState(PlayableCharacter.States.OutOfControl);
            }
            else
            {
                PlayableCharacter.Instance.SetState(PlayableCharacter.States.Idle);
            }
        }
    }
    public bool IsKnockDown {
        get
        {
            return isKnockDown;
        }
        set
        {
            isKnockDown = value;

            if (isKnockDown)
            {
                PlayableCharacter.Instance.SetState(PlayableCharacter.States.OutOfControl);
            }
            else
            {
                PlayableCharacter.Instance.SetState(PlayableCharacter.States.Idle);
            }
        }
    }
}

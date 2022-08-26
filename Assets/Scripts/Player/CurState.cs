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

   
    public bool IsMoving { 
        get
        {
            return isMoving;
        }
        set
        {
            isMoving = value;
            if (isMoving)
                CharacterStateMachine.Instance.SetState(CharacterStateMachine.eCharacterState.Move);
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
                CharacterStateMachine.Instance.SetState(CharacterStateMachine.eCharacterState.Rolling);
            }
            else
            {
                if(CharacterStateMachine.Instance.GetState()!= CharacterStateMachine.eCharacterState.OutOfControl)
                    CharacterStateMachine.Instance.SetState(CharacterStateMachine.eCharacterState.Idle);
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
                CharacterStateMachine.Instance.SetState(CharacterStateMachine.eCharacterState.Attack);
            }
            else
            {
                if (CharacterStateMachine.Instance.GetState() != CharacterStateMachine.eCharacterState.OutOfControl)
                    CharacterStateMachine.Instance.SetState(CharacterStateMachine.eCharacterState.Idle);
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
                CharacterStateMachine.Instance.SetState(CharacterStateMachine.eCharacterState.Guard);
                //Debug.Log("guard들어옴");
            }
            else
            {
                CharacterStateMachine.Instance.SetState(CharacterStateMachine.eCharacterState.Idle);
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
                CharacterStateMachine.Instance.SetState(CharacterStateMachine.eCharacterState.OutOfControl);
            }
            else
            {
                CharacterStateMachine.Instance.SetState(CharacterStateMachine.eCharacterState.Idle);
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
                CharacterStateMachine.Instance.SetState(CharacterStateMachine.eCharacterState.OutOfControl);
            }
            else
            {
                CharacterStateMachine.Instance.SetState(CharacterStateMachine.eCharacterState.Idle);
            }
        }
    }
}

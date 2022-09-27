using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*유저의 입력을 처리한다.*/
public class CInputComponent : BaseComponent
{
    //캐릭터의 모든 컴포넌트를 관리하기 쉽게 하기 위해 basecomponent를 상속받은 스크립트들을 componentmanager에서 관리한다.
    public override void InitComtype()
    {
        p_comtype = CharEnumTypes.eComponentTypes.InputCom;
    }
    
    //사용할 키 지정
    [System.Serializable]
    public class KeySetting
    {
        public KeyCode right = KeyCode.D;

        public KeyCode foward = KeyCode.W;

        public KeyCode left = KeyCode.A;

        public KeyCode back = KeyCode.S;

        public KeyCode Rolling = KeyCode.Space;

        public KeyCode Run = KeyCode.LeftShift;

        public KeyCode skill01 = KeyCode.Alpha1;

        public KeyCode skill02 = KeyCode.Alpha2;

        public KeyCode skill03 = KeyCode.Alpha3;
    }

    [Header("Keys")]
    public KeySetting _key = new KeySetting();

    //input에서 필요한 컴포넌트들
    [Header("Components")]
    //1. move 컴포넌트
    public CMoveComponent movecom;
    //2. Attack 컴포넌트
    public CAttackComponent attackcom;
    //public PlayerAttack attackcom;
    //3. Defence 컴포넌트
    public CGuardComponent guardcom;
    
   
    //키와 마우스 입력을 처리한다.
    void KeyInput()
    {
        if (movecom == null)
            movecom = PlayableCharacter.Instance.GetMyComponent(CharEnumTypes.eComponentTypes.MoveCom) as CMoveComponent;
        CharacterStateMachine.eCharacterState state = CharacterStateMachine.Instance.GetState();


        float v = 0;
        float h = 0;
        movecom.curval.IsMoving = false;


        movecom.MouseMove = new Vector2(0, 0);//마우스 움직임
        movecom.MoveDir = new Vector3(0, 0, 0);//wasd 키 입력에 따른 이동 방향

        ////가드 중 일때는 시점은 정면으로 고정
        //if(state != CharacterStateMachine.eCharacterState.Guard&&
        //    state != CharacterStateMachine.eCharacterState.GuardStun)
        //{
            
        //}

        movecom.MouseMove = new Vector2(Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y"));
        float scroll = Input.GetAxis("Mouse ScrollWheel") * -1;
        

        if (scroll > 0)//줌인 줌아웃에 사용
        {
            movecom.ZoomOut(scroll);
        }
        else 
        {
            movecom.ZoomIn(scroll);
            
            
        }



        if (/*state == CharacterStateMachine.eCharacterState.Attack||*/
            state == CharacterStateMachine.eCharacterState.Rolling||
            state == CharacterStateMachine.eCharacterState.OutOfControl)
        {
            //movecom.curval.IsMoving = false;
            return;
        }

        //if (movecom.curval.IsRolling|| movecom.curval.IsSlip|| movecom.curval.IsAttacking||movecom.curval.IsGuard)//회피중, 떨어지는중, 공격하는 중에는 움직일 수는 없지만 마우스를 움직여 화면을 돌리는것은 가능
        //{
        //    movecom.curval.IsMoving = false;
        //    return;
        //}

        

        //왼쪽 마우스 클릭
        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseDown();
            return;
        }

        if (state == CharacterStateMachine.eCharacterState.Attack)
            return;

        //오른쪽 마우스 클릭
        if (Input.GetMouseButtonDown(1))
        {
            RightMouseDown();
            return;
        }

        if (Input.GetMouseButtonUp(1))
        {
            RightMouseUp();
            return;
        }

        if(/*state == CharacterStateMachine.eCharacterState.Guard||*/
           state == CharacterStateMachine.eCharacterState.GuardStun)
        {
            //movecom.curval.IsMoving = false;
            return;
        }

        

        if(state != CharacterStateMachine.eCharacterState.Guard)//방어 중 일때는 해당 행동들을 할 수 없도록
        {
            //space 처리
            //구르기를 먼저 처리하고 움직임은 처리하지 않게 하기 위해서
            if (Input.GetKey(_key.Rolling))
            {
                movecom.Rolling();
                return;
            }

            //스킬1번
            if (Input.GetKey(_key.skill01))
            {
                SkillAttack(0);
                return;
            }

            //스킬2번
            if (Input.GetKey(_key.skill02))
            {
                SkillAttack(1);
                return;
            }

            //스킬3번
            if (Input.GetKey(_key.skill03))
            {
                SkillAttack(2);
                return;
            }
        }

        //wasd 처리
        if (Input.GetKey(_key.foward))
        {
            if (state == CharacterStateMachine.eCharacterState.Guard)
            {
                movecom.GuardMove(CMoveComponent.Direction.Front);
                return;
            }
            else
                v += 1.0f;
        }
        if (Input.GetKey(_key.back))
        {
            if (state == CharacterStateMachine.eCharacterState.Guard)
            {
                movecom.GuardMove(CMoveComponent.Direction.Back);
                return;
            }
                
            else
                v -= 1.0f; 
        }
        if (Input.GetKey(_key.left)) 
        {
            if (state == CharacterStateMachine.eCharacterState.Guard)
            {
                movecom.GuardMove(CMoveComponent.Direction.Left);
                return;
            }
               
            else
                h -= 1.0f;
        }
        if (Input.GetKey(_key.right)) 
        {
            if (state == CharacterStateMachine.eCharacterState.Guard)
            {
                movecom.GuardMove(CMoveComponent.Direction.Right);
                return;
            }
                
            else
                h += 1.0f;
        }

        movecom.MoveDir = new Vector3(h, 0, v);

        if (state != CharacterStateMachine.eCharacterState.Guard)//방어 중 일때는 해당 행동들을 할 수 없도록
        {
            //left shift 처리
            if (Input.GetKey(_key.Run)) movecom.curval.IsRunning = true;
            else movecom.curval.IsRunning = false;



            //이동값이 조금이라도 있으면 움직이는중으로 판단
            if (movecom.MoveDir.magnitude > 0)
            {
                movecom.curval.IsMoving = true;
                //CharacterStateMachine.Instance.SetState(CharacterStateMachine.eCharacterState.Move);
            }
            else
            {
                CharacterStateMachine.Instance.SetState(CharacterStateMachine.eCharacterState.Idle);
            }


            if (movecom.curval.IsMoving)
            {
                if (movecom.curval.IsRunning)
                {
                    //com.animator.SetPlaySpeed(1f);
                    movecom.com.animator.Play("_Dash");

                }
                else
                {
                    //com.animator.SetPlaySpeed( 1f);
                    movecom.com.animator.Play("_Walk");
                }
            }
            else
            {
                movecom.com.animator.Play("_Idle");
            }
        }
        else
        {
            movecom.com.animator.Play("_Guard");
        }
    }

    void LeftMouseDown()
    {
        if (attackcom == null)
            attackcom = PlayableCharacter.Instance.GetMyComponent(CharEnumTypes.eComponentTypes.AttackCom) as CAttackComponent;
        //attackcom = PlayableCharacter.Instance.GetMyComponent(CharEnumTypes.eComponentTypes.AttackCom) as PlayerAttack;

        attackcom.Attack();
    }


    void RightMouseDown()
    {
        if (guardcom == null)
            guardcom = PlayableCharacter.Instance.GetMyComponent(CharEnumTypes.eComponentTypes.GuardCom) as CGuardComponent;
        guardcom.Guard();
    }

    void RightMouseUp()
    {
        if (guardcom == null)
            guardcom = PlayableCharacter.Instance.GetMyComponent(CharEnumTypes.eComponentTypes.GuardCom) as CGuardComponent;
        guardcom.GuardDown();
    }
    
    void SkillAttack(int num)
    {
        if (attackcom == null)
            attackcom = PlayableCharacter.Instance.GetMyComponent(CharEnumTypes.eComponentTypes.AttackCom) as CAttackComponent;
        //attackcom = PlayableCharacter.Instance.GetMyComponent(CharEnumTypes.eComponentTypes.AttackCom) as PlayerAttack;

        attackcom.SkillAttack(0);
    }

    void Update()
    {
        //넉백 테스트
        if (Input.GetKeyDown(KeyCode.U))
        {
            PlayableCharacter.Instance.BeAttacked(10, this.transform.position,10.0f);
            //movecom.KnockBack();
        }

        //넉다운 테스트
        if (Input.GetKeyDown(KeyCode.I))
        {
            PlayableCharacter.Instance.BeAttacked(90, this.transform.position,40.0f);
            //movecom.KnockDown();
        }

        //키 입력
        KeyInput();
    }
}

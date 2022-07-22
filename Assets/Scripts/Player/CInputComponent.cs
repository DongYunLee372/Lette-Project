using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*������ �Է��� ó���Ѵ�.*/
public class CInputComponent : BaseComponent
{
    //ĳ������ ��� ������Ʈ�� �����ϱ� ���� �ϱ� ���� basecomponent�� ��ӹ��� ��ũ��Ʈ���� componentmanager���� �����Ѵ�.
    public override void InitComtype()
    {
        p_comtype = EnumTypes.eComponentTypes.InputCom;
    }
    
    //����� Ű ����
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

    //input���� �ʿ��� ������Ʈ��
    [Header("Components")]
    //1. move ������Ʈ
    public CMoveComponent movecom;
    //2. Attack ������Ʈ
    public CAttackComponent attackcom;
    //3. Defence ������Ʈ
    public CGuardComponent guardcom;
    
   
    //Ű�� ���콺 �Է��� ó���Ѵ�.
    void KeyInput()
    {
        if (movecom == null)
            movecom = PlayableCharacter.Instance.GetMyComponent(EnumTypes.eComponentTypes.MoveCom) as CMoveComponent;

        

        float v = 0;
        float h = 0;
        movecom.curval.IsMoving = false;


        movecom.MouseMove = new Vector2(0, 0);//���콺 ������
        movecom.MoveDir = new Vector3(0, 0, 0);//wasd Ű �Է¿� ���� �̵� ����

        movecom.MouseMove = new Vector2(Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y"));
        CharacterStateMachine.eCharacterState state = CharacterStateMachine.Instance.GetState();



        if (state == CharacterStateMachine.eCharacterState.Attack||
            state == CharacterStateMachine.eCharacterState.Rolling||
            state == CharacterStateMachine.eCharacterState.OutOfControl)
        {
            //movecom.curval.IsMoving = false;
            return;
        }

        //if (movecom.curval.IsRolling|| movecom.curval.IsSlip|| movecom.curval.IsAttacking||movecom.curval.IsGuard)//ȸ����, ����������, �����ϴ� �߿��� ������ ���� ������ ���콺�� ������ ȭ���� �����°��� ����
        //{
        //    movecom.curval.IsMoving = false;
        //    return;
        //}

        

        //���� ���콺 Ŭ��
        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseDown();
            return;
        }

        //������ ���콺 Ŭ��
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

        if(state == CharacterStateMachine.eCharacterState.Guard)
        {
            //movecom.curval.IsMoving = false;
            return;
        }

        Input.GetAxisRaw("Mouse ScrollWheel");//���� �ܾƿ��� ���

        //space ó��
        //�����⸦ ���� ó���ϰ� �������� ó������ �ʰ� �ϱ� ���ؼ�
        if (Input.GetKey(_key.Rolling))
        {
            movecom.Rolling();
            return;
        }

        if (Input.GetKey(_key.skill01))
        {
            SkillAttack(0);
            return;
        }
            

        if (Input.GetKey(_key.skill02))
        {
            SkillAttack(1);
            return;
        }

        if (Input.GetKey(_key.skill03))
        {
            SkillAttack(2);
            return;
        }

        //wasd ó��
        if (Input.GetKey(_key.foward)) v += 1.0f;
        if (Input.GetKey(_key.back)) v -= 1.0f;
        if (Input.GetKey(_key.left)) h -= 1.0f;
        if (Input.GetKey(_key.right)) h += 1.0f;

        movecom.MoveDir = new Vector3(h, 0, v);

        //left shift ó��
        if (Input.GetKey(_key.Run)) movecom.curval.IsRunning = true;
        else movecom.curval.IsRunning = false;

        

        //�̵����� �����̶� ������ �����̴������� �Ǵ�
        if (movecom.MoveDir.magnitude > 0 )
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

    void LeftMouseDown()
    {
        if (attackcom == null)
            attackcom = PlayableCharacter.Instance.GetMyComponent(EnumTypes.eComponentTypes.AttackCom) as CAttackComponent;
        attackcom.Attack();
    }


    void RightMouseDown()
    {
        if (guardcom == null)
            guardcom = PlayableCharacter.Instance.GetMyComponent(EnumTypes.eComponentTypes.GuardCom) as CGuardComponent;
        guardcom.Guard();
    }

    void RightMouseUp()
    {
        if (guardcom == null)
            guardcom = PlayableCharacter.Instance.GetMyComponent(EnumTypes.eComponentTypes.GuardCom) as CGuardComponent;
        guardcom.GuardDown();
    }
    
    void SkillAttack(int num)
    {
        if (attackcom == null)
            attackcom = PlayableCharacter.Instance.GetMyComponent(EnumTypes.eComponentTypes.AttackCom) as CAttackComponent;
        attackcom.SkillAttack(num);
    }

    void Update()
    {
        ////���� ���콺 Ŭ��
        //if(Input.GetMouseButtonDown(0))
        //{
        //    if (attackcom == null)
        //        attackcom = ComponentManager.GetI.GetMyComponent(EnumTypes.eComponentTypes.AttackCom) as CAttackComponent;
        //    attackcom.Attack();
        //    //movecom.curval.IsAttacking = true;
        //}

        ////������ ���콺 Ŭ��
        //if(Input.GetMouseButtonDown(1))
        //{
        //    if (guardcom == null)
        //        guardcom = ComponentManager.GetI.GetMyComponent(EnumTypes.eComponentTypes.GuardCom) as CGuardComponent;
        //    guardcom.Guard();
        //    //movecom.curval.IsGuard = true;

        //}

        //if(Input.GetMouseButtonUp(1))
        //{
        //    if (guardcom == null)
        //        guardcom = ComponentManager.GetI.GetMyComponent(EnumTypes.eComponentTypes.GuardCom) as CGuardComponent;
        //    guardcom.GuardDown();
        //    //movecom.curval.IsGuard = false;
        //}

        //�˹� �׽�Ʈ
        if(Input.GetKeyDown(KeyCode.U))
        {
            PlayableCharacter.Instance.BeAttacked(10);
            //movecom.KnockBack();
        }

        //�˴ٿ� �׽�Ʈ
        if (Input.GetKeyDown(KeyCode.I))
        {
            PlayableCharacter.Instance.BeAttacked(90);
            //movecom.KnockDown();
        }

        //Ű �Է�
        KeyInput();
    }
}

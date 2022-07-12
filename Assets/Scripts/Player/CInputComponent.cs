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
            movecom = ComponentManager.GetI.GetMyComponent(EnumTypes.eComponentTypes.MoveCom) as CMoveComponent;

        

        float v = 0;
        float h = 0;

        movecom.MouseMove = new Vector2(0, 0);//���콺 ������
        movecom.MoveDir = new Vector3(0, 0, 0);//wasd Ű �Է¿� ���� �̵� ����

        movecom.MouseMove = new Vector2(Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y"));

        //space ó��
        if (Input.GetKey(_key.Rolling))
            movecom.Rolling();

        CharacterStateMachine.eCharacterState state = CharacterStateMachine.Instance.GetState();


        if (state == CharacterStateMachine.eCharacterState.Attack|| 
            state == CharacterStateMachine.eCharacterState.Rolling||
            state == CharacterStateMachine.eCharacterState.Guard|| 
            state == CharacterStateMachine.eCharacterState.OutOfControl)
        {
            movecom.curval.IsMoving = false;
            return;
        }

        if (movecom.curval.IsRolling|| movecom.curval.IsSlip|| movecom.curval.IsAttacking||movecom.curval.IsGuard)//ȸ����, ����������, �����ϴ� �߿��� ������ ���� ������ ���콺�� ������ ȭ���� �����°��� ����
        {
            movecom.curval.IsMoving = false;
            return;
        }

        movecom.curval.IsMoving = false;

        Input.GetAxisRaw("Mouse ScrollWheel");//���� �ܾƿ��� ���

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

    //IEnumerator tempdid;
    //IEnumerator Cor_Test(float time)
    //{
    //    int i = 0;
    //    Debug.Log($"�ٽý���");
    //    while (true)
    //    {
    //        //if ((Time.time - starttime) >= time)
    //        //{
    //        //    Debug.Log("�ð��ٵ�");
    //        //    invoker.Invoke();
    //        //    coroutine = null;
    //        //    //playingCor = false;
    //        //    yield break;
    //        //}
    //        if (i >= 100)
    //            yield break;

    //        Debug.Log($"�ڷ�ƾ{i}");
    //        i++;
    //        yield return new WaitForSeconds(1.0f);
    //    }
    //}

    void Update()
    {
        //���� ���콺 Ŭ��
        if(Input.GetMouseButtonDown(0))
        {
            if (attackcom == null)
                attackcom = ComponentManager.GetI.GetMyComponent(EnumTypes.eComponentTypes.AttackCom) as CAttackComponent;
            attackcom.Attack();
            //movecom.curval.IsAttacking = true;
        }

        //������ ���콺 Ŭ��
        if(Input.GetMouseButtonDown(1))
        {
            if (guardcom == null)
                guardcom = ComponentManager.GetI.GetMyComponent(EnumTypes.eComponentTypes.GuardCom) as CGuardComponent;
            guardcom.Guard();
            //movecom.curval.IsGuard = true;

        }
        if(Input.GetMouseButtonUp(1))
        {
            if (guardcom == null)
                guardcom = ComponentManager.GetI.GetMyComponent(EnumTypes.eComponentTypes.GuardCom) as CGuardComponent;
            guardcom.GuardDown();
            //movecom.curval.IsGuard = false;
        }

        //Ű �Է�
        KeyInput();
    }
}

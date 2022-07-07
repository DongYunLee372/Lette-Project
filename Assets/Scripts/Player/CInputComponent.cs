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
    public CGuardComponent defencecom;
    
   
    //Ű�� ���콺 �Է��� ó���Ѵ�.
    void KeyInput()
    {
        if (movecom == null)
            movecom = ComponentManager.GetI.GetMyComponent(EnumTypes.eComponentTypes.MoveCom) as CMoveComponent;

        movecom.curval.IsMoving = false;

        float v = 0;
        float h = 0;

        movecom.MouseMove = new Vector2(0, 0);//���콺 ������
        movecom.MoveDir = new Vector3(0, 0, 0);//wasd Ű �Է¿� ���� �̵� ����

        movecom.MouseMove = new Vector2(Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y"));

        if (movecom.curval.IsRolling|| movecom.curval.IsSlip|| movecom.curval.IsAttacking||movecom.curval.IsGuard)//ȸ����, ����������, �����ϴ� �߿��� ������ ���� ������ ���콺�� ������ ȭ���� �����°��� ����
        {
            return;
        }

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

        //space ó��
        if (Input.GetKey(_key.Rolling))
            movecom.Rolling();

        //�̵����� �����̶� ������ �����̴������� �Ǵ�
        if (movecom.MoveDir.magnitude > 0 )
        {
            movecom.curval.IsMoving = true;
        }
    }


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
            movecom.curval.IsGuard = true;
            
        }
        if(Input.GetMouseButtonUp(1))
        {
            movecom.curval.IsGuard = false;
        }

        //Ű �Է�
        KeyInput();
    }
}

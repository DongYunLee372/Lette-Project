using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMoveComponent : BaseComponent
{
    public override void InitComtype()
    {
        p_comtype = EnumTypes.eComponentTypes.MoveCom;
    }

    [System.Serializable]
    public class Com
    {
        public Transform CharacterRoot = null;

        public Transform TpCamRig = null;
        public Transform TpCam = null;

        public Transform FpRoot = null;
        public Transform FpCamRig = null;
        public Transform FpCam = null;

        public Rigidbody CharacterRig = null;

        public CapsuleCollider CapsuleCol = null;

        public CAnimationComponent animator = null;
    }

    

    public class CurValues
    {
        public Vector2 MouseMove = Vector2.zero;

        public Vector3 MoveDir = Vector3.zero;

        public Vector3 WorldMove = Vector3.zero;

        public bool IsCursorActive = false;

        public bool IsFPP = true;

        public bool IsMoving = false;

        public bool IsRunning = false;

        public bool IsGrounded = false;

        public bool IsJumping = false;

        public bool IsFalling = false;

        public bool IsSlip = false;

        public bool IsFowordBlock = false;

        public bool IsOnTheSlop = false;

        public bool IsAttacked = false;

        public bool IsOutofControl = false;

        public float CurGravity;//���� ���ν�Ƽ�� y��

        private float LastJump;

        public float CurGroundSlopAngle;

        public float CurFowardSlopAngle;

        public Vector3 CurGroundNomal;

        public Vector3 CurGroundCross;

        public Vector3 CurHorVelocity;

        public Vector3 CurVirVelocity;

        public float MoveAccel;
    }


    public class MoveOption
    {
        public float RotMouseSpeed = 10f;

        public float MoveSpeed;

        public float RunSpeed;

        public float MinAngle;

        public float MaxAngle;

        public float Gravity;//�߷°�(�����Ӵ����� ���������� ��)

        public float JumpPower = 120;//������ �ϸ� �ش� ������ curgravity���� �ٲ��ش�.

        public float JumpcoolTime = 1f;

        public LayerMask GroundMask;

        public float MaxSlop = 70;

        public float SlopAccel;//(�߷°��� ���� �̲������� ���������� ��)
    }

    public Com com = new Com();

    public CurValues curval = new CurValues();

    public MoveOption moveoption = new MoveOption();

    public CInputComponent inputcom = null;

    public Vector3 Capsuletopcenter => new Vector3(transform.position.x, transform.position.y + com.CapsuleCol.height - com.CapsuleCol.radius, transform.position.z);
    public Vector3 Capsulebottomcenter => new Vector3(transform.position.x, transform.position.y + com.CapsuleCol.radius, transform.position.z);

    [Header("============TestVals============")]

    public Vector3 updown;
    public float xnext;

    public Vector3 rightleft;
    public float ynext;

    public Vector3 testtart;
    public Vector3 testend;

    public Transform testcube;

    void Start()
    {
        inputcom = ComponentManager.GetI.GetMyComponent(EnumTypes.eComponentTypes.InputCom) as CInputComponent;
        if (inputcom == null)
            Debug.Log("MoveCom ���� inputcom = null");

        com.CharacterRoot = GameObject.Find("CharacterRoot").transform;
        com.CharacterRig = GetComponent<Rigidbody>();
        com.TpCamRig = GameObject.Find("TPCamRig").transform;
        com.TpCam = GameObject.Find("TPCam").transform;
        com.FpRoot = GameObject.Find("FPRoot").transform;
        com.FpCamRig = GameObject.Find("FPCamRig").transform;
        com.FpCam = GameObject.Find("FPCam").transform;
        com.CapsuleCol = GetComponent<CapsuleCollider>();

        com.animator = ComponentManager.GetI.GetMyComponent(EnumTypes.eComponentTypes.AnimatorCom) as CAnimationComponent;
        if (com.animator == null)
            Debug.Log("MoveCom ���� com.animator = null");
    }

    void Update()
    {
        
    }
}

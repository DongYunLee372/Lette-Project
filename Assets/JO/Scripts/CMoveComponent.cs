using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMoveComponent : BaseComponent
{

    CheckAround checkaround;
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

        public Animator animator = null;
    }

    [System.Serializable]
    public class MoveOption
    {
        [SerializeField]
        public float RotMouseSpeed = 10f;
        [SerializeField]
        public float MoveSpeed;
        [SerializeField]
        public float RunSpeed;
        [SerializeField]
        public float MinAngle;
        [SerializeField]
        public float MaxAngle;
        [SerializeField]
        public float Gravity;//�߷°�(�����Ӵ����� ���������� ��)
        [SerializeField]
        public float JumpPower = 120;//������ �ϸ� �ش� ������ curgravity���� �ٲ��ش�.
        [SerializeField]
        public float JumpcoolTime = 1f;
        [SerializeField]
        public LayerMask GroundMask;
        [SerializeField]
        public float MaxSlop = 70;
        [SerializeField]
        public float SlopAccel;//(�߷°��� ���� �̲������� ���������� ��)

        public float RollingDistance;

        public float RollingTime;

        public float RollingDontHitTime;
    }

    public Vector2 MouseMove = Vector2.zero;

    public Vector3 MoveDir = Vector3.zero;

    public Vector3 WorldMove = Vector3.zero;

    public float CurGravity;//���� ���ν�Ƽ�� y��

    public Com com = new Com();

    public CurState curval = new CurState();
    
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
        if (TryGetComponent<CheckAround>(out checkaround) == false)
        {
            gameObject.AddComponent<CheckAround>();
            TryGetComponent<CheckAround>(out checkaround);
        }

        inputcom = ComponentManager.GetI.GetMyComponent(EnumTypes.eComponentTypes.InputCom) as CInputComponent;
        //if (inputcom == null)
        //    Debug.Log("MoveCom ���� inputcom = null");

        com.CharacterRoot = GameObject.Find("CharacterRoot").transform;
        com.CharacterRig = GetComponent<Rigidbody>();
        com.TpCamRig = GameObject.Find("TPCamRig").transform;
        com.TpCam = GameObject.Find("TPCam").transform;
        com.FpRoot = GameObject.Find("FPRoot").transform;
        com.FpCamRig = GameObject.Find("FPCamRig").transform;
        com.FpCam = GameObject.Find("FPCam").transform;
        com.CapsuleCol = GetComponent<CapsuleCollider>();

        com.animator = GetComponentInChildren<Animator>();
        //if (com.animator == null)
        //    Debug.Log("MoveCom ���� com.animator = null");

        ChangePerspective();
        ShowCursor(false);
    }


    //public void CharacterDirectionMove(Vector3 direction)
    //{
    //    MoveDir = direction;
    //}




    public void Move()
    {

        MoveDir.Normalize();

        WorldMove *= (curval.IsRunning) ? moveoption.RunSpeed * Time.deltaTime : moveoption.MoveSpeed * Time.deltaTime;


        if (curval.IsFowordBlock && !curval.IsGrounded || curval.IsJumping && curval.IsGrounded || curval.IsJumping && curval.IsFowordBlock)
        {
            WorldMove.x = 0;
            WorldMove.z = 0;
        }


        if (curval.IsOnTheSlop)
        {
            curval.CurVirVelocity = new Vector3(0, CurGravity + moveoption.SlopAccel, 0);//�߷°��� ���ο����� �̲��������� ���ӵ���

            //CurVirVelocity = new Vector3(0, 0, 0);
            if (curval.IsSlip)
            {
                curval.CurHorVelocity = new Vector3(WorldMove.x, 0.0f, WorldMove.z);
                Vector3 temp = -curval.CurGroundCross;
                //CurHorVelocity = new Vector3(WorldMove.x, 0, WorldMove.z);
                temp = com.FpRoot.forward;
                curval.CurHorVelocity = Quaternion.AngleAxis(-curval.CurGroundSlopAngle, curval.CurGroundCross) * curval.CurHorVelocity;//���ο� ���� y�� �̵�����
                curval.CurHorVelocity *= moveoption.MoveSpeed;
                curval.CurHorVelocity *= -1.0f;
                //com.CharacterRig.velocity = new Vector3(CurHorVelocity.x, CurGravity, CurHorVelocity.z);
                //com.CharacterRig.velocity = CurHorVelocity + CurVirVelocity;
            }
            else
            {
                curval.CurHorVelocity = new Vector3(WorldMove.x, 0.0f, WorldMove.z);
                curval.CurHorVelocity = Quaternion.AngleAxis(-curval.CurGroundSlopAngle, curval.CurGroundCross) * curval.CurHorVelocity;//���ο� ���� y�� �̵�����
                //com.CharacterRig.velocity = new Vector3(WorldMove.x, CurGravity, WorldMove.z);//������ ����ߴ� ����
                //com.CharacterRig.velocity = new Vector3(CurHorVelocity.x*MoveAccel, CurGravity, CurHorVelocity.z* MoveAccel);//�̰� ���������϶��� �̿��ϵ���
            }
            Debug.DrawLine(this.transform.position, this.transform.position + (curval.CurHorVelocity + curval.CurVirVelocity));
            com.CharacterRig.velocity = curval.CurHorVelocity + curval.CurVirVelocity;
        }
        else
        {
            com.CharacterRig.velocity = new Vector3(WorldMove.x, CurGravity, WorldMove.z);
        }
        //com.CharacterRig.velocity = new Vector3(WorldMove.x, CurGravity, WorldMove.z);
        //com.CharacterRig.velocity = new Vector3(WorldMove.x, CurGravity, WorldMove.z);

        if (curval.IsMoving)
        {
            if(curval.IsRunning)
            {
                AnimationManager.Instance.SetPlaySpeed(com.animator, 1f);
                AnimationManager.Instance.Play(com.animator, "_Dash");
            }
            else
            {
                AnimationManager.Instance.SetPlaySpeed(com.animator, 1f);
                AnimationManager.Instance.Play(com.animator, "_Walk");
            }
        }
        else
        {
            if(curval.IsRolling)
            {
                AnimationManager.Instance.SetPlaySpeed(com.animator,1.8f);
                AnimationManager.Instance.Play(com.animator, "_Rolling");
            }
            else if(curval.IsAttacking)
            {
                AnimationManager.Instance.SetPlaySpeed(com.animator, 1.0f);
                AnimationManager.Instance.Play(com.animator, "_Attack02");
            }
            else
            {
                AnimationManager.Instance.SetPlaySpeed(com.animator, 1f);
                AnimationManager.Instance.Play(com.animator, "_Idle");
            }
            
            
        }
            

    }


    //��� ȸ���� �Ϸ�� ������ �����ؾ� �Ѵ�.
    //x,z���� �������� ��� y���� �������� ���� ����
    public void HorVelocity()
    {
        //CurHorVelocity = com.FpCamRig.forward;


        if (curval.IsSlip)
        {
            //�������� ���� �ٴ� ��簢�� -�� �ؼ� ȸ���� ��Ŵ
        }
        curval.CurHorVelocity = Quaternion.AngleAxis(-curval.CurGroundSlopAngle, curval.CurGroundCross) * curval.CurHorVelocity;//�̷������� ���͸� ȸ����ų �� �ִ�. ���� �������� �ʴ´�.

    }

    public void Falling()
    {
        float deltacof = Time.deltaTime * 10f;

        if (curval.IsGrounded)
        {
            if (curval.IsJumping)
                curval.IsJumping = false;
            CurGravity = 0;
            moveoption.Gravity = 1;
        }
        else
        {
            moveoption.Gravity += 0.098f;
            CurGravity -= deltacof * moveoption.Gravity;
        }
    }

    public void Jump()
    {
        if (Time.time >= curval.LastJump + moveoption.JumpcoolTime)
        {
            curval.LastJump = Time.time;
            curval.IsJumping = true;
            CurGravity = moveoption.JumpPower;
        }

    }

    private void ShowCursorToggle()
    {
        curval.IsCursorActive = !curval.IsCursorActive;
        ShowCursor(curval.IsCursorActive);
    }

    private void ShowCursor(bool value)
    {
        Cursor.visible = value;
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
    }



    

    //������
    public void Rolling()
    {
        //�̹� ������ ������ ������ ���Ѵ�.
        if (curval.IsRolling)
            return;
        //���� �־�� ������ ����
        if (!curval.IsGrounded)
            return;

        curval.IsRolling = true;

        //AnimationManager.Instance.Play(com.animator, "_Rolling");
        //Debug.Log($"{AnimationManager.Instance.GetClipLength(com.animator,"_Rolling")}");
        StartCoroutine(Rolling_Coroutine(AnimationManager.Instance.GetClipLength(com.animator, "_Rolling")));
    }

    IEnumerator Rolling_Coroutine(float time)
    {
        float temptime = time;
        //float speed = AnimationManager.Instance.GetPlaySpeed(com.animator);
        //speed -= 1.0f;
        //speed = Mathf.Abs(speed);
       // Debug.Log($"{temptime}*={speed}");
        temptime /= 1.8f;
        
        int tempval = (int)(temptime / 0.016f);
        Debug.Log($"{temptime}/{0.016} -> {tempval}ȸ �ݺ�");
        int i = 0;
        Vector3 tempmove = Vector3.zero;
        tempmove = com.FpRoot.forward; 
        tempmove *= 100;

        Vector3 dest = this.transform.position + tempmove; 

        while (true)
        {
            if(i>=tempval)
            {
                curval.IsRolling = false;
                yield break;
            }

            this.transform.position = Vector3.Lerp(this.transform.position, dest, Time.deltaTime);

            //com.CharacterRig.velocity = new Vector3(tempmove.x, tempmove.y, tempmove.z);

            i++;
            yield return new WaitForSeconds(0.016f);
        }
    }


    public void Rotation()
    {
        //1 ��Ī �϶�
        //fp root�� �¿�ȸ��
        //fp cam rig�� ����ȸ��
        if (curval.IsFPP)
        {
            RotateFP();
        }
        else//3 ��Ī �϶�
        //fp root�� �¿�ȸ��
        //tp cam rig�� �¿� �� ����ȸ��
        {
            RotateTP();
            RotateTPFP();
        }
    }

    //1��Ī�϶�ȸ�� 3��Ī�� ���ΰ� 1��Ī ĳ���͸� ȸ������ �ش�.
    public void RotateFP()
    {
        float xRotPrev = com.FpRoot.localEulerAngles.y;
        float xRotNext = xRotPrev + MouseMove.x * Time.deltaTime * 50f * moveoption.RotMouseSpeed;
        xnext = xRotNext;
        //if (xRotNext > 180f)
        //    xRotNext -= 360f;

        float yRotPrev = com.FpCamRig.localEulerAngles.x;
        float yRotNext = yRotPrev + MouseMove.y * Time.deltaTime * 50f * moveoption.RotMouseSpeed;
        ynext = yRotNext;


        com.FpRoot.localEulerAngles = Vector3.up * xRotNext;
        updown = com.FpRoot.localEulerAngles;
        com.FpCamRig.localEulerAngles = Vector3.right * yRotNext;
        rightleft = com.FpCamRig.localEulerAngles;

    }


    //3��Ī�϶�
    public void RotateTP()
    {
        float xRotPrev = com.TpCamRig.localEulerAngles.y;
        float xRotNext = xRotPrev + MouseMove.x * Time.deltaTime * 50f * moveoption.RotMouseSpeed;

        //if (xRotNext > 180f)
        //    xRotNext -= 360f;

        float yRotPrev = com.TpCamRig.localEulerAngles.x;
        float yRotNext = yRotPrev + MouseMove.y * Time.deltaTime * 50f * moveoption.RotMouseSpeed;



        //TpCamRig.localEulerAngles = Vector3.up * xRotNext;

        //TpCamRig.localEulerAngles = Vector3.right * yRotNext;

        com.TpCamRig.localEulerAngles = new Vector3(yRotNext, xRotNext, 0);
    }

    //�̋��� ���콺�ο����̴°Ծƴϰ� Ű���� �Է¿� ���� ȸ�� �ؾ� �ϱ⶧���� ���� ����
    public void RotateTPFP()
    {
        float nextRotY = 0;
        WorldMove = com.TpCamRig.TransformDirection(MoveDir);
        float curRotY = com.FpRoot.localEulerAngles.y;

        if (WorldMove.sqrMagnitude != 0)
            nextRotY = Quaternion.LookRotation(WorldMove, Vector3.up).eulerAngles.y;

        if (!curval.IsMoving) nextRotY = curRotY;

        if (nextRotY - curRotY > 180f) nextRotY -= 360f;
        else if (curRotY - nextRotY > 180f) nextRotY += 360f;

        com.FpRoot.eulerAngles = Vector3.up * Mathf.Lerp(curRotY, nextRotY, 0.1f);
    }




    void ChangePerspective()
    {
        curval.IsFPP = !curval.IsFPP;
        com.FpCam.gameObject.SetActive(curval.IsFPP);
        com.TpCam.gameObject.SetActive(!curval.IsFPP);
    }

    

    void Update()
    {
        Falling();
        Rotation();
        HorVelocity();
        Move();
    }
}

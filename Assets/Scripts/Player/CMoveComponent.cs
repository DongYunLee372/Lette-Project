using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//방어 상태일때 
public class CMoveComponent : BaseComponent
{
    public enum Direction
    {
        Front,
        Right,
        Left,
        Back
    }

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

        public AnimationController animator = null;
    }

    [System.Serializable]
    public class MoveOption
    {
        [Header("==================이동 관련 변수들==================")]
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
        public float Gravity;//중력값(프레임단위로 증가시켜줄 값)
        [SerializeField]
        public float JumpPower = 120;//점프를 하면 해당 값으로 curgravity값을 바꿔준다.
        [SerializeField]
        public float JumpcoolTime = 1f;
        [SerializeField]
        public LayerMask GroundMask;
        [SerializeField]
        public float MaxSlop = 70;
        [SerializeField]
        public float SlopAccel;//(중력값과 같이 미끌어질때 점점증가될 값)

        public float RunningStaminaVal;
        [Header("==================회피 관련 변수들==================")]
        public AnimationClip RollingClip;

        public float RollingClipPlaySpeed = 2.3f;

        public float RollingDistance = 80;

        public float RollingTime;

        public float RollingFreeDamageTime;

        public float NextRollingTime = 0.1f;

        

        [Header("==================피격 관련 변수들==================")]
        public AnimationClip KnockDownClip;

        public AnimationClip KnockBackClip;

        public float KnockDownTime;

        public float KnockBackTime;

        [Header("==================가드 중 이동 관련 변수들==================")]
        public AnimationClip GuardFrontMoveClip;
        public AnimationClip GuardRightMoveClip;
        public AnimationClip GuardLeftMoveClip;
        public AnimationClip GuardBackMoveClip;

        public float GuardMoveSpeed;

        [Header("==================카메라 관련 변수들==================")]
        public float FPMaxZoomIn = 60.0f;
        public float FPMaxZoomOut = 90.0f;

        public float TPMaxZoomIn = 60.0f;
        public float TPMaxZoomOut = 90.0f;

        public float ScrollSpeed = 500.0f;
    }

    public Vector2 MouseMove = Vector2.zero;

    public Vector3 MoveDir = Vector3.zero;

    public Vector3 WorldMove = Vector3.zero;

    public float CurGravity;//현재 벨로시티의 y값

    public Com com = new Com();

    public CurState curval = new CurState();
    
    public MoveOption moveoption = new MoveOption();

    public CInputComponent inputcom = null;

    public float RollingStartTime;

    public AnimationEventSystem eventsystem;

    public CorTimeCounter timecounter = new CorTimeCounter();
    //public delegate void invoke();
    public float lastRollingTime;

    public float lastRunningTime;

    public Vector3 Capsuletopcenter => new Vector3(transform.position.x, transform.position.y + com.CapsuleCol.height - com.CapsuleCol.radius, transform.position.z);
    public Vector3 Capsulebottomcenter => new Vector3(transform.position.x, transform.position.y + com.CapsuleCol.radius, transform.position.z);

    public delegate void Invoker(string s_val);

    [Header("============TestVals============")]

    public Vector3 updown;
    public float xnext;

    public Vector3 rightleft;
    public float ynext;

    public Vector3 teststart;
    public Vector3 testend;

    public Transform testcube;

    void Start()
    {
        if (TryGetComponent<CheckAround>(out checkaround) == false)
        {
            gameObject.AddComponent<CheckAround>();
            TryGetComponent<CheckAround>(out checkaround);
        }

        eventsystem = GetComponentInChildren<AnimationEventSystem>();
        inputcom = PlayableCharacter.Instance.GetMyComponent(EnumTypes.eComponentTypes.InputCom) as CInputComponent;
        //if (inputcom == null)
        //    Debug.Log("MoveCom 오류 inputcom = null");

        //com.CharacterRoot = GameObject.Find("CharacterRoot").transform;
        com.CharacterRig = GetComponent<Rigidbody>();
        com.CapsuleCol = GetComponent<CapsuleCollider>();

        com.animator = GetComponentInChildren<AnimationController>();
        //if (com.animator == null)
        //    Debug.Log("MoveCom 오류 com.animator = null");


        eventsystem.AddEvent(new KeyValuePair<string, AnimationEventSystem.beginCallback>(null, null),
                             new KeyValuePair<string, AnimationEventSystem.midCallback>(moveoption.KnockDownClip.name, KnockDownPause),
                             new KeyValuePair<string, AnimationEventSystem.endCallback>(moveoption.KnockDownClip.name, KnockDownEnd));

        eventsystem.AddEvent(new KeyValuePair<string, AnimationEventSystem.beginCallback>(null, null),
                             new KeyValuePair<string, AnimationEventSystem.midCallback>(null, null),
                             new KeyValuePair<string, AnimationEventSystem.endCallback>(moveoption.KnockBackClip.name, KnockBackEnd));

        eventsystem.AddEvent(new KeyValuePair<string, AnimationEventSystem.beginCallback>(moveoption.RollingClip.name, ActivateNoDamage),
                             new KeyValuePair<string, AnimationEventSystem.midCallback>(null, null),
                             new KeyValuePair<string, AnimationEventSystem.endCallback>(moveoption.RollingClip.name, DeActivateNoDamage));


        ChangePerspective();
        ShowCursor(false);


    }


    //public void CharacterDirectionMove(Vector3 direction)
    //{
    //    MoveDir = direction;
    //}
    public void Stop()
    {
        com.CharacterRig.velocity = new Vector3(0, 0, 0);
    }

    public void GuardMove(Direction dir)
    {
        Vector3 tempmove;
        Vector3 tempdir;
        if (dir== Direction.Front)
        {
            tempdir = new Vector3(0, 0, 1);
            
        }
        else if(dir == Direction.Left)
        {
            tempdir = new Vector3(-1, 0, 0);
            Debug.Log("guardleft");
            com.animator.Play(moveoption.GuardLeftMoveClip.name, 1.5f);
        }
        else if(dir == Direction.Right)
        {
            tempdir = new Vector3(1, 0, 0);
            Debug.Log("guardright");
            com.animator.Play(moveoption.GuardRightMoveClip.name, 1.5f);
        }
        else
        {
            tempdir = new Vector3(0, 0, -1);
            Debug.Log("guardback");
            com.animator.Play(moveoption.GuardBackMoveClip.name, 1.5f);
        }
        tempmove = com.FpCamRig.TransformDirection(tempdir);
        tempmove = tempmove * moveoption.GuardMoveSpeed * Time.deltaTime;
        com.CharacterRig.velocity = new Vector3(tempmove.x, CurGravity, tempmove.z);

    }

    //duration 시간동안 목표위치로 이동한다.
    public void DoMove(Vector3 destpos, float duration)
    {
        Vector3 startpos = this.transform.position;
        Vector3 directon = destpos - startpos;
        float speed = directon.magnitude / duration;

        StartCoroutine(CorDoMove(startpos, destpos, duration));
    }

    //duration 시간동안 목표위치로 이동한다.
    public void FowardDoMove(float distnace, float duratoin)
    {
        Vector3 direction = com.FpRoot.forward * distnace;
        Vector3 dest = transform.position + direction;

        StartCoroutine(CorDoMove(transform.position, dest, duratoin));
    }

    public IEnumerator CorDoMove(Vector3 start, Vector3 dest, float duration, Invoker invoker = null)
    {
        float runtime = 0.0f;
        //Debug.DrawLine(start, dest, Color.green);
        teststart = start;
        testend = dest;

        Vector3 direction = dest - start;

        //Move(direction * 2);

        while (true)
        {
            //direction = dest - transform.position;
            //direction = Quaternion.AngleAxis(-curval.CurGroundSlopAngle, curval.CurGroundCross) * direction;//경사로에 의한 y축 이동방향
            //dest = transform.position + direction;

            direction = dest - transform.position;

            if (CharacterStateMachine.Instance.GetState()==CharacterStateMachine.eCharacterState.OutOfControl)
            {
                if (invoker != null)
                    invoker.Invoke("");

                Move(new Vector3(0, 0, 0));
                yield break;
            }

            if(runtime>=duration)
            {
                //this.transform.position = dest;
                if (invoker != null)
                    invoker.Invoke("");

                Move(new Vector3(0, 0, 0));

                yield break;
            }
            runtime += Time.deltaTime;

            //if (!curval.IsFowordBlock)
            //    transform.position = Vector3.Lerp(start, dest, runtime / duration);

            if (!curval.IsFowordBlock)
                Move(direction);

            //if (!curval.IsFowordBlock)
            //{
            //    Vector3 temp = Vector3.Lerp(start, dest, runtime / duration);
            //    //Move()
            //}


            yield return new WaitForSeconds(Time.deltaTime);
        }

    }

    //특정 시간동안 해당 방향과 속도로 움직인다.
    public IEnumerator CorDoDirectionMove(Vector3 direction, float duration, Invoker invoker = null)
    {
        float runtime = 0.0f;
        while (true)
        {

            if (runtime >= duration)
            {
                //this.transform.position = dest;
                if (invoker != null)
                    invoker.Invoke("");
                yield break;
            }
            runtime += Time.deltaTime;

            Move(direction);

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }


    public void FowardMove(float distance)
    {

    }

    public void MoveCalculate()
    {
        MoveDir.Normalize();

        WorldMove *= (curval.IsRunning && PlayableCharacter.Instance.status.CurStamina - moveoption.RunningStaminaVal >= 0) ? moveoption.RunSpeed * Time.deltaTime : moveoption.MoveSpeed * Time.deltaTime;

        //if ()
        //{
        //    //WorldMove *=
        //}

        if (curval.IsFowordBlock && !curval.IsGrounded || curval.IsJumping && curval.IsGrounded || curval.IsJumping && curval.IsFowordBlock ||curval.IsRolling || curval.IsAttacking)
        {
            WorldMove.x = 0;
            WorldMove.z = 0;
            return;
        }

        Move(WorldMove);
    }

    //움직일 방향과 거리를 넣어주면 현재 지형에 따라서 움직여 준다.
    public void Move(Vector3 MoveVal)
    {

        if (curval.IsOnTheSlop)
        {
            curval.CurVirVelocity = new Vector3(0, CurGravity + moveoption.SlopAccel, 0);//중력값과 경사로에서의 미끄러질때의 가속도값

            //CurVirVelocity = new Vector3(0, 0, 0);
            if (curval.IsSlip)
            {
                curval.CurHorVelocity = new Vector3(MoveVal.x, 0.0f, MoveVal.z);
                Vector3 temp = -curval.CurGroundCross;
                //CurHorVelocity = new Vector3(WorldMove.x, 0, WorldMove.z);
                temp = com.FpRoot.forward;
                curval.CurHorVelocity = Quaternion.AngleAxis(-curval.CurGroundSlopAngle, curval.CurGroundCross) * curval.CurHorVelocity;//경사로에 의한 y축 이동방향
                curval.CurHorVelocity *= moveoption.MoveSpeed;
                curval.CurHorVelocity *= -1.0f;
                //com.CharacterRig.velocity = new Vector3(CurHorVelocity.x, CurGravity, CurHorVelocity.z);
                //com.CharacterRig.velocity = CurHorVelocity + CurVirVelocity;
            }
            else
            {
                curval.CurHorVelocity = new Vector3(MoveVal.x, 0.0f, MoveVal.z);
                curval.CurHorVelocity = Quaternion.AngleAxis(-curval.CurGroundSlopAngle, curval.CurGroundCross) * curval.CurHorVelocity;//경사로에 의한 y축 이동방향
                //com.CharacterRig.velocity = new Vector3(WorldMove.x, CurGravity, WorldMove.z);//이전에 사용했던 무브
                //com.CharacterRig.velocity = new Vector3(CurHorVelocity.x*MoveAccel, CurGravity, CurHorVelocity.z* MoveAccel);//이건 슬립상태일때만 이용하도록
            }
            Debug.DrawLine(this.transform.position, this.transform.position + (curval.CurHorVelocity + curval.CurVirVelocity));
            com.CharacterRig.velocity = curval.CurHorVelocity + curval.CurVirVelocity;
        }
        else
        {
            com.CharacterRig.velocity = new Vector3(MoveVal.x, CurGravity, MoveVal.z);
        }
        //com.CharacterRig.velocity = new Vector3(WorldMove.x, CurGravity, WorldMove.z);
        //com.CharacterRig.velocity = new Vector3(WorldMove.x, CurGravity, WorldMove.z);

    }



    //모든 회전이 완료된 다음에 동작해야 한다.
    //x,z축의 움직임을 담당 y축의 움직임은 따로 관리
    public void HorVelocity()
    {
        //CurHorVelocity = com.FpCamRig.forward;


        if (curval.IsSlip)
        {
            //움직임을 현재 바닥 경사각의 -로 해서 회전을 시킴
        }
        curval.CurHorVelocity = Quaternion.AngleAxis(-curval.CurGroundSlopAngle, curval.CurGroundCross) * curval.CurHorVelocity;//이럭식으로 벡터를 회전시킬 수 있다. 역은 성립하지 않는다.

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

    


    public void KnockDown()
    {
        //이미 넉다운 중일때 해당 함수가 다시 들어오면 그냥 리턴
        if(curval.IsKnockDown)
        {
            //curval.IsKnockDown = false;
            return;
        }

        curval.IsKnockDown = true;

        com.animator.Play(moveoption.KnockDownClip.name);
    }


    public void KnockDownPause(string s_val)
    {
        
        if (com.animator.GetPlaySpeed() != 0.0f)
        {
            Debug.Log("멈춤");
            com.animator.Pause();
            StartCoroutine(timecounter.Cor_TimeCounter(moveoption.KnockDownTime, KnockDownPause));
        }
        else
        {
            Debug.Log("다시시작");
            com.animator.Resume();
        }

    }

    public void KnockDownEnd(string s_val)
    {
        Debug.Log($"{s_val} 들어옴");
        curval.IsKnockDown = false;
    }


    public void KnockBack()
    {
        //이미 넉백 중 일때 해당 함수가 다시 들어오면 다시 넉백 실행
        if(curval.IsKnockBack)
        {
            com.animator.Play(moveoption.KnockBackClip.name);
            return;
        }

        curval.IsKnockBack = true;

        Debug.Log($"넉백실행");
        com.animator.Play(moveoption.KnockBackClip.name);
    }

    public void KnockBackEnd(string s_val)
    {
        Debug.Log($"{s_val} 들어옴");
        curval.IsKnockBack = false;
    }

    ////공격이 시작된지 일정 시간 뒤에 이펙트를 실행해야 할 때 사용
    //IEnumerator Cor_TimeCounter(float time, Invoker invoker)
    //{
    //    float starttime = Time.time;

    //    while (true)
    //    {
    //        if ((Time.time - starttime) >= time)
    //        {
    //            invoker.Invoke("");
    //            yield break;
    //        }
    //        yield return new WaitForSeconds(Time.deltaTime);
    //    }
    //}

    public void Damaged_Rolling(float damage,Vector3 hitpoint)
    {
        if (curval.IsRolling && curval.IsNoDamage)
        {
            return;
        }
        else
        {
            PlayableCharacter.Instance.Damaged(damage, hitpoint);
        }
    }


    //구르기
    //무적시간은 처음 구르기가 시작된 시점부터 카운트한다.
    public void Rolling()
    {
        //이미 구르고 있으면 구르지 못한다.
        if (curval.IsRolling || Time.time - lastRollingTime <= moveoption.NextRollingTime) 
            return;
        //땅에 있어야 구르기 가능
        if (!curval.IsGrounded)
            return;

        if (PlayableCharacter.Instance.status.CurStamina - 20 >= 0)
        {
            PlayableCharacter.Instance.status.CurStamina = PlayableCharacter.Instance.status.CurStamina - 20;
        }
        else
        {
            return;
        }

        curval.IsRolling = true;

        
        

        //AnimationManager.Instance.Play(com.animator, "_Rolling");
        //Debug.Log($"{AnimationManager.Instance.GetClipLength(com.animator,"_Rolling")}");

        com.animator.Play("_Rolling", moveoption.RollingClipPlaySpeed);

        //FowardDoMove(10, com.animator.GetClipLength("_Rolling") / moveoption.RollingClipPlaySpeed);
        
        Vector3 tempmove = this.transform.position + com.FpRoot.forward * moveoption.RollingDistance;

        StartCoroutine(CorDoMove(this.transform.position, tempmove, com.animator.GetClipLength("_Rolling") / moveoption.RollingClipPlaySpeed -0.2f, RollingOver));

        Vector3 moveval = com.FpRoot.forward* moveoption.RollingDistance;

        //StartCoroutine(CorDoDirectionMove(moveval, com.animator.GetClipLength("_Rolling") / moveoption.RollingClipPlaySpeed));

        RollingStartTime = Time.time;
    }

    public void RollingOver(string s)
    {
        lastRollingTime = Time.time;
        curval.IsRolling = false;
    }

    public void ActivateNoDamage(string s)
    {
        //moveoption.NowIsNoDamege = true;
        curval.IsNoDamage = true;
    }

    public void DeActivateNoDamage(string s)
    {
        curval.IsNoDamage = false;
    }

    

    IEnumerator Rolling_Coroutine(float time)
    {
        float temptime = time;
        temptime /= moveoption.RollingClipPlaySpeed;

        float curtime = 0.0f;

        //int tempval = (int)(temptime / 0.016f);
        //Debug.Log($"{temptime}/{0.016} -> {tempval}회 반복");

        int i = 0;
        Vector3 tempmove = Vector3.zero;
        tempmove = com.FpRoot.forward; 
        tempmove *= moveoption.RollingDistance;

        //Vector3 dest = this.transform.position + tempmove; 

        while (true)
        {
            curtime += Time.deltaTime;

            if (curtime >= temptime)
            {
                curval.IsRolling = false;
                yield break;
            }


            //if (!curval.IsFowordBlock)
            //    this.transform.position = Vector3.Lerp(this.transform.position, dest, Time.deltaTime);

            if (!curval.IsFowordBlock)
                Move(tempmove);

            //Vector3 temp = Vector3.Lerp(start, dest, runtime / duration);

            //com.CharacterRig.velocity = new Vector3(tempmove.x, tempmove.y, tempmove.z);

            //i++;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    
    public void Rotation()
    {
        //1 인칭 일때
        //fp root로 좌우회전
        //fp cam rig로 상하회전
        if (curval.IsFPP)
        {
            RotateFP();
        }
        else//3 인칭 일때
        //fp root로 좌우회전
        //tp cam rig로 좌우 및 상하회전
        {
            RotateTP();
            RotateTPFP();
        }
    }

    //1인칭일때회전 3인칭은 놔두고 1인칭 캐릭터만 회전시켜 준다.
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


    //3인칭일때
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

    //이떄는 마우스로움직이는게아니고 키보드 입력에 따라서 회전 해야 하기때문에 따로 만듦
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


    //3인칭 일때 해당 방향을 바라보도록 회전
    public void LookAt(Vector3 direction)
    {

    }

    //3인칭 카메라가 정면방향을 바라보도록 회전
    public void LookAtFoward()
    {
        Vector3 foward = com.FpCamRig.forward;

        Vector3 rot = Quaternion.LookRotation(foward, Vector3.up).eulerAngles;

        Vector3 temp = com.TpCamRig.eulerAngles;

        com.TpCamRig.eulerAngles = new Vector3(temp.x, rot.y, temp.z);
    }

    //줌인 
    public void ZoomIn(float scroll)
    {
        if (scroll >=0)
            return;
        Camera curcam;
        

        if(curval.IsFPP)
        {
            curcam = com.FpCam.GetComponent<Camera>();
            if (curcam.fieldOfView <= moveoption.FPMaxZoomIn)
            {
                curcam.fieldOfView = moveoption.FPMaxZoomIn;
                return;
            }



        }
        else
        {
            curcam = com.TpCam.GetComponent<Camera>();

            if (curcam.fieldOfView <= moveoption.TPMaxZoomIn)
            {
                curcam.fieldOfView = moveoption.TPMaxZoomIn;
                return;
            }


        }

        scroll = scroll * moveoption.ScrollSpeed * Time.deltaTime;

        curcam.fieldOfView += scroll;

    }

    //줌 아웃
    public void ZoomOut(float scroll)
    {
        if (scroll<=0)
            return;

        Camera curcam;
        if (curval.IsFPP)
        {
            curcam = com.FpCam.GetComponent<Camera>();
            if (curcam.fieldOfView >= moveoption.FPMaxZoomOut)
            {
                curcam.fieldOfView = moveoption.FPMaxZoomOut;
                return;
            }
        }
        else
        {
            curcam = com.TpCam.GetComponent<Camera>();
            if (curcam.fieldOfView >= moveoption.TPMaxZoomOut)
            {
                curcam.fieldOfView = moveoption.TPMaxZoomOut;
                return;
            }
        }

        scroll = scroll * moveoption.ScrollSpeed * Time.deltaTime;

        curcam.fieldOfView += scroll;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(teststart, testend);
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
        MoveCalculate();
        if(curval.IsMoving&&curval.IsRunning&& PlayableCharacter.Instance.status.CurStamina >= moveoption.RunningStaminaVal)
        {
            if (Time.time - lastRunningTime >= 1.0f)
            {
                lastRunningTime = Time.time;
                PlayableCharacter.Instance.status.CurStamina -= moveoption.RunningStaminaVal;
            }
            
        }
    }
}

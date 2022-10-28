using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//해야할 일 
//상수화 할 상수들을 관리할 크래스 제작
//curval 에서 확인
//컬링 알아보기
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
        p_comtype = CharEnumTypes.eComponentTypes.MoveCom;
    }

    [System.Serializable]
    public class Com
    {
        public Transform CharacterRoot = null;

        public Transform TpCamRig = null;
        public Transform TpCam = null;
        public Transform TpCamPos = null;
        public Transform TpCamPos2 = null;

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
        public bool RightReverse = false;



        [SerializeField]
        [Range(0.0f,1.0f)]
        public float RotSpeed = 0.5f;
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

        public float MaxStep;

        public float RunningStaminaVal;

        [Header("==================계단 이동 관련 변수들==================")]

        public float StepHeight;//올라갈 수 있는 계단 높이

        public float StepCkeckDis;//눈높이에서 해당 위치만큼 이동한 곳에서 수직아래로 레이를 쏜다.

        public float StepWeightVal = 3000;//계단을 올라가기 위한 가중치 연속적인 계단을 올라갈때 움직임 속도가 빨라지는 

        

        [Header("==================회피 관련 변수들==================")]
        public AnimationClip RollingClip;

        public float RollingClipPlaySpeed = 2.3f;

        public float RollingDistance = 80;

        public float RollingTime;

        [Header("무적 시작 시간")]
        public float RollingNoDamageStartTime;
        [Header("무적 유지 시간(회피동작이 끝나면 자동으로 끊김)")]
        public float RollingNoDamageTime;

        //public float RollingFreeDamageTime;

        public float NextRollingTime = 0.1f;

        public float RollingStaminaDown = 20.0f;

        

        [Header("==================피격 관련 변수들==================")]
        public AnimationClip KnockDownClip;

        public AnimationClip KnockBackClip;

        public float KnockDownTime;

        public float KnockBackTime;

        [Header("==================가드 중 이동 관련 변수들==================")]
        public string GuardFrontMoveClip;
        public string GuardRightMoveClip;
        public string GuardLeftMoveClip;
        public string GuardBackMoveClip;

        public float GuardMoveSpeed;

        [Header("==================카메라 관련 변수들==================")]
        public float FPMaxZoomIn = 60.0f;
        public float FPMaxZoomOut = 90.0f;

        public float TPMaxZoomIn = 60.0f;
        public float TPMaxZoomOut = 90.0f;

        public float ScrollSpeed = 500.0f;
    }

    [HideInInspector]
    public Vector2 MouseMove = Vector2.zero;
    //[HideInInspector]
    public Vector3 MoveDir = Vector3.zero;
    //[HideInInspector]
    public Vector3 WorldMove = Vector3.zero;
    [HideInInspector]
    public float CurGravity;//현재 벨로시티의 y값
    //[HideInInspector]
    public Com com = new Com();
    //[HideInInspector]
    public CurState curval = new CurState();

    public MoveOption moveoption = new MoveOption();
    [HideInInspector]
    public CInputComponent inputcom = null;
    [HideInInspector]
    public float RollingStartTime;
    [HideInInspector]
    public AnimationEventSystem eventsystem;
    [HideInInspector]
    public CorTimeCounter timecounter = new CorTimeCounter();
    [HideInInspector]
    public float lastRollingTime;
    [HideInInspector]
    public float lastRunningTime;
    [HideInInspector]
    public Vector3 Capsuletopcenter => new Vector3(transform.position.x, transform.position.y + com.CapsuleCol.height - com.CapsuleCol.radius, transform.position.z);
    [HideInInspector]
    public Vector3 Capsulebottomcenter => new Vector3(transform.position.x, transform.position.y + com.CapsuleCol.radius, transform.position.z);

    public float CurStepWeight = 0;

    public float CharacterHeight => com.CapsuleCol.height;

    [HideInInspector]
    public delegate void Invoker(string s_val);

    private IEnumerator RollingNoDamageStartCor = null;
    private IEnumerator RollingNoDamageEndCor = null;

    //[Header("============TestVals============")]

    //public Vector3 updown;
    //public float xnext;

    //public Vector3 rightleft;
    //public float ynext;

    public Transform playerChestTr;


    public Vector3 teststart;
    public Vector3 testend;

    //public Transform testcube;

    void Start()
    {
        if (TryGetComponent<CheckAround>(out checkaround) == false)
        {
            gameObject.AddComponent<CheckAround>();
            TryGetComponent<CheckAround>(out checkaround);
        }
        eventsystem = GetComponentInChildren<AnimationEventSystem>();
        com.CharacterRig = GetComponent<Rigidbody>();
        com.CapsuleCol = GetComponent<CapsuleCollider>();
        com.animator = GetComponentInChildren<AnimationController>();

        if (playerChestTr == null)
            playerChestTr = com.animator.animator.GetBoneTransform(HumanBodyBones.Head);

        eventsystem.AddEvent(new KeyValuePair<string, AnimationEventSystem.beginCallback>(null, null),
                             new KeyValuePair<string, AnimationEventSystem.midCallback>(moveoption.KnockDownClip.name, KnockDownPause),
                             new KeyValuePair<string, AnimationEventSystem.endCallback>(moveoption.KnockDownClip.name, KnockDownEnd));

        eventsystem.AddEvent(new KeyValuePair<string, AnimationEventSystem.beginCallback>(null, null),
                             new KeyValuePair<string, AnimationEventSystem.midCallback>(null, null),
                             new KeyValuePair<string, AnimationEventSystem.endCallback>(moveoption.KnockBackClip.name, KnockBackEnd));

        //eventsystem.AddEvent(new KeyValuePair<string, AnimationEventSystem.beginCallback>(moveoption.RollingClip.name, ActivateNoDamage),
        //                     new KeyValuePair<string, AnimationEventSystem.midCallback>(null, null),
        //                     new KeyValuePair<string, AnimationEventSystem.endCallback>(moveoption.RollingClip.name, DeActivateNoDamage));


        ChangePerspective();
        ShowCursor(false);
        LookAtFoward();

    }

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
            //Debug.Log("guardleft");
            com.animator.Play(moveoption.GuardFrontMoveClip, 1.5f);
        }
        else if(dir == Direction.Left)
        {
            tempdir = new Vector3(-1, 0, 0);
            //Debug.Log("guardleft");
            com.animator.Play(moveoption.GuardLeftMoveClip, 1.5f);
        }
        else if(dir == Direction.Right)
        {
            tempdir = new Vector3(1, 0, 0);
            //Debug.Log("guardright");
            com.animator.Play(moveoption.GuardRightMoveClip, 1.5f);
        }
        else
        {
            tempdir = new Vector3(0, 0, -1);
            //Debug.Log("guardback");
            com.animator.Play(moveoption.GuardBackMoveClip, 1.5f);
        }
        tempmove = com.TpCamRig.TransformDirection(tempdir);
        tempmove = tempmove * moveoption.GuardMoveSpeed * Time.deltaTime;
        tempmove.y = 0;
        Move(tempmove);
        //com.CharacterRig.velocity = new Vector3(tempmove.x, CurGravity, tempmove.z);

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

    public Camera GetCamera()
    {
        if (curval.IsFPP)
            return com.FpCam.GetComponent<Camera>();
        else
            return com.TpCam.GetComponent<Camera>();
    }
    public IEnumerator CorDoMove(Vector3 start, Vector3 dest, float duration, Invoker invoker = null)
    {
        float runtime = 0.0f;
        teststart = start;
        testend = dest;

        Vector3 direction = dest - start;


        while (true)
        {

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


            if (!curval.IsFowordBlock)
                Move(direction);

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


    bool temptrigger = false;

    //움직임값을 계산해준다.
    public void MoveCalculate()
    {
        //이동이 가능한지 판단
        if (!curval.CheckMoveAble())
        {
            WorldMove.x = 0;
            WorldMove.z = 0;
            return;
        }
        if(MoveDir.sqrMagnitude<=0)
        {
            Move(Vector3.zero);
        }
        if(curval.IsGuard)
        {
            return;
        }
        

        MoveDir.Normalize();

        if (MoveDir.sqrMagnitude <= 0)
        {

        }

        if(curval.IsOnTheSlop)
        {

        }
        else
        {

        }

        //y성분은 버린다.
        WorldMove = com.TpCamRig.TransformDirection(MoveDir);
        WorldMove.y = 0;
        WorldMove = Quaternion.AngleAxis(-curval.CurGroundSlopAngle, curval.CurGroundCross) * WorldMove;//경사로에 의한 y축 이동방향
        WorldMove.Normalize();

        float speed = (curval.IsRunning && PlayableCharacter.Instance.status.CurStamina - moveoption.RunningStaminaVal >= 0) ? moveoption.RunSpeed : moveoption.MoveSpeed;
        //float speed = (curval.IsRunning && PlayableCharacter.Instance.status.CurStamina  > 0) ? moveoption.RunSpeed : moveoption.MoveSpeed;

        if (WorldMove.magnitude > 0)
        {
            int a = 0;
            a = 10;
            temptrigger = true;
        }


        WorldMove = WorldMove * speed * Time.deltaTime;

        Move(WorldMove);
    }


    //움직일 방향과 거리를 넣어주면 현재 지형에 따라서 움직여 준다.
    //기울어진 지형과 계단에서의 움직임 처리 제작 필요
    public void Move(Vector3 MoveVal)
    {
        

        if(curval.CheckStepAble())
        {
            if(CurStepWeight>=moveoption.StepWeightVal)
            {
                this.transform.position = curval.CurStepPos;
                CurStepWeight = 0;
                return;
            }
            else
            {
                CurStepWeight += MoveVal.magnitude;
            }
        }

        //CurStepWeight = 0;

        if (curval.IsOnTheSlop)
        {
            curval.CurVirVelocity = new Vector3(0, CurGravity/* + moveoption.SlopAccel*/, 0);//중력값과 경사로에서의 미끄러질때의 가속도값

            //CurVirVelocity = new Vector3(0, 0, 0);
            if (curval.IsSlip)
            {
                curval.CurHorVelocity = MoveVal;
                //Vector3 temp = -curval.CurGroundCross;
                //CurHorVelocity = new Vector3(WorldMove.x, 0, WorldMove.z);
                //temp = com.FpRoot.forward;
                //curval.CurHorVelocity = Quaternion.AngleAxis(-curval.CurGroundSlopAngle, curval.CurGroundCross) * curval.CurHorVelocity;//경사로에 의한 y축 이동방향
                //curval.CurHorVelocity *= moveoption.MoveSpeed;
                //curval.CurHorVelocity *= -1.0f;
                //com.CharacterRig.velocity = new Vector3(CurHorVelocity.x, CurGravity, CurHorVelocity.z);
                //com.CharacterRig.velocity = CurHorVelocity + CurVirVelocity;
            }
            else
            {
                curval.CurHorVelocity = MoveVal;
                //curval.CurHorVelocity = Quaternion.AngleAxis(-curval.CurGroundSlopAngle, curval.CurGroundCross) * curval.CurHorVelocity;//경사로에 의한 y축 이동방향
                //com.CharacterRig.velocity = new Vector3(WorldMove.x, CurGravity, WorldMove.z);//이전에 사용했던 무브
                //com.CharacterRig.velocity = new Vector3(CurHorVelocity.x*MoveAccel, CurGravity, CurHorVelocity.z* MoveAccel);//이건 슬립상태일때만 이용하도록
            }

            if(temptrigger)
            {
                int a = 0;
            }

            //Debug.DrawLine(this.transform.position, this.transform.position + (curval.CurHorVelocity + curval.CurVirVelocity));
            com.CharacterRig.velocity = curval.CurHorVelocity + curval.CurVirVelocity;
        }
        else
        {
            if (temptrigger)
            {
                int a = 0;
            }
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

    //땅 위에 없음면 땅을 만날때까지 떨어진다.
    //CurGravity를 누적된 증가값에 따라 증가시켜 준다. 해당 중력값은 Move함수에서 y축 방향 움직임으로 사용된다.
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
            //Debug.Log("멈춤");
            com.animator.Pause();
            StartCoroutine(timecounter.Cor_TimeCounter(moveoption.KnockDownTime, KnockDownPause));
        }
        else
        {
            //Debug.Log("다시시작");
            com.animator.Resume();
        }

    }

    public void KnockDownEnd(string s_val)
    {
        //Debug.Log($"{s_val} 들어옴");
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

        //Debug.Log($"넉백실행");
        com.animator.Play(moveoption.KnockBackClip.name);
    }

    public void KnockBackEnd(string s_val)
    {
        //Debug.Log($"{s_val} 들어옴");
        curval.IsKnockBack = false;
    }


    public void Damaged_Rolling(float damage,Vector3 hitpoint, float Groggy)
    {
        //회피 중 피격 당했을때 무적상태인지 아닌지 판단
        if (/*curval.IsRolling && */curval.IsNoDamage)
        {
            return;
        }
        else
        {
            float nextGroggy = PlayableCharacter.Instance.status.CurGroggy + Groggy;
            if (nextGroggy >= PlayableCharacter.Instance.status.player_Groggy || Groggy >= PlayableCharacter.Instance.status.player_Stagger_Groggy)
                RollingOver();
            PlayableCharacter.Instance.Damaged(damage, hitpoint,Groggy);
        }
    }


    //구르기
    //무적시간은 처음 구르기가 시작된 시점부터 카운트한다.
    public void Rolling()
    {
        if (!curval.CheckRollingAble())
            return;

        //이미 구르고 있으면 구르지 못한다.
        if (Time.time - lastRollingTime <= moveoption.NextRollingTime) 
            return;



        //스테미나가 0이 아니면 실행 가능
        if (PlayableCharacter.Instance.status.CurStamina > 0)
        {
            PlayableCharacter.Instance.status.StaminaDown(moveoption.RollingStaminaDown);
        }
        else
        {
            return;
        }

        //Debug.Log("[Attack] 구리기 시작");   
        curval.IsRolling = true;
        com.animator.Play("_Rolling", moveoption.RollingClipPlaySpeed, 0.0f, 0.2f, RollingOver);

        
        Vector3 tempmove = this.transform.position + com.FpRoot.forward * moveoption.RollingDistance;

        RollingNoDamageStartCor = timecounter.Cor_TimeCounter(moveoption.RollingNoDamageStartTime, ActivateNoDamage);
        StartCoroutine(RollingNoDamageStartCor);
        //StartCoroutine(CorDoMove(this.transform.position, tempmove, com.animator.GetClipLength("_Rolling") / moveoption.RollingClipPlaySpeed -0.2f, RollingOver));

        Vector3 moveval = com.FpRoot.forward * moveoption.RollingDistance;

        RollingStartTime = Time.time;

        Move(moveval);
    }

    public void RollingOver()
    {
        lastRollingTime = Time.time;
        curval.IsRolling = false;
        curval.IsNoDamage = false;

        if (RollingNoDamageStartCor != null)
        {
            StopCoroutine(RollingNoDamageStartCor);
            RollingNoDamageStartCor = null;
        }

        if (RollingNoDamageEndCor!=null)
        {
            StopCoroutine(RollingNoDamageEndCor);
            RollingNoDamageEndCor = null;
        }
            
    }

    public void ActivateNoDamage()
    {
        //moveoption.NowIsNoDamege = true;
        curval.IsNoDamage = true;
        RollingNoDamageStartCor = null;


        RollingNoDamageEndCor = timecounter.Cor_TimeCounter(moveoption.RollingNoDamageTime, DeActivateNoDamage);
        StartCoroutine(RollingNoDamageEndCor);
    }

    public void DeActivateNoDamage()
    {
        curval.IsNoDamage = false;
        RollingNoDamageEndCor = null;

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

    public float testyRotPrev;
    public float testyRotNext;

    //1인칭일때회전 3인칭은 놔두고 1인칭 캐릭터만 회전시켜 준다.
    public void RotateFP()
    {
        float xRotPrev = com.FpRoot.localEulerAngles.y;
        float xRotNext = xRotPrev + MouseMove.x * Time.deltaTime * 50f * moveoption.RotMouseSpeed * ((moveoption.RightReverse) ? -1 : 1);

        //xnext = xRotNext;
        //if (xRotNext > 180f)
        //    xRotNext -= 360f;

        float yRotPrev = com.FpCamRig.localEulerAngles.x;
        testyRotPrev = yRotPrev;
        float yRotNext = yRotPrev + MouseMove.y * Time.deltaTime * 50f * moveoption.RotMouseSpeed /** ((moveoption.RightReverse) ? -1 : 1)*/;
        testyRotNext = yRotNext;

        if (yRotNext >= 90)
            yRotNext = yRotPrev;
        if (yRotNext <= 10)
            yRotNext = yRotPrev;
        //ynext = yRotNext;


        com.FpRoot.localEulerAngles = Vector3.up * xRotNext;
        //updown = com.FpRoot.localEulerAngles;
        com.FpCamRig.localEulerAngles = Vector3.right * yRotNext;
        //rightleft = com.FpCamRig.localEulerAngles;

    }


    //3인칭일때
    public void RotateTP()
    {
        float xRotPrev = com.TpCamRig.localEulerAngles.y;
        float xRotNext = xRotPrev + MouseMove.x * Time.deltaTime * 50f * moveoption.RotMouseSpeed * ((moveoption.RightReverse) ? -1 : 1);

        //if (xRotNext > 180f)
        //    xRotNext -= 360f;

        float yRotPrev = com.TpCamRig.localEulerAngles.x;
        float yRotNext = yRotPrev + MouseMove.y * Time.deltaTime * 50f * moveoption.RotMouseSpeed /** ((moveoption.RightReverse) ? -1 : 1)*/;

        testyRotPrev = yRotPrev;
        testyRotNext = yRotNext;

        //if (yRotNext >= 80)
        //    yRotNext = yRotPrev;
        //if (yRotNext <= 10)
        //    yRotNext = yRotPrev;

        //TpCamRig.localEulerAngles = Vector3.up * xRotNext;

        //TpCamRig.localEulerAngles = Vector3.right * yRotNext;






        if (yRotNext == 0 && xRotNext == 0)
            return;

        com.TpCamRig.localEulerAngles = new Vector3(yRotNext, xRotNext, 0);
    }

    

    //이떄는 마우스로움직이는게아니고 키보드 입력에 따라서 회전 해야 하기때문에 따로 만듦
    public void RotateTPFP()
    {
        float nextRotY = 0;
        Vector3 tempworldmove = com.TpCamRig.TransformDirection(MoveDir);
        
        float curRotY = com.FpRoot.localEulerAngles.y;

        if (tempworldmove.sqrMagnitude != 0)
            nextRotY = Quaternion.LookRotation(tempworldmove, Vector3.up).eulerAngles.y;

        if (!curval.IsMoving) nextRotY = curRotY;

        if (nextRotY - curRotY > 180f) nextRotY -= 360f;
        else if (curRotY - nextRotY > 180f) nextRotY += 360f;

        com.FpRoot.eulerAngles = Vector3.up * Mathf.Lerp(curRotY, nextRotY, moveoption.RotSpeed);
    }


    //3인칭 카메라가 정면방향을 바라보도록 회전
    public void LookAtFoward()
    {
        Vector3 foward = com.FpCamRig.forward;

        Vector3 rot = Quaternion.LookRotation(foward, Vector3.up).eulerAngles;

        Vector3 temp = com.TpCamRig.eulerAngles;

        com.TpCamRig.eulerAngles = new Vector3(rot.x, rot.y, rot.z);
    }

    //3인칭 카메라가 해당 위치를 바라보도록 회전
    public void LookAt(Vector3 lookpos)
    {
        //com.TpCamRig.LookAt(lookpos);

        //Debug.DrawLine(com.TpCamRig.transform.position, lookpos);
        //캐릭터 눈높이에서 
        Vector3 dir = lookpos - com.TpCamRig.transform.position;
        Vector3 rot = Quaternion.LookRotation(dir, Vector3.up).eulerAngles;
        com.TpCamRig.eulerAngles = new Vector3(rot.x, rot.y, rot.z);


        //Vector3 dir = (lookpos - GetCamera().transform.position).normalized;
        //Debug.DrawLine(GetCamera().transform.position, lookpos);

        //Vector3 rot = Quaternion.LookRotation(dir, Vector3.up).eulerAngles;



        //Vector3 temp = com.TpCamRig.eulerAngles;

        //com.TpCamRig.eulerAngles = new Vector3(rot.x, rot.y, rot.z);
    }

    //캐릭터가 해당 방향을 바라보도록 
    public void LookAtBody(Vector3 lookdir)
    {
        Vector3 rot = Quaternion.LookRotation(lookdir, Vector3.up).eulerAngles;
        //Quaternion.AngleAxis

        Vector3 temp = com.TpCamRig.eulerAngles;

        com.FpRoot.eulerAngles = new Vector3(rot.x, rot.y, rot.z);
    }

    public void LookAtBody2()
    {
        Vector3 tempworldmove;
        if (MoveDir != Vector3.zero)
            tempworldmove = com.TpCamRig.TransformDirection(MoveDir);
        else
            tempworldmove = com.TpCamRig.forward;

        Vector3 rot = Quaternion.LookRotation(tempworldmove, Vector3.up).eulerAngles;
        //Quaternion.AngleAxis

        Vector3 temp = com.TpCamRig.eulerAngles;

        com.FpRoot.eulerAngles = new Vector3(rot.x, rot.y, rot.z);
    }

    public void LookAtToLookDir()
    {
        Vector3 lookdir = com.TpCamRig.forward;

        Vector3 rot = Quaternion.LookRotation(lookdir, Vector3.up).eulerAngles;

        Vector3 temp = com.TpCamRig.eulerAngles;

        com.FpRoot.eulerAngles = new Vector3(rot.x, rot.y, rot.z);
    }


    //줌인 
    public void ZoomIn(float scroll)
    {
        if (scroll >=0)
            return;
        Camera curcam;
        Camera cCam = null;

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
            cCam = com.TpCam.Find("CCam").GetComponent<Camera>();

            if (curcam.fieldOfView <= moveoption.TPMaxZoomIn)
            {
                curcam.fieldOfView = moveoption.TPMaxZoomIn;
                cCam.fieldOfView = moveoption.TPMaxZoomIn;
                return;
            }


        }

        scroll = scroll * moveoption.ScrollSpeed * Time.deltaTime;

        curcam.fieldOfView += scroll;
        if (cCam != null)
            cCam.fieldOfView += scroll;
    }

    //줌 아웃
    public void ZoomOut(float scroll)
    {
        if (scroll<=0)
            return;

        Camera curcam;
        Camera cCam = null;

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
            cCam = com.TpCam.Find("CCam").GetComponent<Camera>();

            if (curcam.fieldOfView >= moveoption.TPMaxZoomOut)
            {
                curcam.fieldOfView = moveoption.TPMaxZoomOut;
                cCam.fieldOfView = moveoption.TPMaxZoomOut;
                return;
            }
        }

        scroll = scroll * moveoption.ScrollSpeed * Time.deltaTime;

        curcam.fieldOfView += scroll;
        if(cCam!=null)
            cCam.fieldOfView += scroll;
    }

    public void Focusing()
    {
        PlayableCharacter tempinstance = PlayableCharacter.Instance;
        if (tempinstance.IsFocusingOn)
        {
            int index = tempinstance._monsterObject.FindIndex(x => x._monster == tempinstance.CurFocusedMonster);
            if (index != -1)
            {

                //Vector3 pos = tempinstance.CurFocusedMonster.transform.position + tempinstance._monsterObject[index]._searchPoint[0];
                if(tempinstance.CurFocusedMonster!=null)
                {
                    //그냥 캐릭터 시점과 y 높이가 같도록
                    Vector3 pos = tempinstance._monsterObject[index]._coll.bounds.center + new Vector3(0, tempinstance._monsterObject[index]._coll.bounds.extents.y, 0);
                    pos.y = com.TpCamRig.position.y;
                    
                    LookAt(pos);
                }
                
            }
            //else
            //{
            //    Debug.Log("[focus]포커싱 중인 몬스터가 리스트에 없을때 꺼짐");
            //    tempinstance.IsFocusingOn = false;
            //    tempinstance.CurFocusedIndex = 0;
            //    tempinstance.CurFocusedMonster = null;
            //}
        }
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

        if (curval.IsFPP)
        {
            startCamPos = com.FpCam.position - com.TpCamRig.position;
            
            camDistance = startCamPos.magnitude;
        }
        else
        {
            startCamPos = com.TpCam.position - com.TpCamRig.position;
            startCampos_Focusing = com.TpCamPos2.position - com.TpCamRig.position;
            camDistance = startCamPos.magnitude;
            camDistance_Focusing = startCampos_Focusing.magnitude;
        }
           

    }

    public override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 80;
    }

    void Update()
    {
        Falling();
        Rotation();
        HorVelocity();
        MoveCalculate();
        Focusing();
        CameraCollision();
        //LookAtFoward();
        //달리는 중일떄 1초마다 스테미나를 줄여준다.
        if (curval.IsMoving&&curval.IsRunning&& PlayableCharacter.Instance.status.CurStamina >= moveoption.RunningStaminaVal)
        {
            if (Time.time - lastRunningTime >= 1.0f)
            {
                lastRunningTime = Time.time;

                //PlayableCharacter.Instance.status.CurStamina -= moveoption.RunningStaminaVal;
                PlayableCharacter.Instance.status.StaminaDown(moveoption.RunningStaminaVal);
            }
            
        }
    }

    public bool CameraCollOn = false;
    public Vector3 startCamPos;//카메라가 캐릭터로부터 어느정도 거리에 떨어져 있는지 초기값
    public Vector3 startCampos_Focusing;
    public float camDistance;
    public float camDistance_Focusing;

    public void CameraCollision()
    {
        Quaternion rot = Quaternion.LookRotation(com.TpCamRig.forward, Vector3.up);
        Vector3 startpos = (PlayableCharacter.Instance.IsFocusingOn) ? startCampos_Focusing : startCamPos;
        Vector3 dir = (rot * startpos).normalized;
        //Debug.DrawRay(Capsuletopcenter, dir);
        //Debug.DrawLine(com.TpCamRig.position, com.TpCamRig.position + (dir * camDistance));

        if (CameraCollOn)
        {
            //캐릭터에서 본래 카메라 위치 방향

            //Vector3 dir = com.TpCamRig.up + (-com.TpCamRig.forward * startCamPos.magnitude);
            
            //float distance = dir.magnitude;

            Ray ray = new Ray(Capsuletopcenter, dir);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, camDistance))
            {
                //Debug.Log("카메라 충돌");
                GetCamera().transform.position = hit.point;
            }
            else
            {
                GetCamera().transform.position = com.TpCamRig.position + (dir * camDistance);
            }

        }
        //else
        //{
        //    if (PlayableCharacter.Instance.IsFocusingOn)
        //    {
        //        GetCamera().transform.position = com.TpCamRig.position;

        //    }
                
        //}

        //Vector3 rot = Quaternion.LookRotation(lookdir, Vector3.up).eulerAngles;

        //Vector3 temp = com.TpCamRig.eulerAngles;

        //com.FpRoot.eulerAngles = new Vector3(rot.x, rot.y, rot.z);

    }

    //public float testLookY;
    //public float testLookZ;
    //public float testaxisY;
    //public float testaxisZ;
    //private void LateUpdate()
    //{
    //    //상대좌표
    //    Vector3 rot = Quaternion.LookRotation(com.TpCamRig.forward, -playerChestTr.right/*Vector3.up*/).eulerAngles;

    //    testLookY = rot.y;
    //    testLookZ = rot.x;
    //    testaxisY = com.FpRoot.eulerAngles.y;
    //    testaxisZ = com.FpRoot.eulerAngles.z;

    //    if (testaxisY == 0)
    //    {
    //        if (rot.y >= 180)
    //        {
    //            testLookY = rot.y - 360;
    //        }
    //    }
    //    else if(testaxisY >= 360 - 42 || testaxisY <= 85)
    //    {
    //        if (rot.y >= 180)
    //        {
    //            testLookY = rot.y - 360;
    //        }

    //        if(testaxisY>=180)
    //        {
    //            testaxisY -= 360;
    //        }

    //        testLookY = testLookY - testaxisY;
    //    }
    //    else
    //    {
    //        testLookY = rot.y - testaxisY;

    //    }


    //    if (testaxisZ == 0)
    //    {
    //        if (rot.x >= 180)
    //        {
    //            testLookZ = rot.x - 360;
    //        }
    //    }
    //    else if(testaxisZ >= 360 - 33 || testaxisZ <= 35)
    //    {
    //        if (rot.x >= 180)
    //        {
    //            testLookZ = rot.x - 360;
    //        }

    //        if (testaxisZ >= 180)
    //        {
    //            testaxisZ -= 360;
    //        }

    //        testLookZ = testLookZ - testaxisZ;
    //    }
    //    else
    //    {
    //        testLookZ = rot.x - testaxisZ;

    //    }

    //    if(testLookY>=130|| testLookY<=-93)
    //    {
    //        playerChestTr.eulerAngles = new Vector3(playerChestTr.eulerAngles.x, -90, 270);
    //        return;
    //    }

    //    rot.y = GetMinMaxRange(testLookY, -42, 85) + testaxisY;
    //    rot.x = GetMinMaxRange(testLookZ, -33, 35) + testaxisZ;

    //    playerChestTr.eulerAngles = new Vector3(playerChestTr.eulerAngles.x, rot.y - 90, (-rot.x + 270));

    //}

    //float GetMinMaxRange(float val, float min, float max)
    //{
    //    if (val < min)
    //        return min;
    //    if (val > max)
    //        return max;

    //    return val;
    //}

}

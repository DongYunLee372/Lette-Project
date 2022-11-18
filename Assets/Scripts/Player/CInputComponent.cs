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

        public KeyCode Tab = KeyCode.Q;

        public KeyCode Interaction = KeyCode.E;
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


    private GameObject canvas;

    //CharacterStateMachine.eCharacterState state;
    PlayableCharacter.States state;

    public void GetWASD()
    {
        float v = 0;
        float h = 0;
        //wasd 처리
        if (Input.GetKey(_key.foward))
        {
            v += 1.0f;
            if (state == PlayableCharacter.States.Guard)
            {
                movecom.GuardMove(CMoveComponent.Direction.Front);
                //return;
            }


        }
        if (Input.GetKey(_key.back))
        {
            v -= 1.0f;
            if (state == PlayableCharacter.States.Guard)
            {
                movecom.GuardMove(CMoveComponent.Direction.Back);
                //return;
            }



        }
        if (Input.GetKey(_key.left))
        {
            h -= 1.0f;
            if (state == PlayableCharacter.States.Guard)
            {
                movecom.GuardMove(CMoveComponent.Direction.Left);
                //return;
            }



        }
        if (Input.GetKey(_key.right))
        {
            h += 1.0f;
            if (state == PlayableCharacter.States.Guard)
            {
                movecom.GuardMove(CMoveComponent.Direction.Right);
                //return;
            }



        }

        movecom.MoveDir = new Vector3(h, 0, v);
    }

    //키와 마우스 입력을 처리한다.
    void KeyInput()
    {
        MainOption option;
        if (canvas!=null)
        {
            canvas.TryGetComponent<MainOption>(out option);
            if (option != null)
                if (option.ShowOption)
                    return;
        }
        
        

        if (movecom == null)
            movecom = PlayableCharacter.Instance.GetMyComponent(CharEnumTypes.eComponentTypes.MoveCom) as CMoveComponent;
        state = PlayableCharacter.Instance.GetState();


        
        
        movecom.curval.IsMoving = false;


        movecom.MouseMove = new Vector2(0, 0);//마우스 움직임
        movecom.MoveDir = new Vector3(0, 0, 0);//wasd 키 입력에 따른 이동 방향

        ////가드 중 일때는 시점은 정면으로 고정
        //if(state != CharacterStateMachine.eCharacterState.Guard&&
        //    state != CharacterStateMachine.eCharacterState.GuardStun)
        //{

        //}

        GameObject a = UIManager.Instance.Canvasreturn(Canvas_Enum.CANVAS_NUM.start_canvas);
        if (a.GetComponent<MainOption>().ShowOption)
        {
            return;
        }


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

        if (Input.GetKeyDown(_key.Tab))
        {
            PlayableCharacter.Instance.FocusTab();
        }


        if (/*state == CharacterStateMachine.eCharacterState.Attack||*/
            state == PlayableCharacter.States.Rolling ||
            state == PlayableCharacter.States.OutOfControl)
        {
            //movecom.curval.IsMoving = false;
            return;
        }



        //왼쪽 마우스 클릭
        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseDown();
            return;
        }

        if (state == PlayableCharacter.States.Attack)
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

        if (/*state == CharacterStateMachine.eCharacterState.Guard||*/
           state == PlayableCharacter.States.GuardStun)
        {
            //movecom.curval.IsMoving = false;
            return;
        }



        if (state != PlayableCharacter.States.Guard)//방어 중 일때는 해당 행동들을 할 수 없도록
        {
            //space 처리
            //구르기를 먼저 처리하고 움직임은 처리하지 않게 하기 위해서
            if (Input.GetKeyDown(_key.Rolling))
            {
                if (PlayableCharacter.Instance.status.CurStamina > 0)
                    movecom.Rolling();
                return;
            }

            //스킬1번
            if (Input.GetKeyDown(_key.skill01))
            {
                //SkillAttack(0);
                return;
            }

            //스킬2번
            if (Input.GetKeyDown(_key.skill02))
            {
                //SkillAttack(1);
                return;
            }

            //스킬3번
            if (Input.GetKeyDown(_key.skill03))
            {
                //SkillAttack(2);
                return;
            }
            GetWASD();
        }

        //
        movecom.curval.IsRunning = false;

        if (state != PlayableCharacter.States.Guard)//방어 중 일때는 해당 행동들을 할 수 없도록
        {
            //left shift 처리
            if (Input.GetKey(_key.Run))
            {
                if (PlayableCharacter.Instance.status.CurStamina >= movecom.moveoption.RunningStaminaVal)
                    movecom.curval.IsRunning = true;
            }
            //else movecom.curval.IsRunning = false;

            //방어중이 아니고 회피중이 아닐때만 아이템 사용 가능
            if(state != PlayableCharacter.States.Rolling)
            {
                if(Input.GetKeyDown(_key.Interaction))
                    PlayableCharacter.Instance.inventory.UseItem(EnumScp.Key.F1, 1);
            }


            //이동값이 조금이라도 있으면 움직이는중으로 판단
            if (movecom.MoveDir.magnitude > 0)
            {
                movecom.curval.IsMoving = true;
                //CharacterStateMachine.Instance.SetState(CharacterStateMachine.eCharacterState.Move);
            }
            else
            {
                PlayableCharacter.Instance.SetState(PlayableCharacter.States.Idle);
            }


            if (movecom.curval.IsMoving)
            {
                if (movecom.curval.IsRunning)
                {
                    //com.animator.SetPlaySpeed(1f);
                    movecom.com.animator.Play("move_run_01");
                    //movecom.com.animator.SetBool("Walk", false);
                    //movecom.com.animator.SetBool("Run", true);
                    //AudioSource audio = SoundManager.Instance.effectSource.GetComponent<AudioSource>();

                    //if (!audio.isPlaying)
                    //{
                    //    audio.loop = true;
                    //    audio.pitch = 3.0f;
                    //    SoundManager.Instance.effectSource.GetComponent<AudioSource>().PlayOneShot(SoundManager.Instance.Player_Audio[1]);
                    //}
                        
                }
                else
                {
                    //com.animator.SetPlaySpeed( 1f);
                    //movecom.com.animator.SetBool("Run", false);
                    //movecom.com.animator.SetBool("Walk", true);
                    movecom.com.animator.Play("strafe_walk_strafe_front");

                    //AudioSource audio = SoundManager.Instance.effectSource.GetComponent<AudioSource>();

                    //if (!audio.isPlaying)
                    //{
                    //    audio.loop = true;
                    //    audio.pitch = 2.2f;
                    //    SoundManager.Instance.effectSource.GetComponent<AudioSource>().PlayOneShot(SoundManager.Instance.Player_Audio[0]);
                    //}
                        
                }
            }
            else
            {
                //movecom.com.animator.SetBool("Run", false);
                //movecom.com.animator.SetBool("Walk", false);
                movecom.com.animator.Play("idle");
            }
        }
        else
        {
            ////가드중이고 움직이는 중이 아닐때
            //if (movecom.MoveDir.magnitude <= 0)
            //    movecom.com.animator.Play("_Guard");
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

    public dotweentest testtestobj;
    public GameObject testtestobj2;

    void Update()
    {
        //넉백 테스트
        if (Input.GetKeyDown(KeyCode.U))
        {
            PlayableCharacter.Instance.BeAttacked(10, this.transform.position, 10.0f);
            //movecom.KnockBack();
        }

        //넉다운 테스트
        if (Input.GetKeyDown(KeyCode.I))
        {
            PlayableCharacter.Instance.BeAttacked(90, this.transform.position, 40.0f);
            //movecom.KnockDown();
        }

        ////테스트
        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    ResourceCreateDeleteManager.Instance.RegistPoolManager<dotweentest>("testcube");
        //}

        //if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    testtestobj = ResourceCreateDeleteManager.Instance.InstantiateObj<dotweentest>("testcube");
        //}

        //if (Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    testtestobj2 = ResourceCreateDeleteManager.Instance.InstantiateObj<GameObject>("Testcube2");
        //}



        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    ResourceCreateDeleteManager.Instance.RegistPoolManager<GameObject>("Testcube2");
        //}

        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    ResourceCreateDeleteManager.Instance.DestroyObj<dotweentest>("testcube", testtestobj.gameObject);
        //}

        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    ResourceCreateDeleteManager.Instance.DestroyObj<GameObject>("Testcube2", testtestobj2);
        //}



        //키 입력
        KeyInput();
    }
}

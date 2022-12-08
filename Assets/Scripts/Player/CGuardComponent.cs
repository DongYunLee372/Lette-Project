using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGuardComponent : BaseComponent
{
    public CMoveComponent movecom;
    public AnimationController animator;
    public AnimationEventSystem eventsystem;
    //public IEnumerator coroutine;


    [Header("============Guard Options============")]
    public float GuardTime;//최대로 가드를 할 수 있는 시간
    public float GuardStunTime;//가드 경직 시간
    public int MaxGuardGauge;//
    public int BalanceDecreaseVal;
    public AnimationClip GuardStunClip;
    public string GuardStunClipName;
    public AnimationClip GuardClip;
    public GameObject GuardEffect;
    public Transform guardeffectpos;

    public float GuardAngle;

    [Header("============Cur Values============")]
    public int CurGuardGauge;
    public bool nowGuard;
    public float GaugeDownInterval;
    //public bool playingCor;
    public IEnumerator guardcoroutine;
    public IEnumerator stuncoroutine;
    public delegate void Invoker();
    public bool nowGuardStun;
    
    public float hitangle;

    private CorTimeCounter timer = new CorTimeCounter();
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<AnimationController>();
        eventsystem = GetComponentInChildren<AnimationEventSystem>();



        //eventsystem.AddEvent(new KeyValuePair<string, AnimationEventSystem.beginCallback>(null, null),
        //        new KeyValuePair<string, AnimationEventSystem.midCallback>(null, null),
        //        new KeyValuePair<string, AnimationEventSystem.endCallback>(GuardStunClip.name, AttackEnd));
    }


    //포커싱 상태에서 가드를 하면 시점이 캐릭터의 정면으로 고정 되어야 한다.
    public void Guard()
    {
        if (movecom == null)
            movecom = PlayableCharacter.Instance.GetMyComponent(CharEnumTypes.eComponentTypes.MoveCom) as CMoveComponent;

        if (movecom.curval.IsGuard)
            return;

        if(PlayableCharacter.Instance.IsFocusingOn)
            movecom.LookAtToLookDir();

        movecom.curval.IsGuard = true;

        movecom.com.animator.Play(GuardClip.name, 2.0f);

        //movecom.LookAtFoward();

        if (guardcoroutine != null)
        {
            StopCoroutine(guardcoroutine);
            guardcoroutine = null;
        }

        guardcoroutine = timer.Cor_TimeCounter(GuardTime, GuardDown);
        StartCoroutine(guardcoroutine);
    }

    //일정 시간 이후에 가드가 끝나야 할때
    public void GuardDown()
    {
        if (movecom == null)
            movecom = PlayableCharacter.Instance.GetMyComponent(CharEnumTypes.eComponentTypes.MoveCom) as CMoveComponent;

        if (!movecom.curval.IsGuard)
            return;

        if (guardcoroutine != null)
        {
            //playingCor = false;
            //Debug.Log("실행중 나감");
            StopCoroutine(guardcoroutine);
            if(stuncoroutine!=null)
            {
                StopCoroutine(stuncoroutine);
                stuncoroutine = null;
            }
            guardcoroutine = null;
        }
            

        movecom.curval.IsGuard = false;
        //CharacterStateMachine.Instance.SetState(CharacterStateMachine.eCharacterState.Idle);
    }

    


    //가드중일때 데미지가 들어왔을때는 이쪽으로 들어온다.
    public void Damaged_Guard(float damage,Vector3 hitpoint,float Groggy)
    {
        //if (PlayableCharacter.Instance.status.CurBalance >= BalanceDecreaseVal)
        //{
        //    EffectManager.Instance.InstantiateEffect(GuardEffect, guardeffectpos.position, guardeffectpos.rotation);
        //    PlayableCharacter.Instance.status.CurBalance -= BalanceDecreaseVal;
        //    GuardStun();
        //}
        //else
        //{
        //    PlayableCharacter.Instance.Damaged(damage, hitpoint);
        //}

        //피격위치가 캐릭터 정면 일정 각도 안에 있을때만 가드 성공
        
        
        Vector3 front = movecom.com.FpRoot.forward;
        front.y = 0;
        front.Normalize();

        Vector3 hit = hitpoint.normalized;
        hit.y = 0;
        hit.Normalize();

        hitangle = 180 - Mathf.Acos(Vector3.Dot(front, hit)) * 180.0f / 3.14f;


        //스테미나에 따라서 가드 성공 실패 학인
        if (PlayableCharacter.Instance.status.CurStamina >= 10 && hitangle <= GuardAngle && !nowGuardStun) 
        {
            PlayableCharacter.Instance.status.StaminaDown(10);
            EffectManager.Instance.InstantiateEffect(GuardEffect, guardeffectpos.position, guardeffectpos.rotation);
            GuardStun();
            Debug.Log("가드 성공");
        }
        else
        {
            Debug.Log("가드 실패");
            //PlayableCharacter.Instance.status.StaminaDown(10);
            PlayableCharacter.Instance.Damaged(damage, hitpoint, Groggy);
        }


    }

//    if (Time.time - LastDetestTime >= DetectTime)
//        {
//            LastDetestTime = Time.time;

//            //탐지범위에 플레이어가 있는지 판단
//            RaycastHit2D hit = Physics2D.CircleCast(transform.position, DetectRadius, new Vector2(0, 0), 0, PlayerLayer);

//            if (hit.collider != null)
//            {
//                //플레이어가 판단되면 정면벡터와의 내적으로 각도를 구해서 정면이면 탐지
//                direction = hit.transform.position - this.transform.position;
//                direction.Normalize();

//                DetectedAngle = Mathf.Acos(Vector3.Dot(WAYS[(int)Direction], direction)) * 180.0f / 3.14f;
//                if (DetectedAngle <= DetectAngle)
//                {
//                    obj = hit.transform.gameObject;
//                }
//Debug.Log("플레이어 감지");
//            }

//            if (obj != null)
//{
//    if (state != MONSTERSTATE.ATTACK || state != MONSTERSTATE.WALKING)
//    {
//        sc_player = obj.GetComponentInChildren<Player>();
//        NowDetected = true;
//        MoveScript.MoveStart(transform.position, sc_player.transform.position);
//    }

//}
//else
//{
//    sc_player = null;
//    NowDetected = false;
//}
//        }


    //가드넉백상태는 GuardStun 상태로 넘어간다.
    public void GuardStun()
    {
        //PlayableCharacter.Instance.SetState(PlayableCharacter.States.GuardStun);
        Debug.Log("가드 스턴 들어옴");
        animator.Play(GuardStunClipName, 1.0f, 0.0f, 0.1f, StunEnd);
        stuncoroutine = timer.Cor_TimeCounter(animator.GetClipLength(GuardStunClipName), StunEnd);
        nowGuardStun = true;
        //StartCoroutine(stuncoroutine);
        // Cor_TimeCounter
    }

    public void StunEnd()
    {
        Debug.Log("가드 스턴 끝 들어옴");
        nowGuardStun = false;
        movecom.com.animator.Play("Block_Loop", 2.0f);
        //PlayableCharacter.States prestate = PlayableCharacter.Instance.GetLastState();
        //PlayableCharacter.Instance.SetState(prestate);
        //if(prestate == CharacterStateMachine.eCharacterState.Guard)
        //{
        //    movecom.com.animator.Play(GuardClip.name, 2.0f);
        //}
        stuncoroutine = null; 
    }

    public override void InitComtype()
    {
        p_comtype = CharEnumTypes.eComponentTypes.GuardCom;
    }

    
    
}

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

    public float hitangle;

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
            StopCoroutine("Cor_TimeCounter");
            guardcoroutine = null;
        }
        guardcoroutine = Cor_TimeCounter(GuardTime, GuardDown);
        StartCoroutine(guardcoroutine);
    }

    //
    public void GuardDown()
    {
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

    //공격이 시작된지 일정 시간 뒤에 이펙트를 실행해야 할 때 사용
    IEnumerator Cor_TimeCounter(float time, Invoker invoker)
    {
        //playingCor = true;
        float starttime = Time.time;
        //Debug.Log("다시시작");
        while (true)
        {
            if ((Time.time - starttime) >= time)
            {
                //Debug.Log("시간다됨");
                invoker.Invoke();
                //guardcoroutine = null;
                //playingCor = false;
                yield break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
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
        if (PlayableCharacter.Instance.status.CurStamina >= 10 && hitangle <= GuardAngle) 
        {
            PlayableCharacter.Instance.status.StaminaDown(10);
            GuardStun();
        }
        else
        {
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


    //가드넉백상태는 outofcontrol 상태로 넘어가지 않고 가드중인 상태인 것으로 한다.
    public void GuardStun()
    {
        CharacterStateMachine.Instance.SetState(CharacterStateMachine.eCharacterState.GuardStun);
        animator.Play(GuardStunClip.name,2.8f);
        stuncoroutine = Cor_TimeCounter(GuardStunTime, StunEnd);
        StartCoroutine(stuncoroutine);
        // Cor_TimeCounter
    }

    public void StunEnd()
    {
        CharacterStateMachine.eCharacterState prestate = CharacterStateMachine.Instance.GetPreState();
        CharacterStateMachine.Instance.SetState(prestate);
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

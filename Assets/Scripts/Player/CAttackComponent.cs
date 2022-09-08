using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//jo
/*스킬과 공격을 데이터를 입력해 간단하게 조작 할 수 있도록
  또한 이펙트 매니저와도 매끄럽게 연결 되도록 수정*/
[System.Serializable]
public class CAttackComponent : BaseComponent
{
    [SerializeField]
    CurState curval;

    public int CurAttackNum = 0;

    public CMoveComponent movecom;

    public Transform effectparent;

    public WeaponCollider weaponcollider;

    public string monstertag;

    public CorTimeCounter timer = new CorTimeCounter();

    public bool IsLinkable = false;

    public AttackInfo NextAttackInfo = null;
    public bool NextAttack = false;
    public int NextAttackNum = -1;

    //기본 공격 정보 해당 정보를 3개 만들면 기본 공격이 설정값들에 따라 3가지 동작으로 이어진다.
    [System.Serializable]
    public class AttackInfo
    {
        [Tooltip("해당 공격의 타입을 설정한다 (노말, 광역, 투사체, 타겟팅)")]
        public CharEnumTypes.eAttackType AttackType;

        [Tooltip("공격이름")]
        public int AttackName;

        [Tooltip("공격번호")]
        public int AttackNum;

        //해당 매니메이션 클립
        [Tooltip("해당 공격의 애니메이션 클립")]
        public AnimationClip aniclip;

        //애니메이션 배속
        [Tooltip("해당 공격의 애니메이션 재생 속도")]
        [Range(0.0f, 10.0f)]
        public float animationPlaySpeed;

        [Tooltip("선딜")]
        [Range(0.0f, 10.0f)]
        public float StartDelay;

        //후딜레이
        [Tooltip("후딜")]
        [Range(0.0f, 10.0f)]
        public float RecoveryDelay;


        [Tooltip("다음 동작으로 넘어갈 수 있는 시간")]
        public float NextActionInputTime_Start;

        //다음동작으로 넘어가기 위한 시간
        //해당동작이 끝나고 해당 시간 안에 Attack()함수가 호출되어야지 다음동작으로 넘어간다.
        [Tooltip("연속동작이 있을때 다음 동작으로 들어가기 위한 입력 시간")]
        public float NextActionInputTime_End;

        //데미지
        [Tooltip("공격 데미지")]
        public float damage;

        //공격 이펙트
        [Tooltip("공격 이펙트")]
        public GameObject Effect;

        //이펙트 생성 타이밍
        [Tooltip("공격 이펙트 생성 타이밍")]
        public float EffectStartTime;

        //공격 이펙트의 위치
        [Tooltip("공격 이펙트 생성 위치")]
        public Transform EffectPosRot;

        [Tooltip("공격 이펙트 파괴 시간")]
        public float EffectDestroyTime;

        //공격 중 움직일 거리
        [Tooltip("공격할때 움직일 거리")]
        public float movedis;

        //움직일 시간
        [Tooltip("공격할때 움직일 시간")]
        public float movetime;

        [Tooltip("투사체가 있는 공격일때 투사체의 게임 오브젝트")]
        public GameObject ProjectileObj;

        [Tooltip("타겟팅공격일때 타겟오브젝트")]
        public GameObject TargetObj;
    }

    public AttackInfo[] attackinfos;
    public List<AttackInfo> attackinfoList;

    //스킬도 여기서 한번에 처리
    [System.Serializable]
    public class SkillInfo
    {
        [Tooltip("해당 공격의 타입을 설정한다 (노말, 광역, 투사체, 타겟팅)")]
        public CharEnumTypes.eAttackType AttackType;

        [Tooltip("스킬이름")]
        public string SkillName;

        [Tooltip("스킬번호")]
        public int SkillNum;

        //스킬 애니메이션
        public AnimationClip aniclip;

        //스킬 애니메이션 재생속도
        public float animationPlaySpeed;

        //스킬 후딜레이
        public float MovementDelay;

        //
        public float NextMovementTimeVal;

        //데미지
        public float damage;

        //이펙트
        public GameObject Effect;

        //이펙트 생성 시간
        public float EffectStartTime;

        //이펙트 생성 위치
        public Transform EffectPosRot;

        //움직일 거리
        public float Movedis;

        //움직일 시간
        public float MoveTime;

    }

    public SkillInfo[] skillinfos;

    public AnimationController animator;

    public AnimationEventSystem eventsystem;

    public GameObject effectobj;

    public Transform preparent;

    public float lastAttackTime = 0;

    //public delegate void Invoker(GameObject obj);

    //public bool IsTimeCounterActive = false;
    public IEnumerator coroutine;
    public IEnumerator Linkcoroutine;
    void Start()
    {

        animator = GetComponentInChildren<AnimationController>();
        eventsystem = GetComponentInChildren<AnimationEventSystem>();
        weaponcollider = GetComponentInChildren<WeaponCollider>();
        weaponcollider?.SetCollitionFunction(MonsterAttack);

        if (effectparent == null)
        {
            effectparent = new GameObject("EffectsContainer").transform;
        }

        //초기화 할때 각각의 공격 애니메이션의 이벤트들과 실행시킬 함수를 연결시켜 준다.
        for (int i = 0; i < attackinfos.Length; i++)
        {
            eventsystem.AddEvent(new KeyValuePair<string, AnimationEventSystem.beginCallback>(attackinfos[i].aniclip.name, AttackMove),
                new KeyValuePair<string, AnimationEventSystem.midCallback>(null, null),
                new KeyValuePair<string, AnimationEventSystem.endCallback>(attackinfos[i].aniclip.name, AttackEnd));
        }

        //초기화 할때 각각의 스킬 애니메이션의 이벤트들과 실행시킬 함수를 연결시켜 준다.
        for (int i = 0; i < skillinfos.Length; i++)
        {
            eventsystem.AddEvent(new KeyValuePair<string, AnimationEventSystem.beginCallback>(null, null),
                new KeyValuePair<string, AnimationEventSystem.midCallback>(skillinfos[i].aniclip.name, AttackMove),
                new KeyValuePair<string, AnimationEventSystem.endCallback>(skillinfos[i].aniclip.name, AttackEnd));
        }
        NextAttackInfo = null;

        //Dictionary<string, AttackInfo> attackinfos;
        //DB.Instance.Load<AttackInfo>(out attackinfos);
    }

    public void MonsterAttack(Collider collision)
    {

        if (!curval.IsAttacking)
            return;

        if (collision.gameObject.tag == monstertag)
        {
            Debug.Log("공격 들어옴");
        }

    }

    //공격이 시작된지 일정 시간 뒤에 이펙트를 실행해야 할 때 사용
    //IEnumerator Cor_TimeCounter(float time, Invoker invoker,GameObject obj)
    //{
    //    float starttime = Time.time;
    //    //IsTimeCounterActive = true;

    //    while (true)
    //    {
    //        if((Time.time - starttime)>=time)
    //        {
    //            coroutine = null;
    //            if(obj!=null)
    //                invoker.Invoke(obj);
    //            yield break;
    //        }
    //        yield return new WaitForSeconds(Time.deltaTime);
    //    }
    //}



    //스킬을 재생해준다.
    public void SkillAttack(int skillnum)
    {
        if (skillnum < 0 || skillnum > skillinfos.Length)
            return;

        if (movecom == null)
        {
            movecom = PlayableCharacter.Instance.GetMyComponent(CharEnumTypes.eComponentTypes.MoveCom) as CMoveComponent;
            curval = movecom.curval;
        }

        if (curval.IsAttacking)
            return;

        //공격중으로 바꿈
        if (curval.IsAttacking == false)
        {
            movecom.Stop();
            curval.IsAttacking = true;
        }


        //if (attackinfos[CurAttackNum].Effect != null)
        //{
        //    coroutine = timer.Cor_TimeCounter<GameObject, float>
        //    (attackinfos[CurAttackNum].EffectStartTime, CreateEffect, attackinfos[CurAttackNum].Effect, 1.5f);
        //    StartCoroutine(coroutine);
        //}

        //StartCoroutine(Cor_TimeCounter(skillinfos[skillnum].EffectStartTime, CreateEffect));
        animator.Play(skillinfos[skillnum].aniclip.name, skillinfos[skillnum].animationPlaySpeed);
    }

    //공격 함수
    public void Attack()
    {
        //필요한 컴포넌트를 받아온다.
        if (movecom == null)
        {
            movecom = PlayableCharacter.Instance.GetMyComponent(CharEnumTypes.eComponentTypes.MoveCom) as CMoveComponent;
            curval = movecom.curval;
        }

        ////이미 공격중일때는 공격 불가
        //if (curval.IsAttacking)
        //    return;

        //이미 공격 중이고 링크가 불가능하면 공격이 실행되지 않는다.
        if (curval.IsAttacking && !IsLinkable)
        {
            int a = 0;
            Debug.Log("공격나가버림");
            return;
        }



        //이미 공격중이고 링크가 가능하면 다음 공격정보가 있는지 확인한다.
        //이런식으로하면 실제로 다음공격이 실행될때 여기서 걸려버린다.
        if (curval.IsAttacking && IsLinkable && /*NextAttackNum == -1*/NextAttack == false)
        {
            Debug.Log("선입력들어옴");
            NextAttackNum = (CurAttackNum + 1) % attackinfos.Length;
            NextAttackInfo = attackinfos[NextAttackNum];
            NextAttack = true;
            return;
        }

        //아직 공격중 이고 선입력이 들어왔는데 또 공격이 들어오면 리턴한다.
        if (curval.IsAttacking && NextAttack == true)
        {
            if (NextAttackNum == (CurAttackNum + 1) % attackinfos.Length)
                return;
        }

        //공격중으로 바꿈
        if (curval.IsAttacking == false)
        {
            movecom.Stop();
            curval.IsAttacking = true;
        }



        //이전에 공격했던 시간과 현재 공격이 시작된 시간의 차이를 구한다.
        float tempval = Time.time - lastAttackTime;

        //선입력정보가 없으면 링크가능한지 판단해서 동작을 해준다.
        if (/*NextAttackNum == -1*/NextAttack == false)
        {
            //다음 동작으로 넘어가기 위한
            if (/*tempval <= attackinfos[CurAttackNum].NextMovementTimeVal*/IsLinkable)
            {
                Debug.Log("링크가능");
                CurAttackNum = (CurAttackNum + 1) % /*(int)CharEnumTypes.eAniAttack.AttackMax*/attackinfos.Length;

            }
            else
            {
                CurAttackNum = 0;
            }
        }
        else//선입력정보가 있으면 해당 공격을 해주고 넘버를 올려준다.
        {
            CurAttackNum = NextAttackInfo.AttackNum;
        }
        NextAttackInfo = null;
        NextAttackNum = -1;
        NextAttack = false;

        //coroutine = Cor_TimeCounter(attackinfos[CurAttackNum].EffectStartTime, CreateEffect, attackinfos[CurAttackNum].Effect);
        //일정 시간 이후에
        if (attackinfos[CurAttackNum].Effect != null)
        {
            coroutine = timer.Cor_TimeCounter<GameObject, float>
            (attackinfos[CurAttackNum].EffectStartTime, CreateEffect, attackinfos[CurAttackNum].Effect, 1.5f);
            StartCoroutine(coroutine);
        }

        //IsLinkable = false;
        Linkcoroutine = timer.Cor_TimeCounter(attackinfos[CurAttackNum].NextActionInputTime_Start, ActiveLinkable);
        StartCoroutine(Linkcoroutine);

        //Debug.Log($"{attackinfos[AttackNum].aniclip.name}애니메이션 {attackinfos[AttackNum].animationPlaySpeed}속도 록 실핼");
        animator.Play(attackinfos[CurAttackNum].aniclip.name, attackinfos[CurAttackNum].animationPlaySpeed/*,0,attackinfos[CurAttackNum].StartDelay*/);
    }


    public void ActiveLinkable()
    {
        IsLinkable = true;
        Linkcoroutine = null;
    }

    public void DeActiveLinkable()
    {
        IsLinkable = false;
        Linkcoroutine = null;
    }

    //공격중 움직임이 필요할때 애니메이션의 이벤트를 이용해서 호출됨
    public void AttackMove(string clipname)
    {
        for (int i = 0; i < attackinfos.Length; i++)
        {
            if (attackinfos[i].aniclip.name == clipname)
            {
                //movecom.FowardDoMove(5, animator.GetClipLength(attackinfos[AttackNum].aniclip.name) / 2);
                movecom.FowardDoMove(attackinfos[i].movedis, attackinfos[i].movetime);
                return;
            }
        }

        for (int i = 0; i < skillinfos.Length; i++)
        {
            if (skillinfos[i].aniclip.name == clipname)
            {
                movecom.FowardDoMove(skillinfos[i].Movedis, skillinfos[i].MoveTime);
                return;
            }
        }

    }

    //공격 이펙트를 생성
    public void CreateEffect(GameObject obj, float destroyTime)
    {

        effectobj = EffectManager.Instance.InstantiateEffect(obj, destroyTime);
        effectobj.transform.position = attackinfos[CurAttackNum].EffectPosRot.position;
        effectobj.transform.rotation = attackinfos[CurAttackNum].EffectPosRot.rotation;
        effectobj.transform.parent = attackinfos[CurAttackNum].EffectPosRot;

        //copyobj.transform.TransformDirection(movecom.com.FpRoot.forward);
        //Destroy(effectobj, 1.5f);
        //effectobj = null;
    }



    //공격애니메이션이 끝나면 해당 함수가 들어온다 공격 애니메이션의 이벤트를 통해 호출됨
    public void AttackEnd(string s_val)
    {
        //IsLinkable = false;
        //Debug.Log($"공격 끝 들어옴 -> {s_val}");
        if (effectobj != null)
        {
            //Debug.Log($"공격 끝 들어옴 -> {s_val}");
            //effectobj.transform.parent = preparent;
            effectobj.transform.parent = effectparent;
        }

        if (!IsLinkable)
        {
            ActiveLinkable();
        }

        Linkcoroutine = timer.Cor_TimeCounter(attackinfos[CurAttackNum].NextActionInputTime_End, DeActiveLinkable);
        StartCoroutine(Linkcoroutine);

        //후딜레이 구현 필요 
        //if (attackinfos[CurAttackNum].RecoveryDelay > 0.0f)
        //{
        //    //Debug.Log($"후딜레이 -> {attackinfos[CurAttackNum].RecoveryDelay}");

        //}
        //else
        //{
        //    ChangeState();
        //}

        animator.Pause();
        StartCoroutine(timer.Cor_TimeCounter(attackinfos[CurAttackNum].RecoveryDelay, ChangeState));



        //timer.Cor_TimeCounter( ChangeState)
        //StartCoroutine(timer.Cor_TimeCounter<AttackInfo>(attackinfos[CurAttackNum].RecoveryDelay, ChangeState))
    }

    public void ChangeState()
    {
        curval.IsAttacking = false;
        //if (curval.IsAttacking == true)


        lastAttackTime = Time.time;

        //if(Linkcoroutine!=null)
        //{
        //    StopCoroutine(Linkcoroutine);
        //    Linkcoroutine = null;
        //}

        if (/*NextAttackNum != -1*/NextAttack == true)
        {
            Attack();
        }
    }


    //공격이 중간에 끊겨야 할때
    public void AttackCutOff()
    {
        curval.IsAttacking = false;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

    }


    public override void InitComtype()
    {
        p_comtype = CharEnumTypes.eComponentTypes.AttackCom;
    }

}

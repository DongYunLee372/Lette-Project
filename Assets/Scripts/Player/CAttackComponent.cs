using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

   // public AttackInfo_Ex NextAttackInfo = null;
    public AttackInfo NextAttackInfo = null;
    public bool NextAttack = false;
    public int NextAttackNum = -1;

    public Dictionary<string, AttackInfo> LoadedAttackInfoDic;

    public List<AttackInfo> AttackInfos;


    //[SerializeField]
   // AttackInfo_Ex testinfooooo;

    //기본 공격 정보 해당 정보를 3개 만들면 기본 공격이 설정값들에 따라 3가지 동작으로 이어진다.
    //[System.Serializable]
    //public class AttackInfo_Ex
    //{
    //    [Tooltip("공격번호")]
    //    public int AttackNum;

    //    [Tooltip("공격이름")]
    //    public string AttackName;

    //    [Tooltip("해당 공격의 타입을 설정한다 (노말, 광역, 투사체, 타겟팅)")]
    //    public CharEnumTypes.eAttackType AttackType;

    //    //해당 매니메이션 클립
    //    [Tooltip("해당 공격의 애니메이션 클립")]
    //    public AnimationClip aniclip;

    //    //애니메이션 배속
    //    [Tooltip("해당 공격의 애니메이션 재생 속도")]
    //    [Range(0.0f, 10.0f)]
    //    public float animationPlaySpeed;

    //    [Tooltip("선딜")]
    //    [Range(0.0f, 10.0f)]
    //    public float StartDelay;

    //    //후딜레이
    //    [Tooltip("후딜")]
    //    [Range(0.0f, 10.0f)]
    //    public float RecoveryDelay;


    //    [Tooltip("다음 동작으로 넘어갈 수 있는 시간")]
    //    public float BufferdInputTime_Start;

    //    //다음동작으로 넘어가기 위한 시간
    //    //해당동작이 끝나고 해당 시간 안에 Attack()함수가 호출되어야지 다음동작으로 넘어간다.
    //    [Tooltip("연속동작이 있을때 다음 동작으로 들어가기 위한 입력 시간")]
    //    public float BufferdInputTime_End;

    //    //데미지
    //    [Tooltip("공격 데미지")]
    //    public float damage;

    //    //스테미나 소비값
    //    [Tooltip("공격시 줄어들 스테미나 게이지")]
    //    public float StaminaGaugeDown;

    //    //공격 이펙트
    //    [Tooltip("공격 이펙트")]
    //    public GameObject Effect;

    //    //이펙트 생성 타이밍
    //    [Tooltip("공격 이펙트 생성 타이밍")]
    //    public float EffectStartTime;

    //    //공격 이펙트의 위치
    //    [Tooltip("공격 이펙트 생성 위치")]
    //    public Transform EffectPosRot;

    //    [Tooltip("공격 이펙트 파괴 시간")]
    //    public float EffectDestroyTime;

    //    //공격 중 움직일 거리
    //    [Tooltip("공격할때 움직일 거리")]
    //    public float movedis;

    //    //움직일 시간
    //    [Tooltip("공격할때 움직일 시간")]
    //    public float movetime;

    //    [Tooltip("투사체가 있는 공격일때 투사체의 게임 오브젝트")]
    //    public GameObject ProjectileObj;

    //    [Tooltip("타겟팅공격일때 타겟오브젝트")]
    //    public GameObject TargetObj;
    //}

    //public AttackInfo_Ex[] attackinfos;
    //public List<AttackInfo_Ex> attackinfoList;

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

        [Tooltip("선딜")]
        [Range(0.0f, 10.0f)]
        public float StartDelay;

        //후딜레이
        [Tooltip("후딜")]
        [Range(0.0f, 10.0f)]
        public float RecoveryDelay;

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


    public void Init()
    {
        animator = GetComponentInChildren<AnimationController>();
        eventsystem = GetComponentInChildren<AnimationEventSystem>();
        weaponcollider = GetComponentInChildren<WeaponCollider>();
        weaponcollider?.SetCollitionFunction(MonsterAttack);

        if (effectparent == null)
        {
            effectparent = new GameObject("EffectsContainer").transform;
        }



        AnimationEventsSetting();
        Initsetting();

        //AttackInfos = LoadedAttackInfoDic["0"].ToList();
    }


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

        Initsetting();
        AnimationEventsSetting();
        



        //Dictionary<string, AttackInfo> attackinfos;
        //DB.Instance.Load<AttackInfo>(out attackinfos);
    }

    void AnimationEventsSetting()
    {
        //초기화 할때 각각의 공격 애니메이션의 이벤트들과 실행시킬 함수를 연결시켜 준다.
        //for (int i = 0; i < attackinfos.Length; i++)
        //{
        //    eventsystem.AddEvent(new KeyValuePair<string, AnimationEventSystem.beginCallback>(attackinfos[i].aniclip.name, AttackMove),
        //        new KeyValuePair<string, AnimationEventSystem.midCallback>(null, null),
        //        new KeyValuePair<string, AnimationEventSystem.endCallback>(attackinfos[i].aniclip.name, AttackEnd));
        //}


        foreach(var a in LoadedAttackInfoDic)
        {
            eventsystem.AddEvent(new KeyValuePair<string, AnimationEventSystem.beginCallback>(a.Value.aniclipName, AttackMove),
                new KeyValuePair<string, AnimationEventSystem.midCallback>(null, null),
                new KeyValuePair<string, AnimationEventSystem.endCallback>(a.Value.aniclipName, AttackEnd));
        }

        //for (int i = 0; i < LoadedAttackInfoDic.Count; i++)
        //{
        //    eventsystem.AddEvent(new KeyValuePair<string, AnimationEventSystem.beginCallback>(attackinfos[i].aniclip.name, AttackMove),
        //        new KeyValuePair<string, AnimationEventSystem.midCallback>(null, null),
        //        new KeyValuePair<string, AnimationEventSystem.endCallback>(attackinfos[i].aniclip.name, AttackEnd));
        //}


        //초기화 할때 각각의 스킬 애니메이션의 이벤트들과 실행시킬 함수를 연결시켜 준다.
        for (int i = 0; i < skillinfos.Length; i++)
        {
            eventsystem.AddEvent(new KeyValuePair<string, AnimationEventSystem.beginCallback>(null, null),
                new KeyValuePair<string, AnimationEventSystem.midCallback>(skillinfos[i].aniclip.name, AttackMove),
                new KeyValuePair<string, AnimationEventSystem.endCallback>(skillinfos[i].aniclip.name, AttackEnd));
        }
    }

    void Initsetting()
    {
        NextAttackInfo = null;
        //기본공격 정보 받아옴
        LoadFile.Read<AttackInfo>(out LoadedAttackInfoDic);

        //스킬공격 정보 받아옴
        //AttackInfoSetting(LoadedAttackInfoDic["0"], testinfooooo);

        //AttackInfoSetting(LoadedAttackInfoDic["0"], attackinfos[0]);
        //AttackInfoSetting(LoadedAttackInfoDic["1"], attackinfos[1]);
        //AttackInfoSetting(LoadedAttackInfoDic["2"], attackinfos[2]);
    }

    //void AttackInfoSetting(AttackInfo load, /*AttackInfo_Ex myinfo*/)
    //{
    //    myinfo.AttackName = load.attackName;
    //    myinfo.AttackNum = load.attackNum;
    //    myinfo.AttackType = (CharEnumTypes.eAttackType)System.Enum.Parse(typeof(CharEnumTypes.eAttackType), load.attackType);

    //    //myinfo.aniclip = 
    //    myinfo.animationPlaySpeed = load.animationPlaySpeed;
    //    myinfo.StartDelay = load.startDelay;
    //    myinfo.RecoveryDelay = load.recoveryDelay;
    //    myinfo.BufferdInputTime_Start = load.bufferdInputTime_Start;
    //    myinfo.BufferdInputTime_End = load.BufferdInputTime_End;
    //    myinfo.damage = load.damage;
    //    myinfo.StaminaGaugeDown = load.StaminaGaugeDown;

    //    //myinfo.Effect
    //    myinfo.EffectStartTime = load.EffectStartTime;
    //    //myinfo.EffectPosRot = load.EffectPosRot;
    //    myinfo.EffectDestroyTime = load.EffectDestroyTime;

    //    myinfo.movedis = load.movedis;
    //    myinfo.movetime = load.movetime;

    //    //myinfo.ProjectileObj = load.ProjectileObjName;
    //    //myinfo.TargetObj = load.TargetObjName;
    //}

    public void MonsterAttack(Collider collision)
    {

        if (!curval.IsAttacking)
            return;

        if (collision.gameObject.tag == monstertag)
        {
            //collision.GetComponent<Battle_Character>().Damaged((int)attackinfos[CurAttackNum].damage, this.transform.position);
            collision.GetComponent<Battle_Character>().Damaged((int)LoadedAttackInfoDic[CurAttackNum.ToString()].damage, this.transform.position);
            //Debug.Log("공격 들어옴");
        }

    }



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

        //이미 공격중일떄는 스킬 사용이 불가능
        if (curval.IsAttacking)
            return;

        //공격중으로 바꿈
        if (curval.IsAttacking == false)
        {
            movecom.Stop();
            curval.IsAttacking = true;
        }

        if (skillinfos[skillnum].Effect != null)
        {
            coroutine = timer.Cor_TimeCounter<GameObject,Transform, float>
                (skillinfos[skillnum].EffectStartTime, CreateEffect, skillinfos[skillnum].Effect, skillinfos[skillnum].EffectPosRot, 1.5f);
            StartCoroutine(coroutine);
        }

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

        
        if (CharacterStateMachine.Instance.CurState == CharacterStateMachine.eCharacterState.OutOfControl)
            return;

        //if (PlayableCharacter.Instance.status.CurStamina)
        //{

        //}

        //이미 공격 중이고 링크가 불가능하면 공격이 실행되지 않는다.
        if (curval.IsAttacking && !IsLinkable)
        {
            int a = 0;
            Debug.Log("공격나가버림");
            return;
        }



        //이미 공격중이고 링크가 가능하면 다음 공격정보가 있는지 확인한다.
        //이런식으로하면 실제로 다음공격이 실행될때 여기서 걸려버린다. -> 링크 공격이 실행되는 타이밍을 조절하는것으로 해결
        if (curval.IsAttacking && IsLinkable && /*NextAttackNum == -1*/NextAttack == false)
        {
            Debug.Log("선입력들어옴");
            //NextAttackNum = (CurAttackNum + 1) % attackinfos.Length;
            //NextAttackInfo = attackinfos[NextAttackNum];

            NextAttackNum = (CurAttackNum + 1) % LoadedAttackInfoDic.Count;
            NextAttackInfo = LoadedAttackInfoDic[NextAttackNum.ToString()];

            NextAttack = true;
            return;
        }

        //아직 공격중 이고 선입력이 들어왔는데 또 공격이 들어오면 리턴한다.
        if (curval.IsAttacking && NextAttack == true)
        {
            //if (NextAttackNum == (CurAttackNum + 1) % attackinfos.Length)
            //    return;

            if (NextAttackNum == (CurAttackNum + 1) % LoadedAttackInfoDic.Count)
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
                //CurAttackNum = (CurAttackNum + 1) % /*(int)CharEnumTypes.eAniAttack.AttackMax*/attackinfos.Length;

                CurAttackNum = (CurAttackNum + 1) % /*(int)CharEnumTypes.eAniAttack.AttackMax*/LoadedAttackInfoDic.Count;
            }
            else
            {
                CurAttackNum = 0;
            }
        }
        else//선입력정보가 있으면 해당 공격을 해주고 넘버를 올려준다.
        {
            //CurAttackNum = NextAttackInfo.AttackNum;

            CurAttackNum = NextAttackInfo.attackNum;
        }
        NextAttackInfo = null;
        NextAttackNum = -1;
        NextAttack = false;

        //coroutine = Cor_TimeCounter(attackinfos[CurAttackNum].EffectStartTime, CreateEffect, attackinfos[CurAttackNum].Effect);
        //일정 시간 이후에

        //if (attackinfos[CurAttackNum].Effect != null)
        //{
        //    coroutine = timer.Cor_TimeCounter<GameObject,Transform, float>
        //    (attackinfos[CurAttackNum].EffectStartTime, CreateEffect, attackinfos[CurAttackNum].Effect, attackinfos[CurAttackNum].EffectPosRot, 1.5f);
        //    StartCoroutine(coroutine);
        //}

        //if (LoadedAttackInfoDic[CurAttackNum.ToString()].EffectObj != null)
        //{
        //    coroutine = timer.Cor_TimeCounter<GameObject, Transform, float>
        //    (LoadedAttackInfoDic[CurAttackNum.ToString()].EffectStartTime, CreateEffect, LoadedAttackInfoDic[CurAttackNum.ToString()].EffectObj, LoadedAttackInfoDic[CurAttackNum.ToString()].EffectPosRot, 1.5f);
        //    StartCoroutine(coroutine);
        //}


        //IsLinkable = false;
        Linkcoroutine = timer.Cor_TimeCounter(LoadedAttackInfoDic[CurAttackNum.ToString()].bufferdInputTime_Start, ActiveLinkable);
        StartCoroutine(Linkcoroutine);

        //Debug.Log($"{attackinfos[AttackNum].aniclip.name}애니메이션 {attackinfos[AttackNum].animationPlaySpeed}속도 록 실핼");
        animator.Play(LoadedAttackInfoDic[CurAttackNum.ToString()].aniclipName, LoadedAttackInfoDic[CurAttackNum.ToString()].animationPlaySpeed/*,0,attackinfos[CurAttackNum].StartDelay*/);

        PlayableCharacter.Instance.status.StaminaDown(LoadedAttackInfoDic[CurAttackNum.ToString()].StaminaGaugeDown);

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
        //for (int i = 0; i < attackinfos.Length; i++)
        //{
        //    if (attackinfos[i].aniclip.name == clipname)
        //    {
        //        //movecom.FowardDoMove(5, animator.GetClipLength(attackinfos[AttackNum].aniclip.name) / 2);
        //        movecom.FowardDoMove(attackinfos[i].movedis, attackinfos[i].movetime);
        //        return;
        //    }
        //}

        for (int i = 0; i < LoadedAttackInfoDic.Count; i++)
        {
            if (LoadedAttackInfoDic[i.ToString()].aniclipName == clipname)
            {
                //movecom.FowardDoMove(5, animator.GetClipLength(attackinfos[AttackNum].aniclip.name) / 2);
                movecom.FowardDoMove(LoadedAttackInfoDic[i.ToString()].movedis, LoadedAttackInfoDic[i.ToString()].movetime);
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
    public void CreateEffect(GameObject obj,Transform posrot, float destroyTime)
    {

        effectobj = EffectManager.Instance.InstantiateEffect(obj, destroyTime);
        effectobj.transform.position = posrot.position;
        effectobj.transform.rotation = posrot.rotation;
        effectobj.transform.parent = posrot;

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

        //Linkcoroutine = timer.Cor_TimeCounter(attackinfos[CurAttackNum].BufferdInputTime_End, DeActiveLinkable);
        //StartCoroutine(Linkcoroutine);

        Linkcoroutine = timer.Cor_TimeCounter(LoadedAttackInfoDic[CurAttackNum.ToString()].BufferdInputTime_End, DeActiveLinkable);
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
        //StartCoroutine(timer.Cor_TimeCounter(attackinfos[CurAttackNum].RecoveryDelay, ChangeState));

        StartCoroutine(timer.Cor_TimeCounter(LoadedAttackInfoDic[CurAttackNum.ToString()].recoveryDelay, ChangeState));

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
        if(Linkcoroutine!=null)
        {
            StopCoroutine(Linkcoroutine);
            Linkcoroutine = null;
        }

        curval.IsAttacking = false;

        NextAttack = false;
        NextAttackNum = -1;
    }


    public override void InitComtype()
    {
        p_comtype = CharEnumTypes.eComponentTypes.AttackCom;
    }

}

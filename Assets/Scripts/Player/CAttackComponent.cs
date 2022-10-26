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
    [HideInInspector]
    CurState curval;
    [HideInInspector]
    public int CurAttackNum = 0;
    [HideInInspector]
    public CMoveComponent movecom;
    [HideInInspector]
    public Transform effectparent;
    [HideInInspector]
    public WeaponCollider weaponcollider;
    //[HideInInspector]
    public string monstertag;
    [HideInInspector]
    public CorTimeCounter timer = new CorTimeCounter();
    [HideInInspector]
    public bool IsLinkable = false;

    // public AttackInfo_Ex NextAttackInfo = null;
    [HideInInspector]
    public AttackInfo NextAttackInfo = null;
    [HideInInspector]
    public bool NextAttack = false;
    [HideInInspector]
    public int NextAttackNum = -1;


    public List<AttackInfo> AttackInfos;

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

        public string EffectAdressable;

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
    [HideInInspector]
    public AnimationController animator;
    [HideInInspector]
    public AnimationEventSystem eventsystem;
    [HideInInspector]
    public GameObject effectobj;
    [HideInInspector]
    public Transform preparent;
    [HideInInspector]
    public float lastAttackTime = 0;

    //public delegate void Invoker(GameObject obj);

    //public bool IsTimeCounterActive = false;
    [HideInInspector]
    public IEnumerator coroutine;
    [HideInInspector]
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

        Initsetting();
        AnimationEventsSetting();
        
    }

    void AnimationEventsSetting()
    {
        //초기화 할때 각각의 공격 애니메이션의 이벤트들과 실행시킬 함수를 연결시켜 준다.
        for (int i = 0; i < AttackInfos.Count; i++)
        {
            eventsystem.AddEvent(new KeyValuePair<string, AnimationEventSystem.beginCallback>(AttackInfos[i].aniclipName, AttackMove),
                new KeyValuePair<string, AnimationEventSystem.midCallback>(null, null),
                new KeyValuePair<string, AnimationEventSystem.endCallback>(AttackInfos[i].aniclipName, AttackEnd));

            //eventsystem.AddEvent(new KeyValuePair<string, AnimationEventSystem.beginCallback>(null, null),
            //    new KeyValuePair<string, AnimationEventSystem.midCallback>(null, null),
            //    new KeyValuePair<string, AnimationEventSystem.endCallback>(AttackInfos[i].endAniclipName, AttackEnd));

        }

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
        //LoadFile.Read<AttackInfo>(out LoadedAttackInfoDic);

        //스킬공격 정보 받아옴
        //AttackInfoSetting(LoadedAttackInfoDic["0"], testinfooooo);

        //AttackInfoSetting(LoadedAttackInfoDic["0"], attackinfos[0]);
        //AttackInfoSetting(LoadedAttackInfoDic["1"], attackinfos[1]);
        //AttackInfoSetting(LoadedAttackInfoDic["2"], attackinfos[2]);
    }


    public void MonsterAttack(Collider collision)
    {
        if (!curval.IsAttacking)
            return;

        if (collision.gameObject.tag == monstertag)
        {
            //collision.GetComponent<Battle_Character>().Damaged((int)attackinfos[CurAttackNum].damage, this.transform.position);
            collision.GetComponent<Battle_Character>().Damaged((int)AttackInfos[CurAttackNum].damage, this.transform.position);
            //Debug.Log("공격 들어옴");
        }

        if (collision.gameObject.tag == "Box")
        {
            collision.GetComponent<Item_Box>().Ending();
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
            coroutine = timer.Cor_TimeCounter<string,Transform, float>
                (skillinfos[skillnum].EffectStartTime, CreateEffect, skillinfos[skillnum].EffectAdressable, skillinfos[skillnum].EffectPosRot, 1.5f);
            StartCoroutine(coroutine);
        }

        //EffectManager.Instance.SpawnEffectLooping(skillinfos[skillnum].Effect, this.transform.position, Quaternion.identity, 2, 10);

        ColliderSpawnSystem.Instance.SpawnSphereCollider(transform.position, 10, 5, monstertag, MonsterAttack);


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

        //스테미나가 다 떨어졌으면 공격을 못한다.
        if (PlayableCharacter.Instance.status.CurStamina <= 0)
            return;
        //if (PlayableCharacter.Instance.status.CurStamina)
        //{

        //}

        //이미 공격 중이고 링크가 불가능하면 공격이 실행되지 않는다.
        if (curval.IsAttacking && !IsLinkable)
        {
            int a = 0;
            //Debug.Log("[Attack]공격나가버림");
            return;
        }



        //이미 공격중이고 링크가 가능하면 다음 공격정보가 있는지 확인한다.
        //이런식으로하면 실제로 다음공격이 실행될때 여기서 걸려버린다. -> 링크 공격이 실행되는 타이밍을 조절하는것으로 해결
        if (curval.IsAttacking && IsLinkable && /*NextAttackNum == -1*/NextAttack == false)
        {
            //Debug.Log("[Attack]선입력들어옴");

            NextAttackNum = (CurAttackNum + 1) % AttackInfos.Count;
            NextAttackInfo = AttackInfos[NextAttackNum];

            NextAttack = true;
            return;
        }

        //아직 공격중 이고 선입력이 들어왔는데 또 공격이 들어오면 리턴한다.
        if (curval.IsAttacking && NextAttack == true)
        {
            //if (NextAttackNum == (CurAttackNum + 1) % attackinfos.Length)
            //    return;

            if (NextAttackNum == (CurAttackNum + 1) % AttackInfos.Count)
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
            if (IsLinkable)
            {
                //Debug.Log("링크가능");

                CurAttackNum = (CurAttackNum + 1) % AttackInfos.Count;
            }
            else
            {
                CurAttackNum = 0;
            }
        }
        else//선입력정보가 있으면 해당 공격을 해주고 넘버를 올려준다.
        {
            CurAttackNum = NextAttackInfo.attackNum;
        }

        //선입력세팅을 위한 입력은 앞에서 리턴하기 때문에 여기서는 관련 변수들을 초기화 해준다.
        NextAttackInfo = null;
        NextAttackNum = -1;
        NextAttack = false;

        //링크 가능 시간 체크
        Linkcoroutine = timer.Cor_TimeCounter(AttackInfos[CurAttackNum].bufferdInputTime_Start, ActiveLinkable);
        StartCoroutine(Linkcoroutine);

        //애니메이션 실행
        animator.Play(AttackInfos[CurAttackNum].aniclipName, AttackInfos[CurAttackNum].animationPlaySpeed/*,0,attackinfos[CurAttackNum].StartDelay*/);
        
        //스테미나를 줄여준다.
        PlayableCharacter.Instance.status.StaminaDown(AttackInfos[CurAttackNum].StaminaGaugeDown);

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
        for (int i = 0; i < AttackInfos.Count; i++)
        {
            if (AttackInfos[i].aniclipName == clipname)
            {
                movecom.FowardDoMove(AttackInfos[i].movedis, AttackInfos[i].movetime);
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
    public void CreateEffect(string adressableAdress,Transform posrot, float destroyTime)
    {
        effectobj = EffectManager.Instance.InstantiateEffect(adressableAdress, destroyTime);
        effectobj.transform.position = posrot.position;
        effectobj.transform.rotation = posrot.rotation;
        effectobj.transform.parent = posrot;
    }

    //공격애니메이션이 끝나면 해당 함수가 들어온다 공격 애니메이션의 이벤트를 통해 호출됨
    public void AttackEnd(string s_val)
    {
        if (effectobj != null)
        {
            effectobj.transform.parent = effectparent;
        }

        //공격이 끝난 후 일정 시간 동안 입력을 넣음으로써 연결 동작 실행 가능
        if (!IsLinkable)
        {
            ActiveLinkable();
        }

        //공격 끝 이후 연결동작 입력
        Linkcoroutine = timer.Cor_TimeCounter(AttackInfos[CurAttackNum].BufferdInputTime_End, DeActiveLinkable);
        StartCoroutine(Linkcoroutine);

        //후딜레이 구현
        animator.Pause();
        StartCoroutine(timer.Cor_TimeCounter(AttackInfos[CurAttackNum].recoveryDelay, ChangeState));


    }
    

    public void ChangeState()
    {
        lastAttackTime = Time.time;

        //마지막으로 공격을 끝내고 돌아가는데 다음 연결 동작정보가 있으면 연결동작을 실행하고 
        if (/*NextAttackNum != -1*/NextAttack == true)
        {
            curval.IsAttacking = false;
            Attack();
        }
        //없으면 복귀동작을 실행한다.
        else
        {
            animator.Play(AttackInfos[CurAttackNum].endAniclipName, AttackInfos[CurAttackNum].endanimationPlaySpeed, 0, 0.2f, IsAttackingEnd);
        }

    }

    public void IsAttackingEnd()
    {
        //Debug.Log("[Attack] 공격 진짜 마지막 끝");
        curval.IsAttacking = false;
    }

    public void Damaged_Attacking(float damage, Vector3 hitpoint, float Groggy)
    {

        AttackCutOff();
        PlayableCharacter.Instance.Damaged(damage, hitpoint, Groggy);
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

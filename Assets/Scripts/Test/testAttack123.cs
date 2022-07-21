using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//jo
public class testAttack123 : BaseComponent
{
    [SerializeField]
    CurState curval;

    public float LastAttackTime;

    public int AttackNum = 0;
    public CMoveComponent movecom;

    [System.Serializable]
    public class AttackMovementInfo
    {
        public int AttackNum;

        //�ִϸ��̼� ���
        public float animationPlaySpeed;

        //�ش� �Ŵϸ��̼� Ŭ��
        public AnimationClip aniclip;

        //�ĵ�����
        public float MovementDelay;

        //������������ �Ѿ�� ���� �ð�
        //�ش絿���� ������ �ش� �ð� �ȿ� Attack()�Լ��� ȣ��Ǿ���� ������������ �Ѿ��.
        public float NextMovementTimeVal;

        public float damage;

        public float EffectStartTime;

        public GameObject Effect;

        public Transform EffectPosRot;

        public float movedis;

        public float movetime;
    }

    [SerializeField]
    public List<PlayerAttack_Information> Attack_InformationList = new List<PlayerAttack_Information>();

    [SerializeField]
    public PlayerAttack_Information SkillData;

    //��ų�� ���⼭ �ѹ��� ó��
    [System.Serializable]
    public class SkillInfo
    {
        public string SkillName;

        public int SkillNum;

        public AnimationClip aniclip;

        public float animationPlaySpeed;

        public float MovementDelay;

        public float NextMovementTimeVal;

        public float damage;

        public GameObject Effect;

        public float EffectStartTime;

        public Transform EffectPosRot;

        public float Movedis;

        public float MoveTime;

    }

    [SerializeField]
    public Collider[] colliders;

    public AnimationController animator;

    public AnimationEventSystem eventsystem;

    public AttackMovementInfo[] attackinfos;

    public SkillInfo[] skillinfos;

    public GameObject effectobj;

    public Transform preparent;

    public float lastAttackTime = 0;

    public delegate void Invoker();

    public AttackManager testAttckmanager;

    void Start()
    {
        colliders = GetComponentsInChildren<Collider>();
        //for(int i=0;i<Attack_InformationList.Count;i++)
        //{
        //    Debug.Log(Attack_InformationList[i].P_AttackNum);
        //}

        animator = GetComponentInChildren<AnimationController>();
        eventsystem = GetComponentInChildren<AnimationEventSystem>();

        for (int i = 0; i < attackinfos.Length; i++)
        {
            eventsystem.AddEvent(new KeyValuePair<string, AnimationEventSystem.beginCallback>(null, null),
                new KeyValuePair<string, AnimationEventSystem.midCallback>(attackinfos[i].aniclip.name, AttackMove),
                new KeyValuePair<string, AnimationEventSystem.endCallback>(attackinfos[i].aniclip.name, AttackEnd));
        }
        for (int i = 0; i < skillinfos.Length; i++)
        {
            eventsystem.AddEvent(new KeyValuePair<string, AnimationEventSystem.beginCallback>(null, null),
                new KeyValuePair<string, AnimationEventSystem.midCallback>(skillinfos[i].aniclip.name, AttackMove),
                new KeyValuePair<string, AnimationEventSystem.endCallback>(skillinfos[i].aniclip.name, AttackEnd));
        }

        //for (int i = 0; i < skillinfos.Length; i++)
        //{
        //    testAttckmanager.EventAdd(attackinfos.Length, attackinfos[i].aniclip.name, StaticClass.AttackMove , eventsystem);
        //    testAttckmanager.EventAdd(attackinfos.Length, attackinfos[i].aniclip.name, StaticClass.AttackEnd , eventsystem);
        //}

    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
            return;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            SkillAttack();
            return;
        }
    }

    //������ ���۵��� ���� �ð� �ڿ� ����Ʈ�� �����ؾ� �� �� ���
    IEnumerator Cor_TimeCounter(float time, Invoker invoker)
    {
        float starttime = Time.time;

        while (true)
        {
            if ((Time.time - starttime) >= time)
            {
                invoker.Invoke();
                yield break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    //��ų�� ������ش�.
    public void SkillAttack()
    {      
        if (movecom == null)
        {
            movecom = PlayableCharacter.Instance.GetMyComponent(EnumTypes.eComponentTypes.MoveCom) as CMoveComponent;
            curval = movecom.curval;
        }

        if (curval.IsAttacking)
            return;

        if (curval.IsAttacking == false)
            curval.IsAttacking = true;


        testAttckmanager.AttackMana(animator, SkillData.P_aniclip.name, SkillData.P_animationPlaySpeed);
    }

    public void CreateEffect()
    {
        preparent = testAttckmanager.CreateEffect(attackinfos[AttackNum].Effect, attackinfos[AttackNum].EffectPosRot, 1.5f);
    }

    public void Attack()
    {
        if (curval.IsAttacking)
            return;

        if (movecom == null)
        {
            movecom = PlayableCharacter.Instance.GetMyComponent(EnumTypes.eComponentTypes.MoveCom) as CMoveComponent;
            curval = movecom.curval;
        }

        if (curval.IsAttacking == false)
            curval.IsAttacking = true;
        
        float tempval = Time.time - lastAttackTime;
        
        if (tempval <= attackinfos[AttackNum].NextMovementTimeVal)
        {
            AttackNum = (AttackNum + 1) % (int)EnumTypes.eAniAttack.AttackMax;

        }
        else
        {
            AttackNum = 0;
        }

        StartCoroutine(Cor_TimeCounter(attackinfos[AttackNum].EffectStartTime, CreateEffect));

        testAttckmanager.AttackMana(animator, Attack_InformationList[AttackNum].P_aniclip.name, Attack_InformationList[AttackNum].P_animationPlaySpeed);

        foreach (Collider coll in colliders)
        {
            Debug.Log(coll.name);
            coll.enabled = true;
        }

    }

    public void AttackMove(string clipname)
    {
       
        for (int i = 0; i < attackinfos.Length; i++)
        {
            if (attackinfos[i].aniclip.name == clipname)
            {               
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

   
   
    public void AttackEnd(string s_val)
    {
        if (effectobj != null)
        {
            Debug.Log($"dasdw���� �� ���� -> {s_val}");
            effectobj.transform.parent = preparent;
        }
        

        if (curval.IsAttacking == true)
            curval.IsAttacking = false;

        lastAttackTime = Time.time;


        foreach (Collider coll in colliders)
        {
            if (coll.name == "Object003")
                coll.enabled = false;
        }
    }

   
    public override void InitComtype()
    {
        p_comtype = EnumTypes.eComponentTypes.AttackCom;
    }

}


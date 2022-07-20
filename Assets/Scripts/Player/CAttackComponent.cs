using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//jo
public class CAttackComponent : BaseComponent
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


    public AnimationController animator;

    public AnimationEventSystem eventsystem;

    public AttackMovementInfo[] attackinfos;

    public SkillInfo[] skillinfos;

    public GameObject effectobj;

    public Transform preparent;

    public float lastAttackTime = 0;

    public delegate void Invoker();

    

    void Start()
    {
        //animator = ComponentManager.GetI.GetMyComponent(EnumTypes.eComponentTypes.AnimatorCom) as CAnimationComponent;
        
        animator = GetComponentInChildren<AnimationController>();
        eventsystem = GetComponentInChildren<AnimationEventSystem>();

        for(int i=0;i<attackinfos.Length;i++)
        {
            eventsystem.AddEvent(new KeyValuePair<string, AnimationEventSystem.beginCallback>(null, null), 
                new KeyValuePair<string, AnimationEventSystem.midCallback>(attackinfos[i].aniclip.name,AttackMove), 
                new KeyValuePair<string, AnimationEventSystem.endCallback > (attackinfos[i].aniclip.name, AttackEnd));
        }
        for(int i=0;i<skillinfos.Length;i++)
        {
            eventsystem.AddEvent(new KeyValuePair<string, AnimationEventSystem.beginCallback>(null, null),
                new KeyValuePair<string, AnimationEventSystem.midCallback>(skillinfos[i].aniclip.name, AttackMove),
                new KeyValuePair<string, AnimationEventSystem.endCallback>(skillinfos[i].aniclip.name, AttackEnd));
        }
        //eventsystem.AddEvent(null, new KeyValuePair<string, AnimationEventSystem.midCallback>() AttackMove, AttackEnd);


    }

    //������ ���۵��� ���� �ð� �ڿ� ����Ʈ�� �����ؾ� �� �� ���
    IEnumerator Cor_TimeCounter(float time, Invoker invoker)
    {
        float starttime = Time.time;

        while(true)
        {
            if((Time.time - starttime)>=time)
            {
                invoker.Invoke();
                yield break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    //��ų�� ������ش�.
    public void SkillAttack(int skillnum)
    {
        if (skillnum < 0 || skillnum > skillinfos.Length)
            return;

        if (movecom == null)
        {
            movecom = PlayableCharacter.Instance.GetMyComponent(EnumTypes.eComponentTypes.MoveCom) as CMoveComponent;
            curval = movecom.curval;
        }

        if (curval.IsAttacking)
            return;

        if (curval.IsAttacking == false)
            curval.IsAttacking = true;

        //StartCoroutine(Cor_TimeCounter(skillinfos[skillnum].EffectStartTime, CreateEffect));
        animator.Play(skillinfos[skillnum].aniclip.name, skillinfos[skillnum].animationPlaySpeed);
    }


    public void Attack()
    {
        if(movecom==null)
        {
            movecom = PlayableCharacter.Instance.GetMyComponent(EnumTypes.eComponentTypes.MoveCom) as CMoveComponent;
            curval = movecom.curval;
        }

        if (curval.IsAttacking)
            return;

        if (curval.IsAttacking == false)
            curval.IsAttacking = true;

        //Debug.Log("���� ����");
        float tempval = Time.time - lastAttackTime;
        //Debug.Log($"����� �ð�{tempval}, ����ð�{attackinfos[AttackNum].NextMovementTimeVal}");

        if (tempval <= attackinfos[AttackNum].NextMovementTimeVal)
        {
            AttackNum = (AttackNum + 1) % (int)EnumTypes.eAniAttack.AttackMax;

        }
        else
        {
            AttackNum = 0;
        }

        StartCoroutine(Cor_TimeCounter(attackinfos[AttackNum].EffectStartTime, CreateEffect));

        //Debug.Log($"{attackinfos[AttackNum].aniclip.name}�ִϸ��̼� {attackinfos[AttackNum].animationPlaySpeed}�ӵ��� ����");
        animator.Play(attackinfos[AttackNum].aniclip.name, attackinfos[AttackNum].animationPlaySpeed);

        

    }

    public void AttackMove(string clipname)
    {
        for(int i=0;i<attackinfos.Length;i++)
        {
            if(attackinfos[i].aniclip.name == clipname)
            {
                //movecom.FowardDoMove(5, animator.GetClipLength(attackinfos[AttackNum].aniclip.name) / 2);
                movecom.FowardDoMove(attackinfos[i].movedis, attackinfos[i].movetime);

                return;
            }
        }

        for(int i=0;i<skillinfos.Length;i++)
        {
            if(skillinfos[i].aniclip.name == clipname)
            {
                movecom.FowardDoMove(skillinfos[i].Movedis, skillinfos[i].MoveTime);
                return;
            }
        }

    }

    //���� ����Ʈ�� ����
    public void CreateEffect()
    {
        effectobj = GameObject.Instantiate(attackinfos[AttackNum].Effect);
        effectobj.transform.position = attackinfos[AttackNum].EffectPosRot.position;
        effectobj.transform.rotation = attackinfos[AttackNum].EffectPosRot.rotation;

        preparent = effectobj.transform.parent;
        effectobj.transform.parent = attackinfos[AttackNum].EffectPosRot;
        //copyobj.transform.TransformDirection(movecom.com.FpRoot.forward);


        Destroy(effectobj, 1.5f);
        //effectobj = null;
    }



    //���ݾִϸ��̼��� ������ �ش� �Լ��� ���´�
    public void AttackEnd(string s_val)
    {
        

        if(effectobj!=null)
        {
            Debug.Log($"���� �� ���� -> {s_val}");
            effectobj.transform.parent = preparent;
        }

        if (curval.IsAttacking == true)
            curval.IsAttacking = false;

        lastAttackTime = Time.time;
        //animator.SetBool(EnumTypes.eAnimationState.Attack, false);
        //animator.SetBool(EnumTypes.eAnimationState.Idle, true);
        //LastAttackTime = Time.time;
        ////NowAttack = false;
        //StartCoroutine(Cor_AttackTimeCounter());

    }

    //������ �߰��� ���ܾ� �Ҷ�
    public void AttackCutOff()
    {

    }



    public override void InitComtype()
    {
        p_comtype = EnumTypes.eComponentTypes.AttackCom;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAttackComponent : BaseComponent
{
    //public AnimationClip[] Attack
    [SerializeField]
    //private int AttackCount;

    CurState curval;

    [Range(0.0f,5.0f)]
    [Tooltip("���� ����� ������ �ش� �ð� �ȿ� ���ݹ�ư�� Ŭ���ؾ��� ���ᵿ���� ����")]
    public float LinkAttackInterval;

    public float LastAttackTime;

    //public bool NowAttack;

    //
    public bool Linkable;

    public int AttackNum = 0;
    public CMoveComponent movecom;

    //public AnimationController animator;
    //public CAnimationComponent animator;

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

        public GameObject Effect;

        public Transform EffectPosRot;
    }

    public AnimationController animator;

    public AnimationEventSystem eventsystem;

    public AttackMovementInfo[] attackinfos;

    public float lastAttackTime = 0;

    void Start()
    {
        //animator = ComponentManager.GetI.GetMyComponent(EnumTypes.eComponentTypes.AnimatorCom) as CAnimationComponent;
        
        animator = GetComponentInChildren<AnimationController>();
        eventsystem = GetComponentInChildren<AnimationEventSystem>();
        eventsystem.AddEvent(null, null, AttackEnd);
        



    }


    //���� �߿��� 1������ ���� �ݺ����� ���鼭 ������ �޴µ��� ���� ��ȭ�� ������ �ʾҴ��� Ȯ���Ѵ�.
    IEnumerator Cor_AttackTimeCounter()
    {
        Linkable = true;
        float starttime;


        while(true)
        {
            //if()

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }




    public void Attack()
    {
        if(movecom==null)
        {
            movecom = ComponentManager.GetI.GetMyComponent(EnumTypes.eComponentTypes.MoveCom) as CMoveComponent;
            curval = movecom.curval;
        }

        if (curval.IsAttacking)
            return;

        if (curval.IsAttacking == false)
            curval.IsAttacking = true;

        Debug.Log("���� ����");
        float tempval = Time.time - lastAttackTime;
        Debug.Log($"����� �ð�{tempval}, ����ð�{attackinfos[AttackNum].NextMovementTimeVal}");

        if (tempval <= attackinfos[AttackNum].NextMovementTimeVal)
        {
            AttackNum = (AttackNum + 1) % (int)EnumTypes.eAniAttack.AttackMax;

        }
        else
        {
            AttackNum = 0;
        }

        GameObject copyobj = GameObject.Instantiate(attackinfos[AttackNum].Effect);
        copyobj.transform.position = attackinfos[AttackNum].EffectPosRot.position;
        //copyobj.transform.rotation = attackinfos[AttackNum].EffectPosRot.rotation;
        copyobj.transform.LookAt(movecom.com.FpRoot.forward);
        Destroy(copyobj, 1.5f);

        Debug.Log($"{attackinfos[AttackNum].aniclip.name}�ִϸ��̼� {attackinfos[AttackNum].animationPlaySpeed}�ӵ��� ����");
        animator.Play(attackinfos[AttackNum].aniclip.name, attackinfos[AttackNum].animationPlaySpeed);

        //movecom.FowardDoMove(5, animator.GetClipLength(attackinfos[AttackNum].aniclip.name)/2);

    }




    //���ݾִϸ��̼��� ������ �ش� �Լ��� ���´�
    public void AttackEnd(string s_val)
    {
        Debug.Log($"���� �� ���� -> {s_val}");
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

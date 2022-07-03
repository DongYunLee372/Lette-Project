using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAttackComponent : BaseComponent
{
    //public AnimationClip[] Attack
    [SerializeField]
    private int AttackCount;

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

    public AnimationController animator;
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
    }

    


    public AttackMovementInfo[] attckinfos;

    void Start()
    {
        //animator = ComponentManager.GetI.GetMyComponent(EnumTypes.eComponentTypes.AnimatorCom) as CAnimationComponent;
        movecom = ComponentManager.GetI.GetMyComponent(EnumTypes.eComponentTypes.MoveCom) as CMoveComponent;
        //curval = movecom.curval;



    }


    //���� �߿��� 1������ ���� �ݺ����� ���鼭 ������ �޴µ��� ���� ��ȭ�� ������ �ʾҴ��� Ȯ���Ѵ�.
    IEnumerator Cor_AttackTimeCounter()
    {
        Linkable = true;

        while(true)
        {
            //if()


        }

        yield return new WaitForSeconds(LinkAttackInterval);
        Linkable = false;
    }

    public void Attack()
    {
        if (curval.IsAttacking)
            return;


        if (Linkable)
        {
            AttackCount = (AttackCount + 1) % (int)EnumTypes.eAniAttack.AttackMax;

        }
        else
        {
            AttackCount = 0;
        }


        curval.IsAttacking = true;
    }

    //���ݾִϸ��̼��� ������ �ش� �Լ��� ���´�
    public void AttackEnd(int num)
    {
        Debug.Log($"���� �� ����{num}");
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

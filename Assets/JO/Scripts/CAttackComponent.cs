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

    public bool Linkable;

    public int AttackNum = 0;

    public CAnimationComponent animator;

    void Start()
    {
        animator = ComponentManager.GetI.GetMyComponent(EnumTypes.eComponentTypes.AnimatorCom) as CAnimationComponent;
        CMoveComponent movecom = ComponentManager.GetI.GetMyComponent(EnumTypes.eComponentTypes.MoveCom) as CMoveComponent;
        curval = movecom.curval;
    }


    //���� �߿��� 1������ ���� 
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

        if (animator == null)
            animator = ComponentManager.GetI.GetMyComponent(EnumTypes.eComponentTypes.AnimatorCom) as CAnimationComponent;

        animator.SetInt($"{EnumTypes.eAnimationState.Attack}Num", AttackCount);

        animator.SetBool(EnumTypes.eAnimationState.Attack, true);
        curval.IsAttacking = true;
    }

    //���ݾִϸ��̼��� ������ �ش� �Լ��� ���´�
    public void AttackEnd(int num)
    {
        Debug.Log($"���� �� ����{num}");
        //animator.SetBool(EnumTypes.eAnimationState.Attack, false);
        animator.SetBool(EnumTypes.eAnimationState.Idle, true);
        LastAttackTime = Time.time;
        //NowAttack = false;
        StartCoroutine(Cor_AttackTimeCounter());
        
    }


    public void AttackCutOff()
    {

    }



    public override void InitComtype()
    {
        p_comtype = EnumTypes.eComponentTypes.AttackCom;
    }

}

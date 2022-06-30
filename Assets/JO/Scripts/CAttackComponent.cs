using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAttackComponent : BaseComponent
{
    //public AnimationClip[] Attack
    [SerializeField]
    private int AttackCount;


    [Range(0.0f,5.0f)]
    [Tooltip("���� ����� ������ �ش� �ð� �ȿ� ���ݹ�ư�� Ŭ���ؾ��� ���ᵿ���� ����")]
    public float LinkAttackInterval;

    public float LastAttackTime;

    public bool NowAttack;

    public bool Linkable;

    public CAnimationComponent animator;

    void Start()
    {
        animator = ComponentManager.GetI.GetMyComponent(EnumTypes.eComponentTypes.AnimatorCom) as CAnimationComponent;

    }

    IEnumerator Cor_AttackTimeCounter()
    {
        Linkable = true;
        yield return new WaitForSeconds(LinkAttackInterval);
        Linkable = false;
    }

    public void Attack()
    {
        if(!NowAttack)
        {
            if (Linkable)
            {
                AttackCount = (AttackCount + 1) % (int)EnumTypes.eAniAttack.AttackMax;

            }
            else
            {
                AttackCount = 0;
            }

            if(animator==null)
                animator = ComponentManager.GetI.GetMyComponent(EnumTypes.eComponentTypes.AnimatorCom) as CAnimationComponent;

            animator.SetInt($"{EnumTypes.eAnimationState.Attack}Num", AttackCount);

            animator.SetBool(EnumTypes.eAnimationState.Attack, true);
            NowAttack = true;
        }
        
    }

    //���ݾִϸ��̼��� ������ �ش� �Լ��� ���´�
    public void AttackEnd(int num)
    {
        Debug.Log($"���� �� ����{num}");
        //animator.SetBool(EnumTypes.eAnimationState.Attack, false);
        animator.SetBool(EnumTypes.eAnimationState.Idle, true);
        LastAttackTime = Time.time;
        NowAttack = false;
        StartCoroutine(Cor_AttackTimeCounter());
        
    }

    public override void InitComtype()
    {
        p_comtype = EnumTypes.eComponentTypes.AttackCom;
    }

}

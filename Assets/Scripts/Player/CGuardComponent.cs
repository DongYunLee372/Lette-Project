using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGuardComponent : BaseComponent
{
    public CMoveComponent movecom;

    [Header("============Guard Options============")]
    public float GuardTime;//�ִ�� ���带 �� �� �ִ� �ð�
    public float GuardKnockBackTime;
    public int MaxGuardGauge;//



    [Header("============Cur Values============")]
    public int CurGuardGauge;
    public bool nowGuard;
    public float GaugeDownInterval;
    //public bool playingCor;
    public IEnumerator coroutine;
    public delegate void Invoker();

    public void Guard()
    {
        if (movecom == null)
            movecom = PlayableCharacter.Instance.GetMyComponent(EnumTypes.eComponentTypes.MoveCom) as CMoveComponent;

        if (movecom.curval.IsGuard)
            return;

        movecom.curval.IsGuard = true;

        movecom.com.animator.Play("_Guard", 2.0f);

        if (coroutine != null)
        {
            //playingCor = false;
            //Debug.Log("������ ����");
            StopCoroutine("Cor_TimeCounter");
            coroutine = null;

        }
        coroutine = Cor_TimeCounter(GuardTime, GuardDown);
        StartCoroutine(coroutine);
    }

    //
    public void GuardDown()
    {
        if (!movecom.curval.IsGuard)
            return;

        if (coroutine!=null)
        {
            //playingCor = false;
            //Debug.Log("������ ����");
            StopCoroutine(coroutine);
            coroutine = null;
        }
            

        movecom.curval.IsGuard = false;
        //CharacterStateMachine.Instance.SetState(CharacterStateMachine.eCharacterState.Idle);
    }

    //������ ���۵��� ���� �ð� �ڿ� ����Ʈ�� �����ؾ� �� �� ���
    IEnumerator Cor_TimeCounter(float time, Invoker invoker)
    {
        //playingCor = true;
        float starttime = Time.time;
        //Debug.Log("�ٽý���");
        while (true)
        {
            if ((Time.time - starttime) >= time)
            {
                //Debug.Log("�ð��ٵ�");
                invoker.Invoke();
                coroutine = null;
                //playingCor = false;
                yield break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }


    public void GuardKnockBack()
    {

    }

    public override void InitComtype()
    {
        p_comtype = EnumTypes.eComponentTypes.GuardCom;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

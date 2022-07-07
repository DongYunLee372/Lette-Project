using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*����� ��� Ŭ������ ��ϵǾ��ִ� animation controller �� ����� animator �� �ʿ�
  GetAnimationClips()�� �̿��Ͽ� ���� ��ϵ� Ŭ������ �޾ƿ�����, ���� �Է��ϴ��� �ؼ� ����� ���ϴ� Ŭ���� �̸��� �˾ƿͼ�
  Play(Ŭ���̸�, ����ӵ�, ����ð�, �����ӵ�) �Լ��� �̿��� ���*/
public class AnimationController : MonoBehaviour
{
    //[Header("�ִϸ��̼ǵ��� ��ϵ� �ִϸ��̼� ��Ʈ�ѷ��� �ʿ�")]
    //public AnimatorController anicontrol;
    [Header("Ȯ�ο�")]
    public Animator animator;
    public int m_clipsnum;
    public AnimationClip[] m_clips;
    public string currentplayclipname;

    public delegate void Invoker();



    private void Awake()
    {
        if (!TryGetComponent<Animator>(out animator))
        {
            animator = GetComponentInChildren<Animator>();
            if(animator==null)
            {
                Debug.Log($"{gameObject.name} animator component ����!");
            }
        }

        m_clips = animator.runtimeAnimatorController.animationClips;
    }

    

    //Ŭ���̸�, ����ӵ� (�⺻�� 1���), ��� �ð� (����ð��� 0�̸� ��� �ݺ�), ���� �ð�(���� �������� �Ѿ�µ� �ɸ� �ð�) 
    public void Play(string pname, float PlaySpeed = 1.0f, float PlayTime = 0, float blendingtime = 0.1f)
    {

        if (pname == currentplayclipname)
        {
            return;
        }

        if(PlayTime!=0)
        {
            StartCoroutine(Cor_TimeCounter(PlayTime, Stop));
        }

        SetPlaySpeed(PlaySpeed);

        currentplayclipname = pname;
        animator.CrossFade(pname, blendingtime);
    }

    ////Ŭ���̸�, ����ӵ� (�⺻�� 1���), ���� �ð�(���� �������� �Ѿ�µ� �ɸ� �ð�) 
    //public void Play(string pname, float PlaySpeed = 1.0f, float blendingtime = 0.1f)
    //{

    //    if (pname == currentplayclipname)
    //    {
    //        return;
    //    }

    //    SetPlaySpeed(PlaySpeed);

    //    currentplayclipname = pname;
    //    animator.CrossFade(pname, blendingtime);
    //}


    //������ Ŭ���� �� ���̸� �˷��ش�.
    public float GetClipLength(string pname)
    {
        float time = 0;
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        foreach (var a in ac.animationClips)
        {
            if (a.name == pname)
            {
                time = a.length;
            }
        }
        return time;
    }

    //���� �ִϸ����Ϳ� �����Ǿ� �ִ� Ŭ������ �迭�� �޾ƿ´�.
    public AnimationClip[] GetAnimationClips()
    {
        return m_clips;
    }

    //����ӵ��� �����Ѵ�.
    public void SetPlaySpeed(float PlaySpeed)
    {
        if (animator.speed != PlaySpeed)
            animator.speed = PlaySpeed;
    }

    //���� �ִϸ��̼��� ����ǰ� �ִ� �ӵ��� �޾ƿ´�.
    public float GetPlaySpeed()
    {
        return animator.speed;
    }

    //��� ����
    public void Stop()
    {
        animator.StopPlayback();
    }

    //��� �Ͻ�����
    public void Pause(float pausetime)
    {
        animator.speed = 0;
        //animator.CrossFade()
    }

    //�ٽ� ���
    public void resume()
    {
        animator.speed = 1.0f;
    }

    

    //public IEnumerator CountTime(string playname, float desttime)
    //{
    //    float starttime = Time.time;
    //    Debug.Log($"�ڷ�ƾ ����");
    //    while (true)
    //    {
    //        if (Time.time - starttime >= desttime)
    //        {
    //            Debug.Log($"{playname} �ִϸ��̼� ������");
    //            animator.Play(playname);
    //            yield break;
    //        }

    //        yield return new WaitForSeconds(Time.deltaTime);
    //    }

    //}


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

    //public void Play(string pname, int layer, float normalizedTime)
    //{

    //    if (pname == currentplayclipname)
    //    {
    //        //Debug.Log("������ΰ� ���");
    //        return;
    //    }
    //    currentplayclipname = pname;
    //    animator.CrossFade(pname, 0.3f, layer, normalizedTime);
    //}

    //���� ������� Ŭ������ Ȯ���Ѵ�.
    public bool IsNowPlaying(string pname)
    {
        return (currentplayclipname == pname);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

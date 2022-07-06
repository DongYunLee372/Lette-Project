using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator animator;
    public int m_clipsnum;
    public AnimationClip[] m_clips;

    public string currentplayclipname;


    void Start()
    {

        if(!TryGetComponent<Animator>(out animator))
        {
            gameObject.AddComponent<Animator>();
        }

        AnimatorController anicontrol = animator.runtimeAnimatorController as AnimatorController;
        if (anicontrol == null)
            Debug.Log("Is Null!");
        m_clips = animator.runtimeAnimatorController.animationClips;
    }

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

    //Ŭ���̸�, ����ӵ� (�⺻�� 1���), ��� �ð� (����ð��� 0�̸� ��� �ݺ�), ���� �ð�(���� �������� �Ѿ�µ� �ɸ� �ð�) 
    public void Play(string pname, float PlaySpeed=1.0f, float PlayTime=0,float blendingtime = 0.1f)
    {
        
        if (pname == currentplayclipname)
        {
            //Debug.Log("������ΰ� ���");
            return;
        }
        SetPlaySpeed(PlaySpeed);

        currentplayclipname = pname;
        animator.CrossFade(pname, blendingtime);
    }

    public IEnumerator CountTime(string playname, float desttime)
    {
        float starttime = Time.time;
        Debug.Log($"�ڷ�ƾ ����");
        while (true)
        {
            if (Time.time - starttime >= desttime)
            {
                Debug.Log($"{playname} �ִϸ��̼� ������");
                animator.Play(playname);
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

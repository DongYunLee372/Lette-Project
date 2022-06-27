using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//���� ���� �����ϴ� ��� animator�� �޾ƿͼ� 
//���
//1. animator �Ķ���� �� ���� �ش�animator������Ʈ �̸��� ������ �Ķ������ �̸��� �̿��ؼ� 

public class AnimationManager : Singleton<AnimationManager>
{
    public Dictionary<int, AnimationInfos> animatordic = new Dictionary<int, AnimationInfos>();

    public List<Animator> animatorlist;

    public Animator animator;

    public AnimatorControllerParameter[] _params;

    private void Awake()
    {
        animatorlist = GameObject.FindObjectsOfType<Animator>().ToList();
        int i = 0;
        foreach (var a in animatorlist)
        {
            
            animatordic.Add(i++, new AnimationInfos(a));
            
        }

        //for (int i = 0; i < _params.Length; i++)
        //{
        //    Debug.Log($"{i}��, {_params[i].name}, {_params[i].type}");
        //    //Debug.Log($"{_params[i].defaultBool}");
        //    //Debug.Log($"{_params[i].defaultFloat}");
        //    //Debug.Log($"{_params[i].defaultInt}");
        //}

    }


    public void SetInt(Animator animator, string pname, int value)
    {
        animatordic[0].SetInt(pname, value);
    }

    public void SetBool(Animator animator, string pname, bool value)
    {
        animatordic[0].SetBool(pname, value);
    }

    public void SetFloat(Animator animator, string pname, float value)
    {
        animatordic[0].SetFloat(pname, value);
    }

    public void SetTrigger(Animator animator, string pname)
    {
        animatordic[0].SetTrigger(pname);
    }

    public void SetPlaySpeed(Animator animator, float rate)
    {

    }

    private void Update()
    {
        
    }

}

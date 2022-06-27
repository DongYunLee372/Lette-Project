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
            
            animatordic.Add(a.GetInstanceID(), new AnimationInfos(a));
            Debug.Log($"�ִϸ����� �ϳ� �޾ƿ� ID = {a.GetInstanceID()}");
            AnimatorControllerParameter anii;
            AnimationClip clip;
            //animator.
            
        }


        //for (int i = 0; i < _params.Length; i++)
        //{
        //    Debug.Log($"{i}��, {_params[i].name}, {_params[i].type}");
        //    //Debug.Log($"{_params[i].defaultBool}");
        //    //Debug.Log($"{_params[i].defaultFloat}");
        //    //Debug.Log($"{_params[i].defaultInt}");
        //}

    }


    public void SetInt(int id, string pname, int value)
    {
        animatordic[id].SetInt(pname, value);
    }

    public void SetBool(int id, string pname, bool value)
    {
        animatordic[id].SetBool(pname, value);
    }

    public void SetFloat(int id, string pname, float value)
    {
        animatordic[id].SetFloat(pname, value);
    }

    public void SetTrigger(int id, string pname)
    {
        animatordic[id].SetTrigger(pname);
    }

    public void SetPlaySpeed(int id, float rate)
    {
        Debug.Log($"�ӵ� ���� {rate}");
        animatordic[id].SetPlaySpeed(rate);
    }

    private void Update()
    {
        
    }

}

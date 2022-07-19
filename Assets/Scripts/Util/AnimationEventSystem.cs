using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//�ִϸ��̼� �̺�Ʈ���� ����
//�ִϸ��̼� �̺�Ʈ�� �ش� �Լ��� OnBeginEventString, OnMidEventString, OnEndEventString �� ������ְ�
//�� �̺�Ʈ���� �븮�ڿ� �����ϰ��� �ϴ� �Լ��� AddEvent(beginCallback begin, midCallback mid, endCallback end) �Լ��� �̿��� ��������ָ� �̺�Ʈ�� ����Ǹ� �ش� �Լ��� �����

public class AnimationEventSystem : MonoBehaviour
{
	AnimationController animator;

	public AnimationClip[] clips;
	//public AnimationEvent[][] eventlist;
	public List<AnimationEvent[]> eventlist = new List<AnimationEvent[]>();

	public Dictionary<string, AnimationEvent[]> eventdic = new Dictionary<string, AnimationEvent[]>();



	public delegate void beginCallback(string s_val);
	public delegate void midCallback(string s_val);
	public delegate void endCallback(string s_val);

	public beginCallback _beginCallback;
	public midCallback _midCallback;
	public endCallback _endCallback;

	public UnityEvent beginCallBack;
	public UnityAction begi;

	private void Awake()
    {

    }

    private void Start()
    {
		animator = GetComponent<AnimationController>();
		clips = animator.GetAnimationClips();
	}

    //�ִϸ��̼��̺�Ʈ�� �Լ��� ��� �Ϸ��� �ش� �̺�Ʈ�� ������ �ִ� �ִϸ��̼�Ŭ���� �̸��� ���� �־� �ش�.
    public void AddEvent(beginCallback begin, midCallback mid, endCallback end)
    {
		if(begin != null)
			_beginCallback += begin;
		if (mid != null)
			_midCallback += mid;
		if (end != null)
			_endCallback += end;
    }

	//Animation Event
	public void OnBeginEventString(string s_val)
	{
		//if (null != _beginCallback)
		//	_beginCallback();

		_beginCallback?.Invoke(s_val);
		
	}

	public void OnMidEventString(string s_val)
	{
		_midCallback?.Invoke(s_val);
	}

	public void OnEndEventString(string s_val)
	{
		_endCallback?.Invoke(s_val);
	}

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventSystem : MonoBehaviour
{
	Animator animator;

	public delegate void beginCallback();
	public delegate void midCallback();
	public delegate void endCallback();

	public beginCallback _beginCallback;
	public midCallback _midCallback;
	public endCallback _endCallback;


	private void Awake()
    {
		animator = GetComponent<Animator>();

		//AnimationClip clip;
		//clip.events
    }

 //   public void Play(string trigger,
	//	System.Action beginCallback = null,
	//	System.Action midCallback = null,
	//	System.Action endCallback = null
	//	)
	//{
	//	GetComponent<Animator>().SetTrigger(trigger);
	//	_beginCallback = beginCallback;
	//	_midCallback = midCallback;
	//	_endCallback = endCallback;
	//}

	//�ִϸ��̼��̺�Ʈ�� �Լ��� ��� �Ϸ��� �ش� �̺�Ʈ�� ������ �ִ� �ִϸ��̼�Ŭ���� �̸��� ���� �־� �ش�.
	public void AddEvent(beginCallback begin, midCallback mid, endCallback end)
    {
		_beginCallback += begin;
		_midCallback += mid;
		_endCallback += end;
    }

	//Animation Event
	public void OnBeginEvent()
	{
		//if (null != _beginCallback)
		//	_beginCallback();

		_beginCallback?.Invoke();
	}

	public void OnMidEvent()
	{
		_midCallback?.Invoke();
	}

	public void OnEndEvent()
	{

		Debug.Log("Animaton End Event");
		_endCallback?.Invoke();
	}
}


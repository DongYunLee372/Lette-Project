using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//�ִϸ��̼� �̺�Ʈ���� ����
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

		//for(int i=0;i<clips.Length;i++)
  //      {
		//	eventdic.Add(clips[i].name, clips[i].events);
		//	eventlist.Add(clips[i].events);
		//}

  //      AnimationEvent myevent;
		//AnimationEventEffects effects;
		//effects.
		//myevent.
		//AnimationClip clip;
		//clip.events
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

		//Debug.Log("Animaton End Event");
		_endCallback?.Invoke(s_val);
	}

	//public void OnBeginEventFloat(float s_val)
	//{
	//	//if (null != _beginCallback)
	//	//	_beginCallback();

	//	//_beginCallback?.Invoke(s_val);

	//}

	//public void OnMidEventFloat(float s_val)
	//{
	//	//_midCallback?.Invoke(s_val);
	//}

	//public void OnEndEventFloat(float s_val)
	//{

	//	//Debug.Log("Animaton End Event");
	//	//_endCallback?.Invoke(s_val);
	//}

	//public void OnBeginEventInt(int s_val)
	//{
	//	//if (null != _beginCallback)
	//	//	_beginCallback();

	//	//_beginCallback?.Invoke(s_val);

	//}

	//public void OnMidEventInt(int s_val)
	//{
	//	//_midCallback?.Invoke(s_val);
	//}

	//public void OnEndEventInt(int s_val)
	//{

	//	//Debug.Log("Animaton End Event");
	//	//_endCallback?.Invoke(s_val);
	//}
}


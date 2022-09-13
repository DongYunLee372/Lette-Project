using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCutScene
{
    // 공통점 : 스킵 , 시작 , 끝 , 

    

    // IEnumerable asd 의 개념 알아보기
    // 컷신이라는 클래스가 하나 있으면 함수를 매개변수로 받아 순차적으로 실행


    // 외부에서 추가 , 삭제만 가능  / 초기화는 불가능하게 변경
    public delegate void StartCutCallback();
    public delegate void SkipCallback();
    public delegate void StopCutCallback();

    public StartCutCallback _StartCallback;
    public SkipCallback _SkipCallback;
    public StopCutCallback _StopCallback;



    public abstract bool P_CanSkip { get; protected set; }
   

    /// <summary>
    /// 씬 시작
    /// </summary>
    public abstract void Start();
    public abstract void Stop();
    public abstract void Skip();

    public void SetCallBack(StartCutCallback m_startcallback, SkipCallback m_skipcallback, StopCutCallback m_stopcallback)
    {
        if (m_startcallback != null)
            _StartCallback += m_startcallback;
        if (m_skipcallback != null)
            _SkipCallback += m_skipcallback;
        if (m_stopcallback != null)
            _StopCallback += m_stopcallback;

    }
    

}

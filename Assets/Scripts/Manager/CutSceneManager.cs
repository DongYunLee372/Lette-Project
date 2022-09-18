using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneManager : MySingleton<CutSceneManager>
{
    // 시작 , 중지 , 스킵 가능여부 / 자세한 데이터는 컷신매니저는 X 
    // 현재 진행상황 , 진행 여부 , 스킵 가능여부  데이터를 외부에 제공 
    // 컷신의 단계 (ex. 스킵시 모두 스킵 될 것인가 )

    // 옵저버 패턴

    public Dictionary<string, BaseCutScene> CutSceneDic = new Dictionary<string, BaseCutScene>();


    //public delegate void StartCutCallback(); //컷신 매니저가 아닌 컷신 클래스에서 처리 
    //public delegate void SkipCallback();
    //public delegate void EndCutCallback();

    //public Dictionary<string, StartCutCallback> StartEventInvokers = new Dictionary<string, StartCutCallback>();
    //public Dictionary<string, SkipCallback> SkipEventInvokers = new Dictionary<string, SkipCallback>();
    //public Dictionary<string, EndCutCallback> EndEventInvokers = new Dictionary<string, EndCutCallback>();


    //public StartCutCallback _StartCallback;
    //public SkipCallback _SkipCallback;
    //public EndCutCallback _endCallback;

    //public void AddEvent(KeyValuePair<string, StartCutCallback> start, KeyValuePair<string, SkipCallback> skip, KeyValuePair<string, EndCutCallback> end)
    //{
    //    if (start.Key != null)
    //        StartEventInvokers.Add(start.Key, start.Value);
    //    if (skip.Key != null)
    //        SkipEventInvokers.Add(skip.Key, skip.Value);
    //    if (end.Key != null)
    //        EndEventInvokers.Add(end.Key, end.Value);
    //}

    public void AddCutScene(string name , BaseCutScene cut)
    {
        CutSceneDic.Add(name, cut);
        Debug.Log(name);
    }

    public void OnStart(string name)
    {
        BaseCutScene cut;
        if(CutSceneDic.TryGetValue(name , out cut))
        {
            //Debug.Log(cut.name);
            cut.OnStartCallback();
        }
    }

    public void OnStop(string name)
    {
        BaseCutScene cut;
        if (CutSceneDic.TryGetValue(name, out cut))
        {
            //Debug.Log(cut.name);
            cut.OnStopCallback();
        }
    }

    public void OnSkip(string name)
    {
        BaseCutScene cut;
        if (CutSceneDic.TryGetValue(name, out cut))
        {
            if(IsSkipScene(cut))
            {
                cut.OnSkipCallback();
            }
            
        }
    }

    public void NowCutScene()
    {

    }

    public void IsCutScene()
    {

    }

    public bool IsSkipScene(BaseCutScene cut)
    {

        return true;
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}

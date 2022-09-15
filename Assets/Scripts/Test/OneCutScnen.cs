using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneCutScnen : BaseCutScene
{
    private bool CanSkip;
    public override bool P_CanSkip {  get {return CanSkip; } protected set { CanSkip = value; } }

    //string값을 3개를 받아 테스트하기 
    

    public override void Stop()
    {
        Debug.Log("Stop");
    }
    public override void Skip()
    {
        Debug.Log("Skip");
    }
    public override void On()
    {
        Debug.Log("On");
    }

    private void Awake()
    {
        SetCallBack(On, Skip, Stop);
        CutSceneManager.Instance.AddCutScene(this.name, this);
    }

    




}

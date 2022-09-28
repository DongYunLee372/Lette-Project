using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyDotween
{
    public class Tween
    {
        Tween Join;
        Vector3 destpos;
        GameObject TargetObj;
        float Duration;
        Dotween.LoopType LoopType;
        Dotween.Ease Ease;

        public Tween()
        {
            Join = null;
            destpos = Vector3.zero;
            TargetObj = null;
            Duration = 0;
            LoopType = Dotween.LoopType.Restart;
            Ease = Dotween.Ease.Linear;
        }


        public void Start()
        {

        }

        //일정 시간 이후에 시작
        public void Start(float StartTime)
        {

        }


    }




}


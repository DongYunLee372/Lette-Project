using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyDotween
{
    public class Sequence
    {
        Queue<Tween> sequence = new Queue<Tween>();
        Dotween.LoopType loopType;
        int loopCount = 0;
        int CurLoopCount = 0;
        CorTimeCounter TimeCounter = new CorTimeCounter();



        public void Start()
        {

        }


        //루프횟수가 -1이면 무한루프
        public Sequence SetLoop(int loops, Dotween.LoopType loopType = Dotween.LoopType.Restart)
        {


            return this;
        }


        //맨 마지막에 추가
        public Sequence Append(Tween tween)
        {

            sequence.Enqueue(tween);
            return this;
        }

        //순서와 관계없이 일정 시간 이후에 시작
        public Sequence Insert(float inserttime, Tween tween)
        {



            return this;
        }

        //앞에 추가된 트윈과 동시 시작
        public Sequence Join(Tween tween)
        {


            return this;
        }

        //맨 처음에 시작
        public Sequence Prepend(Tween tween)
        {

            
            return this;
        }
    }
}


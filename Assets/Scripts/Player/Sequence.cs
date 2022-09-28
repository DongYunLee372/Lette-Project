using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyDotween
{
    public class Sequence
    {
        ////////////////////////////////////////////////////////////
        /// 원형큐
        ////////////////////////////////////////////////////////////
        int queueSize = 20;
        Tween[] queue;
        int Rear = 0;//맨 마지막원소의 위치 증가시키고 삽입
        int Front = 0;//맨 앞 원소의 한칸 앞
        ////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////
        

        //독립적으로 실행되는 트윈들의 리스트
        List<Tween> InsertTweens = new List<Tween>();

        //루프타입
        Dotween.LoopType loopType;
        int loopCount = 0;
        int CurLoopCount = 0;

        //타이머
        CorTimeCounter TimeCounter = new CorTimeCounter();

        public Sequence()
        {
            queue = new Tween[queueSize];
            Rear = Front = 0;

        }

        public Sequence(int _QueueSize)
        {
            queueSize = _QueueSize;
            queue = new Tween[_QueueSize];
            Rear = Front = 0;

        }

        public bool EnQueue(Tween[] _queue, Tween tween)
        {
            if((Rear+1)%queueSize==Front)
            {
                return false;
            }


            Rear = (Rear + 1) % queueSize;
            _queue[Rear] = tween;

            return true;
        }

        public Tween DeQueue(Tween[] _queue)
        {
            if(Front==Rear)
            {
                return null;
            }

            Front = (Front + 1) % queueSize;
            return _queue[Front];

        }

        public Tween Peek(Tween[] _queue)
        {
            if (Front == Rear)
            {
                return null;
            }

            return _queue[(Front + 1) % queueSize];
        }

        //큐에서 하나씩 빼서 실행하는데 뒤에있는 것이 조인설정이 있으면 해당 트윈도 동시 실행
        public void Start()
        {

        }


        //루프횟수가 -1이면 무한루프
        public Sequence SetLoop(int loops/*루프횟수*/, Dotween.LoopType loopType = Dotween.LoopType.Restart)
        {


            return this;
        }


        //맨 마지막에 추가
        public Sequence Append(Tween tween)
        {
            EnQueue(queue, tween);
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


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyStateMachine
{
    public class Drive
    {
        public Action Update;
    }

    public class StateMachineRunner : MonoBehaviour
    {
        private List<IStateMachine<Drive>> stateMachineList = new List<IStateMachine<Drive>>();

        //private List<StateMachine<TState, Drive>> stateMachineList = new List<StateMachine<TState, Drive>>();
        public void Initialize<TState>(MonoBehaviour component) where TState : struct, IConvertible, IComparable
        {
            var fsm = new StateMachine<TState, Drive>(component);

            stateMachineList.Add(fsm);

        }

        //public void Initialize<TState, TDriver>(StateMachine<TState, TDriver> fsm) where TState : struct, IConvertible, IComparable
        //{
        //    stateMachineList.Add(fsm);
        //}

        void Update()
        {
            //for (int i = 0; i < stateMachineList.Count; i++)
            //{
            //    var fsm = stateMachineList[i];
            //    if (!fsm.IsInTransition && fsm.Component.enabled)
            //    {
            //        fsm.Driver.Update.Invoke();
            //    }
            //}


            for (int i = 0; i < stateMachineList.Count; i++)
            {
                var fsm = stateMachineList[i];
                if (!fsm.IsInTransition && fsm.Component.enabled)
                {
                    if(fsm.GetUpdateAction!=null)
                    {
                        int a = 0;
                    }
                    fsm.GetUpdateAction?.Invoke();
                }
            }
        }

    }
}


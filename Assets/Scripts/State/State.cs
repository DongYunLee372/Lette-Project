using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public bool first_Start; // 최초의 스테이트 시작인지 체크해주는 bool 변수

    public List<State> Trans_List; // 전이 리스트
    public abstract bool Judge(out State _State, Battle_Character b_c);

    public abstract void Run(Battle_Character b_c);

    public virtual List<State> State_Initialize() { return null; }
}
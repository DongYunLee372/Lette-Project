using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public bool first_Start; // 최초의 스테이트 시작인지 체크해주는 bool 변수

    public List<State> Trans_List; // 전이 리스트
    public abstract bool Judge(out State _State, Battle_Character battle_character);

    public abstract void Run(Battle_Character battle_character);

    public virtual void State_Initialize(Battle_Character battle_character) { } // 이닛 스테이트의 초기화가 아닌 초기화 함수가 필요한 State를 위함
}
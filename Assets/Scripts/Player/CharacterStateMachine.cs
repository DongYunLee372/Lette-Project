using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ĳ������ ���¸� ����, ���¿� ���� ����(�ִϸ��̼�)���� ���ش�.
public class CharacterStateMachine : MonoBehaviour
{
    public enum E_CharacterState
    {
        Idle,
        Move,
        Attack,
        Jump,
        Slip,
        OutOfControl,

    }

    public E_CharacterState characterState;


    private void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMG : Singleton<GameMG>
{
    public float PlayTime;  //�÷��� �ð� ����
    private float time_start;
    private float time_current;
    private float time_Max = 5f;
    private bool isEnded;

    //�÷��� �ð�
    private void Check_Timer()
    {
        time_current = Time.time - time_start;
        if (time_current < time_Max)
        {
           // text_Timer.text = $"{time_current:N2}";
            Debug.Log(time_current);
        }
        else if (!isEnded)
        {
            End_Timer();
        }
    }

    private void End_Timer()
    {
        Debug.Log("End");
        time_current = time_Max;
       // text_Timer.text = $"{time_current:N2}";
        isEnded = true;
    }


    private void Reset_Timer()
    {
        time_start = Time.time;
        time_current = 0;
      //  text_Timer.text = $"{time_current:N2}";
        isEnded = false;
        Debug.Log("Start");
    }

    void startGame()
    {
        // ���� ���嵥����
        //�����ʵ�
        //ĳ���� ����
        // UI�Ŵ��� ȣ��
    }

    //�ε� �� �����
    //�ε��� 

    //�������� ����

    //���� ĳ���� �ε�
    
    //���Ͷ� ĳ���� ����� ��Ģ
    //���������� ��� ���� 
    //ĳ���� 
    //���� Ŭ���� 
    //��ų ������ ���

    public void Damage_calculator()
    {
        //������= (����)���ݷ� - (����)���� 
    }

    void Update()
    {
        if (isEnded)
            return;

        Check_Timer();
    }


    void Start()
    {
        Reset_Timer();
    }

}

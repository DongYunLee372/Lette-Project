using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_enemy : Battle_Character
{
    void Start()
    {
        Initalize();
        ai.AI_Initialize(this);
    }

    protected override void Patrol_Enter_Process()
    {
        Debug.Log("��Ʈ�� ����");
    }

    new void Update()
    {
        base.state = ai.AI_Update();
        base.Update();
    }
}

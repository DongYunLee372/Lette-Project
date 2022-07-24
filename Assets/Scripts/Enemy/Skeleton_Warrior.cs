using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Warrior : Battle_Character
{
    void Start()
    {
        Initalize();
        ai.AI_Initialize(this);
    }

    void Update()
    {
        state_handler.state = ai.AI_Update();
        state_handler.State_Handler_Update();
    }

    public override void Skill_1() // 스켈레톤 워리어 1번스킬 내려치기
    {
        Debug.Log("내려치기 발동");
    }

    public override void Skill_2() // 스켈레톤 워리어 2번스킬 찌르기
    {
        Debug.Log("찌르기 발동");
    }

    public override void Die_Process() // 죽을때 호출되는 함수 (부활 처리해야함)
    {
        StartCoroutine(Skeleton_Warrior_Revival());
    }

    IEnumerator Skeleton_Warrior_Revival()
    {
        // 사망하는 애니메이션 처리

        yield return new WaitForSeconds(5f); // 5초 안에 떄리지 않았다면

        Debug.Log("부활부활");
        // 부활하는 애니메이션 재생 후 
        ai.AI_Initialize(this);

        Cur_HP = 100;
    }

    public override void Attack_Process()
    {
        if (Player_Mana >= need_Mana)
        {
            switch (next_Skill)
            {
                case 1: // 1번 스킬
                    Skill_1();
                    break;
                case 2: // 2번 스킬
                    Skill_2();
                    break;
                    // 스킬에 따라 진행
            }
            //Enemy_Skill_Rand(); // 다음 스킬 찾기
        }
        else // 기본 공격
        {
            // 기본 공격 코드
            //anim.SetBool("isAttack", true);
        }
    }
}

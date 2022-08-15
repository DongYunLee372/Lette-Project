using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
배틀 캐릭터에 일반 스킬 클래스나 특수한 스킬 클래스(스킬 클래스를 상속받은)를 인스턴싱 해준 후 인스턴싱 해준 클래스를 호출해서
Skill_Run함수를 호출하면서 데이터를 넘겨줘서 스킬을 사용하는 구조.
*/

public class Skill : MonoBehaviour
{
    public virtual void Skill_Run(Battle_Character b_c, MonsterSkillInformation skill_data)
    {
        b_c.isAttack_Run = true;
        b_c.animator.Play(skill_data.P_skill_Name_En);
        // 애니메이션 재생 ( 애니메이션 클립에 이벤트들을 붙여줌 해당 시간에 판정 
        // 어택매니저로 공격 호출
        // 어택매니저를 통해서 이펙트호출


        //if (skill_data.P_skill_ThrowObj)
        //{
        //    StartCoroutine(Shoot_Coroutine(skill_data));
        //}

        //if (skill_data.P_skill_DiffObj)
        //{
        //    StartCoroutine(Spawn_Coroutine(skill_data));
        //}

        // 캐릭터 현재 마나 - 스킬의 마나
        // //b_c.mon_Info
        b_c.mon_Info.P_mon_haveMP -= skill_data.P_skill_MP;
    }

    protected IEnumerator Shoot_Coroutine(MonsterSkillInformation skill_data)
    {
        yield return new WaitForSeconds(1f/*스킬데이터.시간*/);

        /*
         발사체 생성 후 발사 처리
        이펙트 등등
        */
    }

    protected IEnumerator Spawn_Coroutine(MonsterSkillInformation skill_data)
    {
        yield return new WaitForSeconds(1f/*스킬데이터.시간*/);

        /*
         몬스터 소환 후 위치 지정 등등
        이펙트 등등
         */
    }

    //public virtual void 소환();

    //public virtual void 투사체발사();
}

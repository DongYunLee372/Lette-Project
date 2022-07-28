using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public virtual void Skill_Run(/*스킬 데이터 형태가 들어가야함.*/)
    {
        // 애니메이션 재생 ( 애니메이션 클립에 이벤트들을 붙여줌 해당 시간에 판정 
        // 어택매니저로 공격 호출
        // 어택매니저를 통해서 이펙트호출

        /* 
        if(투사체 판정 여부가 true라면)
        {
            StartCoroutine(Spawn_Coroutine(스킬데이터));
        }

        if(소환 여부가 true라면)
        {
            StartCoroutine(Spawn_Coroutine(스킬데이터));
        }
        
        */
    }

    IEnumerator Shoot_Coroutine(/*스킬데이터*/)
    {
        yield return new WaitForSeconds(1f/*스킬데이터.시간*/);

        /*
         발사체 생성 후 발사 처리
        이펙트 등등
        */
    }

    IEnumerator Spawn_Coroutine(/*스킬데이터*/)
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

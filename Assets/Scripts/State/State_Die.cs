using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Die : State
{
    public override bool Judge(out State _State, Battle_Character battle_character)
    {
        // 0으로 바꿔야함
        if (battle_character.cur_HP <= 0) // 사망
        {
            _State = this;
            return true;
        }

        _State = null;
        return false;
    }

    public override void Run(Battle_Character battle_character)
    {
        // 사망 애니메이션 등 사망 시 부활하고 소환하는 등등의 효과(스킬)을 호출. 
        // 스킬 구조 구현 시 추가해줘야함.
        if (!first_Start)
        {
            // 캐릭터 데이터로 
            battle_character.real_AI.navMesh.SetDestination(battle_character.transform.position);
            battle_character.animator.Play("Death");
            battle_character.real_AI.isPause = true;

            GameData_Load.Instance.MonsterDead(battle_character.gameObject);

            //StartCoroutine(death_Coroutine(battle_character.phase_Effect, battle_character.transform));

            this.first_Start = true;
        }

        battle_character.real_AI.now_State = Trans_List[0];
    }

    IEnumerator death_Coroutine(GameObject eff, Transform transform)
    {
        // 이펙트 매니저로
        yield return new WaitForSeconds(3f);

        GameObject effectobj = GameObject.Instantiate(eff);
        effectobj.transform.position = transform.position;
        effectobj.transform.rotation = transform.rotation;

        //preparent = effectobj.transform.parent;
        //effectobj.transform.parent = attack_Info[i].effect_Pos[2];
        //copyobj.transform.TransformDirection(movecom.com.FpRoot.forward);

        Destroy(effectobj, 8.0f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy_Enum;

public class State_Trace : State
{
    public override bool Judge(out State _State, Battle_Character battle_character)
    {
        // 아래 부분들이 다 필요없고 최종 사거리만 계산해서 최종 사거리안에 진입했다면 Attack 스테이트로 돌리면 될듯
        // 변수 추가 받은 후 수정


        //if (Vector3.Distance(battle_character.transform.position,
        //    battle_character.cur_Target.transform.position)
        //    <= battle_character.mon_Info.P_mon_ShortRange) // 타겟을 공격할 수 있는 사거리 내 진입했다면
        //{
        //    _State = Trans_List[0];
        //    battle_character.real_AI.pre_State = this;
        //    return false;
        //}

        //if ((Vector3.Distance(battle_character.transform.position,
        //    battle_character.cur_Target.transform.position) <= battle_character.now_Skill_Info.P_skill_Range)
        //    && battle_character.mon_Info.P_mon_haveMP >= battle_character.now_Skill_Info.P_skill_MP)
        //{
        //    _State = Trans_List[0];
        //    battle_character.real_AI.pre_State = this;
        //    return false;
        //}

        //if (battle_character.now_Skill_Info.P_skill_Range == 0
        //    && battle_character.mon_Info.P_mon_haveMP >= battle_character.now_Skill_Info.P_skill_MP)
        //{
        //    _State = Trans_List[0];
        //    battle_character.real_AI.pre_State = this;
        //    return false;
        //}

        //if (battle_character.attack_Logic[(int)Enemy_Attack_Logic.Long_Attack] == true && (Vector3.Distance(battle_character.transform.position,
        //    battle_character.cur_Target.transform.position) <= battle_character.mon_Info.P_mon_LongRange)) // 원거리 공격방식이 존재
        //{
        //    _State = Trans_List[0];
        //    battle_character.real_AI.pre_State = this;
        //    return true;
        //}

        _State = this;
        return true;
    }

    public override void Run(Battle_Character battle_character)
    {
        battle_character.animator.Play("Walk");
        battle_character.real_AI.navMesh.SetDestination(battle_character.cur_Target.transform.position);
    }
}

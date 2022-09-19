using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy_Enum;

public class State_Attack : State
{
    [SerializeField]
    private Enemy_Attack_Logic judge_logic; // 리턴받는 공격방식

    public override bool Judge(out State _State, Battle_Character battle_character)
    {
        if (battle_character.isAttack_Run)
        {
            if (battle_character.isStop)
            {
                if (battle_character.attack_Info[battle_character.ani_Index].off_Mesh_Pos[0])
                    battle_character.attack_Info[battle_character.ani_Index].off_Mesh_Pos[0].position = battle_character.begin_Pos;
                battle_character.isAttack_Run = false;
                battle_character.real_AI.pre_State = this;
                battle_character.attack_Collider.SetActive(false);
                _State = Trans_List[0];
                return false;
            }

            judge_logic = Enemy_Attack_Logic.Skill_Wait;
            _State = this;
            return true;
        }

        if (battle_character.isDelay)
        {
            judge_logic = Enemy_Attack_Logic.Skill_Wait;
            _State = this;
            return true;
        }

        // 사거리가 있는 스킬
        if ((Vector3.Distance(battle_character.transform.position,
            battle_character.cur_Target.transform.position) <= battle_character.now_Skill_Info.P_skill_Range)
            && battle_character.mon_Info.P_mon_haveMP >= battle_character.now_Skill_Info.P_skill_MP)
        {
            judge_logic = Enemy_Attack_Logic.Skill_Using;
            _State = this;
            return true;
        }

        // 제자리에서 사용가능한 스킬 ( ex . 소환 )
        if (battle_character.now_Skill_Info.P_skill_Range == 0
            && battle_character.mon_Info.P_mon_haveMP >= battle_character.now_Skill_Info.P_skill_MP)
        {
            judge_logic = Enemy_Attack_Logic.Skill_Using;
            _State = this;
            return true;
        }

        // 근접 공격 사거리 체크
        if ((Vector3.Distance(battle_character.transform.position,
                battle_character.cur_Target.transform.position) <= battle_character.mon_Info.P_mon_ShortRange) && !battle_character.isAttack_Run) 
        {
            judge_logic = Enemy_Attack_Logic.Melee_Attack;
            _State = this;
            return true;
        }

        if (battle_character.attack_Logic[(int)Enemy_Attack_Logic.Long_Attack] == true && (Vector3.Distance(battle_character.transform.position,
            battle_character.cur_Target.transform.position) <= battle_character.mon_Info.P_mon_LongRange)) // 원거리 공격방식이 존재
        {
            judge_logic = Enemy_Attack_Logic.Long_Attack;
            _State = this;
            return true;
        }

        battle_character.real_AI.pre_State = this;
        _State = Trans_List[0];
        return false;
    }

    public override void Run(Battle_Character battle_character)
    {
        switch (judge_logic)
        {
            case Enemy_Attack_Logic.Skill_Wait:
                // 기다리기
                break;
            case Enemy_Attack_Logic.Melee_Attack:
                // 근접 공격이라면 배틀캐릭터 스크립트 내 공격 판정범위 활성화
                battle_character.attack_Type = Enemy_Attack_Type.Normal_Attack;
                battle_character.checkTime = 0f;
                battle_character.isStop = false;
                battle_character.gameObject.transform.LookAt(battle_character.cur_Target.transform);
                battle_character.attack_Collider.SetActive(true);
                Debug.Log("밀리");
                battle_character.isAttack_Run = true;
                battle_character.animator.Play("Melee Attack");
                break;
            case Enemy_Attack_Logic.Long_Attack:
                // 원거리라면 원거리 발사체 발사
                battle_character.attack_Type = Enemy_Attack_Type.Normal_Attack;
                break;
            case Enemy_Attack_Logic.Skill_Using:
                // 이번에 사용할 순서의 스킬을 사용.
                battle_character.attack_Type = Enemy_Attack_Type.Skill_Attack;
                battle_character.skill_handler.Skill_Run(battle_character, battle_character.now_Skill_Info);
                battle_character.Skill_Rand();
                break;
        }
    }


}

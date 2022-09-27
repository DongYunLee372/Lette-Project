using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy_Enum;

public class State_Attack : State
{
    [SerializeField]
    private Enemy_Attack_Logic judge_logic; // 리턴받는 공격방식

    public bool Attack_Result; // 공격 성공인지 실패인지 판별 변수
    public GameObject Result_return_Object; // 위의 판별변수를 반환 해준 오브젝트

    public int attack_Info_Index; // 배틀 캐릭터의 Attack_Info index

    public string[] special_Range = new string[2];

    public override bool Judge(out State _State, Battle_Character battle_character)
    {
        if (battle_character.isAttack_Run)
        {
            if (battle_character.isStop)
            {
                if (battle_character.attack_Info[battle_character.ani_Index].off_Mesh_Pos[0])
                    battle_character.attack_Info[battle_character.ani_Index].off_Mesh_Pos[0].localPosition = battle_character.begin_Pos;
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

        // 근접 공격 사거리 체크
        if ((Vector3.Distance(battle_character.transform.position,
                battle_character.cur_Target.transform.position) <= battle_character.mon_Info.P_mon_CloseAtk) && !battle_character.isAttack_Run)
        {
            judge_logic = Enemy_Attack_Logic.Melee_Attack;
            _State = this;
            return true;
        }


        if ((Vector3.Distance(battle_character.transform.position,
               battle_character.cur_Target.transform.position) <= int.Parse(special_Range[1]))
               &&
               (Vector3.Distance(battle_character.transform.position,
               battle_character.cur_Target.transform.position) >= int.Parse(special_Range[0]))
               &&
               !battle_character.isAttack_Run && battle_character.isSkill_Using)
        {
            judge_logic = Enemy_Attack_Logic.Skill_Using;
            _State = this;
            return true;
        }


        if (battle_character.mon_Info.P_mon_FarAtk != 0 && (Vector3.Distance(battle_character.transform.position,
            battle_character.cur_Target.transform.position) >= battle_character.mon_Info.P_mon_FarAtk) &&
            battle_character.isShooting) // 원거리 공격
        {
            judge_logic = Enemy_Attack_Logic.Long_Attack;
            _State = this;
            return true;
        }

        battle_character.real_AI.pre_State = this;
        _State = Trans_List[0];
        return false;
    }

    public void Rand_Normal_Attack(Battle_Character battle_character)
    {
        int count = battle_character.mon_normal_atak_group.Count;
        int rand = Random.Range(0, count);

        battle_character.animator.animator.SetTrigger("Delay_Trg");
        battle_character.animator.Play(battle_character.mon_normal_atak_group[rand].P_skill_Name_En);
    }

    public void Connect_Process(Battle_Character battle_character)
    {
        if (Attack_Result)
        {
            if (Result_return_Object.GetComponent<Enemy_Weapon>() != null &&
                battle_character.attack_Info[attack_Info_Index].after_skill_name != "")
            {
                battle_character.animator.Play(battle_character.
                    attack_Info[attack_Info_Index].after_skill_name);
                Debug.Log("라스트 : 밀리 공격 : " + battle_character.
                    attack_Info[attack_Info_Index].after_skill_name);

            }
        }
    }

    public override void Run(Battle_Character battle_character)
    {
        switch (judge_logic)
        {
            case Enemy_Attack_Logic.Skill_Wait: // 원거리 공격같은 경우 공격이 성공했는지 실패했는지 판별해야 할 경우
                // isDelay를 통해 판별을 기다림
                Connect_Process(battle_character);
                // 기다리기
                break;
            case Enemy_Attack_Logic.Melee_Attack:
                // 근접 공격이라면 배틀캐릭터 스크립트 내 공격 판정범위 활성화
                Rand_Normal_Attack(battle_character);

                Attack_Result = false;
                Result_return_Object = null;

                battle_character.attack_Type = Enemy_Attack_Type.Normal_Attack;
                battle_character.stop_CheckTime = 0f;
                battle_character.isStop = false;
                battle_character.gameObject.transform.LookAt(battle_character.cur_Target.transform);
                battle_character.attack_Collider.SetActive(true);
                battle_character.isAttack_Run = true;
                break;
            case Enemy_Attack_Logic.Long_Attack:
                // 원거리라면 원거리 발사체 발사

                Debug.Log("라스트 : 원거리 : ");

                Attack_Result = false;
                Result_return_Object = null;

                battle_character.isShooting = false;
                battle_character.long_CheckTime = 0f;
                battle_character.isStop = false;
                battle_character.attack_Type = Enemy_Attack_Type.Normal_Attack;
                battle_character.gameObject.transform.LookAt(battle_character.cur_Target.transform);
                battle_character.isAttack_Run = true;
                battle_character.animator.Play("Long_Attack");
                //GameObject missile
                break;
            case Enemy_Attack_Logic.Skill_Using:
                // 이번에 사용할 순서의 스킬을 사용.
                Attack_Result = false;
                Result_return_Object = null;

                battle_character.isSkill_Using = false;
                battle_character.skill_CheckTime = 0f;
                battle_character.attack_Type = Enemy_Attack_Type.Skill_Attack;
                battle_character.skill_handler.Skill_Run(battle_character, battle_character.now_Skill_Info);

                Debug.Log("라스트 : 스킬 공격 : " + battle_character.now_Skill_Info.P_skill_Name_En);
                break;
        }
    }

    public override void State_Initialize(Battle_Character battle_character)
    {
        special_Range = battle_character.mon_Info.P_mon_SpecialAtk.Split(",");

        Debug.Log("special = " + special_Range[0] + "," + special_Range[1]);
    }

}

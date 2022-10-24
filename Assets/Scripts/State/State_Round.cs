using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class State_Round : State
{
    public float round_CheckTime = 0.0f; // 진입 시 0초로 초기화 받음
    public float round_nextTime = 4.0f;
    public bool round_Check = false;

    float circleR; //반지름
    float deg; //각도
    float objSpeed; //원운동 속도

    public override bool Judge(out State _State, Battle_Character battle_character)
    {
        // 근접 공격 사거리 체크
        if ((Vector3.Distance(battle_character.transform.position,
                battle_character.cur_Target.transform.position) > 5f/*battle_character.mon_Info.P_mon_CloseAtk*/)
                /*&& battle_character.normal_CoolTime.isCheck*/)
        {
            _State = Trans_List[0]; // state_trace로 이동
            battle_character.real_AI.pre_State = this;
            return false;
        }

        if (round_Check)
        {
            _State = Trans_List[1]; // state_attack으로 이동
            round_Check = false;

            battle_character.real_AI.navMesh.speed = battle_character.init_Speed;
            battle_character.real_AI.navMesh.acceleration = 8f;
            // 노말 공격 선정하라는 bool 변수 켜줌
            battle_character.real_AI.pre_State = this;
            battle_character.skill_CoolTime.isCheck = false;
            battle_character.skill_CoolTime.check_Time = 0f;
            battle_character.long_CoolTime.isCheck = false;
            battle_character.long_CoolTime.check_Time = 0f;
            return false;
        }

        _State = this;
        return true;
    }

    public override void Run(Battle_Character battle_character)
    {
        bool fflag = false;
        foreach(AnimationClip clip in battle_character.animator.m_tempclips)
        {
            if(clip.name == "Walk")
            {
                fflag = true;
                battle_character.animator.Play("Walk");
                break;
            }
        }

        if(!fflag)
            battle_character.animator.Play("Idle");

        //battle_character.transform.LookAt(battle_character.cur_Target.transform);

        battle_character.gameObject.transform.DOLookAt(battle_character.cur_Target.transform.position, 0.5f);
        round_CheckTime += Time.deltaTime;
        if (round_CheckTime > round_nextTime)
        {
            //nextTime = Time.time + TimeLeft;
            round_CheckTime = 0f;
            battle_character.normal_CoolTime.isCheck = true;
            round_Check = true;
        }
        else
        {
            deg += Time.deltaTime * 50f;
            if (deg < 360)
            {
                var rad = Mathf.Deg2Rad * (deg);
                var x = 4.5f * Mathf.Sin(rad);
                var y = 4.5f * Mathf.Cos(rad);
                battle_character.real_AI.navMesh.SetDestination(battle_character.cur_Target.transform.position + new Vector3(x, 0, y));
            }
            else
            {
                deg = 0;
            }
        }
    }

    public override void State_Initialize(Battle_Character battle_character)
    {
        round_CheckTime = 0.0f;
        round_nextTime = 4.0f;
        round_Check = false;

        battle_character.real_AI.navMesh.speed = 1.5f;
        battle_character.real_AI.navMesh.acceleration = 8f;
    }
}

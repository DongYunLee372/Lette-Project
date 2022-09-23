using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy_Enum;

public class State_Trace : State
{
    int Longest_range;

    public override bool Judge(out State _State, Battle_Character battle_character)
    {
        // 아래 부분들이 다 필요없고 최종 사거리만 계산해서 최종 사거리안에 진입했다면 Attack 스테이트로 돌리면 될듯
        // 변수 추가 받은 후 수정
        if (Vector3.Distance(battle_character.transform.position,
            battle_character.cur_Target.transform.position)
            <= Longest_range) // 타겟을 공격할 수 있는 사거리 내 진입했다면
        {
            _State = Trans_List[0];
            battle_character.real_AI.pre_State = this;
            return false;
        }

        _State = this;
        return true;
    }

    public int Longest_Range_Find(Battle_Character battle_character)
    {
        List<int> judge_List = new List<int>();

        int short_range = battle_character.mon_Info.P_mon_CloseAtk;
        int long_range = battle_character.mon_Info.P_mon_FarAtk;
        int max = -1;
        string[] special_ranges = battle_character.mon_Info.P_mon_SpecialAtk.Split(",");

        judge_List.Add(short_range);
        judge_List.Add(long_range);
        judge_List.Add(int.Parse(special_ranges[1]));

        foreach (int n in judge_List)
        {
            if (n >= max)
                max = n;
        }

        return max;
    }

    public override void Run(Battle_Character battle_character)
    {
        battle_character.animator.Play("Walk");
        battle_character.real_AI.navMesh.SetDestination(battle_character.cur_Target.transform.position);
    }

    public override void State_Initialize(Battle_Character battle_character)
    {
        Longest_range = Longest_Range_Find(battle_character);
    }
}

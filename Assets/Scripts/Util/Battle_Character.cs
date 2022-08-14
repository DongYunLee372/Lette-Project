using Enemy_Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
ex ) 원거리, 근거리 처럼 다른 스테이트 프로세스를 수행해야 한다면 
스테이트 처리기를 상속받은 클래스를 구현해서, 예를들어서 patrol 이 필요없는 ai 종류의 스테이트 처리기, 필요한 종료의 스테이트 처리기 등을
구현해야함 . 그리고 state_handler 를 상속이 아니라 ai 처럼 가지고 있을 수 있게 해야함. 

시간 값 저장해주는 것도 처리.
 */

[System.Serializable]
public class Attack_Info // 스킬이나 공격info
{
    // 이름
    public string Name;

    //움직일 거리
    public float[] Movedis;

    //움직일 시간
    public float[] MoveTime;

    // 이펙트 있으면 이펙트
    public GameObject[] effect;

    //이펙트 생성 시간
    public float[] EffectStartTime;

    // 이펙트 있으면 이펙트 생성 위치
    public Transform[] effect_Pos;
}

public class Battle_Character : MonoBehaviour
{
    public FSM_AI ai = new FSM_AI();

    [SerializeField]
    protected State_Handler state_handler;

    public EnemyHpbar MyHpbar;

    public AnimationController animator;
    public AnimationEventSystem eventsystem;

    public Skill skill_handler; // 보유한 스킬에따라 필요한 스킬핸들러를 부여.

    [Header("Real Stats")] // 데이터 정리된 것 밑의 데이터들은 정리해야함
    public CharacterInformation char_Info;
    public MonsterInformation mon_Info;
    public List<MonsterSkillInformation> mon_Skill_Info = new List<MonsterSkillInformation>();
    public MonsterSkillInformation now_Skill_Info;
    public MonsterTargetInformation mon_Target_Info;
    public Player_aconstant player_Aconstant;
    public Monster_aconstant mon_Aconstant;

    [Header("Monster Stats")]
    public Enemy_Grade enemy_Grade; // 몬스터 등급
    public Enemy_Type enemy_Type; // 몬스터 타입
    public Vector3 return_Pos; // 초기 좌표
    public Vector3 destination_Pos; // 순찰 좌표
    public int die_Delay; // 사망 딜레이
    public int drop_Reward; // 몬스터의 보상
    public bool patrol_Start = false; // 탐색 시작
    public float mon_attack_Power; // 몬스터 공격력
    public float mon_find_Range; // 탐지범위

    [Header("Etc Stats")]
    public GameObject cur_Target;
    public int next_Skill;
    public bool isReturn; // enemy_Area 에서 나갈경우 true 체크해줘서 ai가 판단할 수 있게끔 하는 변수

    [Header("=========Attack Related=========")]
    public GameObject attack_Collider; // 공격 판정 충돌 범위 콜라이더 
    public Enemy_Attack_Type attack_Type; // 공격 타입
    public bool[] attack_Logic = new bool[(int)(Enemy_Attack_Logic.Attack_Logic_Amount) - 1];
    public bool isHit = false; // 맞았는지 판별 

    [Header("==========Effect=============")]
    public Attack_Info[] attack_Info;
    public CMoveComponent movecom;

    [Header("==========AI=================")]
    public AI real_AI;

    public void Stat_Initialize(MonsterInformation info) // 몬스터 생성 시 몬스터 정보 초기화
    {
        //        st = ScriptableObject.CreateInstance<MonsterInformation>();
        //die_Delay = info.P_dieDelay;
        //drop_Reward = info.P_drop_Reward;
        //mon_attack_Power = info.P_mon_Atk;
        //balance_gauge = info.P_mon_Balance;
        //Armor = info.P_mon_Def;
        //enemy_Grade = (Enemy_Grade)info.P_mon_Default;
        //// index = int.Parse(info.P_mon_Index);
        //Max_HP = info.P_mon_MaxHP;
        //Cur_HP = info.P_mon_MaxHP;
        //move_Speed = info.P_mon_moveSpeed;
        //Character_Name = info.P_mon_nameKor;
        //enemy_Type = (Enemy_Type)info.P_mon_Type;
    }

    protected void Initalize()
    {
        return_Pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        destination_Pos = transform.position;

        attack_Collider = GetComponentInChildren<Enemy_Weapon>()?.gameObject;

        animator = GetComponentInChildren<AnimationController>();
        eventsystem = GetComponentInChildren<AnimationEventSystem>();
        //movecom = GetComponentInChildren<CMoveComponent>();

        real_AI.AI_Init(this);

        for (int i = 0; i < attack_Info.Length; i++)
        {
            eventsystem.AddEvent
                (new KeyValuePair<string, AnimationEventSystem.beginCallback>(attack_Info[i].Name, Animation_Begin),
                new KeyValuePair<string, AnimationEventSystem.midCallback>(attack_Info[i].Name, Animation_Middle),
                new KeyValuePair<string, AnimationEventSystem.endCallback>(attack_Info[i].Name, Animation_End));
        }

        Skill_Rand();
    }


    public virtual void Damaged(float damage_Amount)
    {
        // Cur_HP -= (damage_Amount - Armor);
        //  MyHpbar.Curhp = Cur_HP;
        //MyHpbar.hit();
        isHit = true;

        Debug.Log("아악");
    }

    public void Skill_Rand()
    {
        int rand = Random.Range(0, mon_Skill_Info.Count);

        //now_Skill_Info = mon_Skill_Info[rand];
    }

    public virtual void Attack_Effect(GameObject obj) // 때릴 시 넉백 등 효과.
    {

    }

    public void Animation_Begin(string clipname)
    {
        Debug.Log("ㅋ은ㅁ은맏아");
        for (int i = 0; i < attack_Info.Length; i++)
        {
            if (attack_Info[i].Name == clipname)
            {
                //movecom.FowardDoMove(5, animator.GetClipLength(attackinfos[AttackNum].aniclip.name) / 2);
                //movecom.FowardDoMove(attack_Info[i].Movedis[0], attack_Info[i].MoveTime[0]);

                if (attack_Info[i].effect != null)
                {
                    GameObject effectobj = GameObject.Instantiate(attack_Info[i].effect[0]);
                    effectobj.transform.position = attack_Info[i].effect_Pos[0].position;
                    effectobj.transform.rotation = attack_Info[i].effect_Pos[0].rotation;

                    //preparent = effectobj.transform.parent;
                    effectobj.transform.parent = attack_Info[i].effect_Pos[0];
                    //copyobj.transform.TransformDirection(movecom.com.FpRoot.forward);


                    Destroy(effectobj, 1.5f);
                }

                return;
            }
        }
    }

    public void Animation_Middle(string clipname)
    {
        for (int i = 0; i < attack_Info.Length; i++)
        {
            if (attack_Info[i].Name == clipname)
            {
                //movecom.FowardDoMove(5, animator.GetClipLength(attackinfos[AttackNum].aniclip.name) / 2);
                //movecom.FowardDoMove(attack_Info[i].Movedis[1], attack_Info[i].MoveTime[1]);

                if (attack_Info[i].effect != null)
                {
                    GameObject effectobj = GameObject.Instantiate(attack_Info[i].effect[1]);
                    effectobj.transform.position = attack_Info[i].effect_Pos[1].position;
                    effectobj.transform.rotation = attack_Info[i].effect_Pos[1].rotation;

                    //preparent = effectobj.transform.parent;
                    effectobj.transform.parent = attack_Info[i].effect_Pos[1];
                    //copyobj.transform.TransformDirection(movecom.com.FpRoot.forward);


                    Destroy(effectobj, 1.5f);
                }
                return;
            }
        }
    }

    public void Animation_End(string clipname)
    {
        for (int i = 0; i < attack_Info.Length; i++)
        {
            if (attack_Info[i].Name == clipname)
            {
                //movecom.FowardDoMove(5, animator.GetClipLength(attackinfos[AttackNum].aniclip.name) / 2);
                //movecom.FowardDoMove(attack_Info[i].Movedis[2], attack_Info[i].MoveTime[2]);

                if (attack_Info[i].effect != null)
                {
                    GameObject effectobj = GameObject.Instantiate(attack_Info[i].effect[2]);
                    effectobj.transform.position = attack_Info[i].effect_Pos[2].position;
                    effectobj.transform.rotation = attack_Info[i].effect_Pos[2].rotation;

                    //preparent = effectobj.transform.parent;
                    effectobj.transform.parent = attack_Info[i].effect_Pos[2];
                    //copyobj.transform.TransformDirection(movecom.com.FpRoot.forward);


                    Destroy(effectobj, 1.5f);
                }
                return;
            }
        }
    }

    private void Start()
    {
        Initalize();
        //real_AI.AI_Init(this);
    }

    private void Update()
    {
        real_AI.AI_Update();

        if (Input.GetKeyDown(KeyCode.H))
        {
            animator.Play("Melee Attack");
        }
    }
}

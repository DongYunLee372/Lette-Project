using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어블 캐릭터의 모든것을 관리한다.
//1. 컴포넌트들 관리, 기존 ComponentManager가 하던 일을 그대로 실행
//2. 플레이어 데이터를 받아와서 각각의 컴포넌트 들에게 각각 필요한 데이터들을 넘겨준다.


public class PlayableCharacter : MonoBehaviour
{
    [Header("================UnityComponent================")]
    public CharacterStateMachine statemachine;


    [Header("================BaseComponent================")]
    public BaseComponent[] components = new BaseComponent[(int)EnumTypes.eComponentTypes.comMax];

    public BaseStatus status;


    [Header("================캐릭터 UI================")]
    public UICharacterInfoPanel CharacterInfoPanel;
    //public SlideBar HPBar;
    //public SlideBar MPBar;
    //public SlideBar STMBar;
    //public SlideBar BalanceBar;
    [Header("================피격 이펙트================")]
    public GameObject HitEffect;
    public EffectManager effectmanager;

    /*싱글톤*/
    static PlayableCharacter _instance;
    public static PlayableCharacter Instance
    {
        get
        {
            return _instance;
        }
    }



    public CharacterInformation CharacterDBInfo;

    /*초기화*/
    private void Awake()
    {
        _instance = this;
    }

    /*초기화*/
    private void Start()
    {
        CharacterDBInfo = DataLoad_Save.Instance.Get_PlayerDB(EnumScp.PlayerDBIndex.Level1);
        Debug.Log($"{CharacterDBInfo.P_player_HP}");

        BaseComponent[] temp = GetComponentsInChildren<BaseComponent>();

        foreach (BaseComponent a in temp)
        {
            if(a.gameObject.activeSelf)
                components[(int)a.p_comtype] = a;
        }

        status = new BaseStatus();
        status.Init(DataLoad_Save.Instance,CharacterInfoPanel);


        
        //CharacterInfoPanel = UIManager.Instance.Prefabsload("FriendPanel", UIManager.CANVAS_NUM.player_cavas);

    }

    /*MyComponent 관련 메소드*/
    public BaseComponent GetMyComponent(EnumTypes.eComponentTypes type)
    {
        return components[(int)type];
    }

    public void InActiveMyComponent(EnumTypes.eComponentTypes type)
    {
        components[(int)type].enabled = false;
    }

    public void ActiveMyComponent(EnumTypes.eComponentTypes type)
    {
        components[(int)type].enabled = true;
    }

    public void Init()
    {

    }

    public Camera GetCamera()
    {
        CMoveComponent movecom = GetMyComponent(EnumTypes.eComponentTypes.MoveCom) as CMoveComponent;

        if (movecom.curval.IsFPP)
            return movecom.com.FpCam.GetComponent<Camera>();
        else
            return movecom.com.TpCam.GetComponent<Camera>();
    }

    /*플레이어 캐릭터 상호작용 메소드*/

    /*플에이어가 공격을 받았을때 해당 함수를 호출
      현재 플레이어의 상태에 따라서 넉백, 가드넉백, 회피 등등의 동작을 결정한다.
      공격을 당했을때 공격을 당한 위치(충돌한 위치)도 함께 넘겨준다.(피격 이펙트를 출력하기 위해)*/
    public void BeAttacked(float damage, Vector3 hitpoint)
    {
        CharacterStateMachine.eCharacterState state = CharacterStateMachine.Instance.GetState();
        

        //1. 무조건 공격이 성공하는 상태(Idle, Move, OutOfControl)
        if (state == CharacterStateMachine.eCharacterState.Idle ||
            state == CharacterStateMachine.eCharacterState.Move ||
            state == CharacterStateMachine.eCharacterState.OutOfControl)
        {
            Damaged(damage, hitpoint);
        }

        //2. 가드중 
        //밸런스게이지가 충분이 남아 있으면 가드에 성공하고 밸런스 게이지를 감소 시킨다.
        //밸런스 게이지가 충분히 남아 있지 않으면 가드에 실패하고 데미지를 입는다.
        else if(state == CharacterStateMachine.eCharacterState.Guard)
        {
            CGuardComponent guardcom = GetMyComponent(EnumTypes.eComponentTypes.GuardCom) as CGuardComponent;

            guardcom.Damaged_Guard(damage, hitpoint);
        }

        //3. 회피중
        //캐릭터가 회피중이고 무적시간일때는 공격 회피에 성공하고
        //캐릭터가 회피중이지만 무적시간이 아닐때는 회피에 실패하고 데미지를 입는다.
        else if(state == CharacterStateMachine.eCharacterState.Rolling)
        {
            CMoveComponent movecom = GetMyComponent(EnumTypes.eComponentTypes.MoveCom) as CMoveComponent;

            if(!movecom.curval.IsNoDamage)
                movecom.Damaged_Rolling(damage, hitpoint);

        }

        //4. 공격중
        else if(state == CharacterStateMachine.eCharacterState.Attack)
        {
            PlayerAttack attackcom = GetMyComponent(EnumTypes.eComponentTypes.AttackCom) as PlayerAttack;
            //CAttackComponent attackcom = GetMyComponent(EnumTypes.eComponentTypes.AttackCom) as CAttackComponent;
            attackcom.PlayerHit();
            Damaged(damage, hitpoint);
        }

    }

    
    public void Damaged(float damage,Vector3 hitpoint)
    {
        CMoveComponent movecom = GetMyComponent(EnumTypes.eComponentTypes.MoveCom) as CMoveComponent;
        EffectManager.Instance.InstantiateEffect(HitEffect, hitpoint);
        //최종 데미지 = 상대방 데미지 - 나의 현재 방어막
        float finaldamage = damage - status.Defense;
        status.CurHP -= finaldamage;

        if (finaldamage >= 80)
        {
            movecom.KnockDown();
        }
        else
        {
            movecom.KnockBack();
        }
    }

    public BaseStatus GetCharacterStatus()
    {
        return status;
    }
    
    public void GetExp(int exp)
    {
        status.CurExp += exp;
    }





    private void Update()
    {
        
    }


}

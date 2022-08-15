using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//레벨이 변할때마다 캐릭터 스탯 정보들을 받아와서 초기화 해준다.
public class BaseStatus:MonoBehaviour
{
    [Header("=========================")]
    [Header("Status")]
    [SerializeField]
    private int curLevel;
    [SerializeField]
    private float maxHP;
    [SerializeField]
    private float curHP;
    [SerializeField]
    private float maxStamina;
    [SerializeField]
    private float curStamina;
    [SerializeField]
    private float damage;//공격력
    [SerializeField]
    private float defense;//방어력
    [SerializeField]
    private float maxBalance;
    [SerializeField]
    private float curBalance;
    [SerializeField]
    private float maxMP;
    [SerializeField]
    private float curMP;
    [SerializeField]
    private int curExp;
    [SerializeField]
    private int nextExp;


    [SerializeField]
    public CharacterInformation CharacterDBInfo;
    [SerializeField]
    private DataLoad_Save DBController;
    private UICharacterInfoPanel uiPanel;

    //IEnumerator [] cor

    public int CurLevel
    {
        get
        {
            return curLevel;
        }
        set
        {
            curLevel = value;
            if (curLevel == 1)
            {
                CharacterDBInfo = DBController.Get_PlayerDB(EnumScp.PlayerDBIndex.Level1);
                MaxHP = CharacterDBInfo.P_player_HP;
                MaxStamina = CharacterDBInfo.P_player_Stamina;
                MaxBalance = CharacterDBInfo.P_player_Balance;
                MaxMP = CharacterDBInfo.P_player_MP;

            }
        }
    }
    public float MaxHP
    {
        get => maxHP;
        set
        {
            maxHP = value;
            uiPanel.HPBar.SetMaxValue(value);
            CurHP = maxHP;
        }
    }

    public float CurHP
    {
        get => curHP;
        set
        {
            curHP = value;
            uiPanel.HPBar.SetCurValue(value);
            Debug.Log($"현재 HP 변화 {curHP}");
        }
    }

    

    public float MaxStamina
    {
        get => maxStamina;
        set
        {
            maxStamina = value;
            uiPanel.Staminabar.SetMaxValue(value);
            CurStamina = maxStamina;
        }
    }
    public float CurStamina
    {
        get => curStamina;
        
        set
        {
            
            curStamina = value;
            uiPanel.Staminabar.SetCurValue(value);
            Debug.Log($"현재 stamina 변화 {curStamina}");
        }
        
    }
    public float MaxBalance 
    { 
        get => maxBalance;
        set
        {
            maxBalance = value;
            uiPanel.Balancebar.SetMaxValue(value);
            CurBalance = maxBalance;
        }
    }
    public float CurBalance 
    { 
        get => curBalance;
        set
        {
            curBalance = value;
            uiPanel.Balancebar.SetCurValue(value);
            Debug.Log($"현재 Balance 변화 {curBalance}");
        }

    }
    public float MaxMP 
    { 
        get => maxMP;
        set
        {
            maxMP = value;
            uiPanel.MPBar.SetMaxValue(value);
            CurMP = maxMP;
        }
    }
    public float CurMP 
    { 
        get => curMP;
        set
        {
            curMP = value;
            uiPanel.MPBar.SetCurValue(value);
            Debug.Log($"현재 MP 변화 {curMP}");
        }

    }
    public int CurExp 
    { 
        get => curExp; 
        set => curExp = value; 
    }
    public int NextExp 
    { 
        get => nextExp; 
        set => nextExp = value; 
    }
    public float Damage 
    { 
        get => damage; 
        set => damage = value; 
    }
    public float Defense 
    { 
        get => defense; 
        set => defense = value; 
    }

    public void Init(DataLoad_Save DBController, UICharacterInfoPanel uipanel)
    {
        this.DBController = DBController;
        this.uiPanel = uipanel;
        CurLevel = 1;
    }

    IEnumerator Recovery()
    {
        while(true)
        {



            yield return new WaitForSeconds(Time.deltaTime);
        }
    }


    private void Update()
    {
        
    }


}

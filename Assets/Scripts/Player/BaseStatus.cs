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
    //[SerializeField]
    //private float maxBalance;
    //[SerializeField]
    //private float curBalance;
    [SerializeField]
    private float maxMP;
    [SerializeField]
    private float curMP;
    [SerializeField]
    private int curExp;
    [SerializeField]
    private int nextExp;
    [SerializeField]
    private float maxGroggy;
    [SerializeField]
    private float curGroggy;
    [SerializeField]
    private float testtesttest;
    [SerializeField]
    public CharacterInformation CharacterDBInfo;
    public Dictionary<string, CharacterInformation> CharacterDBInfoDic;

    [SerializeField]
    //private DataLoad_Save DBController;
    private UICharacterInfoPanel uiPanel;

    CorTimeCounter timecounter = new CorTimeCounter();
    delegate bool invoker(float val);
    IEnumerator CorMPCount;
    IEnumerator CorMPRecover;

    IEnumerator CorSTMCount;
    IEnumerator CorSTMRecover;

    IEnumerator CorBALCount;
    IEnumerator CorBALRecover;

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
                //CharacterDBInfo = DBController.Get_PlayerDB(Global_Variable.CharVar.Asha);
                LoadFile.Read<CharacterInformation>(out CharacterDBInfoDic);
                MaxHP = CharacterDBInfoDic[Global_Variable.CharVar.Asha].P_player_HP;
                MaxStamina = CharacterDBInfoDic[Global_Variable.CharVar.Asha].P_player_Stamina;
                testtesttest = CharacterDBInfoDic[Global_Variable.CharVar.Asha].P_player_RotSpeed;
                //MaxBalance = CharacterDBInfoDic[Global_Variable.CharVar.Asha].P_player_Balance;
                //MaxMP = CharacterDBInfoDic[Global_Variable.CharVar.Asha].P_player_MP;

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
            if (curHP > MaxHP)
            {
                curHP = MaxHP;
            }
            uiPanel.HPBar.SetCurValue(value);
        }
    }

    public bool HPUp(float val)
    {
        CurHP = CurHP + val;
        if (CurHP == MaxHP)
        {
            return false;
        }
        return true;
    }


    public float MaxGroggy
    {
        get => maxGroggy;
        set
        {
            maxGroggy = value;
            //uiPanel.HPBar.SetMaxValue(value);
            CurGroggy = maxGroggy;
        }
    }

    public float CurGroggy
    {
        get => curGroggy;
        set
        {
            curGroggy = value;
            if (curGroggy > MaxGroggy)
            {
                curGroggy = MaxGroggy;
            }
            //uiPanel.HPBar.SetCurValue(value);
        }
    }

    public bool GroggyUp(float val)
    {
        CurGroggy = CurGroggy + val;
        if (CurGroggy == MaxGroggy)
        {
            return false;
        }
        return true;
    }

    public bool GroggyDown(float val)
    {
        CurGroggy = CurGroggy + val;
        if (CurGroggy == 0)
        {
            return false;
        }
        return true;
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
            if(curStamina>value)
            {
                if (CorSTMCount != null)
                {
                    StopCoroutine(CorSTMCount);
                    CorSTMCount = null;
                }
                if (CorSTMRecover != null)
                {
                    StopCoroutine(CorSTMRecover);
                    CorSTMRecover = null;
                }

            }
            

            curStamina = value;

            if (curStamina > MaxStamina)
            {
                curStamina = MaxStamina;
            }
            uiPanel.Staminabar.SetCurValue(curStamina);

            if (curStamina != MaxStamina && CorSTMCount == null)
            {
                CorSTMCount = timecounter.Cor_TimeCounter(3.0f, STMRecoveryStart);
                StartCoroutine(CorSTMCount);
            }
                
        }
    }
    public bool StaminaUp(float val)
    {
        CurStamina = CurStamina + val;
        if (CurStamina == MaxStamina)
        {
            return false;
        }
        return true;
    }

    //public float MaxBalance 
    //{ 
    //    get => maxBalance;
    //    set
    //    {
    //        maxBalance = value;
    //        uiPanel.Balancebar.SetMaxValue(value);
    //        CurBalance = maxBalance;
    //    }
    //}
    //public float CurBalance 
    //{ 
    //    get => curBalance;
    //    set
    //    {
    //        if (value > curBalance)
    //        {
    //            if (CorBALCount != null)
    //            {
    //                StopCoroutine(CorBALCount);
    //                CorBALCount = null;
    //            }
    //            if (CorBALRecover != null)
    //            {
    //                StopCoroutine(CorBALRecover);
    //                CorBALRecover = null;
    //            }
    //        }


    //        curBalance = value;
    //        if(curBalance>MaxBalance)
    //        {
    //            curBalance = MaxBalance;
    //        }
    //        uiPanel.Balancebar.SetCurValue(curBalance);

    //        if (curBalance != MaxBalance && CorBALCount == null)
    //        {
    //            CorBALCount = timecounter.Cor_TimeCounter(3.0f, BALRecoveryStart);
    //            StartCoroutine(CorBALCount);
    //        }
    //    }

    //}

    //public bool BalanceUp(float val)
    //{
    //    CurBalance = CurBalance + val;
    //    if(CurBalance==MaxBalance)
    //    {
    //        return false;
    //    }
    //    return true;
    //}


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
            if(value> curMP)
            {
                if (CorMPCount != null)
                {
                    StopCoroutine(CorMPCount);
                    CorMPCount = null;
                }
                if (CorMPRecover != null)
                {
                    StopCoroutine(CorMPRecover);
                    CorMPRecover = null;
                }
            }

            curMP = value;
            if(curMP>=MaxMP)
            {
                curMP = MaxMP;
            }
            uiPanel.MPBar.SetCurValue(curMP);

            if (curMP != MaxMP && CorMPCount == null)
            {
                CorMPCount = timecounter.Cor_TimeCounter(3.0f, MPRecoveryStart);
                StartCoroutine(CorMPCount);
            }
        }

    }

    public bool MPUp(float val)
    {
        CurMP = CurMP + val;
        if (CurMP >= MaxMP)
        {
            return false;
        }
        return true;
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

    public void Init(UICharacterInfoPanel uipanel)
    {
        //this.DBController = DBController;
        this.uiPanel = uipanel;
        CurLevel = 1;
    }


    public void MPRecoveryStart()
    {
        Debug.Log("MPrecover 시작");
        StopCoroutine(CorMPCount);
        CorMPCount = null;
        
        CorMPRecover = Recovery(MPUp);
        StartCoroutine(CorMPRecover);
    }

    public void STMRecoveryStart()
    {
        Debug.Log("Staminarecover 시작");
        StopCoroutine(CorSTMCount);
        CorSTMCount = null;

        CorSTMRecover = Recovery(StaminaUp);
        StartCoroutine(CorSTMRecover);
    }

    //public void BALRecoveryStart()
    //{
    //    Debug.Log("Balancerecover 시작");
    //    StopCoroutine(CorBALCount);
    //    CorBALCount = null;

    //    CorBALRecover = Recovery(BalanceUp);
    //    StartCoroutine(CorBALRecover);
    //}

    IEnumerator Recovery(invoker _invoker)
    {

        while(true)
        {
            Debug.Log("10회복");

            if (!_invoker(10))
            {
                Debug.Log("10회복 종료");
                yield break;
            }
                

            yield return new WaitForSeconds(1.0f);
        }
    }




}

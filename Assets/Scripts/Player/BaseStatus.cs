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
    public float Damage;//공격력
    [SerializeField]
    public float Defense;//방어력
    //[SerializeField]
    //private float maxBalance;
    //[SerializeField]
    //private float curBalance;
    [SerializeField]
    private float maxMP;
    [SerializeField]
    private float curMP;
    [SerializeField]
    public int CurExp;
    [SerializeField]
    public int NextExp;
    [SerializeField]
    private float maxGroggy;
    [SerializeField]
    private float curGroggy;


    [SerializeField]
    public CharacterInformation DBInfo;
    public Dictionary<string, CharacterInformation> CharacterDBInfoDic;

    [SerializeField]
    //private DataLoad_Save DBController;
    private UICharacterInfoPanel uiPanel;

    CorTimeCounter timecounter = new CorTimeCounter();
    delegate bool invoker(float val);
    

    IEnumerator CorSTMCount;
    IEnumerator CorSTMRecover;

    IEnumerator CorGroggyCount;
    IEnumerator CorGroggyRecover;

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
                //csv로부터 캐릭터의 정보들을 받아온다.
                LoadFile.Read<CharacterInformation>(out CharacterDBInfoDic);
                DBInfo = CharacterDBInfoDic[Global_Variable.CharVar.Asha];

                //각각의 필요한 값들을 필요한 곳에 넣어준다.
                MaxHP = DBInfo.P_player_HP;
                MaxStamina = DBInfo.P_player_Stamina;
                Defense = DBInfo.P_player_Def;
                MaxGroggy = DBInfo.P_player_Groggy;
                CurGroggy = 0;
                CMoveComponent movecom = PlayableCharacter.Instance.GetMyComponent(CharEnumTypes.eComponentTypes.MoveCom) as CMoveComponent;

                movecom.moveoption.MoveSpeed = DBInfo.P_player_MoveSpeed;
                movecom.moveoption.RunSpeed = DBInfo.P_player_RunSpeed;
                movecom.moveoption.RotSpeed = DBInfo.P_player_RotSpeed;
                movecom.moveoption.RotMouseSpeed = DBInfo.P_player_MouseSpeed;

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
            if (curHP < 0)
            {
                curHP = 0;
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

    public bool HPDown(float val)
    {
        CurHP = CurHP - val;
        if (CurHP == 0)
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
            //CurGroggy = maxGroggy;
        }
    }

    public float CurGroggy
    {
        get => curGroggy;
        set
        {
            //현재 스테미나보다 변경될 값이 클때
            if (curGroggy < value)
            {
                //돌고있는 카운트가 있거나 이미 회복중이면 중단해준다.
                if (CorGroggyCount != null)
                {
                    StopCoroutine(CorGroggyCount);
                    CorGroggyCount = null;
                }
                if (CorGroggyRecover != null)
                {
                    StopCoroutine(CorGroggyRecover);
                    CorGroggyRecover = null;
                }

            }


            curGroggy = value;
            if (curGroggy > MaxGroggy)
            {
                curGroggy = MaxGroggy;
            }
            if(curGroggy<0)
            {
                curGroggy = 0;
            }
            //uiPanel.HPBar.SetCurValue(value);


            //
            if (curGroggy != 0 && CorGroggyCount == null && CorGroggyRecover == null)
            {
                CorGroggyCount = timecounter.Cor_TimeCounter(DBInfo.P_player_Groggy_Recovery_Time, GroggyRecoveryStart);
                StartCoroutine(CorGroggyCount);
            }


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
        CurGroggy = CurGroggy - val;
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
            //현재 스테미나보다 변경될 값이 작을때
            if(curStamina>value)
            {
                //돌고있는 카운트가 있거나 이미 회복중이면 중단해준다.
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
            
            //값을 변경해주고
            curStamina = value;

            if (curStamina > MaxStamina)
            {
                curStamina = MaxStamina;
            }
            if (curStamina < 0)
            {
                curStamina = 0;
            }

            //ui에 반영시켜준다.
            uiPanel.Staminabar.SetCurValue(curStamina);

            //모든 값이 변경된 뒤에 stamina가 최대치가 아니고 이미 회복중이 아니거나 카운트가 돌고있는 중이 아니면 회복을 위한 카운터를 시작해준다. 
            if (curStamina != MaxStamina && CorSTMCount == null&&CorSTMRecover==null)
            {
                CorSTMCount = timecounter.Cor_TimeCounter(DBInfo.P_player_Stamina_Recovery_Time, STMRecoveryStart);
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

    public bool StaminaDown(float val)
    {
        CurStamina = CurStamina - val;
        if (CurStamina == 0)
        {
            return false;
        }
        return true;
    }





    public void Init(UICharacterInfoPanel uipanel)
    {
        //this.DBController = DBController;
        this.uiPanel = uipanel;
        CurLevel = 1;
    }


    

    public void STMRecoveryStart()
    {
        Debug.Log("Staminarecover 시작");
        StopCoroutine(CorSTMCount);
        CorSTMCount = null;

        CorSTMRecover = Recovery(StaminaUp, DBInfo.P_player_Stamina_Recovery_Val);
        StartCoroutine(CorSTMRecover);
    }

    public void GroggyRecoveryStart()
    {
        Debug.Log("그로기 회복 시작");
        StopCoroutine(CorSTMCount);
        CorSTMCount = null;

        CorSTMRecover = Recovery(GroggyDown, DBInfo.P_player_Groggy_Recovery_Val);
        StartCoroutine(CorSTMRecover);
    }

    IEnumerator Recovery(invoker _invoker,float val)
    {

        while(true)
        {
            Debug.Log($"{val}회복");

            if (!_invoker(val))
            {
                Debug.Log("회복 종료");
                yield break;
            }
                

            yield return new WaitForSeconds(1.0f);
        }
    }

    //public void MPRecoveryStart()
    //{
    //    Debug.Log("MPrecover 시작");
    //    StopCoroutine(CorMPCount);
    //    CorMPCount = null;

    //    CorMPRecover = Recovery(MPUp,DBInfo.);
    //    StartCoroutine(CorMPRecover);
    //}


    //public float MaxMP 
    //{ 
    //    get => maxMP;
    //    set
    //    {
    //        maxMP = value;
    //        uiPanel.MPBar.SetMaxValue(value);
    //        CurMP = maxMP;
    //    }
    //}
    //public float CurMP 
    //{ 
    //    get => curMP;
    //    set
    //    {
    //        if(value> curMP)
    //        {
    //            if (CorMPCount != null)
    //            {
    //                StopCoroutine(CorMPCount);
    //                CorMPCount = null;
    //            }
    //            if (CorMPRecover != null)
    //            {
    //                StopCoroutine(CorMPRecover);
    //                CorMPRecover = null;
    //            }
    //        }

    //        curMP = value;
    //        if(curMP>=MaxMP)
    //        {
    //            curMP = MaxMP;
    //        }
    //        uiPanel.MPBar.SetCurValue(curMP);

    //        if (curMP != MaxMP && CorMPCount == null)
    //        {
    //            CorMPCount = timecounter.Cor_TimeCounter(3.0f, MPRecoveryStart);
    //            StartCoroutine(CorMPCount);
    //        }
    //    }

    //}

    //public bool MPUp(float val)
    //{
    //    CurMP = CurMP + val;
    //    if (CurMP >= MaxMP)
    //    {
    //        return false;
    //    }
    //    return true;
    //}

    //public void BALRecoveryStart()
    //{
    //    Debug.Log("Balancerecover 시작");
    //    StopCoroutine(CorBALCount);
    //    CorBALCount = null;

    //    CorBALRecover = Recovery(BalanceUp);
    //    StartCoroutine(CorBALRecover);
    //}

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
}

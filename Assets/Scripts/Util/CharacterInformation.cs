using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Data", menuName = "Scriptable Object/Player Data", order = int.MaxValue)]

public class CharacterInformation : Data
{
    //캐릭터 이름
    [SerializeField]
    private string character_Name;
    public string Character_Name { get { return character_Name; } set { character_Name = value; } }

    //캐릭터 hp 총량
    [SerializeField]
    private float player_HP;
    public float P_player_HP { get { return player_HP; } set { player_HP = value; } }


    //캐릭터 방어력
    [SerializeField]
    private float player_Def;
    public float P_player_Def { get { return player_Def; } set { player_Def = value; } }


    //캐릭터 Stamina 총량
    [SerializeField]
    private float player_Stamina;
    public float P_player_Stamina { get { return player_Stamina; } set { player_Stamina = value; } }

    // Stamina 자동회복 시간 
    [SerializeField]
    private float player_Stamina_Recovery_Time;
    public float P_player_Stamina_Recovery_Time { get { return player_Stamina_Recovery_Time; } set { player_Stamina_Recovery_Time = value; } }

    // Stamina 자동회복 값
    [SerializeField]
    private float player_Stamina_Recovery_Val;
    public float P_player_Stamina_Recovery_Val { get { return player_Stamina_Recovery_Val; } set { player_Stamina_Recovery_Val = value; } }

    //그로기값 최대치
    [SerializeField]
    private float player_Groggy;
    public float P_player_Groggy { get { return player_Groggy; } set { player_Groggy = value; } }

    // 그로기값 자동회복 시간 
    [SerializeField]
    private float player_Groggy_Recovery_Time;
    public float P_player_Groggy_Recovery_Time { get { return player_Groggy_Recovery_Time; } set { player_Groggy_Recovery_Time = value; } }

    // 그로기값 자동회복 값
    [SerializeField]
    private float player_Groggy_Recovery_Val;
    public float P_player_Groggy_Recovery_Val { get { return player_Groggy_Recovery_Val; } set { player_Groggy_Recovery_Val = value; } }


    //경직 상태에 빠지는 그로기값
    [SerializeField]
    private float player_Stagger_Groggy;
    public float P_player_Stagger_Groggy { get { return player_Stagger_Groggy; } set { player_Stagger_Groggy = value; } }

    //다운 상태에 빠지는 그로기값
    [SerializeField]
    private float player_Down_Groggy;
    public float P_player_Down_Groggy { get { return player_Down_Groggy; } set { player_Down_Groggy = value; } }

    //캐릭터 움직임 속도
    [SerializeField]
    private float player_MoveSpeed;
    public float P_player_MoveSpeed { get { return player_MoveSpeed; } set { player_MoveSpeed = value; } }

    //캐릭터 움직임 속도
    [SerializeField]
    private float player_RunSpeed;
    public float P_player_RunSpeed { get { return player_RunSpeed; } set { player_RunSpeed = value; } }

    [SerializeField]
    private float player_MouseSpeed;
    public float P_player_MouseSpeed { get { return player_MouseSpeed; } set { player_MouseSpeed = value; } }

    [SerializeField]
    private float player_RotSpeed;
    public float P_player_RotSpeed { get { return player_RotSpeed; } set { player_RotSpeed = value; } }


    /*/////////////////////////////////////////////////////////////////////////////////////////////////////////////
      /////////////////////////////////////////////////////////////////////////////////////////////////////////////
      /////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
    



    ////캐릭터 mp 총량
    //[SerializeField]
    //private int player_MP;
    //public int P_player_MP { get { return player_MP; } set { player_MP = value; } }

    //// hp 자동회복 시간 
    //[SerializeField]
    //private int player_HP_Recovery_Time;
    //public int P_player_HP_Recovery_Time { get { return player_HP_Recovery_Time; } set { player_HP_Recovery_Time = value; } }

    //// hp 자동회복 값
    //[SerializeField]
    //private int player_HP_Recovery_Val;
    //public int P_player_HP_Recovery_Val { get { return player_HP_Recovery_Val; } set { player_HP_Recovery_Val = value; } }

    //[SerializeField]
    //private int player_Balance;
    //public int P_player_Balance { get { return player_Balance; } set { player_Balance = value; } }

    //공격에 대한 정보는 따로 관리
    //[SerializeField]
    //private int player_Atk1;
    //public int P_player_Atk1 { get { return player_Atk1; } set { player_Atk1 = value; } }

    //[SerializeField]
    //private int player_Stadown1;
    //public int P_player_Stadown1 { get { return player_Stadown1; } set { player_Stadown1 = value; } }

    //[SerializeField]
    //private int player_MPup1;
    //public int P_player_MPup1 { get { return player_MPup1; } set { player_MPup1 = value; } }

    //[SerializeField]
    //private int player_BalDown1;
    //public int P_player_BalDown1 { get { return player_BalDown1; } set { player_BalDown1 = value; } }

    //[SerializeField]
    //private int player_Atk2;
    //public int P_player_Atk2 { get { return player_Atk2; } set { player_Atk2 = value; } }

    //[SerializeField]
    //private int player_Stadown2;
    //public int P_player_Stadown2 { get { return player_Stadown2; } set { player_Stadown2 = value; } }

    //[SerializeField]
    //private int player_MPup2;
    //public int P_player_MPup2 { get { return player_MPup2; } set { player_MPup2 = value; } }

    //[SerializeField]
    //private int player_BalDown2;
    //public int P_player_BalDown2 { get { return player_BalDown2; } set { player_BalDown2 = value; } }

    //[SerializeField]
    //private int player_Atk3;
    //public int P_player_Atk3 { get { return player_Atk3; } set { player_Atk3 = value; } }

    //[SerializeField]
    //private int player_Stadown3;
    //public int P_player_Stadown3 { get { return player_Stadown3; } set { player_Stadown3 = value; } }

    //[SerializeField]
    //private int player_MPup3;
    //public int P_player_MPup3 { get { return player_MPup3; } set { player_MPup3 = value; } }

    //[SerializeField]
    //private int player_BalDown3;
    //public int P_player_BalDown3 { get { return player_BalDown3; } set { player_BalDown3 = value; } }


    //public void set(int hp, int def, int mp, int stamina, int mspeed, int balance, int atk1, int stadown1, int mpup1, int baldown1, int atk2, int stadown2, int mpup2, int baldown2, int atk3, int stadown3, int mpup3, int baldown3)
    //{
    //    player_HP = hp;
    //    player_Def = def;
    //    player_MP = mp;
    //    player_Stamina = stamina;
    //    player_mSpeed = mspeed;
    //    player_Balance = balance;
    //    player_Atk1 = atk1;
    //    player_Stadown1 = stadown1;
    //    player_MPup1 = mpup1;
    //    player_BalDown1 = baldown1;
    //    player_Atk2 = atk2;
    //    player_Stadown2 = stadown2;
    //    player_MPup2 = mpup2;
    //    player_BalDown2 = baldown2;
    //    player_Atk3 = atk3;
    //    player_Stadown3 = stadown3;
    //    player_MPup3 = mpup3;
    //    player_BalDown3 = baldown3;
    //}




}

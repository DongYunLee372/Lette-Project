using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ ���Ҷ����� ĳ���� ���� �������� �޾ƿͼ� �ʱ�ȭ ���ش�.
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
    private float damage;//���ݷ�
    [SerializeField]
    private float defense;//����
    [SerializeField]
    private float maxBalance;
    [SerializeField]
    private float curBalance;
    [SerializeField]
    private float maxMP;
    [SerializeField]
    private float curMP;


    [SerializeField]
    private CharacterInformation CharacterDBInfo;
    [SerializeField]
    private DataLoad_Save DBController;


    public int CurLevel 
    { 
        get
        {
            return curLevel;
        }
        set
        {
            curLevel = value;
            if(curLevel==1)
            {
                CharacterDBInfo = DBController.Get_PlayerDB(EnumScp.PlayerDBIndex.Level1);
            }
        }
    }
    public float MaxHP { get => maxHP; set => maxHP = value; }
    public float CurHP { get => curHP; set => curHP = value; }
    public float Damage { get => damage; set => damage = value; }
    public float Defense { get => defense; set => defense = value; }
    public float MaxStamina { get => maxStamina; set => maxStamina = value; }
    public float CurStamina { get => curStamina; set => curStamina = value; }
    public float MaxBalance { get => maxBalance; set => maxBalance = value; }
    public float CurBalance { get => curBalance; set => curBalance = value; }
    public float MaxMP { get => maxMP; set => maxMP = value; }
    public float CurMP { get => curMP; set => curMP = value; }

    private void Awake()
    {
        CharacterDBInfo = ScriptableObject.CreateInstance<CharacterInformation>();



    }

    //db���� ĳ���� �����͸� �޾ƿ´�.
    public virtual void Start()
    {
        CurLevel = 1;
    }

}

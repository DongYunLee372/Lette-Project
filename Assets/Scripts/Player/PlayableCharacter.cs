using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//플레이어블 캐릭터의 모든것을 관리한다.
//1. 컴포넌트들 관리, 기존 ComponentManager가 하던 일을 그대로 실행
//2. 플레이어 데이터를 받아와서 각각의 컴포넌트 들에게 각각 필요한 데이터들을 넘겨준다.
public class PlayableCharacter : MonoBehaviour
{
    [Header("================UnityComponent================")]
    public CharacterStateMachine statemachine;


    [Header("================BaseComponent================")]
    public BaseComponent[] components = new BaseComponent[(int)CharEnumTypes.eComponentTypes.comMax];

    [SerializeField]
    public BaseStatus status;


    [Header("================캐릭터 UI================")]
    public UICharacterInfoPanel CharacterUIPanel;
    public InvenTory inventory;

    [Header("================피격 이펙트================")]
    public GameObject HitEffect;
    public string HitEffectAdressableName;
    public EffectManager effectmanager;

    CMoveComponent movecom;

    /*싱글톤*/
    static PlayableCharacter _instance;
    public static PlayableCharacter Instance
    {
        get
        {
            return _instance;
        }
    }

    /*초기화*/
    private void Awake()
    {
        _instance = this;
    }

    public bool ComponentInit()
    {
        BaseComponent[] temp = GetComponentsInChildren<BaseComponent>();
        status = GetComponent<BaseStatus>();


        foreach (BaseComponent a in temp)
        {
            if (a.gameObject.activeSelf)
                components[(int)a.p_comtype] = a;
        }

        if (components[1] == null)
            return false;

        return true;
    }



    /*초기화*/
    private void Start()
    {
        //yield return new WaitForSeconds(0.01f);

        //CharacterDBInfo = DataLoad_Save.Instance.Get_PlayerDB(Global_Variable.CharVar.Asha);
        //Debug.Log($"{CharacterDBInfo.P_player_HP}");

        ComponentInit();


        movecom = GetMyComponent(CharEnumTypes.eComponentTypes.MoveCom) as CMoveComponent;

        //만약 연결되어 있는 UI가 없는 경우 UI객체를 로드해서 생성시켜 준다.
        if (CharacterUIPanel == null)
        {
            CharacterUIPanel = UIManager.Instance.Prefabsload(Global_Variable.CharVar.CharacterUI, Canvas_Enum.CANVAS_NUM.player_cavas).GetComponent<UICharacterInfoPanel>();
        }
        if(inventory == null)
        {
            GameObject obj = UIManager.Instance.Prefabsload("Inven", Canvas_Enum.CANVAS_NUM.player_cavas);
            inventory = obj.GetComponent<InvenTory>();
        }

        status.Init(CharacterUIPanel);

        //Canvas canvas = FindObjectOfType<Canvas>("Playercanvas");
        //Canvas canvas = GameObject.Find("Playercanvas");
        //if (canvas == null)
        //{
        //    GameObject obj = new GameObject("playercanvas");
        //    obj.AddComponent<Canvas>();
        //    canvas = obj.GetComponent<Canvas>();
        //}

        
        CharacterUIPanel.transform.localPosition = status.player_UIPos;


        if (CharacterUIPanel == null)
        {
            Debug.Log("character UI Create Fail");
        }

        GameObject tempui = UIManager.Instance.Canvasreturn(Canvas_Enum.CANVAS_NUM.start_canvas);
        MainOption mainoption = tempui.GetComponent<MainOption>();
        mainoption.r_invoker = SetReverseMouseRot;
        mainoption.a_invoker = SetCameraColl;
        mainoption.l_invoker = SetOutoFocus;
        mainoption.m_invoker = SetMouseSpeed;


        SetOutoFocus(mainoption.LooKon);
        SetMouseSpeed(mainoption.MouseSensetive);
        SetReverseMouseRot(mainoption.ReverseMouse);
        SetCameraColl(mainoption.AutoeVade);
    }

    public void SetOutoFocus(bool val)
    {
        OutoFocus = val;
    }

    public void SetMouseSpeed(float val)
    {
        //CMoveComponent movecom = GetMyComponent(CharEnumTypes.eComponentTypes.MoveCom) as CMoveComponent;
        //0~100의 값을 0~5의 값으로 변환해서 넣어준다.
        val = val * 5 * 0.01f;
        movecom.moveoption.RotMouseSpeed = val;
    }

    public void SetReverseMouseRot(bool val)
    {
        movecom.moveoption.RightReverse = val;
    }

    public void SetCameraColl(bool val)
    {
        movecom.CameraCollOn = val;
    }

    public void CeateUI(GameObject obj)
    {
        CharacterUIPanel = GameObject.Instantiate(obj).GetComponent<UICharacterInfoPanel>();
    }

    /*MyComponent 관련 메소드*/
    public BaseComponent GetMyComponent(CharEnumTypes.eComponentTypes type)
    {
        if(components[(int)type] ==null)
        {
            ComponentInit();
        }

        return components[(int)type];
    }

    public void InActiveMyComponent(CharEnumTypes.eComponentTypes type)
    {
        if (components[(int)type] == null)
        {
            ComponentInit();
        }

        components[(int)type].enabled = false;
    }

    public void ActiveMyComponent(CharEnumTypes.eComponentTypes type)
    {
        if (components[(int)type] == null)
        {
            ComponentInit();
        }

        components[(int)type].enabled = true;
    }

    public Camera GetCamera()
    {
        //CMoveComponent movecom = GetMyComponent(CharEnumTypes.eComponentTypes.MoveCom) as CMoveComponent;
        if(movecom==null)
            movecom = GetMyComponent(CharEnumTypes.eComponentTypes.MoveCom) as CMoveComponent;

        return movecom.GetCamera();
    }

    /*플레이어 캐릭터 상호작용 메소드*/

    /*플에이어가 공격을 받았을때 해당 함수를 호출
      현재 플레이어의 상태에 따라서 넉백, 가드넉백, 회피 등등의 동작을 결정한다.
      공격을 당했을때 공격을 당한 위치(충돌한 위치)도 함께 넘겨준다.(피격 이펙트를 출력하기 위해)*/
    public void BeAttacked(float damage, Vector3 hitpoint, float Groggy)
    {
        CharacterStateMachine.eCharacterState state = CharacterStateMachine.Instance.GetState();

        //float Groggy = 0;

        //1. 무조건 공격이 성공하는 상태(Idle, Move, OutOfControl)
        if (state == CharacterStateMachine.eCharacterState.Idle ||
            state == CharacterStateMachine.eCharacterState.Move ||
            state == CharacterStateMachine.eCharacterState.OutOfControl)
        {
            Damaged(damage, hitpoint, Groggy);
        }

        //2. 가드중 
        //밸런스게이지가 충분이 남아 있으면 가드에 성공하고 밸런스 게이지를 감소 시킨다.
        //밸런스 게이지가 충분히 남아 있지 않으면 가드에 실패하고 데미지를 입는다.
        else if(state == CharacterStateMachine.eCharacterState.Guard)
        {
            CGuardComponent guardcom = GetMyComponent(CharEnumTypes.eComponentTypes.GuardCom) as CGuardComponent;

            guardcom.Damaged_Guard(damage, hitpoint,Groggy);
        }

        //3. 회피중
        //캐릭터가 회피중이고 무적시간일때는 공격 회피에 성공하고
        //캐릭터가 회피중이지만 무적시간이 아닐때는 회피에 실패하고 데미지를 입는다.
        else if(state == CharacterStateMachine.eCharacterState.Rolling)
        {
            //CMoveComponent movecom = GetMyComponent(CharEnumTypes.eComponentTypes.MoveCom) as CMoveComponent;

            if(!movecom.curval.IsNoDamage)
                movecom.Damaged_Rolling(damage, hitpoint,Groggy);

        }

        //4. 공격중
        else if(state == CharacterStateMachine.eCharacterState.Attack)
        {
            PlayerAttack attackcom = GetMyComponent(CharEnumTypes.eComponentTypes.AttackCom) as PlayerAttack;
            //CAttackComponent attackcom = GetMyComponent(CharEnumTypes.eComponentTypes.AttackCom) as CAttackComponent;
            attackcom.PlayerHit();
            Damaged(damage, hitpoint, Groggy);
        }

    }

    
    public void Damaged(float damage,Vector3 hitpoint, float Groggy)
    {
        //CMoveComponent movecom = GetMyComponent(CharEnumTypes.eComponentTypes.MoveCom) as CMoveComponent;
        EffectManager.Instance.InstantiateEffect(HitEffectAdressableName, hitpoint);
        //최종 데미지 = 상대방 데미지 - 나의 현재 방어막
        float finaldamage = damage - status.Defense;
        
        status.GroggyUp(Groggy);
        status.CurHP -= finaldamage;
        //SoundManager.Instance.effectSource.GetComponent<AudioSource>().PlayOneShot(SoundManager.Instance.Player_Audio[2]);

        //캐릭터 사망
        //사망 애니메이션 출력하고 씬 재시작 함수 호출
        if (status.CurHP<=0)
        {
            CharacterStateMachine.Instance.SetState(CharacterStateMachine.eCharacterState.OutOfControl);
            movecom.eventsystem.AddEvent(new KeyValuePair<string, AnimationEventSystem.beginCallback>(null, null),
                             new KeyValuePair<string, AnimationEventSystem.midCallback>(null, null),
                             new KeyValuePair<string, AnimationEventSystem.endCallback>("_Dead", Restart));

            movecom.com.animator.Play("_Dead");    

        }

    }


    public void Restart(string _val)
    {
        CharacterStateMachine.Instance.SetState(CharacterStateMachine.eCharacterState.Idle);
        GameData_Load.Instance.ChangeScene(Scenes_Stage.restart_Loading);
    }

    public BaseStatus GetCharacterStatus()
    {
        return status;
    }
    
    public void GetExp(int exp)
    {
        status.CurExp += exp;
    }

    //적 탐색 & 포커싱 세팅
    [System.Serializable]
    public class Battle_Character_Info
    {
        public Battle_Character_Info(Battle_Character monster)
        {
            _monster = monster;
            _distance = 0;
            _index = -1;
            _isFocused = false;
            _isBlocked = false;
        }

        public Battle_Character _monster;
        public float _distance;
        public int _index;
        public bool _isFocused;
        public bool _isBlocked;
    }

    public bool _outoFocus = false;
    public bool OutoFocus
    {
        get
        {
            return _outoFocus;
        }
        set
        {
            _outoFocus = value;
            if (value)
            {
                if (MonsterSearchCor==null)
                {
                    MonsterSearchCor = MonsterSearchCoroutine();
                    StartCoroutine(MonsterSearchCor);
                }
                FocusTab();
            }

        }
    }

    public List<Battle_Character_Info> _monsterObject = new List<Battle_Character_Info>();
    public float _monsterSearchTime = 3.0f;
    private float lastsearchTime;

    public int CurMonsterIndex = 0;

    public bool IsFocusingOn = false;
    public int CurFocusedIndex = -1;
    public Battle_Character_Info CurFocusedMonster;
    public IEnumerator MonsterSearchCor = null;

    public LayerMask Bosslayer;

    //public bool OutoFoucusing = false;

    //일정 시간마다 화면에 있는 몬스터들을 확인해서 거리별로 리스트에 넣는다.
    public IEnumerator MonsterSearchCoroutine()
    {
        Battle_Character[] temp;
        List<Battle_Character_Info> tempViewMonster = new List<Battle_Character_Info>();
        
        RaycastHit hit;
        while (true)
        {
            //Debug.Log("[focus]몬스터 탐지 시작");
            tempViewMonster.Clear();

            temp = FindObjectsOfType<Battle_Character>();

            //해당 몬스터가 카메라 안에 있는지 확인
            for (int i = 0; i < temp.Length; i++)
            {
                Vector3 screenPos = GetCamera().WorldToViewportPoint(temp[i].gameObject.transform.position);
                if (screenPos.x >= 0 && screenPos.x <= 1 && screenPos.y >= 0 && screenPos.y <= 1 && screenPos.z >= 0)
                {
                    //Debug.Log(temp[i].gameObject.name + "[focus]화면에 탐지");
                    Battle_Character_Info info = new Battle_Character_Info(temp[i]);
                    tempViewMonster.Add(info);
                }
            }

            //카메라 안에 있으면 해당 물체로 ray를 쏴서 장애물이 있는지과 거리를 확인한다.
            for (int i = 0; i < tempViewMonster.Count; i++)
            {
                Vector3 dir = tempViewMonster[i]._monster.gameObject.transform.position - transform.position;
                if (Physics.Raycast(transform.position, dir, out hit, 100.0f, Bosslayer))
                {
                    //if(hit.transform.gameObject.layer)
                    //if(!hit.transform.CompareTag("Enemy"))
                    if (hit.collider == null)
                    {
                        //Debug.Log("[focus]몬스터 탐색 지워져버림");
                        tempViewMonster.RemoveAt(i);
                        //tempViewMonster[i]._isBlocked = true;
                        //tempViewMonster[i]._distance = 0;
                        continue;
                    }
                    else
                    {
                        //Debug.Log("[focus]몬스터 탐색 안지워짐");
                        tempViewMonster[i]._distance = hit.distance;
                    }
                }
            }

            //거리에 따라 정렬
            _monsterObject = tempViewMonster.OrderBy(x => x._distance).ToList();

            //
            if(_monsterObject.Count<=0)
            {
                IsFocusingOn = false;
                CurFocusedIndex = 0;
                CurFocusedMonster = null;
                yield break;
            }

            //탕색과 정렬을 끝냈는데 현재 포커싱 중인 몬스터가 사라졌으면 포커싱을 끝내준다.
            if(IsFocusingOn)
            {
                int index = _monsterObject.FindIndex(x => x == CurFocusedMonster);
                //탐색을 완료 했는데 포커싱 중인 몬스터가 없어졌을때
                if (index == -1)
                {
                    //오토 포커싱 중이면 다른 몬스터가 있으면 그 몬스터로 포커싱을 옮겨주고
                    //아무것도 없으면 그때 끝내준다.
                    if(OutoFocus)
                    {
                        if(_monsterObject.Count>0)
                        {
                            CurFocusedIndex = 0;
                            yield return null;
                        }
                        else
                        {
                            IsFocusingOn = false;
                        }
                    }
                    else
                    {
                        //Debug.Log("[focus]탐색결과 몬스터 존재 X");
                        IsFocusingOn = false;
                        CurFocusedIndex = 0;
                        CurFocusedMonster = null;
                        yield break;
                    }

                }
                else
                {
                    CurFocusedIndex = index;
                }
            }




            yield return new WaitForSeconds(_monsterSearchTime);
        }

        //MonsterSearchCor = null;
          
    }

    public void FocusTab()
    {

        if(MonsterSearchCor==null)
        {
            MonsterSearchCor = MonsterSearchCoroutine();
            StartCoroutine(MonsterSearchCor);
        }


        if(!IsFocusingOn)
        {
            if (_monsterObject.Count > 0)
            {
                
                IsFocusingOn = true;
                CurFocusedIndex = 0;
                CurFocusedMonster = _monsterObject[0];
                //Debug.Log(CurFocusedMonster._monster.gameObject.name + "포커싱 시작");
            }
        }
        else
        {
            if(!OutoFocus)
            {
                if (CurFocusedIndex == _monsterObject.Count - 1)
                {
                    //Debug.Log("[focus]포커싱 꺼짐");
                    IsFocusingOn = false;
                    StopCoroutine(MonsterSearchCor);
                    MonsterSearchCor = null;
                }
            }
            
                
            CurFocusedIndex = (CurFocusedIndex + 1) % _monsterObject.Count;
            CurFocusedMonster = _monsterObject[CurFocusedIndex];


        }


    }



}

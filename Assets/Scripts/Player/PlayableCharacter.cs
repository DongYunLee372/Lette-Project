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

    [Header("================피격 이펙트================")]
    public GameObject HitEffect;
    public string HitEffectAdressableName;
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

    /*초기화*/
    private void Awake()
    {
        _instance = this;
    }

    /*초기화*/
    private void Start()
    {
        //CharacterDBInfo = DataLoad_Save.Instance.Get_PlayerDB(Global_Variable.CharVar.Asha);
        //Debug.Log($"{CharacterDBInfo.P_player_HP}");

        BaseComponent[] temp = GetComponentsInChildren<BaseComponent>();
        status = GetComponent<BaseStatus>();


        foreach (BaseComponent a in temp)
        {
            if(a.gameObject.activeSelf)
                components[(int)a.p_comtype] = a;
        }


        //CharacterInfoPanel = UIManager.Instance.Prefabsload(Global_Variable.CharVar.CharacterUIPanel, UIManager.CANVAS_NUM.player_cavas).GetComponent<UICharacterInfoPanel>();

        //만약 연결되어 있는 UI가 없는 경우 UI객체를 로드해서 생성시켜 준다.
        if(CharacterUIPanel==null)
            AddressablesLoadManager.Instance.SingleAsset_Load<GameObject>(Global_Variable.CharVar.CharacterUI, false, true, CeateUI);


        status.Init(CharacterUIPanel);
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject obj = new GameObject("playercanvas");
            obj.AddComponent<Canvas>();
            canvas = obj.GetComponent<Canvas>();
        }

        CharacterUIPanel.transform.parent = canvas.transform;
        CharacterUIPanel.transform.localPosition = status.player_UIPos;
        

        if (CharacterUIPanel != null)
        {
            
            Debug.Log("UI 로드 성공");
        }
        else
            Debug.Log("character UI Create Fail");

        //StartCoroutine(MonsterSearchCoroutine());
    }

    public void CeateUI(GameObject obj)
    {
        CharacterUIPanel = GameObject.Instantiate(obj).GetComponent<UICharacterInfoPanel>();
    }

    /*MyComponent 관련 메소드*/
    public BaseComponent GetMyComponent(CharEnumTypes.eComponentTypes type)
    {
        return components[(int)type];
    }

    public void InActiveMyComponent(CharEnumTypes.eComponentTypes type)
    {
        components[(int)type].enabled = false;
    }

    public void ActiveMyComponent(CharEnumTypes.eComponentTypes type)
    {
        components[(int)type].enabled = true;
    }

    public void Init()
    {

    }

    public Camera GetCamera()
    {
        CMoveComponent movecom = GetMyComponent(CharEnumTypes.eComponentTypes.MoveCom) as CMoveComponent;

        if (movecom.curval.IsFPP)
            return movecom.com.FpCam.GetComponent<Camera>();
        else
            return movecom.com.TpCam.GetComponent<Camera>();
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
            CMoveComponent movecom = GetMyComponent(CharEnumTypes.eComponentTypes.MoveCom) as CMoveComponent;

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
        CMoveComponent movecom = GetMyComponent(CharEnumTypes.eComponentTypes.MoveCom) as CMoveComponent;
        EffectManager.Instance.InstantiateEffect(HitEffectAdressableName, hitpoint);
        //최종 데미지 = 상대방 데미지 - 나의 현재 방어막
        float finaldamage = damage - status.Defense;
        
        status.GroggyUp(Groggy);
        status.CurHP -= finaldamage;


        //캐릭터 사망
        //사망 애니메이션 출력하고 씬 재시작 함수 호출
        if(status.CurHP<=0)
        {
            movecom.eventsystem.AddEvent(new KeyValuePair<string, AnimationEventSystem.beginCallback>(null, null),
                             new KeyValuePair<string, AnimationEventSystem.midCallback>(null, null),
                             new KeyValuePair<string, AnimationEventSystem.endCallback>("_Dead", Restart));
            movecom.com.animator.Play("_Dead");    

        }

    }


    public void Restart(string _val)
    {
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


    public List<Battle_Character_Info> _monsterObject = new List<Battle_Character_Info>();
    public float _monsterSearchTime = 3.0f;
    private float lastsearchTime;

    public int CurMonsterIndex = 0;

    public bool IsFocusingOn = false;
    public int CurFocusedIndex = -1;
    public Battle_Character_Info CurFocusedMonster;
    public IEnumerator MonsterSearchCor = null;

    public LayerMask Bosslayer;

    //일정 시간마다 화면에 있는 몬스터들을 확인해서 거리별로 리스트에 넣는다.
    public IEnumerator MonsterSearchCoroutine()
    {
        Battle_Character[] temp;
        List<Battle_Character_Info> tempViewMonster = new List<Battle_Character_Info>();
        
        RaycastHit hit;
        while (true)
        {
            Debug.Log("몬스터 탐지 시작");
            tempViewMonster.Clear();

            temp = FindObjectsOfType<Battle_Character>();

            //해당 몬스터가 카메라 안에 있는지 확인
            for (int i = 0; i < temp.Length; i++)
            {
                Vector3 screenPos = GetCamera().WorldToViewportPoint(temp[i].gameObject.transform.position);
                if (screenPos.x >= 0 && screenPos.x <= 1 && screenPos.y >= 0 && screenPos.y <= 1 && screenPos.z >= 0)
                {
                    Debug.Log(temp[i].gameObject.name + "화면에 탐지");
                    Battle_Character_Info info = new Battle_Character_Info(temp[i]);
                    tempViewMonster.Add(info);
                }
            }

            //카메라 안에 있으면 해당 물체로 ray를 쏴서 장애물이 있는지과 거리를 확인한다.
            for (int i = 0; i < tempViewMonster.Count; i++)
            {
                Vector3 dir = tempViewMonster[i]._monster.gameObject.transform.position - transform.position;
                if(Physics.Raycast(transform.position, dir, out hit,100.0f,Bosslayer))
                {
                    //if(hit.transform.gameObject.layer)
                    //if(!hit.transform.CompareTag("Enemy"))
                    if(hit.collider==null)
                    {
                        Debug.Log("몬스터 탐색 지워져버림");
                        tempViewMonster.RemoveAt(i);
                        //tempViewMonster[i]._isBlocked = true;
                        //tempViewMonster[i]._distance = 0;
                        continue;
                    }
                    else
                    {
                        Debug.Log("몬스터 탐색 안지워짐");
                        tempViewMonster[i]._distance = hit.distance;
                    }
                }
            }

            //거리에 따라 정렬
            _monsterObject = tempViewMonster.OrderBy(x => x._distance).ToList();

            //탕색과 정렬을 끝냈는데 현재 포커싱 중인 몬스터가 사라졌으면 포커싱을 끝내준다.
            if(IsFocusingOn)
            {
                //_monsterObject.Find(x => x == CurFocusedMonster);
                int index = _monsterObject.FindIndex(x => x == CurFocusedMonster);
                //탐색을 완료 했는데 포커싱 중인 몬스터가 없어졌을때
                if (index != -1)
                {
                    Debug.Log("탐색결과 몬스터 존재 X");
                    IsFocusingOn = false;
                    CurFocusedIndex = 0;
                    CurFocusedMonster = null;
                }
                else
                {
                    CurFocusedIndex = index;
                }
            }

            yield return new WaitForSeconds(_monsterSearchTime);
        }
    }

    public void FocusTab()
    {
        Debug.Log("탭 들어옴");
        if(MonsterSearchCor!=null)
            StopCoroutine(MonsterSearchCor);


        MonsterSearchCor = MonsterSearchCoroutine();
        StartCoroutine(MonsterSearchCor);

        if(!IsFocusingOn)
        {
            if (_monsterObject.Count > 0)
            {
                
                IsFocusingOn = true;
                CurFocusedIndex = 0;
                CurFocusedMonster = _monsterObject[0];
                Debug.Log(CurFocusedMonster._monster.gameObject.name + "포커싱 시작");
            }
        }
        else
        {
            CurFocusedIndex = (CurFocusedIndex + 1) % _monsterObject.Count;
            CurFocusedMonster = _monsterObject[CurFocusedIndex];
        }


    }

}

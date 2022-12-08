using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillPanelUI : MonoBehaviour
{
    [SerializeField]
    private SkillSlot[] _slotUIList;
    //[SerializeField]
    //private List<SkillSlot> _slotUIList = new List<SkillSlot>();
    [SerializeField]
    private List<PlayerSkillSlot> _playerSkillSlotList = new List<PlayerSkillSlot>();
    [SerializeField]
    private List<SkillData> _skilldataList = new List<SkillData>();

    [SerializeField]
    private GraphicRaycaster gr;
    [SerializeField]
    private PointerEventData _event;
    [SerializeField]
    private List<RaycastResult> _raylist; //레이캐스트 결과를 담을 리스트

    public void setSlot(PlayerSkillSlot slot)
    {
        _playerSkillSlotList.Add(slot);
    }
    private void Awake()
    {
        

        TryGetComponent(out gr);
        if (gr == null)
            gr = gameObject.AddComponent<GraphicRaycaster>();

        // Graphic Raycaster
        _event = new PointerEventData(EventSystem.current);
        _raylist = new List<RaycastResult>(10);
    }
    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            _slotUIList = GetComponentsInChildren<SkillSlot>();
            _slotUIList[i].SetSlot(_skilldataList[i].Icon);
            _slotUIList[i].Index = i;
        }
       // _playerSkillSlotList[0].SetPlayerSkillSlotOn(_skilldataList[0]);

    }

    private T RaycastGetComponent<T>() where T : Component
    {
        _raylist.Clear(); // 레이캐스트 결과를 담을 리스트를 초기화
        gr.Raycast(_event, _raylist);

        //만일 아무런 ui도 없다면
        if (_raylist.Count == 0)
            return null;

        return _raylist[0].gameObject.GetComponent<T>(); //가장 앞의 ui의 컴포넌트 반환
    }

    private void OnPointerDown()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SkillSlot slot = RaycastGetComponent<SkillSlot>();
            
            if (slot != null)
            {
                for(int i=2;i>=0;i--)
                {
                    if(!_playerSkillSlotList[i].HasItem)
                    {
                        _playerSkillSlotList[i].SetPlayerSkillSlotOn(_skilldataList[slot.Index]);
                        //여기서 플레이어한테 데이터 넘겨줌
                        return;
                    }
                }
            }

            
        }

    }

    private void Update()
    {
        _event.position = Input.mousePosition;
        OnPointerDown();
    }
}

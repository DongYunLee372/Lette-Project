using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class EquipmentUI : MonoBehaviour
{
    [SerializeField]
    private List<EquipmentSlot> _slotUIList = new List<EquipmentSlot>();
    private int EquipmentNum = 1;
    public PlayableCharacter Player;

    [SerializeField]
    private GraphicRaycaster gr;
    [SerializeField]
    private PointerEventData _event;
    [SerializeField]
    private List<RaycastResult> _raylist; //레이캐스트 결과를 담을 리스트

    [SerializeField]
    private InventoryCompo _inventory;
    void Start()
    {
        for (int i = 0; i < EquipmentNum; i++)
        {
            _slotUIList.Add(GetComponentInChildren<EquipmentSlot>());
        }

        TryGetComponent(out gr);
        if (gr == null)
            gr = gameObject.AddComponent<GraphicRaycaster>();

        // Graphic Raycaster
        _event = new PointerEventData(EventSystem.current);
        _raylist = new List<RaycastResult>(10);
    }
    private void Update()
    {
        _event.position = Input.mousePosition;
        OnPointerDown();

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
            EquipmentSlot slot = RaycastGetComponent<EquipmentSlot>();
            

            if (slot != null && slot.HasItem)
            {
                Debug.Log("if");
                _inventory.Add(slot.item.Data, 1);
                slot.EndSlot();
            }
            else
            {
                Debug.Log("else");
            }
        }

    }
    public void ItemMounting(int data , Item2 Itemdata) //여기서 플레이어한테 공격력 부여
    {
        //if (Player == null)
        //    Player = PlayableCharacter.Instance;

        //Debug.Log(Player.name);

        //Player.status.HPUp(50);

        _slotUIList[0].SetSlot(Itemdata.Data.Icon , Itemdata);


    }


}

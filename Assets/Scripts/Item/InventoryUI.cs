using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{

    // ************************************************************** //
    // 슬롯과 관련된 필드들
    [Header("Options")]
    [Range(0, 10)]
    [SerializeField]
    private int _horizontalSlotCount; //슬롯 가로 개수

    [Range(0, 10)]
    [SerializeField] 
    private int _verticalSlotCount; //슬롯 세로 개수
    [SerializeField] 
    private float _slotMargin = 8f; //슬롯 상하좌우 여백
    [SerializeField] 
    private float _contentAreaPadding = 20f; //인벤토리 영역 내부 여백

    [Range(32, 64)]
    [SerializeField]
    private float _slotSize = 64f; //슬롯의 크기

    [Header("Objects")]
    [SerializeField]
    private RectTransform _contentAreaRT; //슬롯들이 위치할 영역
    [SerializeField]
    private GameObject _slotUiPrefab; //슬롯 원본 프리팹
    [SerializeField]
    private List<ItemSlotUI> _slotUIList = new List<ItemSlotUI>();
    // ************************************************************** //

    //Graphic Raycaster는 캔버스 안을 검색하는 Raycaster
    //EventSystem 이벤트를 검출하는 수단으로 사용
    [SerializeField]
    private GraphicRaycaster gr;
    [SerializeField]
    private PointerEventData _event;
    [SerializeField]
    private List<RaycastResult> _raylist; //레이캐스트 결과를 담을 리스트
    [SerializeField]
    private ItemSlotUI _beginDragSlot; //드래그 시작 슬롯
    private Transform _beginDragSlotIconTransform; //드래그 시작 슬롯 아이콘 트랜스폼

    private Vector3 _beginDragIconPos; //드래그 시작 시 슬롯 위치
    private Vector3 _beginDragCursorPos; // 드래그 시작 시 커서 위치
    private int _beginDragSlotSiblingIndex;

    [SerializeField]
    private  InventoryCompo _inventory;

    //지정된 개수만큼 슬롯 영역 내 슬롯들 생성
    public void InitSlots()
    {
        _slotUiPrefab.TryGetComponent(out RectTransform slotRect); 
        slotRect.sizeDelta = new Vector2(_slotSize, _slotSize); //만들 슬롯의 사이즈를 정해준 64의 크기로 지정

        _slotUiPrefab.TryGetComponent(out ItemSlotUI itemSlot);
        if (itemSlot == null)
            _slotUiPrefab.AddComponent<ItemSlotUI>();
        _slotUiPrefab.SetActive(false);

        Vector2 beginPos = new Vector2(_contentAreaPadding, -_contentAreaPadding);
        Vector2 curPos = beginPos;

        _slotUIList = new List<ItemSlotUI>(_verticalSlotCount * _horizontalSlotCount);


        for (int j = 0; j < _verticalSlotCount; j++) 
        {
            for (int i = 0; i < _horizontalSlotCount; i++) 
            {
                int slotIndex = (_horizontalSlotCount * j) + i;

                var slotRT = CloneSlot();
                slotRT.pivot = new Vector2(0f, 1f);
                slotRT.anchoredPosition = curPos;
                slotRT.gameObject.SetActive(true);
                slotRT.gameObject.name = $"Item Slot [{slotIndex}]";

                var slotUI = slotRT.GetComponent<ItemSlotUI>();
                slotUI.SetSlotIndex(slotIndex);
                _slotUIList.Add(slotUI);

                curPos.x += (_slotMargin + _slotSize);
                
            }
            curPos.x = beginPos.x;
            curPos.y -= (_slotMargin + _slotSize);
        }

        if (_slotUiPrefab.scene.rootCount != 0)
        {
            Destroy(_slotUiPrefab);
            Debug.Log(_slotUiPrefab.scene.rootCount);
        }
            


        RectTransform CloneSlot()
        {
            GameObject slotGo = Instantiate(_slotUiPrefab);
            RectTransform rt = slotGo.GetComponent<RectTransform>();
            rt.SetParent(_contentAreaRT);

            return rt;
        }
    }
    private void Init()
    {
        TryGetComponent(out gr);
        if (gr == null)
            gr = gameObject.AddComponent<GraphicRaycaster>();

        // Graphic Raycaster
        _event = new PointerEventData(EventSystem.current);
        _raylist = new List<RaycastResult>(10);

    }
    // 무언가를 클릭했을때 그곳으로 레이캐스트를 발사해서 어떤 ui인지 컴포넌트로 파악하여
    // 반환해 접근할 수 있게끔 함 

    private T RaycastGetComponent<T>() where T : Component
    {
        // 레이캐스트 결과를 담을 리스트를 초기화
        _raylist.Clear();
        
        gr.Raycast(_event, _raylist);
        
        Debug.Log(_raylist.Count);
        //만일 아무런 ui도 없다면
        if (_raylist.Count == 0)
            return null;

        for(int i=0;i<_raylist.Count;i++)
        {
            if(_raylist[i].gameObject.GetComponent<T>())
            {
                Debug.Log("오");
                return _raylist[0].gameObject.GetComponent<T>();
            }
                
        }

        return null; //가장 앞의 ui의 컴포넌트 반환
    }

    // 인벤토리 내 슬롯 아이템의 드래그 클릭등을 UnityEngine.EventSystems 안의
    // 인터페이스를 사용하면 각 슬롯마다 스크립트로 구현해야하므로 인벤토리 클래스에서
    // 직접 구현
    private void OnPointerDown()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //Debug.Log("다운");
            _beginDragSlot = RaycastGetComponent<ItemSlotUI>();

            //아이템을 가진 슬롯만 가능
            if (_beginDragSlot != null && _beginDragSlot.HasItem)
            {
                //Debug.Log("합격");
                // 첫 클릭 슬롯 트랜스 폼
                _beginDragSlotIconTransform = _beginDragSlot.IconRect.transform;
                // 첫 클릭 슬롯 위치
                _beginDragIconPos = _beginDragSlotIconTransform.position;
                // 첫 클릭 마우스 위치
                _beginDragCursorPos = Input.mousePosition;

                // 드래그 중인 아이템 UI 가장 위에 띄우기
                _beginDragSlotSiblingIndex = _beginDragSlot.transform.GetSiblingIndex();
                _beginDragSlot.transform.SetAsLastSibling();


            }
            else _beginDragSlot = null;

        }

        if(Input.GetMouseButtonDown(1))
        {
            
            ItemSlotUI slot = RaycastGetComponent<ItemSlotUI>();
            
            //Debug.Log(_event.position);
            //Debug.Log(slot);
            //Debug.Log(slot.HasItem);
            //Debug.Log(slot.IsAccessible);

            if (slot != null && slot.HasItem && slot.IsAccessible)
            {
                //Debug.Log("사용");
                TryUseItem(slot.Index);
            }
            else Debug.Log("뭐지");
        }
       
    }

    private void OnpointerDrag()
    {
        if (_beginDragSlot == null)
            return;

        if(Input.GetMouseButton(0))
        {
            // 마우스 따라서 아이템 슬롯안의 아이템 아이콘 이미지 위치를 이동
            _beginDragSlotIconTransform.position = _beginDragIconPos + (Input.mousePosition - _beginDragCursorPos);
        }
    }

    private void OnPointerUp()
    {
        if(Input.GetMouseButtonUp(0))
        {

            if(_beginDragSlot != null)
            {
                // 위치 복원
                _beginDragSlotIconTransform.position = _beginDragIconPos;

                // UI 순서 복원
                _beginDragSlot.transform.SetSiblingIndex(_beginDragSlotSiblingIndex);

                DragEnd();

                //초기화하기
                _beginDragSlot = null;
                _beginDragSlotIconTransform = null;
            }
        }
    }

    private void DragEnd()
    {
        ItemSlotUI endDragSlot = RaycastGetComponent<ItemSlotUI>();

        // 아이템 슬롯끼리 아이콘 교환 또는 이동
        if (endDragSlot != null && endDragSlot.IsAccessible)
        {
            // 수량 나누기 조건
            // 1) 마우스 클릭 떼는 순간 좌측 Ctrl 또는 Shift 키 유지
            // 2) begin : 셀 수 있는 아이템 / end : 비어있는 슬롯
            // 3) begin 아이템의 수량 > 1
            //bool isSeparatable =
            //    (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftShift)) &&
            //    (_inventory.IsCountableItem(_beginDragSlot.Index) && !_inventory.HasItem(endDragSlot.Index));

            //// true : 수량 나누기, false : 교환 또는 이동
            //bool isSeparation = false;
            //int currentAmount = 0;

            //// 현재 개수 확인
            //if (isSeparatable)
            //{
            //    currentAmount = _inventory.GetCurrentAmount(_beginDragSlot.Index);
            //    if (currentAmount > 1)
            //    {
            //        isSeparation = true;
            //    }
            //}

            // 1. 개수 나누기
            //if (isSeparation)
            //    TrySeparateAmount(_beginDragSlot.Index, endDragSlot.Index, currentAmount);
            //// 2. 교환 또는 이동
            //else
                TrySwapItems(_beginDragSlot, endDragSlot);

            // 툴팁 갱신
            //UpdateTooltipUI(endDragSlot);
            return;
        }

        //// 버리기(커서가 UI 레이캐스트 타겟 위에 있지 않은 경우)
        //if (!IsOverUI())
        //{
        //    // 확인 팝업 띄우고 콜백 위임
        //    int index = _beginDragSlot.Index;
        //    string itemName = _inventory.GetItemName(index);
        //    int amount = _inventory.GetCurrentAmount(index);

        //    // 셀 수 있는 아이템의 경우, 수량 표시
        //    if (amount > 1)
        //        itemName += $" x{amount}";

        //    if (_showRemovingPopup)
        //        _popup.OpenConfirmationPopup(() => TryRemoveItem(index), itemName);
        //    else
        //        TryRemoveItem(index);
        //}
        //// 슬롯이 아닌 다른 UI 위에 놓은 경우
        //else
        //{
        //    EditorLog($"Drag End(Do Nothing)");
        //}
    }

    public void TryUseItem(int index)
    {
        Debug.Log("TryUseItem");
        _inventory.Use(index);
    }

    private void TrySwapItems(ItemSlotUI from, ItemSlotUI to)
    {
        if (from == to)
        {
            //EditorLog($"UI - Try Swap Items: Same Slot [{from.Index}]");
            return;
        }

        //EditorLog($"UI - Try Swap Items: Slot [{from.Index} -> {to.Index}]");

        from.SwapOrMoveIcon(to);
        _inventory.Swap(from.Index, to.Index);
    }
    public void SetAccessibleSlotRange(int accessibleSlotCount)
    {
        for (int i = 0; i < _slotUIList.Count; i++)
        {
            _slotUIList[i].SetSlotAccessibleState(i < accessibleSlotCount);
        }
    }
    public void SetInventoryReference(InventoryCompo inventory)
    {
        _inventory = inventory;
    }
    public void SetItemIcon(int index, Sprite icon)
    {
        //EditorLog($"Set Item Icon : Slot [{index}]");
        //Debug.Log("SetItemIcon");
        _slotUIList[index].SetItem(icon);
    }

    //  해당 슬롯의 아이템 개수 텍스트 지정 
    public void SetItemAmountText(int index, int amount)
    {
        //EditorLog($"Set Item Amount Text : Slot [{index}], Amount [{amount}]");

        // NOTE : amount가 1 이하일 경우 텍스트 미표시
        _slotUIList[index].SetItemAmount(amount);
    }
    public void HideItemAmountText(int index)
    {
        //EditorLog($"Hide Item Amount Text : Slot [{index}]");

        _slotUIList[index].SetItemAmount(1);
    }

    // 슬롯에서 아이템 아이콘 제거, 개수 텍스트 숨기기 
    public void RemoveItem(int index)
    {
        _slotUIList[index].RemoveItem();
    }

    
    
    private void Awake()
    {
        Init();
        InitSlots();
        //gameObject.SetActive(false);
        //StartCoroutine(qwe());
    }

    private void Update()
    {
        
        _event.position = Input.mousePosition;
        OnPointerDown();
        OnpointerDrag();
        OnPointerUp();

        
    }

}

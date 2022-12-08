using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCompo : MonoBehaviour
{
    /// <summary> 아이템 수용 한도 </summary>
    public int Capacity { get; private set; }

    [SerializeField]
    private EquipmentUI _equipmentUI;

    [SerializeField]
    public Item2 temp;

    // 초기 수용 한도
    [SerializeField, Range(8, 64)]
    private int _initalCapacity = 32;

    // 최대 수용 한도(아이템 배열 크기)
    [SerializeField, Range(8, 64)]
    private int _maxCapacity = 64;

    [SerializeField]
    private InventoryUI _inventoryUI; // 연결된 인벤토리 UI

    /// <summary> 아이템 목록 </summary>
    [SerializeField]
    private Item2[] _items;

    private void Awake()
    {
        _items = new Item2[_maxCapacity];
        Capacity = _initalCapacity;
        _inventoryUI.SetInventoryReference(this);
    }

    private void Start()
    {
        UpdateAccessibleStatesAll();
    }
    
    public void Use(int index)
    {
        
        if (_items[index] == null) return;

        // 사용 가능한 아이템인 경우
        if (_items[index] is IUsableItem uItem)
        {

            if (_items[index] is EquipmentItem eItem)
            {
                _equipmentUI.ItemMounting(uItem.E_Use() , _items[index]);
                _inventoryUI.RemoveItem(index);
                return;
            }
            
            // 아이템 사용
            bool succeeded = uItem.Use();

            if (succeeded)
            {
                UpdateSlot(index);
            }
        }
    }

    // 인덱스가 수용 범위 내에 있는지 검사 
    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < Capacity;
    }

    // 앞에서부터 비어있는 슬롯 인덱스 탐색 
    private int FindEmptySlotIndex(int startIndex = 0)
    {
        for (int i = startIndex; i < Capacity; i++)
            if (_items[i] == null)
                return i;
        return -1;
    }


    //모든 슬롯 UI에 접근 가능 여부 업데이트 
    public void UpdateAccessibleStatesAll()
    {
        _inventoryUI.SetAccessibleSlotRange(Capacity);
    }

    //해당 슬롯이 아이템을 갖고 있는지 여부 
    public bool HasItem(int index)
    {
        return IsValidIndex(index) && _items[index] != null;
    }

    // 해당 슬롯이 셀 수 있는 아이템인지 여부 
    public bool IsCountableItem(int index)
    {
        return HasItem(index) && _items[index] is CountableItem;
    }

    
    // 해당 슬롯의 현재 아이템 개수 리턴
    // <para/> - 잘못된 인덱스 : -1 리턴
    // <para/> - 빈 슬롯 : 0 리턴
    // <para/> - 셀 수 없는 아이템 : 1 리턴
    public int GetCurrentAmount(int index)
    {
        if (!IsValidIndex(index)) return -1;
        if (_items[index] == null) return 0;

        CountableItem ci = _items[index] as CountableItem;
        if (ci == null)
            return 1;

        return ci.Amount;
    }

    // 해당 슬롯의 아이템 정보 리턴 
    public ItemData GetItemData(int index)
    {
        if (!IsValidIndex(index)) return null;
        if (_items[index] == null) return null;

        return _items[index].Data;
    }

    // 해당 슬롯의 아이템 이름 리턴 
    public string GetItemName(int index)
    {
        if (!IsValidIndex(index)) return "";
        if (_items[index] == null) return "";

        return _items[index].Data.Name;
    }
    public void Swap(int indexA, int indexB)
    {
        if (!IsValidIndex(indexA)) return;
        if (!IsValidIndex(indexB)) return;

        Item2 itemA = _items[indexA];
        Item2 itemB = _items[indexB];

        // 1. 셀 수 있는 아이템이고, 동일한 아이템일 경우
        //    indexA -> indexB로 개수 합치기
        if (itemA != null && itemB != null &&
            itemA.Data == itemB.Data &&
            itemA is CountableItem ciA && itemB is CountableItem ciB)
        {
            int maxAmount = ciB.MaxAmount;
            int sum = ciA.Amount + ciB.Amount;

            if (sum <= maxAmount)
            {
                ciA.SetAmount(0);
                ciB.SetAmount(sum);
            }
            else
            {
                ciA.SetAmount(sum - maxAmount);
                ciB.SetAmount(maxAmount);
            }
        }
        // 2. 일반적인 경우 : 슬롯 교체
        else
        {
            _items[indexA] = itemB;
            _items[indexB] = itemA;
        }

        // 두 슬롯 정보 갱신
        UpdateSlot(indexA, indexB);
    }
    public void UpdateSlot(int index)
    {
        
        if (!IsValidIndex(index)) return;

        Item2 item = _items[index];

        // 1. 아이템이 슬롯에 존재하는 경우
        if (item != null)
        {
            // 아이콘 등록
            _inventoryUI.SetItemIcon(index, item.Data.Icon);

            // 1-1. 셀 수 있는 아이템
            if (item is CountableItem ci)
            {
                // 1-1-1. 수량이 0인 경우, 아이템 제거
                if (ci.IsEmpty)
                {
                    _items[index] = null;
                    RemoveIcon();
                    return;
                }
                // 1-1-2. 수량 텍스트 표시
                else
                {
                    _inventoryUI.SetItemAmountText(index, ci.Amount);
                }
            }
            // 1-2. 셀 수 없는 아이템인 경우 수량 텍스트 제거
            else
            {
                _inventoryUI.HideItemAmountText(index);
            }
        }
        // 2. 빈 슬롯인 경우 : 아이콘 제거
        else
        {
            RemoveIcon();
        }

        // 로컬 : 아이콘 제거하기
        void RemoveIcon()
        {
            _inventoryUI.RemoveItem(index);
            _inventoryUI.HideItemAmountText(index); // 수량 텍스트 숨기기
        }
    }

    private void UpdateSlot(params int[] indices)
    {
        foreach (var i in indices)
        {
            UpdateSlot(i);
        }
    }
    public int Add(ItemData itemData, int amount = 1)
    {
        Debug.Log("Add");
        int index;

        // 1. 수량이 있는 아이템
        if (itemData is CountableItemData ciData)
        {
            bool findNextCountable = true;
            index = -1;

            while (amount > 0)
            {
                // 1-1. 이미 해당 아이템이 인벤토리 내에 존재하고, 개수 여유 있는지 검사
                if (findNextCountable)
                {
                    index = FindCountableItemSlotIndex(ciData, index + 1);

                    // 개수 여유있는 기존재 슬롯이 더이상 없다고 판단될 경우, 빈 슬롯부터 탐색 시작
                    if (index == -1)
                    {
                        findNextCountable = false;
                    }
                    // 기존재 슬롯을 찾은 경우, 양 증가시키고 초과량 존재 시 amount에 초기화
                    else
                    {
                        CountableItem ci = _items[index] as CountableItem;
                        amount = ci.AddAmountAndGetExcess(amount);

                        UpdateSlot(index);
                    }
                }
                // 1-2. 빈 슬롯 탐색
                else
                {
                    index = FindEmptySlotIndex(index + 1);

                    // 빈 슬롯조차 없는 경우 종료
                    if (index == -1)
                    {
                        break;
                    }
                    // 빈 슬롯 발견 시, 슬롯에 아이템 추가 및 잉여량 계산
                    else
                    {
                        // 새로운 아이템 생성
                        CountableItem ci = ciData.CreateItem() as CountableItem;
                        ci.SetAmount(amount);

                        // 슬롯에 추가
                        _items[index] = ci;

                        // 남은 개수 계산
                        amount = (amount > ciData.MaxAmount) ? (amount - ciData.MaxAmount) : 0;

                        //Debug.Log("Add UpdateSlot");
                        UpdateSlot(index);
                    }
                }
            }
        }
        // 2. 수량이 없는 아이템
        else
        {
            // 2-1. 1개만 넣는 경우, 간단히 수행
            if (amount == 1)
            {
                index = FindEmptySlotIndex();
                if (index != -1)
                {
                    // 아이템을 생성하여 슬롯에 추가
                    _items[index] = itemData.CreateItem();
                   
                    amount = 0;

                    UpdateSlot(index);
                }
            }

            // 2-2. 2개 이상의 수량 없는 아이템을 동시에 추가하는 경우
            index = -1;
            for (; amount > 0; amount--)
            {
                // 아이템 넣은 인덱스의 다음 인덱스부터 슬롯 탐색
                index = FindEmptySlotIndex(index + 1);

                // 다 넣지 못한 경우 루프 종료
                if (index == -1)
                {
                    break;
                }

                // 아이템을 생성하여 슬롯에 추가
                _items[index] = itemData.CreateItem();
                Debug.Log(_items[index].Data.Name);
                temp = _items[index];
                UpdateSlot(index);
            }
        }

        return amount;
    }

    // 앞에서부터 개수 여유가 있는 Countable 아이템의 슬롯 인덱스 탐색
    private int FindCountableItemSlotIndex(CountableItemData target, int startIndex = 0)
    {
        for (int i = startIndex; i < Capacity; i++)
        {
            var current = _items[i];
            if (current == null)
                continue;

            // 아이템 종류 일치, 개수 여유 확인
            if (current.Data == target && current is CountableItem ci)
            {
                if (!ci.IsMax)
                    return i;
            }
        }

        return -1;
    }
}

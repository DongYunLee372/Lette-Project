using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Portion_" , menuName = "Inventory System/Item Data/Portion", order = 3)]
// 포션 아이템
public class PortionItemData : CountableItemData
{
    [SerializeField]
    private float _value; //회복량
    public float Value { get { return _value; } }

    public override Item2 CreateItem()
    {
        //return null;
        return new PortionItem(this);
    }

    
}

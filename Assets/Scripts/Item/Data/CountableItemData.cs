using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 카운트 가능한 아이템 종류
public abstract class CountableItemData : ItemData
{
    [SerializeField]
    private int _maxAmount = 99; //최대 소지 가능
    public int MaxAmount { get { return _maxAmount; } }
}

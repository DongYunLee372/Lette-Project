using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item2
{
    public ItemData Data { get; private set; }
    public Item2(ItemData data) => Data = data;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : EquipmentItem , IUsableItem
{
    public WeaponItem(WeaponItemData data) : base(data) { }

    public bool Use()
    {

        return true;
    }
}


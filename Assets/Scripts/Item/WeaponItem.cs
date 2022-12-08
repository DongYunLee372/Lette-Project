using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : EquipmentItem , IUsableItem
{
    public WeaponItemData tempData;
    public WeaponItem(WeaponItemData data) : base(data) { tempData = data; }

    public int E_Use()
    {
        return tempData.Damage;
    }

    public bool Use()
    {
        Debug.Log(tempData.Damage);
        return true;
    }
}


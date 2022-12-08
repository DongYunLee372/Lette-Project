using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenTester : MonoBehaviour
{
    public InventoryCompo _inventory;
    public InventoryUI _inventoryUI;
    public EquipmentUI _equipmentUI;

    public ItemData[] _itemDataArray;
    // Start is called before the first frame update
    void Start()
    {
        //if (_itemDataArray?.Length > 0)
        //{
        //    for (int i = 0; i < _itemDataArray.Length; i++)
        //    {
        //        _inventory.Add(_itemDataArray[i], 3);
        //    }
        //}
    }

    public void SetTestWeaponInven()
    {
        _inventory.Add(_itemDataArray[0], 5);
        _inventory.Add(_itemDataArray[1], 1);
        _inventory.Add(_itemDataArray[2], 1);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            
            for(int i=0;i<_itemDataArray.Length;i++)
            _inventory.Add(_itemDataArray[i], 3);
            
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (_inventoryUI.gameObject.activeSelf == true)
                _inventoryUI.gameObject.SetActive(false);
            else _inventoryUI.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (_equipmentUI.gameObject.activeSelf == true)
                _equipmentUI.gameObject.SetActive(false);
            else _equipmentUI.gameObject.SetActive(true);
        }
    }
}

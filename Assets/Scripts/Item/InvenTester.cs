using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenTester : MonoBehaviour
{
    public InventoryCompo _inventory;
    public InventoryUI _inventoryUI;
    public EquipmentUI _equipmentUI;
    public SkillPanelUI _skillPanelUI;


    public Vector2 invenUIPos;
    public bool invenb;

    public Vector2 _equipmentUIPos;
    public bool eqb;

    public Vector2 _skillPanelUIPos;
    public bool skillb;

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

        //_inventoryUI.gameObject.SetActive(false);
        //_equipmentUI.gameObject.SetActive(false);
        //_skillPanelUI.gameObject.SetActive(false);

        invenUIPos = _inventoryUI.transform.position;
        _equipmentUIPos = _equipmentUI.transform.position;
        _skillPanelUIPos = _skillPanelUI.transform.position;

        //_inventoryUI.transform.position = new Vector2(5000, 5000);
       // _equipmentUI.transform.position = new Vector2(5000, 5000);
        //_skillPanelUI.transform.position = new Vector2(5000, 5000);

        invenb = true;
        eqb = true;
        skillb = true;
    }

    public void SetTestWeaponInven()
    {
        _inventory.Add(_itemDataArray[0], 5);
        _inventory.Add(_itemDataArray[1], 1);
        _inventory.Add(_itemDataArray[2], 1);

      
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            _inventoryUI.TryUseItem(0);
        }
        if(Input.GetKeyDown(KeyCode.N))
        {
            
            for(int i=0;i<_itemDataArray.Length;i++)
            _inventory.Add(_itemDataArray[i], 3);
            
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!invenb)
            {
                invenb = true;
                _inventoryUI.transform.position = invenUIPos;
            }
            else
            {
                invenb = false;
                _inventoryUI.transform.position = new Vector2(5000, 5000);
            }

            
        }
        if (Input.GetKeyDown(KeyCode.L))
        {            
            
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (!eqb)
            {
                eqb = true;
                _equipmentUI.transform.position = _equipmentUIPos;
            }
            else
            {
                eqb = false;
                _equipmentUI.transform.position = new Vector2(5000, 5000);
            }

            
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (!skillb)
            {
                skillb = true;
                _skillPanelUI.transform.position = _skillPanelUIPos;
            }
            else
            {
                skillb = false;
                _skillPanelUI.transform.position = new Vector2(5000, 5000);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenTory : MySingleton<InvenTory>
{
    

    [SerializeField]
    public GameObject SlotsParent; 
    public ItemSlot[] slots;

    public Image Estus;

    public GameObject Player;
    public void UseItem(EnumScp.Key key , int num) 
    {
        slots[(int)key].MinusItem(1);
    }
    public void DropItem(Sprite image, int count, string dropitemname)
    {

        Debug.Log("드랍아이템");
        //아이템을 얻었을때 새로운 아이템이면 새로운 슬롯에 추가하고 기존의 아이템이면 기존 아이템에 +1합니다
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].P_ItemName == dropitemname)
            {
                slots[i].PlusItem(count);

                Debug.Log("Plus아이템");
                return;

            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].P_ItemName != dropitemname && slots[i].P_ItemCount == 0)
            {
                slots[i].AddItem(image, count, dropitemname);
                Debug.Log("드랍아이템");
                return;
            }
        }
    }
    void Start()
    {
        slots = SlotsParent.GetComponentsInChildren<ItemSlot>(true);
        

        for (int i = 0; i < slots.Length; i++) //인벤토리 슬롯들에게 각자 몇번 슬롯인지 저장하기 위해 코드를 부여합니다 
        {
            slots[i].P_Code = i;
        }

        DropItem(Estus.sprite, 10, "Est");
        //Player = AddressablesController.Instance.find_Asset_in_list("PlayerCharacter");
        
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("F1 ");
            UseItem(EnumScp.Key.F1, 1);
        }
    }
}

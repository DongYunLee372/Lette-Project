using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField]
    private int Code;
    public int P_Code { get { return Code; } set { Code = value; } }

    [SerializeField]
    private int ItemCount;
    public int P_ItemCount { get { return ItemCount; } set { ItemCount = value; } }

    [SerializeField]
    private string ItemName;
    public string P_ItemName { get { return ItemName; } set { ItemName = value; } }


    [SerializeField]
    private Image itemImage;

    [SerializeField]
    private Text ItemCountText;

    public GameObject TempImage_GameObj;
    public void AddItem(Sprite _item, int count, string name)
    {
        //아이템을 새롭게 얻으면 그 아이템의 이름 개수 이미지를 받아옵니다
        itemImage.sprite = _item;
        itemImage.gameObject.SetActive(true);
        ItemCount = count;
        ItemName = name;
        ItemCountText.text = ItemCount.ToString();

        
    }

   

    public bool MinusItem(int i)
    {
        if (ItemCount == 0)
            return false;
        ItemCount -= i;
        ItemCountText.text = ItemCount.ToString();

        return true;
    }

    public void PlusItem(int i)
    {
        ItemCount += i;
        ItemCountText.text = ItemCount.ToString();
    }

    
    // Start is called before the first frame update
    void Start()
    {
        ItemCount = 0;
       
        TempImage_GameObj = transform.GetChild(0).gameObject;
        itemImage = TempImage_GameObj.GetComponent<Image>();
        itemImage.gameObject.SetActive(false);

        ItemCountText = this.gameObject.GetComponentInChildren<Text>();
        ItemCountText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

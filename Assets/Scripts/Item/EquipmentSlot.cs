using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    [Tooltip("아이템 아이콘 이미지")]
    [SerializeField] 
    private Image _iconImage;
    [SerializeField]
    private GameObject _iconGo;
    private void ShowIcon() => _iconGo.SetActive(true);
    private void HideIcon() => _iconGo.SetActive(false);
    public bool HasItem => _iconImage.sprite != null;
    public bool IsAccessible => _isAccessibleSlot && _isAccessibleItem;

    private bool _isAccessibleSlot = true; 
    private bool _isAccessibleItem = true;
    [SerializeField]
    private EquipmentUI _EquimentUI;

    public Item2 item;
    void Start()
    {
        _EquimentUI = GetComponentInParent<EquipmentUI>();
        _iconGo = _iconImage.gameObject;
        HideIcon();
        _iconImage.raycastTarget = false;
        Debug.Log(HasItem);
    }

    public void SetSlot(Sprite sprite ,Item2 _item)
    {
        item = _item;
        _iconImage.sprite = sprite;
        ShowIcon();
        Debug.Log(HasItem);
    }

    public void EndSlot()
    {
        item = null;
        _iconImage.sprite = null;
        HideIcon();
    }
}

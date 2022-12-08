using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    [SerializeField]
    private GameObject _iconGo;
    private void ShowIcon() => _iconGo.SetActive(true);
    private void HideIcon() => _iconGo.SetActive(false);

    [SerializeField]
    private Image sprite;

    [SerializeField]
    private Sprite _icon;
    public Sprite Icon { set { _icon = value; } }

    [SerializeField]
    private int _index;
    public int Index { set { _index = value; } get { return _index; } }

    private void Start()
    {
        //sprite = GetComponent<Image>();
        _iconGo = sprite.gameObject;
        sprite.raycastTarget = false;
    }

    public void SetSlot(Sprite _sprite)
    {
        
        sprite.sprite = _sprite;
        ShowIcon();
       
    }
}

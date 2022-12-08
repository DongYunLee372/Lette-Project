using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillSlot : MonoBehaviour
{
    public SkillPanelUI _skillpanelUI;
    public SkillData skilldata;
    public int index = 0;

    [SerializeField]
    private GameObject _iconGo;
    private void ShowIcon() => _iconGo.SetActive(true);
    private void HideIcon() => _iconGo.SetActive(false);

    [SerializeField]
    private Image _iconImage;
    public bool HasItem => _iconImage.sprite != null;
    void Start()
    {
        _skillpanelUI.setSlot(this);
        _iconGo = _iconImage.gameObject;
        Debug.Log(index);
    }

    public void SetPlayerSkillSlotOn(SkillData data)
    {
        skilldata = data;
        _iconImage.sprite = data.Icon;



        ShowIcon();

    }
}

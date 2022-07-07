using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/*�����̵�� �ν����� �Ǵ� set�Լ����� ���� ũ��, �ִ밪, �ּҰ� ���� ���� ������ SetCurValue(float value) �Լ��� �̿��� ���� �ٲ۴�.*/
public class SlideBar : MonoBehaviour
{
    public Image FrontImage;
    public Image BackImage;

    [SerializeField]
    private RectTransform FrontRect;
    [SerializeField]
    private RectTransform BackRect;

    [SerializeField]
    private float MaxValue = 1;
    [SerializeField]
    private float MinValue = 0;
    [SerializeField]
    private float CurValue = 0;

    public UnityEvent valueChangeEvent;

    void Start()
    {
        FrontRect = FrontImage.rectTransform;
        BackRect = BackImage.rectTransform;
        FrontImage.rectTransform.sizeDelta = new Vector2(BackImage.rectTransform.sizeDelta.x * (CurValue / MaxValue), BackImage.rectTransform.sizeDelta.y);
        //MaxValue = 1;
        //MinValue = 0;
        //CurValue = 1;
    }

    public void SetSlideBarBound(float x,float y, float width, float height)
    {
        FrontRect.position = new Vector3(x, y, 0);
        BackRect.position = new Vector3(x, y, 0);
        FrontRect.sizeDelta = new Vector2(width, height);
        BackRect.sizeDelta = new Vector2(width, height);
    }

    public void SetSlideBarSize(float width, float height)
    {
        FrontRect.sizeDelta = new Vector2(width, height);
        BackRect.sizeDelta = new Vector2(width, height);
    }


    public void SetFrontColor(Color color)
    {
        FrontImage.color = color;
    }

    public void SetBackColor(Color color)
    {
        BackImage.color = color;
    }

    public void AddListener(UnityAction action)
    {
        valueChangeEvent.AddListener(action);
    }

    public void RemoveListener(UnityAction action)
    {
        valueChangeEvent.RemoveListener(action);
    }

    public void SetCurValue(float value)
    {
        CurValue = value;
        FrontImage.rectTransform.sizeDelta = new Vector2(BackImage.rectTransform.sizeDelta.x * (CurValue / MaxValue), BackImage.rectTransform.sizeDelta.y);
        valueChangeEvent.Invoke();
    }

    public float GetCurValue()
    {
        return CurValue;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            SetCurValue(GetCurValue() * 0.8f);
        }
    }

}

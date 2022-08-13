using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/*슬라이드바 인스펙터 또는 set함수들을 통해 크기, 최대값, 최소값 등을 정한 다음에 SetCurValue(float value) 함수를 이용해 값을 바꾼다.*/
public class SlideBar : MonoBehaviour
{
    [Header("설정 필요")]
    public Image FrontImage;
    public Image BackImage;

    [Header("현재 값")]
    [SerializeField]
    private RectTransform FrontRect;
    [SerializeField]
    private RectTransform BackRect;
    [SerializeField]
    private float MaxValue = 1;
    [SerializeField]
    private float MinValue = 0;
    [SerializeField]
    private float CurValue = 1;

    public UnityEvent valueChangeEvent;

    void Start()
    {
        FrontRect = FrontImage.rectTransform;
        BackRect = BackImage.rectTransform;

        FrontRect.sizeDelta = GetComponent<RectTransform>().sizeDelta;
        BackRect.sizeDelta = GetComponent<RectTransform>().sizeDelta;

        FrontImage.rectTransform.sizeDelta = new Vector2(BackImage.rectTransform.sizeDelta.x * (CurValue / MaxValue), BackImage.rectTransform.sizeDelta.y);
        //MaxValue = 1;
        //MinValue = 0;
        //CurValue = 1;
    }

    //슬라이드바의 위치와 크기를 설정한다.
    public void SetSlideBarBound(float x,float y, float width, float height)
    {
        FrontRect.position = new Vector3(x, y, 0);
        BackRect.position = new Vector3(x, y, 0);
        FrontRect.sizeDelta = new Vector2(width, height);
        BackRect.sizeDelta = new Vector2(width, height);
    }

    //슬라이드바의 크기를 세팅한다.
    public void SetSlideBarSize(float width, float height)
    {
        FrontRect.sizeDelta = new Vector2(width, height);
        BackRect.sizeDelta = new Vector2(width, height);
    }

    //전경색 설정
    public void SetFrontColor(Color color)
    {
        FrontImage.color = color;
    }

    //배경색 설정
    public void SetBackColor(Color color)
    {
        BackImage.color = color;
    }

    //값이 변화 되었을때 실행될 이벤트를 만든다.
    public void AddListener(UnityAction action)
    {
        valueChangeEvent.AddListener(action);
    }

    //연결시킨 이벤트를 삭제한다.
    public void RemoveListener(UnityAction action)
    {
        valueChangeEvent.RemoveListener(action);
    }

    //현재 값을 변화시킨다.
    public void SetCurValue(float value)
    {
        CurValue = value;
        FrontImage.rectTransform.sizeDelta = new Vector2(BackImage.rectTransform.sizeDelta.x * (CurValue / MaxValue), BackImage.rectTransform.sizeDelta.y);
        Debug.Log($"frontsize{FrontImage.rectTransform.sizeDelta.x},{FrontImage.rectTransform.sizeDelta.y} / backsize{BackImage.rectTransform.sizeDelta.x},{BackImage.rectTransform.sizeDelta.y}");
        valueChangeEvent.Invoke();
    }

    //현재 값을 리턴한다.
    public float GetCurValue()
    {
        return CurValue;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("v눌림");
            SetCurValue(GetCurValue() * 0.8f);
        }
    }

}

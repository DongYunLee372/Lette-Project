using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ImageClickEvent : MonoBehaviour, IPointerClickHandler,IPointerDownHandler
    //, IDragHandler
    //, IPointerEnterHandler
    //, IPointerExitHandler
{

    public void OnPointerClick(PointerEventData eventData)
    {
       // Debug.Log("히히Click");
      //  GameData_Load.Instance.ImageClick();

    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    void OnMouseDown()
    {
        if(this.gameObject.activeSelf)
        {
            GameData_Load.Instance.ImageClick();

        }

        // Debug.Log("히히Click");
        //GameData_Load.Instance.ImageClick();
    }


}

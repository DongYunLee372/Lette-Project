using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class Image_Map : MonoBehaviour, IPointerClickHandler
{
    public GameObject camera=null;
    bool flag = false;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!flag)
        {
            int i_width = Screen.width;
            int i_height = Screen.height;

            Debug.Log("화면 width" + i_width);
            Debug.Log("화면 i_height" + i_height);

            RawImage thisImage = null;
            thisImage = this.GetComponent<RawImage>();

            RectTransform rt = (RectTransform)thisImage.transform;
            rt.sizeDelta = new Vector2(i_width, i_height);
            flag = true;
            return;
        }
        

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("마우스 클릭e" + eventData.position);

            Vector2 mousepos = Input.mousePosition;
            Debug.Log("마우스 클릭 2" + mousepos);

            //  Vector3 tempPos = new Vector3(mousepos.x,mousepos.y,0) - camera.gameObject.transform.position;
            Vector3 tempPos = new Vector3(mousepos.x, mousepos.y, 0);

            Vector3 returnPos = camera.GetComponent<RayScripts>().Ray(tempPos);
            if(returnPos != Vector3.zero)
            {
                Debug.Log("반환"+ returnPos);
            }
            RectTransform rect = GetComponent<RectTransform>();
            Debug.Log("사이즈 " + rect.rect.size);
            Debug.Log("사이즈2 " + rect.offsetMin);


            // Vector2 clickPosTemp = eventData.position - rect.offsetMin;
            Vector2 temp = eventData.position / rect.rect.size;

            Vector3 worldPos;
            worldPos.x = temp.x * 100;
            worldPos.z = temp.y * 100;
            worldPos.y = 0;

            Vector3 realWolrdPos = new Vector3(-50 + worldPos.x, 0, -50 + worldPos.z);

            Debug.Log("계산된 좌표 " + worldPos);
            Debug.Log("실제 타겟" + realWolrdPos);

            MapManager.Instance.MoveUnit(realWolrdPos);
        }
    }
}

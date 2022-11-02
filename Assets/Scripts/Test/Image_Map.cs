using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Image_Map : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("마우스 클릭e" + eventData.position);

            Vector2 mousepos = Input.mousePosition;
            Debug.Log("마우스 클릭 2" + mousepos);

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayScripts : MonoBehaviour
{
    RaycastHit hit;

    public Vector3 Ray(Vector3 target)
    {
        Debug.Log("ray");
        if (Physics.Raycast(transform.position, target, out hit))
        {
            Vector3 position = new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z);
            Debug.Log("pos 반환" + position);
            return position;
        }
        return Vector3.zero;
    }

}

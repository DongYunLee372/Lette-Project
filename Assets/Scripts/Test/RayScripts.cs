using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayScripts : MonoBehaviour
{
    RaycastHit hit;
    public Transform MeshCube;
   public Camera m_LinkCam = null;
    void Start()
    {
        m_LinkCam = GetComponent<Camera>();
    }

    public Vector3 Ray(Vector3 target)
    {
        // Ray ray = m_LinkCam.ViewportPointToRay(target);
        Ray ray = m_LinkCam.ViewportPointToRay(target);

        //  Debug.DrawLine(ray.origin, ray.origin + ray.direction * 1000f, Color.red, 300f);
        Debug.DrawLine(ray.origin, ray.origin+ray.direction * 1000f, Color.red, 15f);


        Debug.Log("ray");
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000f))
        {
            // Vector3 position = new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z);
            //Vector3 position = new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z);

            Vector3 position = hit.point;

            Debug.Log("pos 반환" + position);

            MeshCube.position = position;
            return position;
        }
        return Vector3.zero;
    }

}

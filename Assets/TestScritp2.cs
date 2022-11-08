using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScritp2 : MonoBehaviour
{


    [SerializeField]
    Camera m_LinkCam = null;

    // Start is called before the first frame update
    void Start()
    {
        m_LinkCam = GetComponent<Camera>();
    }


    void UpdateRayCast()
    {
        Ray ray = m_LinkCam.ViewportPointToRay(new Vector3(.2f, 0.1f, 0));


        //if( Physics.Raycast() )
        //{

        //}

        //Debug.DrawRay(ray.origin, ray.direction, Color.red, 3f);
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 1000f, Color.red, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

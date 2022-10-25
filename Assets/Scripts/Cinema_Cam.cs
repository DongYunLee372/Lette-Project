using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinema_Cam : MonoBehaviour
{
    public Camera cam;
    public float PosZ = 0f;
    Vector3 velo = Vector3.zero;
    Vector3 v;
    float x;
    public float val = 0.001f;
    
    void Start()
    {
        cam = GetComponent<Camera>();
        v = cam.transform.position;
        v.z += 20f;
        CamStart();
    }
    void CamStart()
    {
        StartCoroutine(MoveCam());
        PlayableCharacter.Instance.gameObject.SetActive(false);
    }
    IEnumerator MoveCam()
    {
        x = Time.time;
        
        while (true)
        {
           
            cam.transform.position = Vector3.SmoothDamp(cam.transform.position, v, ref velo, val);


            if (Time.time - x >= 4f)
            {
                cam.gameObject.SetActive(false);
                break;
            }

            yield return null;
        }

        PlayableCharacter.Instance.gameObject.SetActive(true);
    }

    void Update()
    {
        //cam.transform.position = Vector3.SmoothDamp(cam.transform.position, v, ref velo, 0.1f);
    }
}

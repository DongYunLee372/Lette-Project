using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    public Camera Playercamera;

    public void SetPlayerCamera( GameObject obj)
    {
        Playercamera = obj.transform.GetChild(0).GetChild(0).GetComponent<Camera>();
    }
    // Start is called before the first frame update
 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera minimap_camera;
    public GameObject player;
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 a = new Vector3();
        a = minimap_camera.transform.position;
        a.x = player.transform.position.x;
        a.z = player.transform.position.z;
        minimap_camera.transform.position = a;
    }
}

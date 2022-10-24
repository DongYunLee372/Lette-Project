using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSkyboxM : MonoBehaviour
{
    public Material mat;


    void Start()
    {

    }

    void Test()
    {
        RenderSettings.skybox.SetFloat("Exposure", 1.0f);
        //mat.SetFloat("Exposure", 1.0f);
        //mat.SetFloat("ss", 25f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            Test();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
    public List<Material> skybox_Mat;

    public void SkyBox_Setting(string scene_name)
    {
        switch (scene_name)
        {
            case "BoatScene":
                RenderSettings.skybox = skybox_Mat[0];
                break;
            case "Roomtest":
                RenderSettings.skybox = skybox_Mat[1];
                break;
        }

    }

    void Update()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxManager : Singleton<SkyboxManager>
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

    public void SkyBox_Change(string skybox_name)
    {
        foreach (Material m in skybox_Mat)
        {
            Debug.Log("m 네임 : " + m.name + "스카이박스 네임 : " + skybox_name);
            if (m.name == skybox_name)
                RenderSettings.skybox = m;
        }
    }

}

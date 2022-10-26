using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MySingleton<Fog>
{
    public ParticleSystem FogParticle;
    void Start()
    {
        FogParticle = GetComponent<ParticleSystem>();
        //OffFog();
        
    }

    public void OffFog()
    {
        FogParticle.loop = false;
    }

    public void OnFog()
    {
        FogParticle.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

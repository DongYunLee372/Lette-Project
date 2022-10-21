using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDataSCP : MonoBehaviour
{
    public float abc = 0;
    public float qqq = 3f;
    void Start()
    {
        
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            LoadFile.Save<TestDataSCP>(this, "abc");
        }
    }
}

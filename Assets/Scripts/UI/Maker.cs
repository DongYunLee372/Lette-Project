using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maker : MonoBehaviour
{
   // public GameObject Player;
    private Vector3 p;
    // Update is called once per frame
    void Update()
    {
        PlayableCharacter a = FindObjectOfType<PlayableCharacter>();
        if(a!=null)
        {
            //a.transform.position;
            this.transform.position = a.transform.position; ;
        }
      
    }
}

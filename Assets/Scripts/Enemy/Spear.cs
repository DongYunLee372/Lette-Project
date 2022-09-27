using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == Global_Variable.CharVar.Player_Tag)
        {
            other.gameObject.GetComponent<PlayableCharacter>().BeAttacked(10f, other.transform.position, 10f);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

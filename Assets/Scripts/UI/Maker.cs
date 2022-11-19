using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maker : MonoBehaviour
{
    public GameObject Player;
    private Vector3 p;
    // Update is called once per frame
    void Update()
    {
        this.transform.position = Player.transform.position;
    }
}

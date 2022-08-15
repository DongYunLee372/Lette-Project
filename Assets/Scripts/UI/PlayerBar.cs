using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBar : MonoBehaviour
{
    public List<Image> image = new List<Image>();
    void Start()
    {
        image.Add(this.GetComponent<Image>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

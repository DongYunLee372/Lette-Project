using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyDotween;

public class dotweentest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("Doooooooooooooooostart");
            Dotween dotween = new Dotween();
            dotween.SetEase(Dotween.Ease.easeInCubic);
            dotween.DoMove(this.gameObject, new Vector3(20, 0, 0), 3);
        }
    }
}

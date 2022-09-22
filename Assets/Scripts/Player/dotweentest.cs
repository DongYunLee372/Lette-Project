using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dotweentest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("Doooooooooooooooostart");
            Dotween.Instance.SetEase(Dotween.Ease.easeOutCirc);
            Dotween.Instance.DoMove(this.gameObject, new Vector3(20, 0, 0), 5);
        }
    }
}

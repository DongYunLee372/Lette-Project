using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOnoff : MonoBehaviour
{
    public GameObject Canvas;

    public void ShowImage(bool show)
    {
        if (show)
        {
            Debug.Log("켜짐");
            Canvas.SetActive(true);

        }
        else
        {
            Debug.Log("꺼짐");

            Canvas.SetActive(false);

        }

    }

}

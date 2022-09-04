using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonListner : MonoBehaviour
{
    public Button B_GameStart;
    public Button B_Option;
    public Button B_Restart;
    public Button B_Exitgame;
    // Start is called before the first frame update
    void Start()
    {
        B_GameStart.onClick.AddListener(GameStart);
        B_Option.onClick.AddListener(Option);
        B_Restart.onClick.AddListener(Restart);
        B_Exitgame.onClick.AddListener(Exitgame);

    }

    // Update is called once per frame
    void Exitgame()
    {

    }
    void Restart()
    {

    }

    void Option()
    {

    }
    void GameStart()
    {
        Debug.Log("gd");
    }
  
}

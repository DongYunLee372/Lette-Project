using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    // Start is called before the first frame update
   
    public void NewGame()
    {
       // UIManager.Instance.Canvasoff(UIManager.CANVAS_NUM.ex_Icon);
        GameMG.Instance.tempObj_Manager.Add(GameMG.Instance.Resource.Instantiate("PlayerCharacter", null));
     //   UIManager.Instance.Canvason(UIManager.CANVAS_NUM.ex_skill);
    }
}

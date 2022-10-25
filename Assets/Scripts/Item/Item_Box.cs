using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Box : MonoBehaviour
{
    // Start is called before the first frame update

    public void Ending()
    {
        GameData_Load.Instance.ChangeScene(Scenes_Stage.BossEnd);
    }
}

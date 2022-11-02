using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager :Singleton<MapManager>
{
  
    public GameObject unit;
  
    public void MoveUnit(Vector3 target)
    {
        Debug.Log("gkgk" + unit.transform.position);
        PathRequestManager.RequestPath(unit.transform.position, target, unit.GetComponent<Unit>().OnPathFound);
    }
}

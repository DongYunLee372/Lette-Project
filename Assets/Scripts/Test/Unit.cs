using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    float speed = 20;
    Vector3[] path;
    int targetIndex;

   
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
         
            StopCoroutine("FollowPath");
            targetIndex = 0;
            StartCoroutine("FollowPath");
        }
    }

    bool moveEndCheck=true;
    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];
        while (true)
        {
            if (moveEndCheck)
            {
                moveEndCheck = false;

                if (transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        targetIndex = 0;
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }
                //  transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);

                PlayableCharacter.Instance.AutoMove(currentWaypoint,MoveEnd);
            }
            yield return null;
        }
    }
    public void MoveEnd()
    {
        moveEndCheck = true;
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }

}

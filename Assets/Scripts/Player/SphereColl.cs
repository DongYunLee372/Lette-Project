using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//원에 반으로
public class SphereColl : Colliders
{
    private void Awake()
    {
        VirtualStart();
    }
    private void Start()
    {
        VirtualStart();
    }

    public override void VirtualStart()
    {
        base.VirtualStart();
        colltype = CharEnumTypes.eCollType.SphereColl;
        Mycollider = GetComponent<SphereCollider>();
    }

    public SphereCollider GetCollider()
    {
        return Mycollider as SphereCollider;
    }

    public override void SetRadious(float radius)
    {
        SphereCollider col = Mycollider as SphereCollider;
        col.radius = radius;
        
    }


    //IEnumerator FollowPath()
    //{
    //    Vector3 currentWaypoint = path[0];
    //    while (true)
    //    {
    //        if (moveEnd)
    //        {
    //            moveEnd = false;
    //            targetIndex++;
    //            if (targetIndex >= path.Length)
    //            {
    //                targetIndex = 0;
    //                yield break;
    //            }
    //            currentWaypoint = path[targetIndex];
    //            //끝 지점까지 가면 종료

    //            PlayableCharacter.Instance.AutoMove(currentWaypoint, MoveEnd);
                
    //        }

            

    //        yield return null;
    //    }
    //}

    //public void MoveEnd()
    //{
    //    moveEnd = true;
    //}

}

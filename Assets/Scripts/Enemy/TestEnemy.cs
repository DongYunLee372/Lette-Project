using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestEnemy : MonoBehaviour
{
    public GameObject target;

    NavMeshAgent nav;


    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    public float cur_Radian = .0f;
    void CircleMove()
    {
        //if (cur_Radian >= 360.0f)
        //{
        //    cur_Radian = 0.0f;
        //}

        //cur_Radian += Time.deltaTime * 10f;

        ////float deRad = cur_Radian * Mathf.Rad2Deg;

        ////float sinValue = Mathf.Sin(deRad);
        ////float cosValue = Mathf.Cos(deRad);

        ////float y = 0;
        ////float x = 0;

        ////y = sinValue * 50;
        ////x = cosValue * 50;
        //Vector3 nextpos = Quaternion.Euler(0, cur_Radian, 0) * Vector3.right * 5;

        //nav.SetDestination((transform.position - nextpos) * cur_Radian);

        transform.RotateAround(target.transform.position, Vector3.up, 5f * Time.deltaTime);
    }

    void Update()
    {
        CircleMove();
    }
}

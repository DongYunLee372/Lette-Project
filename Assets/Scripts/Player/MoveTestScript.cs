using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTestScript : MonoBehaviour
{
    public float GroundCheckDis;

    public float FowardCheckDis;


    public bool IsGrounded;
    public bool IsFowardBlocked;

    public float movespeed;

    public Vector3 movedir;
    public Vector3 worldmove;

    public BoxCollider box;

    public float Gravity = 0;

    public float GravityAccel = 1;

    public Rigidbody rig = null;

    public int vir;
    public int hor;

    public bool W_KeyDown = false;
    public bool S_KeyDown = false;
    public bool D_KeyDown = false;
    public bool A_KeyDown = false;



    // Start is called before the first frame update
    void Start()
    {
        box = GetComponent<BoxCollider>();
        rig = GetComponent<Rigidbody>();
    }

    public void CheckGround()
    {
        IsGrounded = false;
        RaycastHit[] hit = Physics.BoxCastAll(box.transform.position, box.size * 0.5f, transform.up * -1, transform.rotation, 10);


        if (hit.Length>0)
        {
            Debug.Log($"hit 개수 {hit.Length}");
            foreach(var a in hit)
            {
                Debug.Log($"tag = {a.transform.tag}");
                if (a.transform.tag == "Ground")
                {
                    IsGrounded = true;
                    return;
                }
            }
        }
    }


    public void CheckFoward()
    {

    }

    public void KeyInput()
    {
        vir = 0;
        hor = 0;

        if (Input.GetKey(KeyCode.W))
        {
            vir = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            vir = -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            hor = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            hor = -1;
        }

        movedir = new Vector3(vir, 0, hor);
    }

    public void Falling()
    {
        if (IsGrounded)
        {
            Gravity = 0;
            GravityAccel = 1;
            return;
        }

        GravityAccel += 0.098f;
        Gravity -= GravityAccel;

    }


    public void Move()
    {
        worldmove = new Vector3(movedir.x * movespeed, Gravity, movedir.z * movespeed);
        rig.velocity = worldmove;
    }

    // Update is called once per frame
    void Update()
    {
        //CheckGround();
        //Falling();
        KeyInput();
        Move();
    }
}

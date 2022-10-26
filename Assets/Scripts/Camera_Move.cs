using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Camera_Move : MonoBehaviour
{
    public float CameraMoveSpeed = 0f;
    public float CameraRotationSpeed = 0f;
    public Camera cam;
    public Vector3 Pos = Vector3.zero;
    public Vector3 Dir = Vector3.zero;
    Quaternion DirQua;
    public void Move()
    {
        //transform.DoMove(Vector3 목표값, float 변화시간, (bool 정수단위 이동여부));
        if (Input.GetKey(KeyCode.A))
        {
            Pos = cam.transform.position - (cam.transform.right * CameraMoveSpeed * Time.deltaTime);
            cam.transform.DOMove(Pos, 1f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Pos = cam.transform.position + (cam.transform.right * CameraMoveSpeed * Time.deltaTime);
            cam.transform.DOMove(Pos, 1f);
        }

        if (Input.GetKey(KeyCode.W))
        {
            Pos = cam.transform.position + (cam.transform.forward * CameraMoveSpeed * Time.deltaTime);
            cam.transform.DOMove(Pos, 1f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Pos = cam.transform.position - (cam.transform.forward * CameraMoveSpeed * Time.deltaTime);
            cam.transform.DOMove(Pos, 1f);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            Pos = cam.transform.position + (cam.transform.up * CameraMoveSpeed * Time.deltaTime);
            cam.transform.DOMove(Pos, 1f);
        }
        if (Input.GetKey(KeyCode.E))
        {
            Pos = cam.transform.position - (cam.transform.up * CameraMoveSpeed * Time.deltaTime);
            cam.transform.DOMove(Pos, 1f);
        }            
    }
    

    public void Rotation()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            cam.transform.Rotate(Vector3.up, Time.deltaTime * CameraRotationSpeed);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            cam.transform.Rotate(-(Vector3.up), Time.deltaTime * CameraRotationSpeed);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            cam.transform.Rotate(-(Vector3.right), Time.deltaTime * CameraRotationSpeed);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            cam.transform.Rotate(Vector3.right, Time.deltaTime * CameraRotationSpeed);
        }

        if(Input.GetKey(KeyCode.K))
        {
            
            cam.transform.Rotate(Vector3.forward, Time.deltaTime * CameraRotationSpeed);
        }
        if (Input.GetKey(KeyCode.J))
        {
            cam.transform.Rotate(-(Vector3.forward), Time.deltaTime * CameraRotationSpeed);
        }

    }

    void Start()
    {
        cam = GetComponent<Camera>();
        Pos = cam.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
       
        Move();
        Rotation();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveCom : BaseCom
{
    [System.Serializable]
    public class Com
    {
        public Transform tpCamera;
        public Transform tpRig;

        public Transform fpCamera;
        public Transform fpRoot;
        public Transform fpRig;

        public CapsuleCollider capsulecollider;
        public Transform CharacterRoot;
        public Rigidbody rigidbody;
    }

    public Com com = new Com();

    public Vector3 _moveDir = Vector3.zero;
    public Vector3 _worldMove = Vector3.zero;
    public Vector2 mouseMove = Vector2.zero;

    public bool isMoving = false;
    public bool IsRunning = false;
    public bool isGrounded = false;
    public bool isCursorActive = false;
    public bool IsCameraTpOn = true;
    public bool IsFPP = true;

    public float rotationSpeed = 2f; // 카메라 상하좌우 회전속도
    public float lookUpDegree = -60f; // 올려다보기 제한 각도
    public float LookDownDegree = 75f; //내려다보기 제한 각도
    public float RotMouseSpeed = 10f;
    public float Accel;
    public float MoveSpeed;
    public float Gra;














   
    public BaseInteractive InteractiveObj;

    public void Test_Save_Interactive(BaseInteractive obj)
    {
        InteractiveObj = obj;



        InteractiveObj.Oninteractive();
    }
    public void Test_DeleteInteractive()
    {
        InteractiveObj = null;
    }
































    public void KeyInput()
    {
        



        float h = 0f, v = 0f;
        if (Input.GetKeyDown(KeyCode.LeftAlt)) ShowCursorToggle();
        if (Input.GetKeyDown(KeyCode.Tab)) CameraSwap();

        if (Input.GetKey(KeyCode.W)) v += 1.0f;
        if (Input.GetKey(KeyCode.S)) v -= 1.0f;
        if (Input.GetKey(KeyCode.A)) h -= 1.0f;
        if (Input.GetKey(KeyCode.D)) h += 1.0f;

        mouseMove = new Vector2(Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y"));
        _moveDir = new Vector3(h, 0, v);

        isMoving = _moveDir.sqrMagnitude > 0.01f;

    }

    public void Move()
    {
        _moveDir.Normalize();
        
        if(!isMoving)
        {
            com.rigidbody.velocity = new Vector3(0f, com.rigidbody.velocity.y, 0f);
            return;
        }

        //if(IsCameraTpOn)
        //{
        //    Debug.Log("1");
        //    _worldMove = com.tpRig.TransformDirection(_moveDir);
        //}
        //else
        //{
        //    _worldMove = com.fpRoot.TransformDirection(_moveDir);
        //}

        if (IsFPP)
        {
            Debug.Log("1");
            _worldMove = com.fpRoot.TransformDirection(_moveDir);
        }
        else
        {
            _worldMove = com.tpRig.TransformDirection(_moveDir);
            
        }

        _worldMove *= MoveSpeed;
        com.rigidbody.velocity = new Vector3(_worldMove.x, Gra, _worldMove.z);
    }
    public void Rotate()
    {
        if (IsFPP)
        {
            Debug.Log("true");
            RotateFP();
        }
        else
        {
            
            RotateTP();
            RotateFPRoot();
        }
    }
    public void RotateTP()
    {
        float deltaCoef = Time.deltaTime * 50f;

        float xRotPrev = com.tpRig.localEulerAngles.x;
        float xRotNext = xRotPrev + mouseMove.y * RotMouseSpeed * deltaCoef;

        float yRotPrev = com.tpRig.localEulerAngles.y;
        float yRotNext = yRotPrev + mouseMove.x * RotMouseSpeed * deltaCoef;

        com.tpRig.localEulerAngles = new Vector3(xRotNext, yRotNext, 0f);
    }
    public void RotateFP()
    {        
        float deltaCoef = Time.deltaTime * 50f;

        float xRotPrev = com.fpRig.localEulerAngles.x;
        float xRotNext = xRotPrev + mouseMove.y * RotMouseSpeed * deltaCoef;

        //if (xRotNext > 180f)
        //    xRotNext -= 360f;

        float yRotPrev = com.fpRoot.localEulerAngles.y;
        float yRotNext = yRotPrev + mouseMove.x * RotMouseSpeed * deltaCoef;

        com.fpRig.localEulerAngles = Vector3.right * xRotNext;
        com.fpRoot.localEulerAngles = Vector3.up * yRotNext;
    }

    
    public void RotateFPRoot()
    {
        if (!isMoving) return;

        Vector3 dir = com.tpRig.TransformDirection(_moveDir);
       
        float currentY = com.fpRoot.localEulerAngles.y;        
        float nextY = Quaternion.LookRotation(dir, Vector3.up).eulerAngles.y;
        
        if (nextY - currentY > 180f) nextY -= 360f;
        else if (currentY - nextY > 180f) nextY += 360f;

        com.fpRoot.eulerAngles = Vector3.up * Mathf.Lerp(currentY, nextY, 0.1f);
    }
    public void CameraSwap()
    {
        IsFPP = !IsFPP;

        com.tpCamera.gameObject.SetActive(!IsFPP);
        com.fpCamera.gameObject.SetActive(IsFPP);

        Debug.Log(IsCameraTpOn);
    }
    private void ShowCursorToggle()
    {
        isCursorActive = !isCursorActive;
        ShowCursor(isCursorActive);
    }
    private void ShowCursor(bool value)
    {
        Cursor.visible = value;
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
    }

    void Start()
    {
        com.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        Gra = 0f;
        IsFPP = false;
        CameraSwap();
        ShowCursor(false);
    }

    // Update is called once per frame
    void Update()
    {
       
        KeyInput();
        
        Rotate();
        Move();
    }

    public override void InitCom()
    {
        P_comlist = EnumScp.ComponentList.Move;
    }

    public override void Awake()
    {
        base.Awake();
    }
}

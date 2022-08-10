using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/*캐릭터 주변의 콜라이더들을 검사
  1. 현재 나의 발 밑에 바닥이 있는지
  2. 현재 나의 진행 방향 앞에 벽이 있는지
  3. 내가 가야할 곳의 각도*/
public class CheckAround : MonoBehaviour
{
    CurState curval;
    CMoveComponent movecom;


    public CapsuleCollider CapsuleCol = null;
    public Vector3 Capsuletopcenter => new Vector3(transform.position.x, transform.position.y + CapsuleCol.height - CapsuleCol.radius, transform.position.z);
    public Vector3 Capsulebottomcenter => new Vector3(transform.position.x, transform.position.y + CapsuleCol.radius, transform.position.z);


    
    Vector3 temppos;

    GameObject tempcube;

    NavMeshAgent testnavagent;

    private void Awake()
    {
        CapsuleCol = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        if(movecom==null)
        {
            movecom = PlayableCharacter.Instance.GetMyComponent(EnumTypes.eComponentTypes.MoveCom) as CMoveComponent;
            curval = movecom.curval;
        }
        tempcube = GameObject.Find("tempcube");
        testnavagent = GetComponent<NavMeshAgent>();
    }

    public void CheckFront()
    {
        if (movecom == null)
        {
            movecom = PlayableCharacter.Instance.GetMyComponent(EnumTypes.eComponentTypes.MoveCom) as CMoveComponent;
            curval = movecom.curval;
        }
        RaycastHit hit;
        curval.CurFowardSlopAngle = 0;
        curval.IsFowordBlock = false;
        //Vector3 temp = new Vector3(WorldMove.x, 0, WorldMove.z);
        //temp = com.FpRoot.forward /*+ Vector3.down*/;
        //NavMesh.Raycast()
        bool cast = Physics.CapsuleCast(Capsuletopcenter, Capsulebottomcenter, CapsuleCol.radius - 0.2f, movecom.com.FpRoot.forward, out hit, 0.3f);
        if (cast)
        {
            Debug.DrawLine(Capsulebottomcenter, hit.point,Color.cyan);
            curval.CurFowardSlopAngle = Vector3.Angle(hit.normal, Vector3.up);
            if (curval.CurFowardSlopAngle >= 70.0f)
            {
                curval.IsFowordBlock = true;
            }
        }
    }

    public void CheckFoward()
    {
        if (movecom == null)
        {
            movecom = PlayableCharacter.Instance.GetMyComponent(EnumTypes.eComponentTypes.MoveCom) as CMoveComponent;
            curval = movecom.curval;
        }
        RaycastHit hit;
        curval.CurFowardSlopAngle = 0;
        curval.IsFowordBlock = false;
        //Vector3 temp = new Vector3(WorldMove.x, 0, WorldMove.z);
        //temp = com.FpRoot.forward /*+ Vector3.down*/;
        //NavMesh.Raycast()
        bool cast = Physics.CapsuleCast(Capsuletopcenter, Capsulebottomcenter, CapsuleCol.radius - 0.2f, movecom.com.FpRoot.forward, out hit, 0.3f);
        if (cast)
        {
            Debug.DrawLine(Capsulebottomcenter, hit.point, Color.cyan);
            curval.CurFowardSlopAngle = Vector3.Angle(hit.normal, Vector3.up);
            if (curval.CurFowardSlopAngle >= 70.0f)
            {
                curval.IsFowordBlock = true;
            }
        }
    }


    public void CheckGround()
    {
        if (movecom == null)
        {
            movecom = PlayableCharacter.Instance.GetMyComponent(EnumTypes.eComponentTypes.MoveCom) as CMoveComponent;
            curval = movecom.curval;
        }
        curval.IsGrounded = false;
        curval.IsSlip = false;
        curval.IsOnTheSlop = false;
        curval.CurGroundSlopAngle = 0;


        if (Time.time >= curval.LastJump + 0.2f)//점프하고 0.2초 동안은 지면검사를 하지 않는다.
        {
            RaycastHit hit;

            NavMeshHit navhit;



            temppos = new Vector3(this.transform.position.x, this.transform.position.y  - 10, this.transform.position.z);
            tempcube.transform.position = temppos;

            //bool cast = testnavagent.Raycast(temppos, out navhit);
            //bool cast = NavMesh.Raycast(this.transform.position + new Vector3(0,2,0), temppos, out navhit, NavMesh.GetAreaFromName("Walkable"));
            //Debug.DrawLine(this.transform.position + new Vector3(0, 2, 0), temppos, cast ? Color.red : Color.blue);
            bool cast = Physics.SphereCast(Capsulebottomcenter, CapsuleCol.radius - 0.2f, Vector3.down, out hit, CapsuleCol.radius - 0.1f,LayerMask.GetMask("Ground"));

            if (cast)
            {
                Debug.DrawLine(Capsulebottomcenter, hit.point, Color.blue);

                curval.IsGrounded = true;
                curval.CurGroundNomal = hit.normal;
                curval.CurGroundSlopAngle = Vector3.Angle(hit.normal, Vector3.up);

                curval.CurFowardSlopAngle = Vector3.Angle(hit.normal, movecom.com.FpRoot.forward) - 90f;

                if (curval.CurGroundSlopAngle > 1.0f)
                {
                    curval.IsOnTheSlop = true;
                    if (curval.CurGroundSlopAngle >= movecom.moveoption.MaxSlop)
                    {
                        curval.IsSlip = true;
                    }
                }
                curval.CurGroundCross = Vector3.Cross(curval.CurGroundNomal, Vector3.up);

            }
        }

    }

    private void Update()
    {
        CheckFront();
        CheckGround();

    }

}

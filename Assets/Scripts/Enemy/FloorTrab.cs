using EnumScp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTrab : BaseInteractive
{
    public Collider TrabCollider = null;
    public bool IsInteractive = false;
    public GameObject Spear = null;
    public GameObject Floor = null;
    public IEnumerator coroutine = null;
    public float MoveSpeed = 0f;

    private int Instance;
    private InteractiveIndex interactive;

    public override int P_Instance { get { return Instance; } protected set { Instance = value; } }
    public override InteractiveIndex P_interactive { get { return interactive; } protected set { interactive = value; } }

    public override void Init()
    {
        
        TrabCollider = GetComponentInChildren<BoxCollider>();
        coroutine = StartTrab();
        MoveSpeed = 10f;

        P_Instance = GetInstanceID();
        P_interactive = InteractiveIndex.Trab;

        InteractiveObjManager.Instance.SetInteractiveObj(P_interactive, this);
    }

    public IEnumerator StartTrab()
    {
        IsInteractive = true;
        Vector3 temppos = Spear.transform.position;
        Vector3 dir = (Floor.transform.position - Spear.transform.position).normalized;
        while (true)
        {
            if(Spear.transform.position.y >= Floor.transform.position.y)
            {
                break;
            }
            //temppos.y = MoveSpeed * Time.deltaTime;
            Spear.transform.position += dir * MoveSpeed * Time.deltaTime;
            yield return new WaitForSeconds(0.1f);
        }

        
        IsInteractive = false;
        yield return null;
    }

    public override void Oninteractive()
    {
        StartCoroutine(coroutine);
    }


    public void OnTriggerEnter(Collider other)
    {
        Oninteractive();
        //InteractiveObjManager.Instance.EndInteractiveObj(this.GetType().ToString());
    }

    public override void Awake()
    {
        base.Awake();
    }

    
    void Start()
    {
        
    }

    
    void Update()
    {

    }
}

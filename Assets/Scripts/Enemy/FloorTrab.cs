using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTrab : BaseInteractive
{
    public Collider TrabCollider = null;
    public bool IsInteractive = false;
    public GameObject Spear = null;
    public IEnumerator coroutine = null;
    public float MoveSpeed = 0f;


    public override void Init()
    {
        InteractiveObjManager.Instance.SetInteractiveObj(EnumScp.InteractiveIndex.Trab, this);
        TrabCollider = GetComponentInChildren<BoxCollider>();
        Debug.Log(TrabCollider.name);
        coroutine = StartTrab();
        MoveSpeed = 7f;
    }

    public IEnumerator StartTrab()
    {
        IsInteractive = true;
        Vector3 temppos = Spear.transform.position;
        Vector3 dir = (this.transform.position - Spear.transform.position).normalized;
        while (true)
        {
            if(Spear.transform.position.y >= this.transform.position.y)
            {
                yield break;
            }
            //temppos.y = MoveSpeed * Time.deltaTime;
            Spear.transform.position += dir * MoveSpeed * Time.deltaTime;
            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log("끝");
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNPCSCP : BaseInteractive
{
    [SerializeField]
    TEST_ScriptableOBJ NPC_Infor;

    BaseInteractive test;
    public override void Init()
    {
        P_interactive = EnumScp.InteractiveIndex.TestNPC_Sangmin;
        
    }

    public override void Oninteractive()
    {
        Debug.Log(NPC_Infor.P_NPC_Text);
        InteractiveObjManager.Instance.SetInteractiveObj(EnumScp.InteractiveIndex.TestNPC_Sangmin, this);

        //test = InteractiveObjManager.Instance.GetInteractiveObj(EnumScp.InteractiveIndex.TestNPC_Sangmin);
        //Debug.Log(test.name);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerMoveCom>().Test_Save_Interactive(this);

            
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerMoveCom>().Test_DeleteInteractive();
            InteractiveObjManager.Instance.EndInteractiveObj(EnumScp.InteractiveIndex.TestNPC_Sangmin);

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

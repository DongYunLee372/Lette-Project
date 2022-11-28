using EnumScp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : BaseInteractive
{

    [SerializeField]
    TEST_ScriptableOBJ NPC_Infor;

    public bool IsInteractive;
    public int Instance;
    public InteractiveIndex interactive;
    public override bool P_IsInteractive { get { return IsInteractive; }  set { IsInteractive = value; } }
    public override int P_Instance { get { return Instance; } protected set { Instance = value; } }
    public override InteractiveIndex P_interactive { get { return interactive; } protected set { interactive = value; } }

    public GameObject npc_panel;
    public Text Panel_Text;
    public override void Init()
    {
        P_interactive = EnumScp.InteractiveIndex.TestNPC_Sangmin;
    }

    public override void Oninteractive()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            npc_panel.SetActive(true);
            Panel_Text = npc_panel.GetComponentInChildren<Text>();
            Panel_Text.text = NPC_Infor.P_NPC_Text;

        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            npc_panel.SetActive(false);
        }
    }
}

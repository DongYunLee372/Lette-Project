using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC Data", menuName = "Scriptable Object/NPC Data", order = int.MaxValue)]
public class TEST_ScriptableOBJ : ScriptableObject
{
    [SerializeField]
    private string NPC_Text;
    public string P_NPC_Text { get { return NPC_Text; } set { NPC_Text = value; } }

    
    
}

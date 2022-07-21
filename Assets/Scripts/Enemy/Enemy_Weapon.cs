using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Weapon : MonoBehaviour
{
    [SerializeField]
    private Battle_Character parent_BC;

    void Start()
    {
        parent_BC = GetComponentInParent<Battle_Character>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (parent_BC.isAttack_Effect)
        {
            parent_BC.Attack_Effect(collision.gameObject);
            parent_BC.isAttack_Effect = false;
        }

        // ���� ����� damaged �Լ��� ȣ���ؼ� �������� ������.
        switch (parent_BC.attack_Type) // ���� Ÿ�Կ� �°� �������� ������.
        {
            case Enemy_Enum.Enemy_Attack_Type.Normal_Attack:

                break;
            case Enemy_Enum.Enemy_Attack_Type.Skill_1_sel:

                break;
            case Enemy_Enum.Enemy_Attack_Type.Skill_2_sel:

                break;
            case Enemy_Enum.Enemy_Attack_Type.Skill_3_sel:

                break;
        }
    }

    void Update()
    {

    }
}

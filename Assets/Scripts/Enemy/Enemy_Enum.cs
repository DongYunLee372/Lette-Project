using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy_Enum
{
    public enum Enemy_Grade
    {
        Normal = 1, // �Ϲ� ���� 
        General,  // ���� ����
        Boss // ���� ����
    }

    public enum Enemy_Type
    {
        Preemptive = 1, // ������
        Non_Preemptive,  // �񼱰���
    }
}
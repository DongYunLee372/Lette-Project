using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//ĳ����  ��������.
//��ų 
//�׼� -


public class LoadMG : MonoBehaviour
{
    [SerializeField]
     List<Battle_Character> MonsterLIst;

     void Awake()
    {
        //���� ������ ����Ʈ
        MonsterLIst = new List<Battle_Character>();
        //ĳ���� ����
        Battle_Character Player = new Battle_Character();

        //���� ���� �� ����Ʈ�� ���� �ϴ� 8����...
        for(int i=0; i < StaticClass.MonsterCount; i++)
        {
            Battle_Character Monster = new Battle_Character();
            MonsterLIst.Add(Monster);
        }
    }

    [SerializeField]
     MonsterInformation data;
    [SerializeField]
    DataLoad_Save TestDataLoad;
    //ĳ����,����,������Ʈ�� �ʱ�ȭ,����Ʈ �߰� ��Ű��
    void ObjectInIt()
    {
        EnumScp.MonsterIndex tempindex = 0;

        //���Ͷ� ĳ���� ���� ���� �޾ƿͼ� �ʱ�ȭ ���ֱ�.

        
        foreach(var monster in MonsterLIst)
        {
            //��� �޾ƿ���
            data = ScriptableObject.CreateInstance<MonsterInformation>();
            data = TestDataLoad.TestScp(tempindex);
            tempindex++;
            //Ȥ�� ���� �Ѿ�� �ʱ�ȭ
            if(tempindex<=EnumScp.MonsterIndex.Max)
            {
                break;
            }
            //���� �ʱ�ȭ �۾� (���� �ȸ������) �Լ� ������ ����

            Debug.Log(data.P_mon_nameKor);
        }
    }

    //���ҽ��� �ε� ��Ű��
    void ResourceInit()
    {

    }



    void Start()
    {
        ObjectInIt();
      //  ResourceInit();
    }

    void Update()
    {
        
    }
}


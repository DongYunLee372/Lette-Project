using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObjManager : MySingleton<InteractiveObjManager>
{


    [SerializeField]
    Dictionary<EnumScp.InteractiveIndex, BaseInteractive> InteractiveObjDic = new Dictionary<EnumScp.InteractiveIndex, BaseInteractive>();
    public void SetInteractiveObj(EnumScp.InteractiveIndex index, BaseInteractive obj) //플레이어와 상호작용 하는 obj의 정보를 딕셔너리에 저장
    {
        InteractiveObjDic.Add(index, obj);
        Debug.Log(index + " " + obj.name);
    }

    public BaseInteractive GetInteractiveObj(EnumScp.InteractiveIndex index) //obj 정보 리턴
    {
        return InteractiveObjDic[index];
    }

    public void EndInteractiveObj(EnumScp.InteractiveIndex index) //obj 딕셔너리에서 삭제
    {
        InteractiveObjDic.Remove(index);
    }

    public void IsInteractiveObj(EnumScp.InteractiveIndex index)
    {

    }

    private void Awake()
    {
        BaseInteractive[] temp = GetComponentsInChildren<BaseInteractive>();
       
        //foreach (BaseInteractive a in temp)
        //{
            
        //   a.P_interactive
        //}
    }
}

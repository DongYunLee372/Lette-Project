using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObjManager : MySingleton<InteractiveObjManager>
{
    [SerializeField]
    Dictionary<EnumScp.InteractiveIndex, BaseInteractive> InteractiveObjDic = new Dictionary<EnumScp.InteractiveIndex, BaseInteractive>();
    public void SetInteractiveObj(EnumScp.InteractiveIndex index ,BaseInteractive obj) //플레이어와 상호작용 하는 obj의 정보를 딕셔너리에 저장
    {
        InteractiveObjDic.Add(index, obj);
        Debug.Log(index + " " + obj.name);
    }

    public BaseInteractive GetInteractiveObj(EnumScp.InteractiveIndex index) //플레이어와 상호작용 중인 obj의 정보를 리턴
    {
        return InteractiveObjDic[index];
    }

    public void EndInteractiveObj(EnumScp.InteractiveIndex index) //플레이어와 상호작용이 끝난 obj는 딕셔너리에서 삭제
    {
        InteractiveObjDic.Remove(index);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//생성을 원하는 이펙트와 위치, 생성되어 있을 시간을 넘겨주면 해당 이펙트를 생성해준다.
//이펙트의 부모를 설정해주면 이펙트가 따라다니도록
//리소스의 모든 이펙트들을 받아와서 가지고 있는다

public class EffectManager : MonoBehaviour
{
    //public List<GameObject> CurEffects;
    public Transform BaseEffect;// 기본 이펙트 생성 위치

    public Dictionary<string, GameObject> CurEffects;

    public GameObject InstantiateEffect(GameObject effect)
    {
        GameObject copy = GameObject.Instantiate(effect);
        copy.transform.parent = BaseEffect;
        return copy;
    }

    public GameObject InstantiateEffect(GameObject effect,Vector3 pos)
    {
        GameObject copy = InstantiateEffect(effect);
        copy.transform.position = pos;
        return copy;
    }

    public GameObject InstantiateEffect(GameObject effect, Vector3 pos,float DestroyTime)
    {
        GameObject copy = InstantiateEffect(effect);
        copy.transform.position = pos;
        GameObject.Destroy(copy, DestroyTime);
        return copy;
    }

    public GameObject InstantiateEffect(GameObject effect, Vector3 pos, float DestroyTime, Transform parent)
    {
        GameObject copy = InstantiateEffect(effect);
        copy.transform.position = pos;
        copy.transform.parent = parent;
        GameObject.Destroy(copy, DestroyTime);
        return copy;
    }

    public void SetParent(string name, Transform parent)
    {
        GameObject effect;
        CurEffects.TryGetValue(name, out effect);
        if (effect == null)
        {
            Debug.Log($"{this.name} not exist effect name");
            return;
        }
            
        if(parent ==null)
        {
            effect.transform.parent = BaseEffect;
        }
        else
        {
            effect.transform.parent = parent;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        BaseEffect = new GameObject("Effects").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

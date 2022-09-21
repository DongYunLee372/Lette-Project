using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//생성을 원하는 이펙트와 위치, 생성되어 있을 시간을 넘겨주면 해당 이펙트를 생성해준다.
//이펙트의 부모를 설정해주면 이펙트가 따라다니도록
//리소스의 모든 이펙트들을 받아와서 가지고 있는다

public class EffectManager : MySingleton<EffectManager>
{
    //public List<GameObject> CurEffects;
    //public Transform BaseEffect;// 기본 이펙트 생성 위치

    public Dictionary<int, GameObject> CurEffects = new Dictionary<int, GameObject>();

    public List<GameObject> Effects = new List<GameObject>();

    public CorTimeCounter timer = new CorTimeCounter();

    public IEnumerator cor;

    public GameObject InstantiateEffect(GameObject effect)
    {
        GameObject copy = GameObject.Instantiate(effect);
        copy.transform.parent = null;
        CurEffects.Add(copy.GetInstanceID(), copy);
        return copy;
    }

    public GameObject InstantiateEffect(GameObject effect, float DestroyTime)
    {
        GameObject copy = GameObject.Instantiate(effect);
        copy.transform.parent = null;
        CurEffects.Add(copy.GetInstanceID(), copy);
        cor = timer.Cor_TimeCounter(DestroyTime, GameObject.Destroy, copy);
        StartCoroutine(cor);
        return copy;
    }

    public GameObject InstantiateEffect(GameObject effect,Vector3 pos)
    {
        GameObject copy = InstantiateEffect(effect);
        copy.transform.position = pos;
        return copy;
    }

    public GameObject InstantiateEffect(GameObject effect, Vector3 pos, Quaternion rotation, float DestroyTime=1.0f)
    {
        GameObject copy = InstantiateEffect(effect);
        copy.transform.position = pos;
        copy.transform.rotation = rotation;
        cor = timer.Cor_TimeCounter(DestroyTime, GameObject.Destroy, copy);
        StartCoroutine(cor);
        return copy;
    }

    public GameObject InstantiateEffect(GameObject effect, Transform posrot, float DestroyTime = 1.0f)
    {
        GameObject copy = InstantiateEffect(effect);
        copy.transform.position = posrot.position;
        copy.transform.rotation = posrot.rotation;
        cor = timer.Cor_TimeCounter(DestroyTime, GameObject.Destroy, copy);
        StartCoroutine(cor);
        return copy;
    }

    //
    public GameObject InstantiateEffect(GameObject effect, Vector3 pos, float DestroyTime, Transform parent)
    {
        GameObject copy = InstantiateEffect(effect);
        copy.transform.position = pos;
        copy.transform.parent = parent;
        cor = timer.Cor_TimeCounter(DestroyTime, GameObject.Destroy, copy);
        StartCoroutine(cor);
        return copy;
    }

    public GameObject InstantiateEffect(GameObject effect, Vector3 pos, Quaternion rotation, float DestroyTime, Transform parent)
    {
        GameObject copy = InstantiateEffect(effect);
        copy.transform.position = pos;
        copy.transform.parent = parent;
        copy.transform.rotation = rotation;
        cor = timer.Cor_TimeCounter(DestroyTime, GameObject.Destroy, copy);
        StartCoroutine(cor);
        return copy;
    }

    public GameObject InstantiateEffect(GameObject effect, Vector3 pos, Vector3 size, Quaternion rotation, float DestroyTime, Transform parent)
    {
        GameObject copy = InstantiateEffect(effect);
        copy.transform.position = pos;
        copy.transform.localScale = size;
        copy.transform.rotation = rotation;

        copy.transform.parent = parent;
        cor = timer.Cor_TimeCounter(DestroyTime, GameObject.Destroy, copy);
        StartCoroutine(cor);
        return copy;
    }


    public void SetParent(GameObject effectobj, Transform parent)
    {
        GameObject effect;
        CurEffects.TryGetValue(effectobj.GetInstanceID(), out effect);
        if (effect == null)
        {
            Debug.Log($"{this.name} not exist effect");
            return;
        }
            
        if(parent ==null)
        {
            effect.transform.parent = null;
        }
        else
        {
            effect.transform.parent = parent;
        }
    }

    //// Start is called before the first frame update
    //void Start()
    //{
    //    BaseEffect = new GameObject("Effects").transform;
    //}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//생성을 원하는 이펙트와 위치, 생성되어 있을 시간을 넘겨주면 해당 이펙트를 생성해준다.
//이펙트의 부모를 설정해주면 이펙트가 따라다니도록
//리소스의 모든 이펙트들을 받아와서 가지고 있는다 => 어드레서블로 로드 가능하도록


//2022.09.22 까지 만들 것
//첫번쨰 루프만 실행하고 사라지는 기능
//루프설정 가능하도록
//한번에 같은 소스를 가지고 있는 이펙트가 2개 이상 씬에 생기면 해당 이펙트는 오브젝트 풀에 넣어서 관리한다. 
//이펙트의 모든 파티클들의 duration을 조사해서 해당 duration이 끝나면 삭제되ㅗ도록
//일정 주기로 다시실행하도록(비활성화시켰다가 활성화?)


public class EffectManager : MySingleton<EffectManager>
{
    //public List<GameObject> CurEffects;
    //public Transform BaseEffect;// 기본 이펙트 생성 위치

    public Dictionary<int, GameObject> CurEffects = new Dictionary<int, GameObject>();

    public List<GameObject> Effects = new List<GameObject>();

    public CorTimeCounter timer = new CorTimeCounter();

    public IEnumerator cor;

    //한번만 실행하고 사라진다.
    public GameObject SpawnEffectOneLoop(GameObject effect, Vector3 pos, Quaternion rotation)
    {
        GameObject copyeffect = InstantiateEffect(effect);
        ParticleSystem[] particles = null;
        particles = effect.GetComponentsInChildren<ParticleSystem>();

        copyeffect.transform.position = pos;
        copyeffect.transform.rotation = rotation;

        float maxduration = 0;
        foreach(ParticleSystem particle in particles)
        {
            if(particle.main.duration>maxduration)
            {
                maxduration = particle.main.duration;
            }
        }

        cor = timer.Cor_TimeCounter(maxduration, GameObject.Destroy, copyeffect);
        StartCoroutine(cor);

        return copyeffect;
    }

    //한번만 재생하고 사라진다.
    public GameObject SpawnEffectOneLoop(GameObject effect, Transform posrot)
    {
        GameObject copyeffect = InstantiateEffect(effect);
        ParticleSystem[] particles = null;
        particles = effect.GetComponentsInChildren<ParticleSystem>();

        copyeffect.transform.position = posrot.position;
        copyeffect.transform.rotation = posrot.rotation;

        float maxduration = 0;
        foreach (ParticleSystem particle in particles)
        {
            if (particle.main.duration > maxduration)
            {
                maxduration = particle.main.duration;
            }
        }

        cor = timer.Cor_TimeCounter(maxduration, GameObject.Destroy, copyeffect);
        StartCoroutine(cor);

        return copyeffect;
    }

    //일정 주기로 재시작
    public GameObject SpawnEffectLooping(GameObject effect, Vector3 pos, Quaternion rotation, float _duration, float destroyTime)
    {
        GameObject copyeffect = InstantiateEffect(effect);
        ParticleSystem[] particles = null;
        particles = effect.GetComponentsInChildren<ParticleSystem>();

        copyeffect.transform.position = pos;
        copyeffect.transform.rotation = rotation;

        cor = timer.Cor_TimeCounterLoop<GameObject>(destroyTime, GameObject.Destroy, Restart, 3, copyeffect, copyeffect);
        StartCoroutine(cor);

        return copyeffect;
    }

    public void Restart(GameObject effect)
    {
        ParticleSystem[] particles = null;
        particles = effect.GetComponentsInChildren<ParticleSystem>();
        foreach(ParticleSystem particle in particles)
        {
            particle.Play();
        }
    }


    public void SetLoop(GameObject effect, bool flag)
    {

    }

    



    //기본 스폰
    public GameObject InstantiateEffect(GameObject effect)
    {
        GameObject copy = GameObject.Instantiate(effect);
        copy.transform.parent = null;
        CurEffects.Add(copy.GetInstanceID(), copy);
        return copy;
    }

    //사라질 시간
    public GameObject InstantiateEffect(GameObject effect, float DestroyTime)
    {
        GameObject copy = GameObject.Instantiate(effect);
        copy.transform.parent = null;
        CurEffects.Add(copy.GetInstanceID(), copy);
        cor = timer.Cor_TimeCounter(DestroyTime, GameObject.Destroy, copy);
        StartCoroutine(cor);
        return copy;
    }

    //위치
    public GameObject InstantiateEffect(GameObject effect,Vector3 pos)
    {
        GameObject copy = InstantiateEffect(effect);
        copy.transform.position = pos;
        return copy;
    }

    //위치, 회전, 파괴시간
    public GameObject InstantiateEffect(GameObject effect, Vector3 pos, Quaternion rotation, float DestroyTime=1.0f)
    {
        GameObject copy = InstantiateEffect(effect);
        copy.transform.position = pos;
        copy.transform.rotation = rotation;
        cor = timer.Cor_TimeCounter(DestroyTime, GameObject.Destroy, copy);
        StartCoroutine(cor);
        return copy;
    }

    //transform, 파괴시간
    public GameObject InstantiateEffect(GameObject effect, Transform posrot, float DestroyTime = 1.0f)
    {
        GameObject copy = InstantiateEffect(effect);
        copy.transform.position = posrot.position;
        copy.transform.rotation = posrot.rotation;
        cor = timer.Cor_TimeCounter(DestroyTime, GameObject.Destroy, copy);
        StartCoroutine(cor);
        return copy;
    }

    //위치, 파괴시간, 부모transform
    public GameObject InstantiateEffect(GameObject effect, Vector3 pos, float DestroyTime, Transform parent)
    {
        GameObject copy = InstantiateEffect(effect);
        copy.transform.position = pos;
        copy.transform.parent = parent;
        cor = timer.Cor_TimeCounter(DestroyTime, GameObject.Destroy, copy);
        StartCoroutine(cor);
        return copy;
    }

    //위치, 회전, 파괴시간, 부모transform
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

    //위지, 크기, 회전, 파괴시간, 부모transform
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

    //해당 이펙트의 부모transform을 설정 null 가능
    public void SetParent(GameObject effectobj, Transform parent)
    {
        GameObject effect;
        CurEffects.TryGetValue(effectobj.GetInstanceID(), out effect);

        if (effect == null)
        {
            Debug.Log($"{this.name} not exist effect");
            return;
        }

        effect.transform.parent = parent;
    }

    public void SetLoop(ParticleSystem effect, bool b)
    {

    }

}

public class TempObjectPoolManager
{

}



public class PoolableObject
{

}
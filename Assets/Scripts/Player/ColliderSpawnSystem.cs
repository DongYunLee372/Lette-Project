using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderSpawnSystem : Singleton<ColliderSpawnSystem>
{
    //public CharEnumTypes.eCollType colltype;

    public Colliders[] collprefabs = new Colliders[(int)CharEnumTypes.eCollType.collMax];

    public List<Colliders> SpawnedCollList = new List<Colliders>();

    CorTimeCounter timer = new CorTimeCounter();
    //
    public Colliders SpawnBoxCollider(Vector3 pos, Vector3 size, float SpawnTime,LayerMask targetLayer,  Colliders.CollFunction func)
    {
        Colliders copycoll = null;
        //colltype = type;

        copycoll = GameObject.Instantiate<Colliders>(collprefabs[(int)CharEnumTypes.eCollType.Box]);
        //copycoll.GetComponent<GameObject>().SetActive(true);
        copycoll.GetComponent<GameObject>().transform.position = pos;
        copycoll.targetLayer = targetLayer;
        copycoll.SetCollitionFunction(func);
        copycoll.SetSize(size);
        SpawnedCollList.Add(copycoll);


        StartCoroutine(timer.Cor_TimeCounter<GameObject>(SpawnTime, DestroyCol, copycoll.gameObject));


        return copycoll;
    }

    public Colliders SpawnSphereCollider(Vector3 pos, float radius, float SpawnTime, LayerMask targetLayer, Colliders.CollFunction func)
    {
        Colliders copycoll = null;

        copycoll = GameObject.Instantiate<Colliders>(collprefabs[(int)CharEnumTypes.eCollType.Sphere]);
        //copycoll.GetComponent<GameObject>().SetActive(true);
        copycoll.GetComponent<GameObject>().transform.position = pos;
        copycoll.targetLayer = targetLayer;
        copycoll.SetCollitionFunction(func);
        copycoll.SetRadious(radius);

        SpawnedCollList.Add(copycoll);


        StartCoroutine(timer.Cor_TimeCounter<GameObject>(SpawnTime, DestroyCol, copycoll.gameObject));


        return copycoll;
    }

    public Colliders SpawnBoxCollider(Vector3 pos, Vector3 size, float SpawnTime, string targetLayer, Colliders.CollFunction func)
    {
        Colliders copycoll = null;
        //colltype = type;

        copycoll = GameObject.Instantiate<Colliders>(collprefabs[(int)CharEnumTypes.eCollType.Box]);
        //copycoll.GetComponent<GameObject>().SetActive(true);
        copycoll.GetComponent<GameObject>().transform.position = pos;
        copycoll.targetTag = targetLayer;
        copycoll.SetCollitionFunction(func);
        copycoll.SetSize(size);
        SpawnedCollList.Add(copycoll);


        StartCoroutine(timer.Cor_TimeCounter<GameObject>(SpawnTime, DestroyCol, copycoll.gameObject));


        return copycoll;
    }

    public Colliders SpawnSphereCollider(Vector3 pos, float radius, float SpawnTime, string targetLayer, Colliders.CollFunction func)
    {
        Colliders copycoll = null;

        copycoll = GameObject.Instantiate<Colliders>(collprefabs[(int)CharEnumTypes.eCollType.Sphere]);
        //copycoll.GetComponent<GameObject>().SetActive(true);
        copycoll.GetComponent<GameObject>().transform.position = pos;
        copycoll.targetTag = targetLayer;
        copycoll.SetCollitionFunction(func);
        copycoll.SetRadious(radius);

        SpawnedCollList.Add(copycoll);


        StartCoroutine(timer.Cor_TimeCounter<GameObject>(SpawnTime, DestroyCol, copycoll.gameObject));


        return copycoll;
    }


    //public Colliders SpawnCollider(CharEnumTypes.eCollType type, Vector3 pos, Vector3 size, float radius, float SpawnTime, string targerTag, Colliders.CollFunction func)
    //{
    //    Colliders copycoll = null;
    //    colltype = type;

    //    copycoll = GameObject.Instantiate<Colliders>(collprefabs[(int)colltype]);
    //    //copycoll.GetComponent<GameObject>().SetActive(true);
    //    copycoll.gameObject.transform.position = pos;
    //    copycoll.targetTag = targerTag;
    //    copycoll.SetCollitionFunction(func);

    //    if (colltype == CharEnumTypes.eCollType.Sphere)
    //        copycoll.SetRadious(radius);
    //    else
    //        copycoll.SetSize(size);

    //    SpawnedCollList.Add(copycoll);


    //    StartCoroutine(timer.Cor_TimeCounter<GameObject>(SpawnTime, DestroyCol, copycoll.gameObject));


    //    return copycoll;
    //}

    public void DestroyCol(GameObject obj)
    {
        Debug.Log("파괴 들러옴" + obj.name);
        GameObject.Destroy(obj);
    }


    public void SetSize(Colliders coll, Vector3 size)
    {
        //SpawnedollList.Find(coll).SetSize(size);


    }

    public void SetRadius(float radius)
    {

    }

    //public void SetFunction(Colliders.CollFunction func)
    //{
    //    collprefabs[(int)colltype].SetCollitionFunction(func);
    //}



    //미리 만들
    void Start()
    {
        for(CharEnumTypes.eCollType i =0;i<CharEnumTypes.eCollType.collMax;i++)
        {
            collprefabs[(int)i] = Resources.Load<Colliders>("Prefabs/" + i.ToString() + "Coll");
            //Debug.Log(colltype.ToString() + "찾음");
        }
        //Resources.Load<Colliders>("Prefabs/" + colltype + "Coll");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderSpawnSystem : MonoBehaviour
{
    public CharEnumTypes.eCollType colltype;

    public Colliders[] collprefabs = new Colliders[(int)CharEnumTypes.eCollType.collMax];

    public List<Colliders> SpawnedCollList = new List<Colliders>();

    CorTimeCounter timer = new CorTimeCounter();
    //
    public Colliders SpawnBoxCollider(CharEnumTypes.eCollType type, Vector3 pos, Vector3 size, float radius, float SpawnTime,LayerMask targetLayer,  Colliders.CollFunction func)
    {
        Colliders copycoll = null;
        colltype = type;

        copycoll = GameObject.Instantiate<Colliders>(collprefabs[(int)colltype]);
        copycoll.GetComponent<GameObject>().SetActive(true);
        copycoll.GetComponent<GameObject>().transform.position = pos;
        copycoll.targetLayer = targetLayer;
        copycoll.SetCollitionFunction(func);
        if (colltype == CharEnumTypes.eCollType.Sphere)
            copycoll.SetRadious(radius);
        else
            copycoll.SetSize(size);

        SpawnedCollList.Add(copycoll);


        timer.Cor_TimeCounter<GameObject>(SpawnTime, GameObject.Destroy, copycoll.gameObject);


        return copycoll;
    }


    public Colliders SpawnCollider(CharEnumTypes.eCollType type, Vector3 pos, Vector3 size, float radius, float SpawnTime, string targerTag, Colliders.CollFunction func)
    {
        Colliders copycoll = null;
        colltype = type;

        copycoll = GameObject.Instantiate<Colliders>(collprefabs[(int)colltype]);
        copycoll.GetComponent<GameObject>().SetActive(true);
        copycoll.GetComponent<GameObject>().transform.position = pos;
        copycoll.targetTag = targerTag;
        copycoll.SetCollitionFunction(func);

        if (colltype == CharEnumTypes.eCollType.Sphere)
            copycoll.SetRadious(radius);
        else
            copycoll.SetSize(size);

        SpawnedCollList.Add(copycoll);


        timer.Cor_TimeCounter<GameObject>(SpawnTime, GameObject.Destroy, copycoll.gameObject);


        return copycoll;
    }


    public void SetSize(Colliders coll, Vector3 size)
    {
        //SpawnedollList.Find(coll).SetSize(size);


    }

    public void SetRadius(float radius)
    {

    }

    public void SetFunction(Colliders.CollFunction func)
    {
        collprefabs[(int)colltype].SetCollitionFunction(func);
    }



    //미리 만들
    void Start()
    {
        for(int i=0;i<(int)CharEnumTypes.eCollType.collMax;i++)
        {
            Resources.Load<Colliders>("Prefabs/" + colltype + "Coll");
        }
        //Resources.Load<Colliders>("Prefabs/" + colltype + "Coll");
    }
}

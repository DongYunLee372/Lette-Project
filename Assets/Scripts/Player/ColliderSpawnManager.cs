using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;


//작업할것
//1. 생성한 공격판정을 다른 오브젝트의 하위에 붙일 수 있도록 OK
//2. 공격판정이 원하는 곳으로 움직일 수 있도록 OK
//3. 들어올때 나갈때 계속 있을때 각각 실행 가능하도록 OK
public class ColliderSpawnManager : Singleton<ColliderSpawnManager>
{
    public Colliders[] collprefabs = new Colliders[(int)CharEnumTypes.eCollType.collMax];

    public List<Colliders> SpawnedCollList = new List<Colliders>();

    CorTimeCounter timer = new CorTimeCounter();

    public MyDotween.Dotween dotween = new MyDotween.Dotween();
 

    //
    public Colliders SpawnBoxCollider(Vector3 pos, Vector3 size, float SpawnTime, LayerMask targetLayer, Colliders.CollFunction func)
    {
        Colliders copycoll = null;
        //colltype = type;
        //GameObject copy = ResourceCreateDeleteManager.Instance.InstantiateObj<GameObject>(adressableAdress);

        //copycoll = GameObject.Instantiate<Colliders>(collprefabs[(int)CharEnumTypes.eCollType.Box]);

        //copycoll = ResourceCreateDeleteManager.Instance.InstantiateObj<Colliders>(collprefabs[(int)CharEnumTypes.eCollType.BoxColl].name);
        copycoll = GameMG.Instance.Resource.Instantiate<GameObject>(collprefabs[(int)CharEnumTypes.eCollType.BoxColl].name).GetComponent<Colliders>();

        //copycoll.GetComponent<GameObject>().SetActive(true);
        copycoll.gameObject.transform.position = pos;
        copycoll.targetLayer = targetLayer;
        copycoll.SetCollitionFunction(func, null, null);
        copycoll.SetSize(size);
        SpawnedCollList.Add(copycoll);


        StartCoroutine(timer.Cor_TimeCounter<GameObject>(SpawnTime, CollDestroy, copycoll.gameObject));


        return copycoll;
    }

    public Colliders SpawnSphereCollider(Vector3 pos, float radius, float SpawnTime, LayerMask targetLayer, Colliders.CollFunction func)
    {
        Colliders copycoll = null;

        copycoll = ResourceCreateDeleteManager.Instance.InstantiateObj<Colliders>(collprefabs[(int)CharEnumTypes.eCollType.SphereColl].name);
        //copycoll = GameMG.Instance.Resource.Instantiate<GameObject>(collprefabs[(int)CharEnumTypes.eCollType.SphereColl].name).GetComponent<Colliders>();

        //copycoll = GameObject.Instantiate<Colliders>(collprefabs[(int)CharEnumTypes.eCollType.Sphere]);
        //copycoll.GetComponent<GameObject>().SetActive(true);
        copycoll.gameObject.transform.position = pos;
        copycoll.targetLayer = targetLayer;
        copycoll.SetCollitionFunction(func, null, null);
        copycoll.SetRadious(radius);

        SpawnedCollList.Add(copycoll);


        StartCoroutine(timer.Cor_TimeCounter<GameObject>(SpawnTime, CollDestroy, copycoll.gameObject));


        return copycoll;
    }

    public Colliders SpawnBoxCollider(Vector3 pos, Vector3 size, float SpawnTime, string targetLayer, Colliders.CollFunction func)
    {
        Colliders copycoll = null;
        //colltype = type;

        copycoll = ResourceCreateDeleteManager.Instance.InstantiateObj<Colliders>(collprefabs[(int)CharEnumTypes.eCollType.BoxColl].name);
        //copycoll = GameMG.Instance.Resource.Instantiate<GameObject>(collprefabs[(int)CharEnumTypes.eCollType.BoxColl].name).GetComponent<Colliders>();

        //copycoll.GetComponent<GameObject>().SetActive(true);
        copycoll.gameObject.transform.position = pos;
        copycoll.targetTag = targetLayer;
        copycoll.SetCollitionFunction(func, null, null);
        copycoll.SetSize(size);
        SpawnedCollList.Add(copycoll);


        StartCoroutine(timer.Cor_TimeCounter<GameObject>(SpawnTime, CollDestroy, copycoll.gameObject));


        return copycoll;
    }

    public Colliders SpawnSphereCollider(Vector3 pos, float radius, float SpawnTime, string targetLayer, Colliders.CollFunction func)
    {
        Colliders copycoll = null;

        copycoll = ResourceCreateDeleteManager.Instance.InstantiateObj<Colliders>(collprefabs[(int)CharEnumTypes.eCollType.SphereColl].name);
        //copycoll = GameMG.Instance.Resource.Instantiate<GameObject>(collprefabs[(int)CharEnumTypes.eCollType.SphereColl].name).GetComponent<Colliders>();


        //copycoll.GetComponent<GameObject>().SetActive(true);
        copycoll.gameObject.transform.position = pos;
        copycoll.targetTag = targetLayer;
        copycoll.SetCollitionFunction(func, null, null);
        copycoll.SetRadious(radius);

        SpawnedCollList.Add(copycoll);


        StartCoroutine(timer.Cor_TimeCounter<GameObject>(SpawnTime, CollDestroy, copycoll.gameObject));


        return copycoll;
    }

    public Colliders SpawnSphereCollider(Transform parent, float radius, float SpawnTime, string targetLayer, Colliders.CollFunction func)
    {
        Colliders copycoll = null;

        copycoll = ResourceCreateDeleteManager.Instance.InstantiateObj<Colliders>(collprefabs[(int)CharEnumTypes.eCollType.SphereColl].name);
        //copycoll = GameMG.Instance.Resource.Instantiate<GameObject>(collprefabs[(int)CharEnumTypes.eCollType.SphereColl].name).GetComponent<Colliders>();

        //copycoll.GetComponent<GameObject>().SetActive(true);
        copycoll.gameObject.transform.parent = parent;
        copycoll.gameObject.transform.localPosition = Vector3.zero;
        copycoll.targetTag = targetLayer;
        copycoll.SetCollitionFunction(func, null, null);
        copycoll.SetRadious(radius);

        SpawnedCollList.Add(copycoll);


        StartCoroutine(timer.Cor_TimeCounter<GameObject>(SpawnTime, CollDestroy, copycoll.gameObject));


        return copycoll;
    }

    public void CollDestroy(GameObject obj)
    {

        //GameMG.Instance.Resource.Destroy<GameObject>(obj);
        ResourceCreateDeleteManager.Instance.DestroyObj<GameObject>(collprefabs[(int)CharEnumTypes.eCollType.SphereColl].name, obj);

    }

    public void SetSize(Colliders coll, Vector3 size)
    {
        coll.SetSize(size);
    }

    public void SetRadius(Colliders coll, float radius)
    {
        coll.SetRadious(radius);
    }

    public void SetParent(Colliders coll, Transform parent)
    {
        coll.SetParent(parent);
    }

    public void SetFunction(Colliders coll, Colliders.CollFunction enterfunc, Colliders.CollFunction outerfunc, Colliders.CollFunction stayfunc)
    {
        coll.SetCollitionFunction(enterfunc, outerfunc, stayfunc);
    }


    public void DoMove(Colliders coll, Vector3 dest, float duration, MyDotween.Dotween.Ease ease = MyDotween.Dotween.Ease.Linear)
    {
        dotween.SetEase(ease);
        dotween.DoMove(coll.gameObject, dest, duration);
    }



    //미리 만들
    void Start()
    {
        for (CharEnumTypes.eCollType i = 0; i < CharEnumTypes.eCollType.collMax; i++)
        {
            //collprefabs[(int)i] = Resources.Load<Colliders>("Prefabs/" + i.ToString() + "Coll");
            var temp = Addressables.LoadAssetAsync<GameObject>(i.ToString());
            GameObject result = temp.WaitForCompletion();
            collprefabs[(int)i] = result.GetComponent<Colliders>();
        }

        

    }
}

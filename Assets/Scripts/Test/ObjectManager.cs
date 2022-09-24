using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PoolData<T>
    where T : UnityEngine.Object

{
    public T obj;
    public bool poolable;
}

class Pool<T>
    where T : UnityEngine.Object
{
    public T Original { get; private set; }
    public Transform Root { get; set; }

  //  Stack<T> _poolStack = new Stack<T>();

    Stack<PoolData<T>> _poolDataStack = new Stack<PoolData<T>>();

    public void Init(T original, int count = 60)
    {
        Original = original;

        //Root = new GameObject().transform;
     //   Root.name = $"{original.name}_Root";

        for (int i = 0; i < count; i++)
            Push(Create());
    }

    T Create()
    {
        T go = Object.Instantiate<T>(Original);
        go.name = Original.name;
        return go;
    }

    public void Push(T poolable)
       // where T : UnityEngine.Object
    {
        if (poolable == null)
            return;

        PoolData<T> poolData = new PoolData<T>();
        poolData.poolable = false;
        poolData.obj = poolable;
        var a = poolable as GameObject;
        a.SetActive(false);
      
      //  Object.
        _poolDataStack.Push(poolData);

        // (GameObject)poolable.transform.parent = Root;
        //(GameObject)poolable.gameObject.SetActive(false);
        //poolable.isUsing = false;

        //_poolStack.Push(poolable);
    }

    public T Pop()
    {
        PoolData<T> poolData=new PoolData<T>();
        T poolable;

        if (_poolDataStack.Count > 0)
            //여기에서 그러면 사용가능 여부를 체크하고 반환을 해줘야지
        { 
            poolData = _poolDataStack.Pop();
            poolable = poolData.obj;
        }
        else
        {
            poolable = Create();
            poolData.obj = poolable;
            poolData.poolable = true;
        }

        //    if (_poolStack.Count > 0)
        //    poolable = _poolStack.Pop();
        //else
        //    poolable = Create();

        //poolable.

        //poolable.gameObject.SetActive(true);

        //poolable.transform.parent = parent;
        //poolable.isUsing = true;

        return poolable;
    }
}

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager Instance;

    Dictionary<string, UnityEngine.Object> _pool = new Dictionary<string, UnityEngine.Object>();
    Transform _root;

    //    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();


    private void Awake()
    {
        Instance = this;
    }

    public void Init()
    {
        if (_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }
    }

    public void Push<T>(T poolable)
       where T : UnityEngine.Object
    {
        string name = poolable.name;//gameObject.name;

        if (_pool.ContainsKey(name) == false)
        {
            GameObject.Destroy(poolable);//.gameObject);
            return;
        }

        //PoolData<T> poolData = new PoolData<T>();


        var pooldata = _pool[name] as Pool<T>;//.Push(poolable);
       // pooldata.
        pooldata.Push(poolable);



    }

    public void CreatePool<T>(T original, int count = 60)
        where T : UnityEngine.Object
    {
        Pool<T> pool = new Pool<T>();
       // pool.
        pool.Init(original, count);
       // pool.Root.parent = _root;

        _pool.Add(original.name, pool as T);
    }

    public T Pop<T>(T original, Transform parent = null)
        where T : UnityEngine.Object
    {
        if (_pool.ContainsKey(original.name) == false)
        {
            Debug.Log("pop하러 왔는디"+original.name);
            CreatePool<T>(original);
           // _pool.Add()
        }

        var pool = _pool[original.name] as Pool<T>;

       return pool.Pop();
        //return _pool[original.name].Pop(parent);
    }

    public T GetOriginal<T>(string name)
        where T : UnityEngine.Object
    {
        if (_pool.ContainsKey(name) == false)
            return default;

        var pool = _pool[name] as Pool<T>;
        return pool.Original;
       // return _pool[name].Original;
    }

    public void Clear()
    {
        foreach (Transform child in _root)
            GameObject.Destroy(child.gameObject);

        _pool.Clear();
    }
}




//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//class Pool
//{
//    public GameObject Original { get; private set; }
//    public Transform Root { get; set; }

//    Stack<Poolable> _poolStack = new Stack<Poolable>();

//    public void Init(GameObject original, int count = 60)
//    {
//        Original = original;

//        Root = new GameObject().transform;
//        Root.name = $"{original.name}_Root";

//        for (int i = 0; i < count; i++)
//            Push(Create());
//    }

//    Poolable Create()
//    {
//        GameObject go = Object.Instantiate<GameObject>(Original);
//        go.name = Original.name;
//        return go.GetComponent<Poolable>();
//    }

//    public void Push(Poolable poolable)
//    {
//        if (poolable == null)
//            return;

//        poolable.transform.parent = Root;
//        poolable.gameObject.SetActive(false);
//        poolable.isUsing = false;

//        _poolStack.Push(poolable);
//    }

//    public Poolable Pop(Transform parent)
//    {
//        Poolable poolable;

//        if (_poolStack.Count > 0)
//            poolable = _poolStack.Pop();
//        else
//            poolable = Create();

//        poolable.gameObject.SetActive(true);

//        poolable.transform.parent = parent;
//        poolable.isUsing = true;

//        return poolable;
//    }
//}

//public class ObjectManager : MonoBehaviour
//{
//    public static ObjectManager Instance;

//    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
//    Transform _root;

//    private void Awake()
//    {
//        Instance = this;
//    }

//    public void Init()
//    {
//        if (_root == null)
//        {
//            _root = new GameObject { name = "@Pool_Root" }.transform;
//            Object.DontDestroyOnLoad(_root);
//        }
//    }

//    public void Push(Poolable poolable)
//    {
//        string name = poolable.gameObject.name;

//        if (_pool.ContainsKey(name) == false)
//        {
//            GameObject.Destroy(poolable.gameObject);
//            return;
//        }

//        _pool[name].Push(poolable);
//    }

//    public void CreatePool(GameObject original, int count = 60)
//    {
//        Pool pool = new Pool();
//        pool.Init(original, count);
//        pool.Root.parent = _root;

//        _pool.Add(original.name, pool);
//    }

//    public Poolable Pop(GameObject original, Transform parent = null)
//    {
//        if (_pool.ContainsKey(original.name) == false)
//            CreatePool(original);

//        return _pool[original.name].Pop(parent);
//    }

//    public GameObject GetOriginal(string name)
//    {
//        if (_pool.ContainsKey(name) == false)
//            return null;

//        return _pool[name].Original;
//    }

//    public void Clear()
//    {
//        foreach (Transform child in _root)
//            GameObject.Destroy(child.gameObject);

//        _pool.Clear();
//    }
//}


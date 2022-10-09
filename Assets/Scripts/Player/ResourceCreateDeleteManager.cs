using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;


//게임 실행중 생성되고 파괴되는 모든 리로스들의 생성과 파괴를 담당한다.
//생성과 파괴는 Addressable을 이용
//오브젝트 풀링 설정 가능하도록
public class ResourceCreateDeleteManager : Singleton<ResourceCreateDeleteManager>
{
    ObjectPoolManager poolManager = new ObjectPoolManager();
    //어드레서블로 로드 & 생성
    public T InstantiateObj<T>(string adressableName)
    {
        //어드레서블로
        //var temp = Addressables.InstantiateAsync(adressableName);
        //var result = temp.WaitForCompletion();

        //일단 해당
        var temp = Addressables.LoadAssetAsync<GameObject>(adressableName);
        GameObject result = temp.WaitForCompletion();
        if(result==null)
        {
            Debug.LogError("어드레서블 로드 오류" + adressableName + "존재하지 않음");
            return default(T);
        }

        T resulttype = result.GetComponent<T>();
        Debug.Log("타입" + resulttype.GetType().ToString());
        if(poolManager.IsPooling(result.GetType().ToString()))//풀링을 하고 있는 객체면 풀링에서 꺼내서 주고
        {
            return poolManager.GetObject<T>();
        }
        else
        {
            return Instantiate(result).GetComponent<T>();
        }

    }

    public void Create(GameObject obj)
    {

    }

    //public T InstantiateObj<T> (T obj)
    //{
    //    T getObj;
    //    if(poolManager.IsPooling(obj.GetType().Name))
    //    {
    //        getObj = poolManager.GetObject<T>();
    //    }
    //    else
    //    {
    //        getObj = GameObject.Instantiate(obj);
    //    }
    //    return default(T);
    //}

    public void RegistPoolManager<T>(string _adressablename)
    {
        poolManager.CreatePool<T>(_adressablename);
    }


    public void DestroyObj<T>(T obj)
    {

    }



}

//해당 타입의 풀들을 만들어서 관리한다.
public class ObjectPoolManager
{
    public Dictionary<string, ObjectPool> PoolDic = new Dictionary<string, ObjectPool>();

    public bool IsPooling(string typestring)
    {
        return PoolDic.ContainsKey(typestring);
    }


    public void CreatePool<T>(string adressableName,int poolsize=10) 
    {
        //string typename = obj.GetType().Name;
        ObjectPool pool = null;

        //이미 해당 타입의 풀이 있는지 확인하고 없으면 만들어 준다.
        PoolDic.TryGetValue(typeof(T).Name, out pool);
        Debug.Log(typeof(T).Name +"풀 생성 들어옴");

        if(pool==null)
        {
            Debug.Log(typeof(T).Name + "풀 생성 시도");
            pool = new ObjectPool(adressableName, typeof(T), poolsize);
            PoolDic.Add(typeof(T).Name, pool);
        }

    }

    public T GetObject<T>()
    {
        ObjectPool pool = null;
        PoolDic.TryGetValue(typeof(T).Name, out pool);

        if(pool!=null)
        {
            return pool.GetObj().GetComponent<T>();
        }

        Debug.LogError("존재하지 않는 타입");
        return default(T);
    }

    public void ReturnObject(System.Type _type, GameObject obj)
    {
        ObjectPool pool = null;

        bool flag = PoolDic.ContainsKey(_type.Name);
        //PoolDic.TryGetValue(typeof(T).Name, out pool);

        if (flag)
        {
            //pool = (ObjectPool<T>)PoolDic[typeof(T).Name];

            pool.ReturnObj(obj);
        }
    }



}


public class ObjectPoolBase
{
    
    

}



//풀은 게임오브젝트로 관리
//객체는 기본적으로 10개 생성
public class ObjectPool/*<T>:ObjectPoolBase*/
{
    //T _poolObj;
    string _adressableName;
    System.Type _type;
    public Stack<GameObject> _stack;
    int _poolSize = 0;
    Transform _parent;


    public ObjectPool(string adressableName, System.Type type,int poolsize)
    {
        _stack = new Stack<GameObject>();
        //System.Type _type = System.Type.GetType(_Name);
        _type = type;
        _adressableName = adressableName;
        _poolSize = poolsize;


        CreateObj();

        Debug.Log(_type.Name + "풀 생성 완료");

        
        
    }


    public void CreateObj()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            var temp = Addressables.InstantiateAsync(_adressableName);
            var result = temp.WaitForCompletion();
            result.SetActive(false);
            _stack.Push(result);
        }
    }


    public GameObject GetObj()
    {
        GameObject temp = null;
        if (_stack.Count>0)
        {
            temp = _stack.Pop();
            temp.SetActive(true);
            temp.transform.SetParent(null);
        }
        return temp;
    }

    public void ReturnObj(GameObject obj)
    {
        obj.SetActive(false);
        _stack.Push(obj);
    }


}
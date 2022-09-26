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

    public void RegistPoolManager<T>(string _name)
    {
        poolManager.CreatePool<T>(_name);
    }


    public void DestroyObj<T>(T obj)
    {

    }



}


public class ObjectPoolManager
{
    public Dictionary<string, ObjectPool<object>> PoolDic = new Dictionary<string, ObjectPool<object>>();

    public bool IsPooling(string typestring)
    {
        return PoolDic.ContainsKey(typestring);
    }


    public void CreatePool<T>(string adressableName) 
    {
        //string typename = obj.GetType().Name;
        ObjectPool<object> pool = null;

        //이미 해당 타입의 풀이 있는지 확인하고 없으면 만들어 준다.
        PoolDic.TryGetValue(typeof(T).Name, out pool);
        Debug.Log(typeof(T).Name +"풀 생성 들어옴");

        if(pool==null)
        {
            pool = new ObjectPool<object>(adressableName, typeof(T));
            
        }



    }

    public T GetObject<T>()
    {
        return default(T);
    }

    public void ReturnObject<T>(T obj)
    {

    }



}




public class ObjectPool<T>
{

    string AdressableName;
    public Queue<object> _queue;

    public ObjectPool(string _Name, System.Type type)
    {
        AdressableName = _Name;
    }






}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;


public class AddressablesLoadManager : Singleton<AddressablesLoadManager>
{
  
    private  List<string> Load_String_List = new List<string>();  //로드하는 이름

    private  List<UnityEngine.Object> CreateObjectList = new List<UnityEngine.Object>();  //Instantiate로 생성된 오브젝트들 관리 =>어드레서블 에셋이 아니라 에셋으로 복사 된 오브젝트를 말함.
    private  List<UnityEngine.Object> InstList = new List<UnityEngine.Object>();  //바로 생성된 오브젝트 리스트에 로드 자산 관리 시키기 , 핸들 x
    private  List<UnityEngine.Object> AssetList = new List<UnityEngine.Object>();  //로드된 자산 관리 시키기 ,핸들 x

    public static List<AsyncOperationHandle<UnityEngine.Object>> handleList = new List<AsyncOperationHandle<UnityEngine.Object>>();  //핸들 저장해서 언로드 관리 시키기.
  
    //test 끝나면 헤더파일로 이동시키기
    string Inst_String = "(Clone)";  //InstList 찾을때 필요
    string Load_String= " (UnityEngine.GameObject)";  //List찾을때

    //자주 사용하실 함수 목록
    // 1. SingleAsset_Load<T>(string label, bool async=false, bool cashing = true, Action<T> Complete = null)

    //오른쪽부터 순서대로 (1.string 로드할 이름, 2.false=동기/true=비동기, 3.true=저장/false=저장 안하고 바로 객체 생성, 4.로드 후 실행할 함수)
    //동기로드와 저장을 기본으로 합니다. 그래서 평소 리소스 로드 하는거처럼 쓰고싶다 하시면
    //Single_Load_Task_Test<T>(string 로드할에셋 이름) 넣어주시면 됩니다. 
    //만약 비동기 로드를 원하신다면 async를 true로 , 저장없이 바로 Instantiate하는것처럼 오브젝트만 생성하고싶다면 caching 부분 false로 해주시면됩니다.
    //default로 사용하시면 상관 없는데 caching 없이 사용하실 때 주의하실게 한번만 생성하고 끝날거에만 사용해주세요 !
    //두번이상 생성해야 한다 라고 하면 cashing기본값으로 사용하시면 됩니다. 후에 오브젝트 풀링 적용해야 하는거도 cashing 기본값으로 사용

  
    //2. MultiAsset_Load<T>(List<string> keyList, bool async=false, Action Complete = null)

    //오른쪽부터 순서대로 (1.string 로드할 이름, 2.false=동기/true=비동기 , 3. 비동기 로드 후 실행할 함수)
    //앞에거랑 마찬가지로 동기를 기본으로 합니다.
    //비동기 다 필요없고 많은 양의 에셋을 한번에 동기로 로드하고 싶다면 MultiAsset_Load<T>(List<string> keyList) 쓰시면 됩니다. 비동기로 할거면 async만 true로 하십셔

    
    //3. 기존에 쓰던 Load_Name입니다. <T> 타입만 추가 되고 기존과 동작은 같습니다.

    
    //4. Instantiate_LoadObject<T>(string key,Vector3 vec=default(Vector3)) 
    // 리소스 로드했으면 생성하실때 쓰시면 됩니다. 오른쪽부터 이름, 위치 입니다. (T타입 반환)입니다. List<string>형식으로 넣어주셔도 됩니다. (멀티 로드 하시고 한번에 생성하고싶으실때 사용하셔도 됩니다) 마찬가지로 List<T> 반환

   
    //5. Find_InstantiateObj<T>(string key) 
    //만약 1번에서 캐싱없이 생성을 하셨는데 얘를 제어하고싶다. 하시면 key값에 로드 할때 썼던거 쓰세요 

   
    //6. Delete_Object<T>(T delete)
    //삭제하실때 이거로 삭제 부탁드립니다
    
    //ps. 멀티 객체 생성은 로드 말고는 잘 안쓸거라 생각해서 함수에 넣지는 않았는데 필요하시다면 바로 밑에 나온 함수 사용하시면됩니다. 그리고 더 필요하다 싶은 기능 있으시거나 함수 알아보기 힘들다 하시면 언제든 말씀해주십셔

    //label가져와서 바로 생성 시키기, 멀티 ,비동기
    public async Task InitAssets_label<T>(string label)
     where T : UnityEngine.Object

    {
        ErrorCode error = ErrorCode.None;

        if (!LoadCheck(label, out error))
        {
            Debug.Log("에러" + error);
            return;
        }

        Load_String_List.Add(label);  //로드되는 label

        Debug.Log("생성전" + label);



             var locations = await Addressables.LoadResourceLocationsAsync(label).Task;
            Debug.Log("생성가ㅣ져옴" + label);


            foreach (var location in locations)
            {
                var temp = await Addressables.InstantiateAsync(location).Task;
                InstList.Add(temp as T);  //List에 저장 (생성된 오브젝트)
                Load_String_List.Add(temp.ToString());  //로드되는 오브젝트 이름
                Debug.Log("생성" + temp);
            }
     
        return;

    }

    //단일 객체 바로 생성 /비동기  확인 완
    public async Task<T> InitAssets_Instantiate_async<T>(string name)
     where T : UnityEngine.Object
    {

        ErrorCode error = ErrorCode.None;

        if (!LoadCheck(name, out error))
        {
            Debug.Log("에러" + error);
            return null;
        }

        Load_String_List.Add(name); //로드되는 에셋 이름

        var temp = await Addressables.InstantiateAsync(name).Task;

        InstList.Add(temp as T);

        foreach (var t in InstList)
        {
            Debug.Log("List 리스트 들어감" + t);
        }

        return temp as T;
    }

    //단일 객체 바로 생성 /동기  확인 완
    public void InitAssets_Instantiate_sync<T>(string name)
     where T : UnityEngine.Object
    {

        ErrorCode error = ErrorCode.None;

        if (!LoadCheck(name, out error))
        {
            Debug.Log("에러" + error);
            return;
        }

        Load_String_List.Add(name); //로드되는 에셋 이름

      
        var temp =  Addressables.InstantiateAsync(name);
        var go = temp.WaitForCompletion();

        InstList.Add(go as T);

        foreach (var t in InstList)
        {
            Debug.Log("List 리스트 들어감" + t);
        }

        return;
    }

    //비동기 로드  확인 완  //단일 로드 (객체 생성아님
    public async Task InitAssets_name_async<T>(string object_name)
     where T : UnityEngine.Object

    {
        ErrorCode error = ErrorCode.None;

        if (!LoadCheck(object_name, out error))
        {
            Debug.Log("에러" + error);
            return;
        }
        Load_String_List.Add(object_name); //로드되는 에셋 이름

        Debug.Log("시작" + object_name);

        //Task te = Addressables.LoadAssetAsync<T>(object_name).Task;

        var temp = await Addressables.LoadAssetAsync<T>(object_name).Task;
        Debug.Log("가져옴" + temp);

      
        AssetList.Add(temp as T);
         
        Debug.Log("저장기다림");

        foreach (var t in AssetList)
        {
            Debug.Log("요소 출력: " + t);
        }

        return ; 

    }

    //동기 로드 단일 (객체 생성 아님)
    public void InitAssets_name_sync<T>(string object_name)
   where T : UnityEngine.Object

    {
        ErrorCode error = ErrorCode.None;

        if (!LoadCheck(object_name, out error))
        {
            Debug.Log("에러" + error);
            return;
        }
        Load_String_List.Add(object_name); //로드할때 쓰는 키 (중복 로딩 방지 위함)

        Debug.Log("시작" + object_name);

        //Task te = Addressables.LoadAssetAsync<T>(object_name).Task;

        var temp = Addressables.LoadAssetAsync<T>(object_name);
        T go = temp.WaitForCompletion();

        Debug.Log("가져옴" + temp);
        AssetList.Add(go);

        foreach (var t in AssetList)
        {
            Debug.Log("요소 출력: " + t);
        }
        return;
    }

    //동기 로드 멀티
    public void InitAssets_name_sync<T>(List<string> keyList)
   where T : UnityEngine.Object

    {
        ErrorCode error = ErrorCode.None;

        foreach(var t in keyList)
        {
            if (!LoadCheck(t, out error))
            {
                Debug.Log("에러" + error);
                return;
            }
            else
            {
                Load_String_List.Add(t); //로드되는 에셋 이름
                Debug.Log("시작" + t);
            }

            var temp = Addressables.LoadAssetAsync<T>(t);
            T go = temp.WaitForCompletion();

            Debug.Log("가져옴" + go);
            AssetList.Add(go);

        }
  
        foreach (var t in AssetList)
        {
            Debug.Log("요소 출력: " + t);
        }
        return;
    }



    //원본
    //동기 로드  확인 완  //단일 로드 (객체 생성아님
    //public async Task InitAssets_name_<T>(string object_name)
    // where T : UnityEngine.Object

    //{
    //    ErrorCode error = ErrorCode.None;

    //    if (!LoadCheck(object_name, out error))
    //    {
    //        Debug.Log("에러" + error);
    //        return;
    //    }
    //    Load_String_List.Add(object_name); //로드되는 에셋 이름

    //    Debug.Log("시작" + object_name);

    //    var temp = await Addressables.LoadAssetAsync<T>(object_name).Task;
    //    Debug.Log("가져옴" + object_name);

    //    AssetList.Add(temp);
    //    Debug.Log("저장" + temp);


    //    foreach (var t in AssetList)
    //    {
    //        Debug.Log("요소 출력: " + t);
    //    }

    //    return;

    //}

    //멀티 로드,리스트 ,비동기
    public async Task MultiLoadAsset<T>(List<string> keyList)
     where T : UnityEngine.Object

    {
        ErrorCode error = ErrorCode.None;

        foreach (var s in keyList)
        {
            if (!LoadCheck(s, out error))
            {
                Debug.Log("에러" + error);
                return;
            }
            else
            {
                Load_String_List.Add(s); //로드되는 에셋 이름

            }

            var temp = await Addressables.LoadAssetAsync<T>(s).Task;
            Debug.Log("가져옴" + temp.name);

            AssetList.Add(temp);
            Debug.Log("저장" + temp);

        }

        foreach (var t in AssetList)
        {
            Debug.Log("요소 출력: " + t);
        }

        return;

    }

    //단일 동기 - 생성
    //멀티 동기 - 생성


    //하나 로드 할때, 비동기면 false, 동기면 true  생성까지 되는거 아님.
    //로드할 이름, 동기 여부 (동기면 false,비동기면 true, 기본적으로 동기 설정), 저장 여부(저장만 할건지, 저장 없이 바로 객체만 생성 할건지), 로드만 하는 경우에는 Action 매개 변수 통해서 로드 된 객체 찾아 갈 수 있음.
    //주의점이 cashing 안하고 바로 생성하는 경우는 Gameobject 형식만 불러올수 있음.
    public async void SingleAsset_Load<T>(string label, bool async=false, bool caching = true, Action<T> Complete = null)
     where T : UnityEngine.Object
    {
        if (!async)  //동기여부 false면 동기
        {

            Debug.Log("시작하러 옴");
            if (caching)  //캐싱 여부 true 면 저장 (로드만 해오는거)
            {
                InitAssets_name_sync<T>(label);  //단일 로드만 해옴 동기

                T findobj = FindLoadAsset<T>(label);  //찾을거 반환해서 가져감.
                if (Complete != null)
                {
                    Complete(findobj); //동기 단일 에셋 로드 한게 Complete의 매개변수로 들어감.

                }
            }
            else  //false면 오브젝트만 바로 생성.
            {
                InitAssets_Instantiate_sync<T>(label);  //단일 객체 생성 동기
                //동기니까 따로 필요 없음.
            }
          
            Debug.Log("대기 끝");

        }
        else //async 동기 여부 true면 비동기
        {
            if (caching)  //로드해서 저장만.
            {
                await InitAssets_name_async<T>(label);  //단일로드 비동기 ,객체 저장
                
            }
            else  //저장 안하고 바로 생성
            {
                await InitAssets_Instantiate_async<T>(label);  //단일로드 비동기 객체 바로 생성

            }
            // Complete의 매개변수로 결과 반환
            T findobj = FindLoadAsset<T>(label);
            if (Complete != null)
            {
                Complete(findobj); //찾은거 
            }
        }

        return;
    }

    //이거를 생성하고 반환할수있는게 필요 =>Instantiate_LoadObject(string or List<string>)
    //하나 로드 할때, 비동기면 false, 동기면 true  생성까지 되는거 아님.
    //로드할 키(string) 리스트형식, 비동기는 true,동기는 false, 비동기 실행후 실행할 함수
    public async void MultiAsset_Load<T>(List<string> keyList, bool async=false, Action Complete = null)
     where T : UnityEngine.Object
    {
        if (!async)  //false 기본값일때 동기로드
        {

            Debug.Log("시작하러 옴");
            InitAssets_name_sync<T>(keyList);  //동기 멀티 로드

            //동기 멀티 로드 생성 저장x

            Debug.Log("대기 끝");

        }
        else  //비동기 로드
        {
            await MultiLoadAsset<T>(keyList);  //비동기 멀티 로드
            //비동기 멀티 로드 생성 저장x
            if(Complete!=null)
            {
                Complete();
            }
        }

        return;
    }

    //await안써도 되는거
    //하나 로드 할때, 비동기면 false, 동기면 true
    public async void Single_Load<T>(string label, bool sync,Action<T> Complete=null)
     where T : UnityEngine.Object

    {
        if(!sync)
        {
            await InitAssets_name_async<T>(label);
            T findobj = FindLoadAsset<T>(label);
          
            if(Complete!=null)
            {
                Complete(findobj); //동기 작업 하고싶은 내용 (밖에서 호출하고 await안쓰면 그냥 넘어감.

            }
        }
        else
        {
            InitAssets_name_async<T>(label);
            T findobj = FindLoadAsset<T>(label);
            if (Complete != null)
            {
                Complete(findobj); //동기 작업 하고싶은 내용 (밖에서 호출하고 await안쓰면 그냥 넘어감.

            }
        }
    }

    //하나 로드 할때  (Gameobject생성)
    public async void Single_Instantiate<T>(string key, bool sync, Action<T> action = null)
     where T : UnityEngine.Object

    {
        if(!sync)
        {
            await InitAssets_Instantiate_async<T>(key);
            T findobj = FindLoadAsset<T>(key);
            action(findobj);  //동기 작업 하고싶은 내용
        }
        else
        {
            InitAssets_Instantiate_async<T>(key);
            T findobj = FindLoadAsset<T>(key);
            action(findobj);   //비동기로 이루어짐 (비동기면 굳이 여기로 안넘기고 밖에서 호출해도 됌)
        }
    }

    //여러개 로드 할때
    public async void Multi_List_Load<T>(List<string> keyList,bool sync, Action action =null)
     where T : UnityEngine.Object
    {

        if (!sync)
        {
            await MultiLoadAsset<T>(keyList);
            action();  //동기 작업 하고싶은 내용
        }
        else
        {
            MultiLoadAsset<T>(keyList);
            action();  //동기 작업 하고싶은 내용
        }
    }

    //여러개 로드 label값 (Gameobject생성)
    public async void Multi_Lable_Instantiate<T>(string label, bool sync, Action action = null)
     where T : UnityEngine.Object
    {
        if(!sync)
        {
            await InitAssets_label<T>(label);
            action();  //동기 작업 하고싶은 내용
        }
        else
        {
            InitAssets_label<T>(label);
            action();  //동기 작업 하고싶은 내용
        }
    }


    //비동기 로드 한개,  함수에 로드 결과 가져가기
    public IEnumerator AsyncLoad_single<T>(string key, Action<T> Complete = null)
        where T : UnityEngine.Object
    {

        ErrorCode error = ErrorCode.None;

        if (!LoadCheck(key, out error))
        {
            Debug.Log("에러" + error);
            yield break;
        }

        Debug.Log("load");
        Load_String_List.Add(key);  //로드되는 label
        // AsyncOperationHandle<T> goHandle = Addressables.LoadAssetAsync<T>(key);
        AsyncOperationHandle<UnityEngine.Object> goHandle = Addressables.LoadAssetAsync<UnityEngine.Object>(key);
        //goHandle.Completed += Complete;

        yield return goHandle;
        if (goHandle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log(goHandle.Result.name);
            if(Complete!=null)
            {
                Complete(goHandle.Result as T);
            }
            Debug.Log(goHandle.Result + "가져옴");
            //T gameObject = temp.Result;
            handleList.Add(goHandle);
        }
    }

    //찾기
    private T FindLoadAsset<T>(string key)
     where T : UnityEngine.Object

    {

        Debug.Log("FindLoadAsset찾으러 들어옴");

        T findAsset = default;

        foreach (var t in InstList)
        {
            Debug.Log("InstList : " + t);

            if (key+Inst_String+ Load_String == t.ToString())
            {
                Debug.Log("InstList 발견" + t);
                findAsset = t as T;
                return findAsset;
            }
        }

        foreach (var t in AssetList)
        {
            Debug.Log("List : " + t.name);

            if (key == t.name)
            {
                Debug.Log("List 발견" + t);
                findAsset = t as T;
                return findAsset;
            }
        }

        foreach (var t in handleList)
        {
            Debug.Log("handleList : " + t);

            if (t.Result.name==key)
            {
                Debug.Log("handleList 발견"+t.Result.name);

                //핸들말고 결과만
                findAsset = t.Result as T;
                return findAsset;

            }
        }

        findAsset = Find_InstantiateObj<T>(key);
        if(findAsset!=null)
        {
            return findAsset;
        }

        //foreach (var t in handleIList)
        //{
        //  foreach(var e in t.Result)
        //    {
        //        Debug.Log("handleIList : " + e);

        //        if (e.name==key)
        //        {
        //            Debug.Log("handleIList 발견"+e.name);

        //            findAsset = e as T;
        //            return findAsset;

        //        }
        //    }
        //}
        return findAsset;
    }

    //밖에서 삭제 하고 싶을 때 string형으로 검색해서 삭제.
    //핸들 리턴해서 조작하지 않기 위함임. (언로드가 제대로 되지 않을 수도 있기 때문에 삭제하는 곳을 하나로 통합함. 비용이 많이 들수도 있겠지만... 최적화는 나중에..)
    public bool Delete_Object<T>(string delete)
   where T : UnityEngine.Object
    {
        var find_delete = FindLoadAsset<T>(delete);
        if(find_delete!=null)
        {
           bool result= Delete_Object<T>(find_delete);

            if(!result)
            {
                return false;
            }
            Load_String_List.Remove(delete);  //로드할때 쓰였던 이름삭제 (다시 로드 가능)
            return true;
        }

        return false;
    }

    //비동기 언로드 (리스트사용 하면 비동기가 필요할듯 싶지만 걍 코루틴..)
    public IEnumerator Delete_Object<T>(List<string> deleteList)
     where T : UnityEngine.Object

    {
        Debug.Log("최초삭제호출");

        
             foreach (string key in deleteList)
             {
            yield return Delete_Object<T>(key);
             }


        Debug.Log("최초삭제호출끝");
    }

    //삭제
    public bool Delete_Object<T>(T delete)
     where T : UnityEngine.Object

    {
        //OnRelease();

        Debug.Log("Delete_Object : " );

        //바로 생성된 객체들  확인 완
        if (InstList.Contains(delete))
        {
            Debug.Log("삭제 InstList : " + delete);

            Addressables.ReleaseInstance(delete as GameObject);
            //Addressables.Release(delete);
            InstList.Remove(delete);
            Load_String_List.Remove(delete.ToString());

            return true;
        }
        //로드 확인 완
        if (AssetList.Contains(delete))
        {
            Debug.Log("삭제 AssetList : " + delete);


            //Addressables.ReleaseInstance(delete as GameObject);
            Addressables.Release(delete);
            AssetList.Remove(delete);
            Load_String_List.Remove(delete.ToString());

            return true;
        }

        if (CreateObjectList.Contains(delete))
        {
            Debug.Log("삭제 CreateObjectList : " + delete);
            Destroy(delete);
            Load_String_List.Remove(delete.ToString());

            return true;
        }

        AsyncOperationHandle<UnityEngine.Object> temp = new AsyncOperationHandle<UnityEngine.Object>();
        bool result = false;

        foreach (var t in handleList)
        {
            Debug.Log("handleList : " + t);

            if (t.Result== delete)
            {
                Debug.Log("handleList 삭제" + t.Result.name);

                Addressables.Release(delete);
                Load_String_List.Remove(delete.ToString());

                temp = t ;
                result= true;

            }
        }

        if(result)
        {
            handleList.Remove(temp);
            return result;
        }
        //이거 요소 다 빠지면 해제 해줘야됌  OnRelease
        //foreach (var t in handleIList)
        //{
        //    foreach (var e in t.Result)
        //    {
        //        Debug.Log("handleIList : " + e);

        //        if (e == delete)
        //        {
        //            Debug.Log("handleIList 발견" + e.name);
        //            Addressables.ReleaseInstance(delete as GameObject);

        //            t.Result.Remove(e);
        //           // OnRelease();  이거 안댕
        //            return true;

        //        }
        //    }
        //}
        Debug.Log("찾진 못했고 그냥 Destroy");
        Destroy(delete);
        return false;

    }

    //멀티 로드를 했을때, 생성된 리스트를 반환하거나, 객체를 반환할수 있게끔. 근데 이게 핸들은 아니어야됌.

    //이미 로드 된 객체에서 복사 해서 반환함. string 형식으로 key값 받음.
    public T Instantiate_LoadObject<T>(string key,Vector3 vec=default(Vector3))  //바깥에서 string으로 반환
        where T : UnityEngine.Object
    {
        T findobj = FindLoadAsset<T>(key);
        T return_obj=null;
        if(findobj!=null)
        {
            return_obj=Instantiate(findobj, vec, Quaternion.identity);
            CreateObjectList.Add(return_obj); 
            Debug.Log("핸들에서 복사 반환");
            return return_obj;
        }
        else
        {
            //만약 로드 된 핸들이 아니라면, 생성된 오브젝트들 검색하고 반환.
            findobj = Find_InstantiateObj<T>(key);
            Debug.Log("생성된 오브젝트에서 복사 반환");
            
        }
        //그래도 널이면 없는것..
        return return_obj;
    }

    //리스트로 반환하기. 
    public List<T> Instantiate_LoadObject<T>(List<string> keyList, Vector3 vec = default(Vector3))  //바깥에서 string으로 반환
        where T : UnityEngine.Object
    {
        List<T> findobjList = new List<T>();

        foreach(var name in keyList)
        {
            T findobj = FindLoadAsset<T>(name);
            T return_obj = null;
            if (findobj != null)
            {
                return_obj = Instantiate(findobj, vec, Quaternion.identity);
                CreateObjectList.Add(return_obj);
                findobjList.Add(return_obj);
                Debug.Log("핸들에서 복사 반환");
               
            }
            else
            {
                //만약 로드 된 핸들이 아니라면, 생성된 오브젝트를 검색하고 반환.
                findobj = Find_InstantiateObj<T>(name);
                if(findobj!=null)
                {
                    findobjList.Add(return_obj);
                    Debug.Log("생성된 오브젝트에서 복사 반환");
                }
                else
                {
                    Debug.Log("원하는 객체를 찾을 수 없습니다. 로드 필요: "+name);
                }
            }
        }

        //그래도 널이면 없는것..
        return findobjList;

    }

    //핸들로 복사되어 생성된 객체를 저장해 놓은 리스트에서 찾기. ->저장하지 않은 객체들만 가능함. 또는 캐싱 없이 생성된 오브젝트만.
    public T Find_InstantiateObj<T>(string key)
        where T : UnityEngine.Object
    {
        T findobj = default;
        foreach(var t in CreateObjectList)
        {
            if(key+Inst_String==t.name)
            {
                Debug.Log(name + "CreateObjectList안에서 발견");
                findobj = t as T;
                return findobj;
            }
        }

        foreach (var t in InstList)
        {
            if (key + Inst_String+ Load_String == t.ToString())
            {
                Debug.Log(name + "InstList 발견");
                findobj = t as T;
                return findobj;
            }
        }
        return findobj;
    }

    //IList 다 빠지면 핸들 해제 해줘야 함 
    public void OnRelease()
    {
        Debug.Log("OnRelease실행");

        //foreach (var t in handleIList)
        //{
        //   if(t.Result.Count==0)
        //    {

        //        Addressables.Release(t );
        //        Debug.Log("핸들 삭제됌");

        //    }
        //    handleIList.Remove(t);

        //    return;
        //}
    }

    //잠시 핸들 확인하려고 하는거 (요소 다 빠지는지 확인)
    public void tempListCheck()
    {

        Debug.Log("Load_String_List조회");
        foreach (var t in Load_String_List)
        {
            Debug.Log("Load_String_List : " + t);
        }


        Debug.Log("AssetList조회");
        foreach (var t in AssetList)
        {
                Debug.Log("AssetList : " + t);
        }

        Debug.Log("InstList");
        foreach (var t in InstList)
        {
            Debug.Log("InsList : " + t);
        }

        Debug.Log("CreateObjectList");
        foreach (var t in CreateObjectList)
        {
            Debug.Log("CreateObjectList : " + t);
        }

        Debug.Log("handleList");
        foreach (var t in handleList)
        {
            Debug.Log("handleList : " + t);
        }
        
    }

    //로드 되었는지 체크
    public bool LoadCheck(string key,out ErrorCode error)
    {
        bool result = true;

       if(Load_String_List.Contains(key))
        {
            result = false;
            error = ErrorCode.Assets_Already_Loaded;
        }
       else
        {
            error = ErrorCode.None;
        }

        return result;
    }

    //ture같이 들어오면 삭제 수행  =>삭제 할때마다 해줘야 함 (고민좀)
    public bool LoadCheck(string key,bool remove)
    {
        if(remove)
        {
            if (Load_String_List.Contains(key))
            {
                Load_String_List.Remove(key);
                return true;
            }
            else
            {
                return false;
            }
        }

        //기본적으로 string일치하는거 있으면 false (이미 로드 or 로드요청 된거 있으면)
        bool result = true;

        if (Load_String_List.Contains(key))
        {
            result = false;
        }

        return result;
    }


    //기존에 Load_Name이랑 같음.
    public IEnumerator Load_Name<T>(string name, Transform parent,Action<T> Complete=null)
        where T : UnityEngine.Object
    {

        Debug.Log("로드" + name);

        if(Complete!=null)
        {
            yield return StartCoroutine(AsyncLoad_single<T>(name,Complete));
        }
        else
        {
            yield return StartCoroutine(AsyncLoad_single<T>(name));
        }
        var findobj = FindLoadAsset<T>(name);
        CreateObjectList.Add(Instantiate(findobj, parent.position, Quaternion.identity));

    }

    //델리게이트
    //public IEnumerator LoadGameObjectAndMaterial<T>(string name, Action<AsyncOperationHandle<T>> Complete)
    // where T : UnityEngine.Object

    //{
    //    Debug.Log("LoadGameObjectAndMaterial호출");



    //    if (Load_String_List.Contains(name))
    //    {
    //        Debug.Log("이미 로드된 파일입니다. 동일한 이름의 소스 이미 로드 요청되어있음.");
    //        yield return null;
    //    }
    //    else
    //    {
    //        Load_String_List.Add(name);
    //    }
    //    //같은거 로드 못하게 예외 처리 
    //    //못찾으면 로드,찾으면 리턴,
    //    GameObject findGameobj = AddressablesController.Instance.find_Asset_in_list(name);
    //    if (findGameobj != null)
    //    {
    //        Debug.Log("이미 로드된 파일입니다.");
    //        yield return null;
    //    }
    //    else
    //    {
    //        //Load a GameObject
    //        AsyncOperationHandle<T> goHandle = Addressables.LoadAssetAsync<T>(name);
    //        goHandle.Completed += Complete;
    //        yield return goHandle;
    //        if (goHandle.Status == AsyncOperationStatus.Succeeded)
    //        {
    //            T gameObject = goHandle.Result;
             
    //            Debug.Log(gameObject.name + "로드");
    //        }

    //    }
    //    ////Load a Material
    //    //AsyncOperationHandle<IList<IResourceLocation>> locationHandle = Addressables.LoadResourceLocationsAsync("materialKey");
    //    //yield return locationHandle;
    //    //AsyncOperationHandle<Material> matHandle = Addressables.LoadAssetAsync<Material>(locationHandle.Result[0]);
    //    //yield return matHandle;
    //    //if (matHandle.Status == AsyncOperationStatus.Succeeded)
    //    //{
    //    //	Material mat = matHandle.Result;
    //    //	//etc...
    //    //}

    //    //Use this only when the objects are no longer needed
    //    //Addressables.Release(goHandle);
    //    //Addressables.Release(matHandle);
    //}




    static SceneInstance m_LoadedScene;

    public static void OnSceneAction(string SceneName)
    {
        if (m_LoadedScene.Scene.name == null)
        {
            Addressables.LoadSceneAsync(SceneName, LoadSceneMode.Additive).Completed += OnSceneLoaded;
        }
        else
        {
            //Addressables.UnloadSceneAsync(m_LoadedScene).Completed += OnSceneUnloaded;
            Debug.Log("로드 실패");
        }
    }

    public static void OnUnloadedAction(string SceneName)
    {
        if (m_LoadedScene.Scene.name != null)
        {
            Addressables.UnloadSceneAsync(m_LoadedScene).Completed += OnSceneUnloaded;
        }
        else
        {
            Debug.Log("언로드 실패");
        }
    }

    public static void OnSceneUnloaded(AsyncOperationHandle<SceneInstance> obj)
    {
        switch (obj.Status)
        {
            case AsyncOperationStatus.Succeeded:
                m_LoadedScene = new SceneInstance();
                break;
            case AsyncOperationStatus.Failed:
                Debug.LogError("씬 언로드 실패: " /*+ addSceneReference.AssetGUID*/);
                break;
            default:
                break;
        }
    }

    public static void OnSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
    {
        switch (obj.Status)
        {
            case AsyncOperationStatus.Succeeded:
                m_LoadedScene = obj.Result;
                break;
            case AsyncOperationStatus.Failed:
                Debug.LogError("씬 로드 실패: " /*+ addSceneReference.AssetGUID*/);
                break;
            default:
                break;
        }
    }



}

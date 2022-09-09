using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineHelper : MonoBehaviour
{
    private static MonoBehaviour monoInstance;

    [RuntimeInitializeOnLoadMethod]
    private static void Initializer()
    {
        monoInstance = new GameObject($"[{nameof(CoroutineHelper)}]").AddComponent<CoroutineHelper>();
        DontDestroyOnLoad(monoInstance.gameObject);
    }

    public new static Coroutine StartCoroutine(IEnumerator coroutine)
    {
        return monoInstance.StartCoroutine(coroutine);
    }

    public new static void StopCoroutine(Coroutine coroutine)
    {
        monoInstance.StopCoroutine(coroutine);
    }
}


//using UnityEngine;
//using System.Collections; 

 

//public class CoroutineHandler : MonoBehaviour

//{

//    IEnumerator enumerator = null;

//    private void Coroutine(IEnumerator coro)
//    {
//        enumerator = coro;
//        StartCoroutine(coro);
//    }



//    void Update()
//    {
//        if (enumerator != null)
//        {
//            if (enumerator.Current == null)
//            {
//                Destroy(gameObject);
//            }
//        }
//    }



//    public void Stop()
//    {
//        StopCoroutine(enumerator.ToString());
//        Destroy(gameObject);
//    }



//    public static CoroutineHandler Start_Coroutine(IEnumerator coro)
//    {
//        GameObject obj = new GameObject("CoroutineHandler");
//        CoroutineHandler handler = obj.AddComponent<CoroutineHandler>();
//        if (handler)
//        {
//            handler.Coroutine(coro);
//        }
//        return handler;
//    }
//}
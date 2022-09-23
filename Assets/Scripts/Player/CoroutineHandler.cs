using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineHandler : MonoBehaviour
{
    private static MonoBehaviour monoinstance;

    [RuntimeInitializeOnLoadMethod]
    private static void Initializer()
    {
        monoinstance = new GameObject("CoroutimeHandler").AddComponent<CoroutineHandler>();
        DontDestroyOnLoad(monoinstance.gameObject);
    }

    public static Coroutine Start_Coroutine(IEnumerator cor)
    {
        return monoinstance.StartCoroutine(cor);
    }


    public static void Stop_Coroutine(Coroutine cor)
    {
        if (monoinstance != null)
        {
            monoinstance.StopCoroutine(cor);
        }
    }


}

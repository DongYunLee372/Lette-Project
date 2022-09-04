using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorTimeCounter
{
    public delegate void Invoker();
    public delegate void SInvoker(string s);
    public delegate void ObjInvoker(Object o);
    public IEnumerator Cor_TimeCounter(float time, Invoker invoker)
    {
        float starttime = Time.time;

        while (true)
        {
            if ((Time.time - starttime) >= time)
            {
                invoker.Invoke();
                yield break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public IEnumerator Cor_TimeCounter(float time, SInvoker invoker,string str ="")
    {
        float starttime = Time.time;

        while (true)
        {
            if ((Time.time - starttime) >= time)
            {
                invoker.Invoke(str);
                yield break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public IEnumerator Cor_TimeCounter(float time, ObjInvoker invoker, Object o)
    {
        float starttime = Time.time;

        while (true)
        {
            if ((Time.time - starttime) >= time)
            {
                invoker.Invoke(o);
                yield break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}

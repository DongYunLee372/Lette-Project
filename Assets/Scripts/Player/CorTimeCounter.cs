using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorTimeCounter
{
    public delegate void Invoker();
    public delegate void SInvoker(string s);
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

    public IEnumerator Cor_TimeCounter(float time, SInvoker invoker)
    {
        float starttime = Time.time;

        while (true)
        {
            if ((Time.time - starttime) >= time)
            {
                invoker.Invoke("");
                yield break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownloadProgress : MonoBehaviour
{
    public int downloadProgressInput;
    private int cachedDownloadProgressInput;
    public int downloadProgressOutput;
    
    // Start is called before the first frame update
    void Start()
    {
       // cachedDownloadProgressInput = 0;
        downloadProgressOutput = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(cachedDownloadProgressInput != downloadProgressInput)
        {
            downloadProgressOutput = downloadProgressInput;
            cachedDownloadProgressInput = downloadProgressInput;
        }
    }
}

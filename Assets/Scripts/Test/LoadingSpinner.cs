using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSpinner : MonoBehaviour
{

    public RectTransform rectTransform;
    public float speed;
    public TextMeshProUGUI displayPercent;
    public Slider slider;
    private int percentComplete;
    private int cachedPercentComplete;
    public DownloadProgress DownloadProgressScript;
    float timer;
    void Start()
    {
        percentComplete = 0;
      displayPercent.text="0";
        timer = 0f;

    }

void OnEnable()
    {
        percentComplete = 0;
    }

    // Update is called once per frame
    void Update()
    {
      
        //  rectTransform.Rotate(new Vector3(0, 0, -speed * Time.deltaTime));
        if (percentComplete!=DownloadProgressScript.downloadProgressOutput)
        {
            timer += Time.deltaTime;
            slider.value = Mathf.Lerp(DownloadProgressScript.downloadProgressOutput,1f, timer);
            Debug.Log(DownloadProgressScript.downloadProgressOutput.ToString() + "%");
            displayPercent.text = DownloadProgressScript.downloadProgressOutput.ToString() + "%";
            percentComplete = DownloadProgressScript.downloadProgressOutput;

        }    
    }
}

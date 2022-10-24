using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource bgmSource;
    public AudioSource effectSource;

    public AudioClip HitAudio;

    public AudioClip[] Bgm; // 0 : 메인로비 , 1 : 보스전 음악 
    public AudioClip[] Player_Audio; // 0 walk , 1 Hit

    public float bgmSave;
    public float effectSave;

    public GameObject option;

    void Start()
    {
        option = UIManager.Instance.Canvasreturn(Canvas_Enum.CANVAS_NUM.start_canvas);

        bgmSource.GetComponent<AudioSource>().volume = option.GetComponent<MainOption>().Backgroundsound * 0.01f;
        effectSource.GetComponent<AudioSource>().volume = option.GetComponent<MainOption>().Effectsound * 0.01f;

       
    }

    void Volume_Update()
    {
        bgmSource.GetComponent<AudioSource>().volume = option.GetComponent<MainOption>().Backgroundsound * 0.01f;
        effectSource.GetComponent<AudioSource>().volume = option.GetComponent<MainOption>().Effectsound * 0.01f ;
    }

    public void Volume_Save()
    {
        PlayerPrefs.SetFloat("bgmVolume", bgmSave);
        PlayerPrefs.SetFloat("effectVolume", effectSave);
    }

    void Update()
    {
        Volume_Update();
    }
}

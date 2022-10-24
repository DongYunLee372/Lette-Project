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

    [SerializeField]
    private Slider bgmSlider;

    [SerializeField]
    private Slider effectSlider;

    void Start()
    {
        if (PlayerPrefs.HasKey("bgmVolume"))
        {
            bgmSave = PlayerPrefs.GetFloat("bgmVolume");

            bgmSource.volume = bgmSave;
        }
        if (PlayerPrefs.HasKey("effectVolume"))
        {
            effectSave = PlayerPrefs.GetFloat("effectVolume");

            effectSource.volume = effectSave;
        }
    }

    void Volume_Update()
    {
        if (bgmSlider != null)
        {
            bgmSave = bgmSlider.value;
            bgmSource.volume = bgmSave;
        }
        if (effectSlider != null)
        {
            effectSave = effectSlider.value;
            effectSource.volume = effectSave;
        }
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

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    AudioSource bgmPlayer = null;
    float _effectVolume = 1.0f;

    public float BgmVolume
    {
        get => bgmPlayer.volume;
        set
        {
            PlayerPrefs.SetFloat("BGM_Volume", 1.0f - value);
            bgmPlayer.volume = value;
        }
    }

    public float EffectVolume
    {
        get => _effectVolume;
        set
        {
            PlayerPrefs.SetFloat("Effect_Volume", 1.0f - value);
            _effectVolume = value;
        }
    }

    private void Awake()
    {
        this.Initialize_DontDestroyOnLoad();                                    // 씬에 SoundManager가 유일한 객체로서 동작할 수 있도록 초기화
        if (bgmPlayer == null)
        {
            bgmPlayer = Camera.main.gameObject.AddComponent<AudioSource>();
        }

        bgmPlayer.volume = 1.0f - PlayerPrefs.GetFloat("BGM_Volume");           // default 값이 0이기 때문에 1.0f - value로 저장
        _effectVolume = 1.0f - PlayerPrefs.GetFloat("Effect_Volume");

    } 

    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        bgmPlayer.clip = clip;
        bgmPlayer.loop = loop;
        bgmPlayer.Play();
    }
}

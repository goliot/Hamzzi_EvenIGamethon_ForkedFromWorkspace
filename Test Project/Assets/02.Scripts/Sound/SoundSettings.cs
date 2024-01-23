using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    public Slider bgmSlider;

    void Start()
    {
        SoundManager.Inst.PlayBGM(Resources.Load("GAME_MAIN_BGM_01") as AudioClip);
        //bgmSlider.value = SoundManager.Inst.BgmVolume;

    }

    public void OnChangeBGMVolume(float v)
    {
        SoundManager.Inst.BgmVolume = v;
        // PlayerPrefs에 저장하는 로직이 필요하다면 여기에 추가할 수 있습니다.
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;

    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = AudioManager.Inst;
    }

    void Start()
    {
        if (bgmSlider != null)
        {
            bgmSlider.value = audioManager.GetVolume(AudioManager.AudioType.BGM);
            bgmSlider.onValueChanged.AddListener(value => audioManager.OnVolumeChanged(AudioManager.AudioType.BGM, value));
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = audioManager.GetVolume(AudioManager.AudioType.SFX);
            sfxSlider.onValueChanged.AddListener(value => audioManager.OnVolumeChanged(AudioManager.AudioType.SFX, value));
        }
    }
}

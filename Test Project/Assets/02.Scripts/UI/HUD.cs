using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, Level, Time, Health, Wave, ChapterStage, Seed}
    public InfoType type;

    TextMeshProUGUI lvText;
    TextMeshProUGUI timeText;
    TextMeshProUGUI waveText;
    TextMeshProUGUI chapterStageText;
    TextMeshProUGUI seedText;
    Slider expSlider;
    Slider hpSlider;

    public Spawner spawner;

    private void Awake()
    {
        lvText = GetComponent<TextMeshProUGUI>();
        timeText = GetComponent<TextMeshProUGUI>();
        waveText = GetComponent<TextMeshProUGUI>();
        chapterStageText = GetComponent<TextMeshProUGUI>();
        seedText = GetComponent<TextMeshProUGUI>();
        expSlider = GetComponent<Slider>();
        hpSlider = GetComponent<Slider>();
    }

    private void Start()
    {
        spawner = FindObjectOfType<Spawner>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                float curExp = GameManager.Inst.exp;
                float maxExp = GameManager.Inst.nextExp[GameManager.Inst.level];
                expSlider.value = curExp / maxExp;
                break;
            case InfoType.Level:
                lvText.text = string.Format("Lv.{0:F0}", GameManager.Inst.level); // 문자열 보간, 0번째 인자 값 
                break;
            case InfoType.Time:
                float time = GameManager.Inst.gameTime;
                int min = Mathf.FloorToInt(time / 60); // 버림 함수 사용
                int sec = Mathf.FloorToInt(time % 60);
                timeText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
            case InfoType.Health:
                float curHealth = GameManager.Inst.wall.health;
                float maxHealth = GameManager.Inst.wall.maxHealth;
                hpSlider.value = curHealth / maxHealth;
                break;
            case InfoType.Wave:
                waveText.text = string.Format("WAVE : {0:D2} / {1:D2}", spawner.currentWave ,spawner.maxWave);
                break;
            case InfoType.ChapterStage:
                if (StageSelect.instance == null) break;
                chapterStageText.text = string.Format("STAGE {0:F0} - {1:F0}", StageSelect.instance.chapter, StageSelect.instance.stage);
                break;
            case InfoType.Seed:
                seedText.text = string.Format($"{GameManager.Inst.seed}");
                break;
        }
    }
}

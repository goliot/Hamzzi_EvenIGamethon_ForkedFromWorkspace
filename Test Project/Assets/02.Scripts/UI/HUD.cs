using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, Level, Time, Health}
    public InfoType type;

    TextMeshProUGUI lvText;
    TextMeshProUGUI timeText;
    Slider expSlider;

    private void Awake()
    {
        lvText = GetComponent<TextMeshProUGUI>();
        timeText = GetComponent<TextMeshProUGUI>();
        expSlider = GetComponent<Slider>();
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
                break;

        }
    }
}

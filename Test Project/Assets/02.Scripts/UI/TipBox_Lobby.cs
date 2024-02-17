using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TipBox_Lobby : MonoBehaviour
{
    public TextMeshProUGUI tipText;

    private List<string> tips = new List<string>();

    private void Awake()
    {
        tips.Add("Tip! 햄찌를 옮겨봐!");
        tips.Add("Tip! 광고 싫쮸？ 상점으로ㄱㄱ");
        tips.Add("Tip! 햄찌를 움직이면 귀여운 일이 발생해오!");
        tips.Add("Tip! 도감을 봐줄 사람 어디 없나");
        tips.Add("Tip! 스토리 속 이스터에그를 찾아보세요!");
        tips.Add("Tip! 옥수수로 많은 걸 살 수 있다는데...?");
        tips.Add("Tip! 해바라기씨가 모이면? 용병 소환 가능!");
        tips.Add("Tip! 게임을 정지하면 튜토리얼이!");
        tips.Add("Tip! 배속할 수록 재미도 1.5배!");
        tips.Add("Tip! 햄찌 일상 대공개 → @magic_hamzzi");
        tips.Add("Tip! 유튜브에 햄찌 용병단 검색 ㄱㄱ");
    }

    private void Start()
    {
        InvokeRepeating("ShowTips", 0, 4);
    }

    private void ShowTips()
    {
        int idx = Random.Range(0, tips.Count);

        tipText.text = tips[idx];
        TMPDOText(tipText, 2.0f);
    }

    public void TMPDOText(TextMeshProUGUI text, float duration)
    {
        text.maxVisibleCharacters = 0;
        DOTween.To(x => text.maxVisibleCharacters = (int)x, 0f, text.text.Length, duration);
    }
}

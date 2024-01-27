using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    [SerializeField]
    public enum Chapter { Chapter1 = 1, Chapter2, Chapter3, Chapter4 };

    RectTransform rect;
    Card[] cards;
    public Player player;

    public Chapter chapter;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        cards = GetComponentsInChildren<Card>();

        ActivateByChapter();
    }

    private void ActivateByChapter()
    {
        StageSelect stageSelect = StageSelect.instance;

        // 현재 챕터에 따라 활성화 여부 결정
        bool shouldActivate = ((int)chapter == stageSelect.chapter);

        // 활성화 여부에 따라 오브젝트 활성화 또는 비활성화
        gameObject.SetActive(shouldActivate);

        if (shouldActivate)
        {
            Debug.Log($"챕터 {chapter}의 카드 덱");
        }
    }

    /*
    private void ActivateByChapter()
    {
        StageSelect stageSelect = StageSelect.instance;

        // 챕터에 따라 활성화 여부 결정
        switch (chapter)
        {
            case Chapter.Chapter1:
                if (stageSelect.chapter == 1)
                {
                    Debug.Log("챕터 1의 카드 덱");
                    gameObject.SetActive(true);
                }
                else
                {
                    gameObject.SetActive(false);
                }
                break;
            case Chapter.Chapter2:
                if (stageSelect.chapter == 2)
                {
                    Debug.Log("챕터 2의 카드 덱");
                    gameObject.SetActive(true);
                }
                else
                {
                    gameObject.SetActive(false);
                }
                break;
            case Chapter.Chapter3:
                if (stageSelect.chapter == 3)
                {
                    Debug.Log("챕터 3의 카드 덱");
                    gameObject.SetActive(true);
                }
                else
                {
                    gameObject.SetActive(false);
                }
                break;
            case Chapter.Chapter4:
                if (stageSelect.chapter == 4)
                {
                    Debug.Log("챕터 4의 카드 덱");
                    gameObject.SetActive(true);
                }
                else
                {
                    gameObject.SetActive(false);
                }
                break;
            default:
                gameObject.SetActive(false);
                break;
        }
    }*/

    public void Show()
    {
        Next();
        Debug.Log(chapter);
        rect.localScale = Vector3.one * 1.3f;
        GameManager.Inst.Stop();
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.Inst.Resume();
    }

    void Next()
    {
        // 1. 모든 카드 비활성화
        foreach (Card card in cards)
        {
            card.gameObject.SetActive(false);
        }

        // 2. 그중에서 랜덤 3개 카드 활성화
        int[] rnd = new int[3];

        if (rnd.Length > cards.Length) // 고를 아이템의 수를 모든 아이템 종류수보다 크게 설정하면 오류 발생
        {
            Debug.LogError("");
            return;
        }

        for (int idx = 0; idx < rnd.Length; idx++)
        {
            Card rndCard;
            do
            {
                rnd[idx] = Random.Range(0, cards.Length);
                rndCard = cards[rnd[idx]];
            } while (IsDuplicate(rnd, idx) || !IsValidCard(rndCard));

            rndCard.gameObject.SetActive(true);
        }
        
    }

    // 중복 체크
    bool IsDuplicate(int[] array, int currentIndex)
    {
        for (int i = 0; i < currentIndex; i++)
        {
            if (array[i] == array[currentIndex])
            {
                return true;
            }
        }
        return false;
    }

    // 유효한 카드 체크
    bool IsValidCard(Card card)
    {
        return !card.cardData.isLocked && !card.cardData.noExplosion && !card.cardData.noPenetration && card.level < card.cardData.levels.Length;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Card[] cards;
    public Player player;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        cards = GetComponentsInChildren<Card>();
    }

    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
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

        bool isSkill1Unlocked = player.playerData[0].isUnlocked; // 1번 스킬 해금 여부
        bool isSkill3Unlocked = player.playerData[2].isUnlocked; // 3번 스킬 해금 여부
        bool isSkill5Unlocked = player.playerData[4].isUnlocked; // 5번 스킬 해금 여부
        bool isSkill6Unlocked = player.playerData[5].isUnlocked; // 6번 스킬 해금 여부

        // 더블업 체크
        //if ((isSkill1Unlocked && isSkill5Unlocked) || (isSkill3Unlocked && isSkill6Unlocked))
        {
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
        //else
        //{
        //    // 다시 뽑는 로직 추가
        //    Debug.Log("해금 조건이 맞지 않습니다. 다시 뽑습니다.");
        //    Next();
        //}
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

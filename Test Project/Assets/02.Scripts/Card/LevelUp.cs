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

        while (true)
        {
            rnd[0] = Random.Range(0, cards.Length);
            rnd[1] = Random.Range(0, cards.Length);
            rnd[2] = Random.Range(0, cards.Length);

            if (rnd[0] != rnd[1] && rnd[1] != rnd[2] && rnd[0] != rnd[2]) break; // 중복 카드 제거
        }

        for (int idx = 0; idx < rnd.Length; idx++)
        {
            Card rndCard = cards[rnd[idx]];

            if (rndCard.level == rndCard.cardData.levels.Length)
            {
                // 만렙인 경우 다시 뽑기
                do
                {
                    rnd[idx] = Random.Range(0, cards.Length);
                } while (rndCard.level == rndCard.cardData.levels.Length);

                rndCard = cards[rnd[idx]];
            }
            else rndCard.gameObject.SetActive(true);
        }
        /*
        rnd[0] = Random.Range(0, cards.Length);
        for (int i = 0; i < rnd.Length; i++)
        {
            do
            {
                // 카드 뽑기 전에 해금됬는지 체크하는 로직
                

                isTry = false;
                rnd[i] = Random.Range(0, cards.Length);     // n번째 아이템을 고르고
                if (check[rnd[i]]) isTry = true;            // 중복되는 게 있다면 다시 하고
                else check[rnd[i]] = true;
            } while (isTry);
        }


        for (int idx = 0; idx < rnd.Length; idx++)
        {
            Card rndCard = cards[rnd[idx]];

            // 3. 만렙 카드의 경우 더 이상 뜨지 않게
            if (rndCard.level == rndCard.cardData.levels.Length)
            {
                // 카드가 모두 만렙인 경우
                Debug.Log(rndCard.level); // 디버깅

                // 추가 로직 구현 필요 (체력회복등의 카드 업그레이드와 상관없는 패시브 카드를 넣으면 가장 좋음)
            }
            else
            {
                rndCard.gameObject.SetActive(true);
            }
        }*/

    }



}

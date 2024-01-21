using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlock : MonoBehaviour
{
    public CardData[] cardDatas;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        foreach (CardData cardData in cardDatas)
        {
            if (cardData.cardId == 2)
            {
                cardData.noExplosion = true;
            }
            else if (cardData.cardId == 3)
            {
                cardData.noPenetration = true;
            }
            else
            {
                cardData.isLocked = true;
            }
        }

        // 마력구 Pn Type 체크를 막는 코드
        if (this.gameObject.GetComponent<Card>().cardData.cardType == CardData.CardType.MagicBolt)
        {
            this.gameObject.GetComponent<Card>().cardData.isLocked = false;
        }
    }

    public void UnLockSkills()
    {
        foreach(CardData cardData in cardDatas)
        {
            cardData.isLocked = false;
        }
    }

    public void UnLockExplosion()
    {
        foreach (CardData cardData in cardDatas)
        {
            cardData.noExplosion = false;
        }
    }

    public void UnLockPenetration()
    {
        foreach (CardData cardData in cardDatas)
        {
            cardData.noPenetration = false;
        }
    }
}

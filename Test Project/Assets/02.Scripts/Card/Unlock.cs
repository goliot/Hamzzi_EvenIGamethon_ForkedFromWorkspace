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
                if (cardData.cardType != (int)CardData.CardType.MagicBolt)
                {

                    cardData.isLocked = true;
                }
                else
                {
                    if (cardData.cardId == 8)
                    {
                        cardData.isLocked = true;
                    }
                    else
                    {
                        cardData.isLocked = false;
                    }
                }
            }
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

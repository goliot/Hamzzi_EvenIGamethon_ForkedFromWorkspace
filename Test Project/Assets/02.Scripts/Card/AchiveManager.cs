using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardArray
{
    public GameObject[] card;
}

public class AchiveManager : MonoBehaviour
{
    public Player player;
    public CardArray[] unlockCards;           // 해금 상태의 전체 카드들

    // 6개의 획득 해금과 폭발, 관통 추가 상태의  해금을 열거형으로 관리
    // 우선 획득 해금부터 구현
    enum Achive { UnlockBoom, UnlockAqua }
    Achive[] achives;

    private void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive));

        Init();                                                    // 배틀 씬이 시작될 때마다 카드 해금 상태는 초기화
    }

    void Init()
    {
        foreach (Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);   // 각 스킬 
        }
    }

    private void Start()
    {
        UnlockCard();
    }

    void UnlockCard()
    {
        for (int idx = 0; idx < unlockCards.Length; idx++)
        {
            string achiveName = achives[idx].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;

            for (int j = 0; j < unlockCards[idx].card.Length; j++)
            {
                unlockCards[idx].card[j].SetActive(isUnlock);
            }
        }
    }

    private void LateUpdate()
    {
        foreach (Achive achive in achives)
        {
            CheckAchive(achive);
        }
    }

    void CheckAchive(Achive achive)
    {
        bool isAchive = false;

        switch (achive)
        {
            case Achive.UnlockBoom:
                isAchive = player.playerData[1].isUnlocked == true;
                break;
            case Achive.UnlockAqua:
                isAchive = player.playerData[2].isUnlocked == true;
                break;
        }

        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0) // 해당 업적이 처음 달성했다는 조건
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);
        }
    }
}

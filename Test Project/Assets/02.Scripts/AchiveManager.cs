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
    public CardArray[] unlockCards;           // 해금 상태의 전체 카드들

    // 6개의 획득 해금과 폭발, 관통 추가 상태의  해금을 열거형으로 관리
    // 우선 획득 해금부터 구현
    enum Achive { UnlockBoom, UnlockAqua }
    Achive[] achives;

    private void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive));
    }

}

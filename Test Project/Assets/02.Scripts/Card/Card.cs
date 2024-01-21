using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class Card : MonoBehaviour
{
    [SerializeField] GameObject cardMenu;

    public CardData cardData;
    public int level;
    public Player player;

    Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;

    private void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = cardData.cardIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        textName.text = cardData.cardName;
    }

    // 레벨 텍스트 로직
    private void OnEnable()
    {
        textLevel.text = "Lv." + (level + 1);

        textDesc.text = string.Format(cardData.cardDesc);
    }

    public void OnClick()
    {
        switch (cardData.cardType)
        {
            case CardData.CardType.MagicBolt:
                
                // 마력구 스킬 2번 카드 증폭(폭발)  : 폭발 피해 +30%
                
                

                // 마력구 스킬 3번 카드 관통        : 관통+2 / 피해 +10%
                if (cardData.cardId == 3)
                {
                    player.playerData[(int)cardData.cardType].penetrate += 2;
                    player.playerData[(int)cardData.cardType].damage *= 1.1f;
                    Debug.Log("damage : " + player.playerData[(int)cardData.cardType].damage + " penetrate : + " + player.playerData[(int)cardData.cardType].penetrate);
                }
                // 마력구 스킬 7번 더블 증폭        : 액서니아와 피해량 +40%
                else if (cardData.cardId == 7)
                {
                    player.playerData[(int)cardData.cardType].damage *= 1.4f;
                    player.playerData[(int)CardData.CardType.Aegs].damage *= 1.4f;
                    Debug.Log("Mag damage : " + player.playerData[(int)cardData.cardType].damage);
                    Debug.Log("Aegs damage : " + player.playerData[(int)CardData.CardType.Aegs].damage);
                }
                break;
            // 각 케이스 안에 1~6번 카드 중 어떤 카드인지 검사
            case CardData.CardType.Boom:
                // 해금을 하는 시스템
                // 레벨로 제어하는게 편할 것 같다. level0 일때는 playerdata 안에서 비활성화 상태이다가 여기서 클릭되서 레벨 1이 되면 아래 로직 활성화
                // playerData의 isUnlock 불린값 활용

                // 스킬 4번 스킬 지속시간 증가
                if (cardData.cardId == 4)
                {
                    player.playerData[(int)cardData.cardType].duration += 2f;
                    Debug.Log($"duration : {player.playerData[(int)cardData.cardType].duration}");
                }
                if (cardData.cardId == 3)
                {
                    player.playerData[(int)cardData.cardType].penetrate += 1;
                    player.playerData[(int)cardData.cardType].damage *= 1.2f;
                    Debug.Log("damage : " + player.playerData[(int)cardData.cardType].damage + " penetrate : + " + player.playerData[(int)cardData.cardType].penetrate);
                }
                else if (cardData.cardId == 8)
                {
                    player.playerData[(int)cardData.cardType].isUnlocked = true;
                }
                break;

            case CardData.CardType.Aqua:

                if (cardData.cardId == 4)
                {
                    player.playerData[(int)cardData.cardType].duration += 2.7f;
                    Debug.Log($"duration : {player.playerData[(int)cardData.cardType].duration}");
                }
                else if (cardData.cardId == 5)
                {
                    player.playerData[(int)cardData.cardType].splashRange *= 1.15f;
                    Debug.Log("splashRange : " + player.playerData[(int)cardData.cardType].splashRange);
                }
                else if (cardData.cardId == 7)
                {
                    player.playerData[(int)cardData.cardType].duration += 2.1f;
                    player.playerData[(int)CardData.CardType.Aegs].duration += 2.1f;
                }
                else if (cardData.cardId == 8)
                {
                    player.playerData[(int)cardData.cardType].isUnlocked = true;
                }
                break;
            case CardData.CardType.Lumos:
                if (cardData.cardId == 4)
                {
                    player.playerData[(int)cardData.cardType].duration += 2f;
                    Debug.Log($"duration : {player.playerData[(int)cardData.cardType].duration}");
                }
                else if (cardData.cardId == 8)
                {
                    player.playerData[(int)cardData.cardType].isUnlocked = true;
                }
                break;
            case CardData.CardType.Aegs:
                if (cardData.cardId == 4)
                {
                    player.playerData[(int)cardData.cardType].duration += 2.1f;
                    Debug.Log($"duration : {player.playerData[(int)cardData.cardType].duration}");
                }
                else if (cardData.cardId == 5)
                {
                    player.playerData[(int)cardData.cardType].atkRange *= 1.15f;
                    Debug.Log("atkRange : " + player.playerData[(int)cardData.cardType].atkRange);
                }
                else if (cardData.cardId == 8)
                {
                    player.playerData[(int)cardData.cardType].isUnlocked = true;
                }
                break;
            case CardData.CardType.Momen:
                if (cardData.cardId == 4)
                {
                    player.playerData[(int)cardData.cardType].duration += 2.7f;
                    Debug.Log($"duration : {player.playerData[(int)cardData.cardType].duration}");
                }
                else if (cardData.cardId == 5)
                {
                    player.playerData[(int)cardData.cardType].splashRange *= 1.15f;
                    Debug.Log("splashRange : " + player.playerData[(int)cardData.cardType].splashRange);
                }
                else if (cardData.cardId == 8)
                {
                    player.playerData[(int)cardData.cardType].isUnlocked = true;
                }
                break;
            case CardData.CardType.Pines:
                // 공통
                // 스킬 1번 카드 증폭(피해)  : 피해량 +60 %
                if (cardData.cardId == 1)
                {
                    player.playerData[(int)cardData.cardType].damage *= 1.6f;
                    Debug.Log($"damage : {player.playerData[(int)cardData.cardType].damage}");
                }
                else if (cardData.cardId == 2)
                {
                    player.playerData[(int)cardData.cardType].explodeDamage *= 1.3f;
                    Debug.Log("explodeDamage : " + player.playerData[(int)cardData.cardType].explodeDamage);
                }
                else if (cardData.cardId == 6)
                {
                    player.playerData[(int)cardData.cardType].atkSpeed *= 0.8f;
                    Debug.Log($"atkSpeed : {player.playerData[(int)cardData.cardType].atkSpeed}");
                }
                // 해금
                else if (cardData.cardId == 8)
                {
                    player.playerData[(int)cardData.cardType].isUnlocked = true;
                }
                else if (cardData.cardId == 9)
                {
                    // 폭발 추가
                }
                else if (cardData.cardId == 10)
                {
                    // 관통 추가
                }

                // 피네스타만 분기 걸림
                else if (cardData.cardId == 3)  
                {
                    player.playerData[(int)cardData.cardType].penetrate += 1;
                    player.playerData[(int)cardData.cardType].damage *= 1.2f;
                    Debug.Log("damage : " + player.playerData[(int)cardData.cardType].damage + " penetrate : + " + player.playerData[(int)cardData.cardType].penetrate);
                }

                break;
        }

        level++;

        // 각 카드별 제한 개수를 두지 않을 것인가? 무한으로 선택지가 나오게 할 것인가?
        if(level == cardData.levels.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }

    /// <summary>
    /// 카드 선택했을 때, 카드 메뉴창 끄고 게임 재개
    /// 카드 선택 시 스탯 업데이트 하는 로직 설계 필요
    /// </summary>
    public void ChooseCard()
    {
        cardMenu.SetActive(false);
        Time.timeScale = 1;
    }


}

/*[System.Serializable]
public class CardData2
{
    public int cardId;
    public int skillId;
    public float damageUp;
    public int penetrateUp;
    public string desc;
}*/
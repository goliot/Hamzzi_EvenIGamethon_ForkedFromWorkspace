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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1)) // 테스트용
        {
            cardMenu.SetActive(true);
            Time.timeScale = 0;
        }
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
                // 마력구 스킬 1번 카드 증폭(피해)  : 피해량 +60%
                if(cardData.cardId == 1)
                {
                    player.playerData[0].damage *= 1.6f;
                    Debug.Log("damage : " + player.playerData[0].damage);
                }
                // 마력구 스킬 2번 카드 증폭(폭발)  : 폭발 피해 +30%    
                
                // 마력구 스킬 3번 카드 관통        : 관통+2 / 피해 +10% 
                else if (cardData.cardId == 3)
                {
                    player.playerData[0].penetrate += 2;
                    player.playerData[0].damage *= 1.1f;
                    Debug.Log("damage : " + player.playerData[0].damage + " penetrate : + " + player.playerData[0].penetrate);
                }

                // 마력구 스킬 4번 카드 쿨타임 감소 : -20%
                else if (cardData.cardId == 4)
                {
                    player.playerData[0].atkSpeed *= 1.2f;
                }
                // 마력구 스킬 5번 카드 더블 증폭   : 액서니아와 피해량 +40%
                // 마력구 스킬 6번 카드 추가        : 폭발 / 관통 (이 선택지가 해금 되어야지 2번과 3번 선택지가 나온다

                // 각 케이스 안에 1~6번 카드 중 어떤 카드인지 검사
                break;
            case CardData.CardType.Boom:
                // 해금을 하는 시스템
                // 레벨로 제어하는게 편할 것 같다. level0 일때는 playerdata 안에서 비활성화 상태이다가 여기서 클릭되서 레벨 1이 되면 아래 로직 활성화

                break;
            case CardData.CardType.Aqua:
                break;
            case CardData.CardType.Lumos:
                break;
            case CardData.CardType.Exo:
                break;
            case CardData.CardType.Momen:
                break;
            case CardData.CardType.Fines:
                break;
        }

        level++;

        // 각 카드별 제한 개수를 두지 않을 것인가? 무한으로 선택지가 나오게 할 것인가?
        if(level == cardData.damages.Length)
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
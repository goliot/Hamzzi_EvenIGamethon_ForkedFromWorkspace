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

    /*public List<CardData2> cardData2 = new List<CardData2>();
    string xmlFileName = "CardData";

    private void Start()
    {
        LoadXML(xmlFileName);
    }

    private void LoadXML(string _fileName)
    {
        TextAsset txtAsset = (TextAsset)Resources.Load(_fileName);
        if (txtAsset == null)
        {
            Debug.LogError("Failed to load XML file: " + _fileName);
            return;
        }

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(txtAsset.text);

        // 전체 아이템 가져오기 예제.
        XmlNodeList all_nodes = xmlDoc.SelectNodes("root/Sheet1");
        foreach (XmlNode node in all_nodes)
        {
            CardData2 newData = new CardData2();

            newData.cardId = int.Parse(node.SelectSingleNode("cardId").InnerText);
            newData.skillId = int.Parse(node.SelectSingleNode("skillId").InnerText);
            newData.damageUp = float.Parse(node.SelectSingleNode("damageUp").InnerText);
            newData.penetrateUp = int.Parse(node.SelectSingleNode("penetrateUp").InnerText);
            newData.desc = node.SelectSingleNode("Desc").InnerText;

            cardData2.Add(newData);
        }
    }*/

    private void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = cardData.cardIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1)) // 테스트용
        {
            cardMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void LateUpdate()
    {
        textLevel.text = "Lv." + (level + 1);
    }

    public void OnClick()
    {
        switch (cardData.cardType)
        {
            case CardData.CardType.MagicBolt:
                // 마력구 스킬 1번 카드 증폭(피해)  : 피해량 +60%
                // 마력구 스킬 2번 카드 증폭(폭발)  : 폭발 피해 +30%    
                // 마력구 스킬 3번 카드 관통        : 관통+2 / 피해 +10% 
                // 마력구 스킬 4번 카드 쿨타임 감소 : -20%
                // 마력구 스킬 5번 카드 더블 증폭   : 액서니아와 피해량 +40%
                // 마력구 스킬 6번 카드 추가        : 폭발 / 관통 (이 선택지가 해금 되어야지 2번과 3번 선택지가 나온다
                player.playerData[0].damage *= 1.6f;
                //각 케이스 안에 1~6번 카드 중 어떤 카드인지 검사
                break;
            case CardData.CardType.Boom:
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
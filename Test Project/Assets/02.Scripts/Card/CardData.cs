using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class CardData : ScriptableObject
{
    public enum CardType { MagicBolt, Boom, Aqua, Lumos, Aegs, Momen, Pines}

    [Header("# Main Info")]
    public CardType cardType;
    public int cardId;          // 카드의 ID
    public string cardName;     // 카드 이름
    public bool isLocked;       // 카드 잠겨 있는지
    public bool noExplosion;    // 폭발 스킬 해금 안됨
    public bool noPenetration;  // 투과 스킬 해금 안됨

    [TextArea]                  // Inspector에 텍스트를 여러 줄 넣을 수 있게하는 Attribute 
    public string cardUpg;      // 업그레이드 내용
    [TextArea]                   
    public string cardDesc;     // 아이템 설명
    public Sprite cardIcon;     // 아이템의 UI를 담기위한 아이콘

    [Header("# Level Data")]
    public float[] levels;     // 이 카드의 속성에 해당하는 부분 (살리기)

    private void Awake()
    {
        isLocked = false;
        noExplosion = false;
        noPenetration = false;
    }
}

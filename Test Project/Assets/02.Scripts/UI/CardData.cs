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
    [TextArea]                  // Inspector에 텍스트를 여러 줄 넣을 수 있게하는 Attribute 
    public string cardDesc;     // 아이템 설명
    public Sprite cardIcon;     // 아이템의 UI를 담기위한 아이콘

    // 제거 필요
    #region
    [Header("# Level Data")]
    // public float baseDamage;    // 0레벨 기준 기본 공격력
    // public int baseCount;       // 0레벨 기준 개수
    public float[] levels;     // 이 카드의 속성에 해당하는 부분 (살리기)
    // public int[] counts;
    #endregion
}

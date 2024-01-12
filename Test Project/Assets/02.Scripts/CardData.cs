using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : ScriptableObject
{
    [Header("# Main Info")] // 
    public int cardId; // 카드의 ID
    public string cardName;
    public string cardDesc; // 아이템 설명
    public Sprite cardIcon; // 아이템의 UI를 담기위한 아이콘
    
    //[Header("# Level Data")]

    //[Header("# Weapon?")] // 명칭 변경 추후 필요
}

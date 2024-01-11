using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] GameObject cardMenu;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1)) // 테스트용
        {
            cardMenu.SetActive(true);
            Time.timeScale = 0;
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

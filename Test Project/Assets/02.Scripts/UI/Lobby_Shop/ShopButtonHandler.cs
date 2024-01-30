using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButtonHandler : MonoBehaviour
{
    public Button bottonCorn01;
    public Button bottonCorn05;
    public Button bottonCorn10;

    void Start()
    {
        Init();
    }

    void Init()
    {
        if (bottonCorn01 != null)
        {
            bottonCorn01.onClick.AddListener(() =>
            {
                // 옥수수 한개 사는 함수 추가
            });
        }

        if (bottonCorn05 != null)
        {
            bottonCorn05.onClick.AddListener(() =>
            {
                // 옥수수 다섯개 사는 함수 추가
            });
        }

        if (bottonCorn10 != null)
        {
            bottonCorn05.onClick.AddListener(() =>
            {
                // 옥수수 열개 사는 함수 추가
            });
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButtonHandler : MonoBehaviour
{
    public Button bottonCorn01;
    public Button bottonCorn05;
    public Button bottonCorn10;

    public GameObject lobbyGrid; // 로비 그리드
    public GameObject shopGrid; // 상점 그리드

    void Start()
    {
        Init();
    }

    void Init()
    {
        //if (bottonCorn01 != null)
        //{
        //    bottonCorn01.onClick.AddListener(() =>
        //    {
        //        //옥수수 한개 사는 함수 추가
        //        BackendGameData.Instance.UserGameData.corn += 1;
        //        BackendGameData.Instance.GameDataUpdate();
        //        LobbyScene.Instance.UpdateCurrencyData();
        //    });
        //}

        //if (bottonCorn05 != null)
        //{
        //    bottonCorn05.onClick.AddListener(() =>
        //    {
        //        //옥수수 다섯개 사는 함수 추가
        //        BackendGameData.Instance.UserGameData.corn += 5;
        //        BackendGameData.Instance.GameDataUpdate();
        //        LobbyScene.Instance.UpdateCurrencyData();
        //    });
        //}

        //if (bottonCorn10 != null)
        //{
        //    bottonCorn05.onClick.AddListener(() =>
        //    {
        //        //옥수수 열개 사는 함수 추가
        //        BackendGameData.Instance.UserGameData.corn += 10;
        //        BackendGameData.Instance.GameDataUpdate();
        //        LobbyScene.Instance.UpdateCurrencyData();
        //    });
        //}

        lobbyGrid = FindObjectOfType<Grid>().gameObject.transform.GetChild(0).gameObject;
        shopGrid = FindObjectOfType<Grid>().gameObject.transform.GetChild(1).gameObject;

        OnPopupOpened();
    }

    // 팝업 창이 열릴 때 호출되는 함수
    public void OnPopupOpened()
    {
        // 로비씬에 해당하는 그리드를 비활성화하고, 상점 맵에 해당하는 그리드를 활성화
        lobbyGrid.SetActive(false);
        shopGrid.SetActive(true);
    }

    // 팝업 창이 닫힐 때 호출되는 함수
    public void OnPopupClosed()
    {
        // 로비씬에 해당하는 그리드를 활성화하고, 상점 맵에 해당하는 그리드를 비활성화
        lobbyGrid.SetActive(true);
        shopGrid.SetActive(false);
    }


    public void OnClickConr60()
    {
        BackendGameData.Instance.UserGameData.corn += 60;
        BackendGameData.Instance.GameDataUpdate();
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Purchase_Effect);
    }

    public void OnClickCorn300()
    {
        BackendGameData.Instance.UserGameData.corn += 300;
        BackendGameData.Instance.GameDataUpdate();
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Purchase_Effect);
    }

    public void OnClickCorn600()
    {
        BackendGameData.Instance.UserGameData.corn += 600;
        BackendGameData.Instance.GameDataUpdate();
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Purchase_Effect);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopButtonHandler : MonoBehaviour
{
    public GameObject lobbyGrid; // 로비 그리드
    public GameObject shopGrid; // 상점 그리드
    public GameObject warningPanel;

    public Button arrowBtn;
    public Button bombBtn;
    public Button blackBtn;
    public Button tankBtn;
    public Button healBtn;

    public Button yesBtn;
    public Button noBtn;
    public TextMeshProUGUI warningMsg;
    private int cornProductId;

    void Start()
    {
        Init();
        warningMsg.text = "";
        cornProductId = 0;

        if(BackendGameData.Instance.TowerDB.t0) arrowBtn.interactable = false;
        if(BackendGameData.Instance.TowerDB.t1) bombBtn.interactable = false;
        if(BackendGameData.Instance.TowerDB.t2) blackBtn.interactable = false;
        if(BackendGameData.Instance.TowerDB.t3) tankBtn.interactable = false;
        if(BackendGameData.Instance.TowerDB.t4) healBtn.interactable = false;
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
        BackendGameData.Instance.GameDataLoad();
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Purchase_Effect);
    }

    public void OnClickCorn300()
    {
        BackendGameData.Instance.UserGameData.corn += 300;
        BackendGameData.Instance.GameDataUpdate();
        BackendGameData.Instance.GameDataLoad();
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Purchase_Effect);
    }

    public void OnClickCorn600()
    {
        BackendGameData.Instance.UserGameData.corn += 600;
        BackendGameData.Instance.GameDataUpdate();
        BackendGameData.Instance.GameDataLoad();
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Purchase_Effect);
    }

    public void OnClickBread05()
    {
        if(BackendGameData.Instance.UserGameData.corn < 20)
        {
            StartCoroutine(NotEnoughCorn());
            return;
        }
        warningPanel.SetActive(true);
        warningMsg.text = "옥수수 20개로\n정말 구매하시겠습니까?";
        cornProductId = 0;
    }

    public void OnClickThreadmill01()
    {
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_UI);
        if(BackendGameData.Instance.UserGameData.threadmill == 10)
        {
            StartCoroutine(FullThreadmill());
            return;
        }
        if (BackendGameData.Instance.UserGameData.corn < 10)
        {
            StartCoroutine(NotEnoughCorn());
            return;
        }
        warningPanel.SetActive(true);
        warningMsg.text = "옥수수 10개로\n정말 구매하시겠습니까?";
        cornProductId = 1;
    }

    public void OnClickRemoveAds()
    {
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_UI);
        //그냥 IAP 함수 수행
        warningPanel.SetActive(true);
        warningMsg.text = "준비 중 입니다...";
        cornProductId = 2;
    }

    public void OnClickArrowBtn()
    {
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_UI);
        if (BackendGameData.Instance.UserGameData.corn < 250)
        {
            StartCoroutine(NotEnoughCorn());
            return;
        }
        warningPanel.SetActive(true);
        warningMsg.text = "옥수수 250개로\n정말 구매하시겠습니까?";
        cornProductId = 3;
    }

    public void OnClickBombBtn()
    {
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_UI);
        if (BackendGameData.Instance.UserGameData.corn < 480)
        {
            StartCoroutine(NotEnoughCorn());
            return;
        }
        warningPanel.SetActive(true);
        warningMsg.text = "옥수수 480개로\n정말 구매하시겠습니까?";
        cornProductId = 4;
    }

    public void OnClickBlackBtn()
    {
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_UI);
        if (BackendGameData.Instance.UserGameData.corn < 800)
        {
            StartCoroutine(NotEnoughCorn());
            return;
        }
        warningPanel.SetActive(true);
        warningMsg.text = "옥수수 800개로\n정말 구매하시겠습니까?";
        cornProductId = 5;
    }

    public void OnClickTankBtn()
    {
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_UI);
        if (BackendGameData.Instance.UserGameData.corn < 1500)
        {
            StartCoroutine(NotEnoughCorn());
            return;
        }
        warningPanel.SetActive(true);
        warningMsg.text = "옥수수 1500개로\n정말 구매하시겠습니까?";
        cornProductId = 6;
    }

    public void OnClickHealBtn()
    {
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_UI);
        if (BackendGameData.Instance.UserGameData.corn < 1500)
        {
            StartCoroutine(NotEnoughCorn());
            return;
        }
        warningPanel.SetActive(true);
        warningMsg.text = "옥수수 1500개로\n정말 구매하시겠습니까?";
        cornProductId = 7;
    }

    public void YesBtn()
    {
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Purchase_Effect);
        if (cornProductId == 0)//식빵 구매일 경우
        {
            BackendGameData.Instance.UserGameData.corn -= 20;
            BackendGameData.Instance.UserGameData.bread += 5;
            BackendGameData.Instance.GameDataUpdate();
            BackendGameData.Instance.GameDataLoad();
        }
        else if(cornProductId == 1) //쳇바퀴 구매일 경우
        {
            BackendGameData.Instance.UserGameData.corn -= 10;
            BackendGameData.Instance.UserGameData.threadmill += 1;
            BackendGameData.Instance.GameDataUpdate();
            BackendGameData.Instance.GameDataLoad();
        }
        else if(cornProductId == 2)
        {
            warningPanel.SetActive(false);
            warningMsg.text = string.Empty;
        }
        else if(cornProductId == 3)
        {
            BackendGameData.Instance.TowerDB.t0 = true;
            BackendGameData.Instance.UserGameData.corn -= 250;
            BackendGameData.Instance.GameDataUpdate();
            BackendGameData.Instance.GameDataLoad();
            BackendGameData.Instance.TowerDataUpdate();
            BackendGameData.Instance.TowerDataLoad();
        }
        else if (cornProductId == 4)
        {
            BackendGameData.Instance.TowerDB.t1 = true;
            BackendGameData.Instance.UserGameData.corn -= 480;
            BackendGameData.Instance.GameDataUpdate();
            BackendGameData.Instance.GameDataLoad();
            BackendGameData.Instance.TowerDataUpdate();
            BackendGameData.Instance.TowerDataLoad();
        }
        else if (cornProductId == 5)
        {
            BackendGameData.Instance.TowerDB.t2 = true;
            BackendGameData.Instance.UserGameData.corn -= 800;
            BackendGameData.Instance.GameDataUpdate();
            BackendGameData.Instance.GameDataLoad();
            BackendGameData.Instance.TowerDataUpdate();
            BackendGameData.Instance.TowerDataLoad();
        }
        else if (cornProductId == 6)
        {
            BackendGameData.Instance.TowerDB.t3 = true;
            BackendGameData.Instance.UserGameData.corn -= 1500;
            BackendGameData.Instance.GameDataUpdate();
            BackendGameData.Instance.GameDataLoad();
            BackendGameData.Instance.TowerDataUpdate();
            BackendGameData.Instance.TowerDataLoad();
        }
        else if (cornProductId == 7)
        {
            BackendGameData.Instance.TowerDB.t4 = true;
            BackendGameData.Instance.UserGameData.corn -= 1500;
            BackendGameData.Instance.GameDataUpdate();
            BackendGameData.Instance.GameDataLoad();
            BackendGameData.Instance.TowerDataUpdate();
            BackendGameData.Instance.TowerDataLoad();
        }
        warningPanel.SetActive(false);
        warningMsg.text = string.Empty;
    }

    public void NoBtn()
    {
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_UI);
        warningPanel.SetActive(false);
        warningMsg.text = string.Empty;
    }

    IEnumerator NotEnoughCorn()
    {
        warningPanel.SetActive(true);
        yesBtn.gameObject.SetActive(false);
        noBtn.gameObject.SetActive(false);
        warningMsg.text = "옥수수가\n부족합니다.";
        yield return new WaitForSeconds(1.5f);

        warningMsg.text = string.Empty;
        yesBtn.gameObject.SetActive(true);
        noBtn.gameObject.SetActive(true);
        warningPanel.SetActive(false);
    }

    IEnumerator FullThreadmill()
    {
        warningPanel.SetActive(true);
        yesBtn.gameObject.SetActive(false);
        noBtn.gameObject.SetActive(false);
        warningMsg.text = "쳇바퀴가\n최대치 입니다.";
        yield return new WaitForSeconds(1.5f);

        warningMsg.text = string.Empty;
        yesBtn.gameObject.SetActive(true);
        noBtn.gameObject.SetActive(true);
        warningPanel.SetActive(false);
    }
}

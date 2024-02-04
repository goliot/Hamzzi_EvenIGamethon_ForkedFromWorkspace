using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// PopUpManager에서 팝업 이름(PopUpNames)을 가져와 사용할 수 있다
// 이미 이름을 PopUpManager에서 생성 및 초기화 해놔서, 동작 함수만 추가 작성하면 됨
public class PopUpHandler : MonoBehaviour
{
    public GameObject[] lobbyBackground;

    private void Start()
    {
        //로비씬에서만 수행
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            lobbyBackground[0] = GameObject.Find("Home_Background");
            lobbyBackground[1] = GameObject.Find("Shop_Background");
            lobbyBackground[2] = GameObject.Find("Dogam_Background");
        }
    }

    #region PopUpButton
    public void OnClickPopUpStageSelect()
    {

        PopUpManager.Inst.CreatePopup(PopUpManager.Inst.PopUpNames.strStageSelectUI);
    }

    public void OnClickPopUpSettings()
    {
        PopUpManager.Inst.CreatePopup(PopUpManager.Inst.PopUpNames.strSettingsUI);
    }

    public void OnClickPopUpExplainStamina()
    {
        PopUpManager.Inst.CreatePopup(PopUpManager.Inst.PopUpNames.strExplainStaminaUI);
    }

    public void OnClickPopUpExplainCorn()
    {
        PopUpManager.Inst.CreatePopup(PopUpManager.Inst.PopUpNames.strExplainCornUI);
    }

    public void OnClickPopUpProfile()
    {
        PopUpManager.Inst.CreatePopup(PopUpManager.Inst.PopUpNames.strProfileUI);
    }

    public void OnClickPopUpLevelUp()
    {
        PopUpManager.Inst.CreatePopup(PopUpManager.Inst.PopUpNames.strLevelUpUI);
    }

    public void OnClickPopUpStart()
    {
        PopUpManager.Inst.CreatePopup(PopUpManager.Inst.PopUpNames.strStageStartUI);
    }

    public void OnClickPopUpShop()
    {
        PopUpManager.Inst.CreatePopup(PopUpManager.Inst.PopUpNames.strShopUI);
        lobbyBackground[0].SetActive(false);
        lobbyBackground[1].SetActive(true);
        lobbyBackground[2].SetActive(false);
    }

    public void OnClickPopUpDogam()
    {
        //도감 프리팹 활성화 코드 넣고
        lobbyBackground[0].SetActive(false);
        lobbyBackground[1].SetActive(false);
        lobbyBackground[2].SetActive(true);
    }

    #endregion

    public void OnClickExit()
    {
        PopUpManager.Inst.popUpList.Peek().OnClose();
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            lobbyBackground[0].SetActive(true);
            lobbyBackground[1].SetActive(false);
            lobbyBackground[2].SetActive(false);
        }
    }

    public void OnClickLevelUp()
    {
        int level = BackendGameData.Instance.UserGameData.level;
        int bread = BackendGameData.Instance.UserGameData.bread;
        int corn = BackendGameData.Instance.UserGameData.corn;

        //if() ==> 레벨업이 가능한 경우
        if(bread > BackendGameData.Instance.UserGameData.levelUpData[level - 1] && corn > BackendGameData.Instance.UserGameData.cornCostToLevelUp[level - 1])
        {
            BackendGameData.Instance.UserGameData.bread -= BackendGameData.Instance.UserGameData.levelUpData[level - 1];
            BackendGameData.Instance.UserGameData.level += 1;
            BackendGameData.Instance.GameDataUpdate();
        }
        //else ==> 레벨업이 불가능한 경우
        else StartCoroutine(NotEnoughCorn());
    }

    IEnumerator NotEnoughCorn()
    {
        transform.parent.parent.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        transform.parent.parent.GetChild(1).gameObject.SetActive(false);
    }
}

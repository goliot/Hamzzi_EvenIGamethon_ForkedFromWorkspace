using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// PopUpManager에서 팝업 이름(PopUpNames)을 가져와 사용할 수 있다
// 이미 이름을 PopUpManager에서 생성 및 초기화 해놔서, 동작 함수만 추가 작성하면 됨
public class PopUpHandler : MonoBehaviour
{
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
    }

    #endregion

    public void OnClickExit()
    {
        PopUpManager.Inst.popUpList.Peek().OnClose();
    }

    public void OnClickLevelUp()
    {
        //if() ==> 레벨업이 가능한 경우
        //else ==> 레벨업이 불가능한 경우
        StartCoroutine(NotEnoughCorn());
    }

    IEnumerator NotEnoughCorn()
    {
        transform.parent.parent.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        transform.parent.parent.GetChild(1).gameObject.SetActive(false);
    }
}

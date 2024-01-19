using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PopUpManager에서 팝업 이름(PopUpNames)을 가져와 사용할 수 있다
// 이미 이름을 PopUpManager에서 생성 및 초기화 해놔서, 동작 함수만 추가 작성하면 됨
public class PopUpHandler : MonoBehaviour
{
    public void OnClickExit()
    {
        PopUpManager.Inst.popUpList.Peek().OnClose();
    }

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

}

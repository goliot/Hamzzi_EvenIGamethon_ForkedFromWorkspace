using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 팝업 이름을 관리
public class PopUpNames
{
    public string strStageSelectUI { get; set; }
    public string strSettingsUI { get; set; }
    public string strExplainStaminaUI { get; set; }
    public string strExplainCornUI { get; set; }
    public string strProfileUI { get; set; }

    public PopUpNames(string stageSelectUI, string optionsUI, string explainStaminaUI, string explainCornUI, string profileUI)
    {
        strStageSelectUI = stageSelectUI;
        strSettingsUI = optionsUI;
        strExplainStaminaUI = explainStaminaUI;
        strExplainCornUI = explainCornUI;
        strProfileUI = profileUI;
    }

}

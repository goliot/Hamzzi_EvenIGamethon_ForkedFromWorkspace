using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUpUI : MonoBehaviour
{
    public static LevelUpUI Inst = null;
    public TextMeshProUGUI cornCost;

    private void Awake()
    {
        Inst = this;
    }

    private void OnEnable()
    {
        cornCost.text = BackendGameData.Instance.UserGameData.cornCostToLevelUp[BackendGameData.Instance.UserGameData.level - 1].ToString();
    }

    public void ChangeCornText()
    {
        cornCost.text = BackendGameData.Instance.UserGameData.cornCostToLevelUp[BackendGameData.Instance.UserGameData.level - 1].ToString();
    }
}

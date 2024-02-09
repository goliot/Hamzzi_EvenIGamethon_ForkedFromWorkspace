using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectViewr : MonoBehaviour
{
    public Button[] buttons;
    public Dictionary<Button, int> buttonsKey = new Dictionary<Button, int>();
    private int lastStage;
    public Color unlockColor;
    public int chapter;

    public void Awake()
    {
        lastStage = BackendGameData.Instance.ClearData.lastClear;
        for(int i=0; i<buttons.Length; i++)
        {
            buttonsKey.Add(buttons[i], i);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)) //전체 해금 치트
        {
            BackendGameData.Instance.ClearData.lastClear = 19;
            BackendGameData.Instance.ClearDataUpdate();
        }
        lastStage = BackendGameData.Instance.ClearData.lastClear;
        chapter = StageSelect.instance.chapter;

        switch (chapter)
        {
            case 1:
                for(int i=0; i<buttons.Length; i++)
                {
                    buttonsKey[buttons[i]] = i;
                }
                break;
            case 2:
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttonsKey[buttons[i]] = i + 5;
                }
                break;
            case 3:
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttonsKey[buttons[i]] = i + 10;
                }
                break;
            case 4:
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttonsKey[buttons[i]] = i + 15;
                }
                break;
        }
        for(int i=0; i<5; i++)
        {
            if(buttonsKey[buttons[i]] > lastStage)
            {
                buttons[i].interactable = false;
                buttons[i].GetComponent<Image>().color = unlockColor;
            }
            else
            {
                buttons[i].interactable = true;
                buttons[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }
}

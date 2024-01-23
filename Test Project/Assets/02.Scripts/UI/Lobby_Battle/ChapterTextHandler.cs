using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChapterTextHandler : MonoBehaviour
{
    public enum InfoType { Chapter, Total }
    public InfoType type;

    TextMeshProUGUI chapterText;
    TextMeshProUGUI totalText;

    private void Awake()
    {
        chapterText = GetComponent<TextMeshProUGUI>();
        totalText = GetComponent<TextMeshProUGUI>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Chapter:
            chapterText.text = string.Format($"{StageSelect.instance.chapter}");
                break;
            case InfoType.Total:
            totalText.text = string.Format($"{StageSelect.instance.chapter} : {StageSelect.instance.stage}");
                break;
        }
    }
}

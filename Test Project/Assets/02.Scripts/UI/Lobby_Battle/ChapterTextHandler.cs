using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterTextHandler : MonoBehaviour
{
    public enum InfoType { Chapter, Total }
    public InfoType type;

    Text chapterText;
    Text totalText;

    private void Awake()
    {
        chapterText = GetComponent<Text>();
        totalText = GetComponent<Text>();
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

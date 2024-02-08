using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryButtonHandler : MonoBehaviour
{
    void Awake()
    {
        Init();
    }

    void Init()
    {
        ChapterButtonHandler chapterButtonHandler = FindObjectOfType<ChapterButtonHandler>();
        if (chapterButtonHandler != null)
        {
            //chapterButtonHandler.OnChapterChanged.AddListener(UpdateChapter);
        }
        else
        {
            Debug.LogError("ChapterButtonHandler를 찾을 수 없습니다.");
        }
    }

    // UnityEvent 구독
    void UpdateChapter()
    {
        int currentChapter = StageSelect.instance.chapter;
        Debug.Log(currentChapter);
    }

    public void LoadCutScene()
    {
        SceneManager.LoadScene("CutScene");
    }
}

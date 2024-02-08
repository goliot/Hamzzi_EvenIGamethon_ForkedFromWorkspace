using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryButtonHandler : MonoBehaviour
{
    int currentChapter;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        currentChapter = 1;

        ChapterButtonHandler chapterButtonHandler = FindObjectOfType<ChapterButtonHandler>();
        if (chapterButtonHandler != null)
        {
            chapterButtonHandler.OnChapterChanged.AddListener(UpdateChapter);
        }
        else
        {
            Debug.LogError("ChapterButtonHandler를 찾을 수 없습니다.");
        }
    }

    
    // UnityEvent 구독
    void UpdateChapter()
    {
        currentChapter = StageSelect.instance.chapter;
        Debug.Log(currentChapter);
        CutSceneManager.Inst.cutSceneType = (CutSceneData.CutSceneType)currentChapter;
        Debug.Log(CutSceneManager.Inst.cutSceneType);
    }

    public void LoadChapterCutScene()
    {
        SceneManager.LoadScene("CutScene");

        //switch (currentChapter)
        //{
        //    case 1:
        //        CutSceneManager.Inst.PlayCutScene(CutSceneData.CutSceneType.Chapter01);
        //        break;
        //    case 2:
        //        CutSceneManager.Inst.PlayCutScene(CutSceneData.CutSceneType.Chapter02);
        //        break;
        //    case 3:
        //        CutSceneManager.Inst.PlayCutScene(CutSceneData.CutSceneType.Chapter03);
        //        break;
        //    case 4:
        //        CutSceneManager.Inst.PlayCutScene(CutSceneData.CutSceneType.Chapter04);
        //        break;
        //}
    }
}

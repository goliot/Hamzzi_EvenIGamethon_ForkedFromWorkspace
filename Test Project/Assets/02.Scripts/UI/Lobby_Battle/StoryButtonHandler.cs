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

        ChapterButtonHandler[] chapterButtonHandlers = FindObjectsOfType<ChapterButtonHandler>();
        if (chapterButtonHandlers != null)
        {
            foreach(var cbh in chapterButtonHandlers)
            {
                cbh.OnChapterChanged.AddListener(UpdateChapter);
            }
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
        // 비동기 로드가 완료된 후에 PlayCutScene 함수를 호출
        SceneManager.LoadSceneAsync("CutScene").completed += OnLoadCutSceneComplete; // SceneManager.LoadSceneAsync를 사용
    }

    // 로드 완료 콜백을 등록하여 로드가 완료된 후에 PlayCutScene을 호출
    private void OnLoadCutSceneComplete(AsyncOperation operation)
    {
        if (operation.isDone)
        {
            // 씬 로드가 완료된 후에 PlayCutScene을 호출
            CutSceneManager.Inst.PlayCutScene((CutSceneData.CutSceneType)currentChapter);
        }
    }
}

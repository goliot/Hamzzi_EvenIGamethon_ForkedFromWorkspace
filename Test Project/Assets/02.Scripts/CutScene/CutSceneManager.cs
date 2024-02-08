using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static CutSceneData;

public class CutSceneManager : MonoBehaviour
{
    public CutSceneData[] cutSceneData;
    public Image cutSceneImage;
    int currentFrameIndex = 0;
    CutSceneData currentCutSceneData;

    private void Awake()
    {
        // 버튼 리스너 추가
        Button nextButton = GameObject.Find("NextButton").GetComponent<Button>();
        Button prevButton = GameObject.Find("PrevButton").GetComponent<Button>();
        nextButton.onClick.AddListener(ShowNextFrame);
        prevButton.onClick.AddListener(ShowPreviousFrame);
    }

    void Start()
    {
        if (cutSceneImage == null)
        {
            cutSceneImage = GetComponent<Image>();
        }

        // 시작 시 오프닝 컷씬 실행
        PlayCutScene(CutSceneType.Opening);
    }

    // 컷씬 재생 함수
    public void PlayCutScene(CutSceneType cutSceneType)
    {
        // 주어진 컷씬 타입에 해당하는 CutSceneData를 찾음
        currentCutSceneData = GetCutSceneData(cutSceneType);
        if (currentCutSceneData != null)
        {
            // 컷씬 재생 처리
            currentFrameIndex = 0;
            ShowFrame(currentCutSceneData.frameSprites[currentFrameIndex]);
        }
    }

    // 주어진 컷씬 타입에 해당하는 CutSceneData를 찾는 함수
    private CutSceneData GetCutSceneData(CutSceneType cutSceneType)
    {
        foreach (CutSceneData data in cutSceneData)
        {
            if (data.cutSceneType == cutSceneType)
            {
                return data;
            }
        }
        return null; // 해당하는 컷씬 데이터를 찾지 못한 경우
    }

    // 이전 프레임을 보여주는 함수
    public void ShowPreviousFrame()
    {
        if (currentFrameIndex <= 0) return;
        currentFrameIndex--;
        ShowCurrentFrame();
    }

    // 다음 프레임을 보여주는 함수
    public void ShowNextFrame()
    {
        int frameCount = currentCutSceneData.frameSprites.Length;

        if (currentFrameIndex >= frameCount - 1)
        {
            MoveToFirstPlay();
        }
        else
        {
            // 다음 프레임 표시
            currentFrameIndex++;
            ShowCurrentFrame();
        }
    }

    // 현재 프레임을 보여주는 함수
    private void ShowCurrentFrame()
    {
        // 현재 프레임의 스프라이트를 표시
        if (currentFrameIndex >= 0 && currentFrameIndex < currentCutSceneData.frameSprites.Length)
        {
            ShowFrame(currentCutSceneData.frameSprites[currentFrameIndex]);
        }
    }

    // 스프라이트를 이미지에 표시하는 함수
    private void ShowFrame(Sprite sprite)
    {
        cutSceneImage.sprite = sprite;
    }

    void MoveToLobby()
    {
        SceneManager.LoadScene("Lobby");
    }

    void MoveToFirstPlay()
    {
        SceneManager.LoadScene("FirstPlay");
    }
}
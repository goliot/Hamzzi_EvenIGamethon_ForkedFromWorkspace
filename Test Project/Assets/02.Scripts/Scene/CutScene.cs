using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutScene : MonoBehaviour
{
    public Image comicImage;
    public Sprite[] comicFrames;
    public Button nextButton;
    public Button prevButton;

    public int currentFrameIndex = 0;

    private void Start()
    {
        ShowCurrentFrame(); // 첫번째 프레임 보여주기
    }

    private void ShowCurrentFrame()
    {
        if (currentFrameIndex >= 0 && currentFrameIndex < comicFrames.Length)
        {
            comicImage.sprite = comicFrames[currentFrameIndex];
        }
        UpdateButtonInteractivity();
    }

    private void UpdateButtonInteractivity()
    {
        // 다음 버튼이 더 이상 상호작용하지 않아야 하는 경우
        nextButton.interactable = currentFrameIndex < comicFrames.Length - 1;

        // 이전 버튼이 더 이상 상호작용하지 않아야 하는 경우
        prevButton.interactable = currentFrameIndex > 0;
    }


    public void ShowNextFrame()
    {
        currentFrameIndex++;
        if (currentFrameIndex >= comicFrames.Length)
        {
            // 만화의 끝에 도달했을 경우, 다음 작업 수행
            // 여기에 필요한 작업 추가
        }
        else
        {
            ShowCurrentFrame();
        }
    }

    public void ShowPreviousFrame()
    {
        if (currentFrameIndex <= 0) return;
        currentFrameIndex--;
        ShowCurrentFrame();
    }
}

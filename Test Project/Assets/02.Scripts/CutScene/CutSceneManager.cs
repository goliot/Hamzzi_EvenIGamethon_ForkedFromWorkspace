using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static CutSceneData;

public class CutSceneManager : Singleton<CutSceneManager>
{
    [SerializeField]
    private UserInfo user = new UserInfo();

    public CutSceneData[] cutSceneData;
    public Image cutSceneImage;
    public Button nextButton;
    public Button prevButton;
    public Button skipButton;

    int currentFrameIndex = 0;
    CutSceneData currentCutSceneData;
    public CutSceneType cutSceneType;

    void Awake()
    {
        Init();
        base.Initialize_DontDestroyOnLoad();
    }

    void Init()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 로드될 때마다 이미지와 버튼을 찾아서 연결
        if (scene.name == "CutScene")
        {
            cutSceneImage = GameObject.Find("CutSceneImage").GetComponent<Image>();
            nextButton = GameObject.Find("NextButton").GetComponent<Button>();
            prevButton = GameObject.Find("PrevButton").GetComponent<Button>();
            skipButton = GameObject.Find("SkipButton").GetComponent<Button>();

            if (nextButton != null)
            {
                nextButton.onClick.AddListener(ShowNextFrame);
            }

            if (prevButton != null)
            {
                prevButton.onClick.AddListener(ShowPreviousFrame);
            }

            if(skipButton != null && skipButton.gameObject.activeSelf)
            {
                skipButton.onClick.AddListener(ToLobby);
            }
            PlayCutScene(cutSceneType);
        }
    }

    void Start()
    {
        // 시작 시 오프닝 컷씬 실행
        //PlayCutScene(cutSceneType);
    }

    // 컷씬 재생 함수
    public void PlayCutScene(CutSceneType cutSceneType)
    {
        if (cutSceneType == 0) skipButton.gameObject.SetActive(true);
        else skipButton.gameObject.SetActive(false);
        AudioManager.Inst.StopBgm();
        switch ((int)cutSceneType) 
        {
            case 0:
                AudioManager.Inst.PlayBgm(AudioManager.BGM.BGM_OpeningCartoon);
                break;
            case 1:
                AudioManager.Inst.PlayBgm(AudioManager.BGM.BGM_Chapter01Cartoon);
                AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Chapter01_Cartoon01);
                break;
            case 2:
                AudioManager.Inst.PlayBgm(AudioManager.BGM.BGM_Chapter02Cartoon);
                AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Chapter02_Cartoon01);
                break;
            case 3:
                AudioManager.Inst.PlayBgm(AudioManager.BGM.BGM_Chapter03Cartoon);
                break;
            case 4:
                AudioManager.Inst.PlayBgm(AudioManager.BGM.BGM_Chapter04Cartoon);
                break;
        }
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
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_UI);
        if (currentFrameIndex <= 0) return;
        currentFrameIndex--;
        ShowCurrentFrame();
    }

    // 다음 프레임을 보여주는 함수
    public void ShowNextFrame()
    {
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_UI);
        int frameCount = currentCutSceneData.frameSprites.Length;

        if (currentFrameIndex >= frameCount - 1)
        {
            /*if (cutSceneType == CutSceneType.Opening) MoveToFirstPlay();
            else MoveToLobby();*/
            MoveToLobby();
        }
        else
        {
            // 다음 프레임 표시
            currentFrameIndex++;
            ShowCurrentFrame();
        }
    }

    public void ToLobby()
    {
        SceneManager.LoadScene("Lobby");
    }

    // 현재 프레임을 보여주는 함수
    private void ShowCurrentFrame()
    {
        if(currentCutSceneData.cutSceneType == 0 && currentFrameIndex == 5) AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Change_Up);

        StoryCutSceneSound(); // 스토리 컷씬 사운드 재생

        // 현재 프레임의 스프라이트를 표시
        if (currentFrameIndex >= 0 && currentFrameIndex < currentCutSceneData.frameSprites.Length)
        {
            ShowFrame(currentCutSceneData.frameSprites[currentFrameIndex]);
        }
    }

    void StoryCutSceneSound()
    {
        switch (currentCutSceneData.cutSceneType)
        {
            case CutSceneType.Chapter01:
                if(currentFrameIndex == 0)
                {
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Chapter01_Cartoon01);
                }
                else if(currentFrameIndex == 1)
                {
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Chapter01_Cartoon02);
                }
                else
                {
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Chapter01_Cartoon03);
                }
                break;
            case CutSceneType.Chapter02:
                if (currentFrameIndex == 0)
                {
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Chapter02_Cartoon01);
                }
                else if (currentFrameIndex == 1)
                {
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Chapter02_Cartoon02);
                }
                else
                {
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Chapter02_Cartoon03);
                }
                break;
            case CutSceneType.Chapter03:
                break;
            case CutSceneType.Chapter04:
                break;
        }
    }

    // 스프라이트를 이미지에 표시하는 함수
    private void ShowFrame(Sprite sprite)
    {
        cutSceneImage.sprite = sprite;
    }

    // 최초 진행인지 아닌지는 FirstPlay 씬에서 무조건 검사
    void MoveToLobby()
    {
        SceneManager.LoadScene("Lobby");
    }

    void MoveToFirstPlay()
    {
        SceneManager.LoadScene("FirstPlay");
    }
}
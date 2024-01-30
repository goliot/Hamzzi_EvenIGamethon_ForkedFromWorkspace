using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] GameObject pauseMenu;
    public GameObject victoryUI;
    public GameObject gameOverUI;

    public Button speedControlButton; // 버튼 추가
    public Button victoryUINoButton;
    public Button victoryUIYesButton;
    public Button gameOverUINoButton;
    public Button gameOverUIYesButton;

    GameObject speed_2times;  // 2배속 버튼 이미지 

    public Image[] rewards;

    private void Awake()
    {
        this.Initialize();

        InitSpeedControllBtn();
    }

    void InitSpeedControllBtn()
    {
        if (speedControlButton != null)
        {
            speedControlButton.onClick.AddListener(ToggleGameSpeed);
            speed_2times = speedControlButton.gameObject.transform.GetChild(0).gameObject;
            speed_2times.SetActive(false);
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    public void GoToHome()
    {
        GameManager.Inst.Resume();
        SceneManager.LoadScene("Lobby");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void RetryGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Battle_Proto");
    }

    // 게임 시간 조절 함수
    public void ToggleGameSpeed()
    {
        GameManager.Inst.isGameSpeedIncreased = !GameManager.Inst.isGameSpeedIncreased; // 상태 토글

        if (GameManager.Inst.isGameSpeedIncreased)
        {
            Debug.Log("게임 속도 : 1.5배속");
            Time.timeScale = 1.5f; // 게임 속도 2배로
            speed_2times.SetActive(true);
        }
        else
        {
            Debug.Log("게임 속도 : 1배속");
            Time.timeScale = 1.0f; // 원래 속도로
            speed_2times.SetActive(false);
        }
    }

    // 클리어 시 다음 스테이지로 이동하는 함수 (VictoryUI의 Yes를 눌렀을 때)
    public void GoToNextStage()
    {
        if (StageSelect.instance.stage < 5)
        {
            StageSelect.instance.stage++;
            RetryGame();
        }
        else if (StageSelect.instance.stage == 5 && StageSelect.instance.chapter < 4)
        {
            StageSelect.instance.stage = 1;
            StageSelect.instance.chapter++;
            RetryGame();
        }
        else if (StageSelect.instance.stage == 5 && StageSelect.instance.chapter == 4) // 마지막 판
        {
            GoToHome();
        }
    }
}
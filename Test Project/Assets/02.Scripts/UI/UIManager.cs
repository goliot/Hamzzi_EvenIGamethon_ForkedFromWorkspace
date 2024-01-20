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

    private bool isGameSpeedIncreased = false; // 기본 1배속

    private void Awake()
    {
        this.Initialize();
    }

    private void Start()
    {
        if (speedControlButton != null)
        {
            speedControlButton.onClick.AddListener(ToggleGameSpeed);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)) // 디버그용
        {
            SceneManager.LoadScene("Lobby");
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

    public void ToggleGameSpeed()
    {
        isGameSpeedIncreased = !isGameSpeedIncreased; // 상태 토글

        if (isGameSpeedIncreased)
        {
            Debug.Log("게임 속도 : 1.5배속");
            Time.timeScale = 1.5f; // 게임 속도 2배로
        }
        else
        {
            Debug.Log("게임 속도 : 1배속");
            Time.timeScale = 1.0f; // 원래 속도로
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
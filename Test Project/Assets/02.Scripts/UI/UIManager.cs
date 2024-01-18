using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    public Button speedControlButton; // 버튼 추가

    private bool isGameSpeedIncreased = false; // 기본 1배속

    private void Start()
    {
        if(speedControlButton != null)
        {
            speedControlButton.onClick.AddListener(ToggleGameSpeed);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
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
        SceneManager.LoadScene("Home_Proto");
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
}

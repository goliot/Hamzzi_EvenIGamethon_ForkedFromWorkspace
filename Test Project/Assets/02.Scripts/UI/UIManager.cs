using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;
using DG.Tweening;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] GameObject pauseMenu;
    public GameObject victoryUI;
    public GameObject gameOverUI;
    public GameObject tutorialUI;

    public Button speedControlButton; // 버튼 추가
    public Button victoryUINoButton;
    public Button victoryUIYesButton;
    public Button gameOverUINoButton;
    public Button gameOverUIYesButton;
    public TextMeshProUGUI gameOverUITipText;

    public GameObject speed_2times;  // 2배속 버튼 이미지 

    public Image[] rewards;

    private void Awake()
    {
        base.Initialize();

        InitSpeedControllBtn();
        UpdateSpeedControllBtn();
    }

    private void Start()
    {
        if (!BackendGameData.Instance.UserGameData.isAdRemoved)
        {
            AdmobManager.instance.ShowInterstitialAd();
            PauseGame();
        }
    }

    void InitSpeedControllBtn()
    {
        if (speedControlButton != null)
        {
            speedControlButton.onClick.AddListener(ToggleGameSpeed);
            speed_2times = speedControlButton.gameObject.transform.GetChild(0).gameObject;
        }
    }

    public void UpdateSpeedControllBtn()
    {
        if(GameManager.Inst.isGameSpeedIncreased)
        {
            GameManager.Inst.isGameSpeedIncreased = true;
            speed_2times.SetActive(true);
        }
        else
        {
            GameManager.Inst.isGameSpeedIncreased = false;
            speed_2times.SetActive(false);
        }
    }

    public void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateSpeedControllBtn();
    }

    public void PauseGame()
    {
        GameManager.Inst.Stop();                // 게임 매니저에 있는 Stop 함수로 변경 (UIManager가 GameTime을 변경하는 건 Non-Logical)
        pauseMenu.SetActive(true);
    }

    public void GoToHome()
    {
        BackendGameData.Instance.GameDataUpdate();
        BackendGameData.Instance.ClearDataUpdate();
        BackendGameData.Instance.StarDataUpdate();
        GameManager.Inst.Resume();
        SceneManager.LoadScene("Lobby");
    }

    public void ResumeGame()
    {
        UpdateSpeedControllBtn();               // 속도 버튼 동기화
        pauseMenu.SetActive(false);             // Pause 이후 속도 내려가는 버그 수정
        GameManager.Inst.Resume();
    }

    public void RetryGame()
    {
        GameManager.Inst.Resume();
        //StageSelect.instance.speedIncreased = GameManager.Inst.isGameSpeedIncreased;
        PopUpManager.Inst.CreatePopup(PopUpManager.Inst.PopUpNames.strStageStartUI);
        //SceneManager.LoadScene("Battle_Proto");
    }

    // 게임 시간 조절 함수
    public void ToggleGameSpeed()
    {
        GameManager.Inst.isGameSpeedIncreased = !GameManager.Inst.isGameSpeedIncreased; // 상태 토글

        if (GameManager.Inst.isGameSpeedIncreased)
        {
            Debug.Log("게임 속도 : 1.5배속");
            Time.timeScale = 1.5f; // 게임 속도 1.5배로
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
            PopUpManager.Inst.CreatePopup(PopUpManager.Inst.PopUpNames.strStageStartUI);
            //RetryGame();
        }
        else if (StageSelect.instance.stage == 5 && StageSelect.instance.chapter < 4)
        {
            StageSelect.instance.stage = 1;
            StageSelect.instance.chapter++;
            PopUpManager.Inst.CreatePopup(PopUpManager.Inst.PopUpNames.strStageStartUI);
            //RetryGame();
        }
        else if (StageSelect.instance.stage == 5 && StageSelect.instance.chapter == 4) // 마지막 판
        {
            GoToHome();
        }
    }

    // 튜토리얼 Open, Close 함수
    public void OpenTutorial()
    {
        pauseMenu.SetActive(false);
        tutorialUI.SetActive(true);
        AdmobManager.instance.DestroyBannerView();
    }

    public void CloseTutorial()
    {
        pauseMenu.SetActive(true);
        tutorialUI.SetActive(false);
        AdmobManager.instance.LoadAd();
    }

    public void TMPDOText(TextMeshProUGUI text, float duration)
    {
        text.maxVisibleCharacters = 0;
        DOTween.To(x => text.maxVisibleCharacters = (int)x, 0f, text.text.Length, duration);
    }
}
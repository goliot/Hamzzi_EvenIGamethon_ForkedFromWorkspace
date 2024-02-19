using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class LobbyScene : MonoBehaviour
{
    private static LobbyScene instance = null;
    public static LobbyScene Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LobbyScene();
            }
            return instance;
        }
    }

    [SerializeField]
    private UserInfo user;
    [SerializeField]
    private TextMeshProUGUI textThreadmill;
    [SerializeField]
    private TextMeshProUGUI textCorn;
    [SerializeField]
    private TextMeshProUGUI textBread;

    [Header("# Background")]
    public GameObject lobbyBackground;
    public GameObject shopBackground;
    public GameObject dogamBackground;

    private void Awake()
    {
        if (UserInfo.Data.nickname == null)
        {
            user.onUserInfoEvent.AddListener(isFirstTime);
            user.GetUserInfoFromBackend();
        }

        // 게임 데이터 업데이트 이벤트 리스너 등록
        if (!EventListenerManager.Instance.IsGameDataLoadListenerAdded)
        {
            BackendGameData.Instance.onGameDataUpdateEvent.AddListener(BackendGameData.Instance.GameDataLoad);
            EventListenerManager.Instance.SetGameDataLoadListenerAdded(true);
        }

        // 클리어 데이터 업데이트 이벤트 리스너 등록
        if (!EventListenerManager.Instance.IsClearDataLoadListenerAdded)
        {
            BackendGameData.Instance.onClearDataUpdateEvent.AddListener(BackendGameData.Instance.ClearDataLoad);
            EventListenerManager.Instance.SetClearDataLoadListenerAdded(true);
        }

        // 타워 데이터 업데이트 이벤트 리스너 등록
        if (!EventListenerManager.Instance.IsTowerDataLoadListenerAdded)
        {
            BackendGameData.Instance.onTowerDataUpdateEvent.AddListener(BackendGameData.Instance.TowerDataLoad);
            EventListenerManager.Instance.SetTowerDataLoadListenerAdded(true);
        }

        // 스타 데이터 업데이트 이벤트 리스너 등록
        if (!EventListenerManager.Instance.IsStarDataLoadListenerAdded)
        {
            BackendGameData.Instance.onStarDataUpdateEvent.AddListener(BackendGameData.Instance.StarDataLoad);
            EventListenerManager.Instance.SetStarDataLoadListenerAdded(true);
        }

        if (Time.timeScale != 1.0f) Time.timeScale = 1.0f; // 게임씬이 끝나고 로비씬으로 왔을때, 기존 게임 씬에서 1.5배인 상태에서 로비씬으로 오면 1.5배 유지 되는 버그가 있음
    }

    private void isFirstTime()
    {
        if (UserInfo.Data.nickname == null)
        {
            SceneManager.LoadScene("FirstPlay");
            return;
        }
        else
        {
            /*BackendGameData.Instance.onGameDataLoadEvent.AddListener(UpdateCurrencyData); //GameData관련 리스너
            //BackendGameData.Instance.onGameDataUpdateEvent.AddListener(UpdateCurrencyData);
            BackendGameData.Instance.GameDataLoad();
            BackendGameData.Instance.TowerDataLoad();
            //BackendGameData.Instance.DogamDataLoad();
            //BackendGameData.Instance.ClearDataLoad();*/
            return;
        }
    }

    private void Start()
    {
        AudioManager.Inst.StopBgm();
        AudioManager.Inst.PlayBgm(AudioManager.BGM.BGM_Lobby);
        PopUpHandler.Inst.lobbyLoadEvent.AddListener(loadLobby);

        //AdmobManager.instance.ShowInterstitialAd();

        /*await BackendGameData.Instance.GameDataLoad(); // await를 추가하여 비동기 메서드가 완료될 때까지 대기
        UpdateCurrencyData(); //위 비동기가 종료되면 호출*/
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.B))
        {
            BackendGameData.Instance.UserGameData.bread += 1000;
            BackendGameData.Instance.GameDataUpdate();
        }

        UpdateCurrencyData();
    }

    public void UpdateCurrencyData()
    {
        //Debug.Log("자원 업데이트");
        //textThreadmill.text = $"{BackendGameData.Instance.UserGameData.threadmill} " + "/ 10";
        textCorn.text = $"{BackendGameData.Instance.UserGameData.corn}";
        textBread.text = $"{BackendGameData.Instance.UserGameData.bread}";
        if (BackendGameData.Instance.UserGameData.isAdRemoved)
        {
            AdmobManager.instance.DestroyBannerView();
            AdmobManager.instance.DestroyInterstitialView();
        }
    }

    public void OnClickRefreshThreadmill()
    {
        Threadmill.instance.m_HeartAmount = 10;
        Threadmill.instance.SaveHeartInfo();
    }

    public void loadLobby()
    {
        lobbyBackground.SetActive(true);
        shopBackground.SetActive(false);
        dogamBackground.SetActive(false);
    }

    public void loadShop()
    {
        lobbyBackground.SetActive(false);
        shopBackground.SetActive(true);
        dogamBackground.SetActive(false);
    }

    public void loadDogam()
    {
        lobbyBackground.SetActive(false);
        shopBackground.SetActive(false);
        dogamBackground.SetActive(true);
    }
}

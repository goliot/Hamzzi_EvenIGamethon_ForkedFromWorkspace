using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        BackendGameData.Instance.onGameDataLoadEvent.AddListener(UpdateCurrencyData); //GameData관련 리스너
        BackendGameData.Instance.onGameDataUpdateEvent.AddListener(BackendGameData.Instance.GameDataLoad);
        BackendGameData.Instance.onClearDataUpdateEvent.AddListener(BackendGameData.Instance.ClearDataLoad);
        BackendGameData.Instance.onTowerDataUpdateEvent.AddListener(BackendGameData.Instance.TowerDataLoad);

        BackendGameData.Instance.GameDataLoad();
        BackendGameData.Instance.TowerDataLoad();
        BackendGameData.Instance.DogamDataLoad();
        BackendGameData.Instance.ClearDataLoad();
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
    }

    public void UpdateCurrencyData()
    {
        if (!BackendGameData.Instance.UserGameData.isAdRemoved) AdmobManager.instance.ShowInterstitialAd();
        Debug.Log("자원 업데이트");
        //textThreadmill.text = $"{BackendGameData.Instance.UserGameData.threadmill} " + "/ 10";
        textCorn.text = $"{BackendGameData.Instance.UserGameData.corn}";
        if (BackendGameData.Instance.UserGameData.isAdRemoved) AdmobManager.instance.DestroyBannerView();
    }

    public void OnClickRefreshThreadmill()
    {
        BackendGameData.Instance.UserGameData.threadmill = 10;
        BackendGameData.Instance.GameDataUpdate();
        BackendGameData.Instance.GameDataLoad();
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

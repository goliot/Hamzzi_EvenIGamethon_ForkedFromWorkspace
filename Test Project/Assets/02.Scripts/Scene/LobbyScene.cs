using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class LobbyScene : MonoBehaviour
{
    private static LobbyScene instance = null;
    public static LobbyScene Instance
    {
        get
        {
            if(instance == null)
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


    private void Awake()
    {
        user.GetUserInfoFromBackend();
        BackendGameData.Instance.onGameDataLoadEvent.AddListener(UpdateCurrencyData);
        BackendGameData.Instance.GameDataLoad();
    }
    private void Start()
    {
        AudioManager.Inst.StopBgm();
        AudioManager.Inst.PlayBgm(AudioManager.BGM.BGM_Lobby);

        /*await BackendGameData.Instance.GameDataLoad(); // await를 추가하여 비동기 메서드가 완료될 때까지 대기
        UpdateCurrencyData(); //위 비동기가 종료되면 호출*/
    }

    public void UpdateCurrencyData()
    {
        Debug.Log("자원 업데이트");
        textThreadmill.text = $"{BackendGameData.Instance.UserGameData.threadmill} " + "/ 10";
        textCorn.text = $"{BackendGameData.Instance.UserGameData.corn}";
    }

    public void OnClickRefreshThreadmill()
    {
        BackendGameData.Instance.UserGameData.threadmill = 10;
        BackendGameData.Instance.GameDataUpdate();
    }
}

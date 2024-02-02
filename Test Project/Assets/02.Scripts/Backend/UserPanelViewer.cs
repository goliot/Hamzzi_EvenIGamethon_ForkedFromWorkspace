using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UserPanelViewer : MonoBehaviour
{
    public static UserPanelViewer instance;

    [SerializeField]
    private TextMeshProUGUI inputFieldNickname;
    [SerializeField]
    private TextMeshProUGUI textGamerId; //출력되는 닉네임
    [SerializeField]
    private TextMeshProUGUI textLevel;
    [SerializeField]
    private Slider sliderExperience;

    private void Awake()
    {
        instance = this;
        BackendGameData.Instance.onGameDataLoadEvent.AddListener(UpdateGameData);
    }

    public void showNickname()
    {
        inputFieldNickname.GetComponent<TextMeshProUGUI>().text = UserInfo.Data.nickname == null ?
                            UserInfo.Data.gamerId : UserInfo.Data.nickname;

        textGamerId.text = inputFieldNickname.GetComponent<TextMeshProUGUI>().text;
    }

    public void UpdateGameData()
    {
        textLevel.text = $"{BackendGameData.Instance.UserGameData.level}";
        //sliderExperience.value = BackendGameData.Instance.UserGameData.bread / BackendGameData.Instance.UserGameData.levelUpData[BackendGameData.Instance.UserGameData.level - 1];
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

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
    [SerializeField]
    private TextMeshProUGUI sliderText;

    private void Awake()
    {
        instance = this;
        BackendGameData.Instance.onGameDataLoadEvent.AddListener(UpdateGameData);
    }

    private void Update()
    {
        UpdateGameData();
        int bread = BackendGameData.Instance.UserGameData.bread;
        int nextBread = BackendGameData.Instance.UserGameData.levelUpData[BackendGameData.Instance.UserGameData.level - 1];
        float value = (float)bread / nextBread;
        sliderExperience.value = sliderExperience.value = Mathf.Clamp01(value);
        sliderText.text = bread.ToString() + "/" + nextBread.ToString();
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

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class UserPanelViewer : MonoBehaviour
{
    public static UserPanelViewer instance;

    [SerializeField]
    private TextMeshProUGUI textGamerId; //출력되는 닉네임
    [SerializeField]
    private TextMeshProUGUI textGamerId_2;
    [SerializeField]
    private TextMeshProUGUI textLevel;
    [SerializeField]
    private Slider sliderExperience;
    [SerializeField]
    private TextMeshProUGUI sliderText;
    [SerializeField]
    private TextMeshProUGUI upgradeInfoText;
    [SerializeField]
    private TextMeshProUGUI cornText;

    private void Awake()
    {
        instance = this;
        BackendGameData.Instance.onGameDataLoadEvent.AddListener(UpdateGameData);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            UpdateGameData();
            int bread = BackendGameData.Instance.UserGameData.bread;
            int nextBread = BackendGameData.Instance.UserGameData.levelUpData[BackendGameData.Instance.UserGameData.level - 1];
            float value = (float)bread / nextBread;
            sliderExperience.value = Mathf.Clamp01(value);
            sliderText.text = bread.ToString() + "/" + nextBread.ToString();

            textGamerId.text = UserInfo.Data.nickname == null ? UserInfo.Data.gamerId : UserInfo.Data.nickname;
            textGamerId_2.text = textGamerId.text;
            cornText.text = BackendGameData.Instance.UserGameData.cornCostToLevelUp[BackendGameData.Instance.UserGameData.level-1].ToString() + " 소모";

            int damageUpAmount = BackendGameData.Instance.UserGameData.damageUpgradeAmount[BackendGameData.Instance.UserGameData.level - 1];
            upgradeInfoText.text = "현재:\n모든 스킬 데미지 +" + damageUpAmount.ToString();
        }
    }

    public void showNickname()
    {
        textGamerId.text = UserInfo.Data.nickname == null ? UserInfo.Data.gamerId : UserInfo.Data.nickname;
    }

    public void UpdateGameData()
    {
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            textLevel.text = "Lv." + $"{BackendGameData.Instance.UserGameData.level}";
            //sliderExperience.value = BackendGameData.Instance.UserGameData.bread / BackendGameData.Instance.UserGameData.levelUpData[BackendGameData.Instance.UserGameData.level - 1];
        }
    }

    public void OnClickUISound()
    {
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_UI);
    }
}

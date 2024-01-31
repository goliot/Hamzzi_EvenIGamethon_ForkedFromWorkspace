using UnityEngine;
using TMPro;

public class UserPanelViewer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI inputFieldNickname;
    [SerializeField]
    private TextMeshProUGUI textGamerId;

    public void showNickname()
    {
        inputFieldNickname.GetComponent<TextMeshProUGUI>().text = UserInfo.Data.nickname == null ?
                            UserInfo.Data.gamerId : UserInfo.Data.nickname;

        textGamerId.text = inputFieldNickname.GetComponent<TextMeshProUGUI>().text;
    }
}

using UnityEngine;
using TMPro;

public class UserPanelViewer : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputFieldNickname;

    public void showNickname()
    {
        inputFieldNickname.placeholder.GetComponent<TextMeshProUGUI>().text = UserInfo.Data.nickname == null ?
                            UserInfo.Data.gamerId : UserInfo.Data.nickname;
    }

    //이 밑에 닉네임 변경 버튼 온클릭 만들기
    public void UpdateNickname()
    {

    }
}

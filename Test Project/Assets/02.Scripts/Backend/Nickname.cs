using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;
using UnityEditor.VersionControl;

public class Nickname : LoginBase
{
    [System.Serializable]
    public class NicknameEvent : UnityEngine.Events.UnityEvent { }
    public NicknameEvent onNicknameEvent = new NicknameEvent();

    [SerializeField]
    private Image imageNickname;
    [SerializeField]
    private TMP_InputField inputFieldNickname;
    [SerializeField]
    private Button btnUpdateNickname;
    [SerializeField]
    private GameObject nicknameWarningPanel;
    [SerializeField]
    private GameObject nicknameWaringBoard;
    [SerializeField]
    private TextMeshProUGUI warningText;

    private void OnEnable()
    {
        ResetUI(imageNickname);
        SetMessage("");
    }

    public void OnClickUpdateNickname()
    {
        ResetUI(imageNickname);
        if (IsFieldDataEmpty(imageNickname, inputFieldNickname.text, "Nickname")) return;

        btnUpdateNickname.interactable = false;
        SetMessage("닉네임 변경중입니다..");

        UpdateNickname();
    }

    private void UpdateNickname()
    {
        string message = string.Empty;

        Backend.BMember.UpdateNickname(inputFieldNickname.text, callback =>
        {
            btnUpdateNickname.interactable = true;

            if (callback.IsSuccess())
            {
                SetMessage($"{inputFieldNickname.text}\n(으)로 닉네임이\n변경되었습니다.");
                message = $"{inputFieldNickname.text}\n(으)로 닉네임이\n변경되었습니다.";
                onNicknameEvent.Invoke();
            }
            else
            {
                switch(int.Parse(callback.GetStatusCode())) 
                {
                    case 400:
                        message = "닉네임이 비었거나\n20자 이상이거나\n앞/뒤에 공백이 있습니다.";
                        break;
                    case 409:
                        message = "이미 존재하는\n닉네임입니다.";
                        break;
                    default:
                        message = callback.GetMessage();
                        break;
                }
                GuideForIncorrectlyEnteredData(imageNickname, message);
            }
            nicknameWarningPanel.SetActive(true);
            nicknameWaringBoard.SetActive(true);
            warningText.text = message;
        });
    }
}

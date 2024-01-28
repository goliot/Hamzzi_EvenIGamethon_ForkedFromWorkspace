using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;

public class FindID : LoginBase
{
    [SerializeField]
    private Image imageEmail;
    [SerializeField]
    private TMP_InputField inputFieldEmail;
    [SerializeField]
    private Button btnFindID;

    public void OnClickFindID()
    {
        ResetUI(imageEmail);

        if (IsFieldDataEmpty(imageEmail, inputFieldEmail.text, "메일 주소")) return;
        if(!inputFieldEmail.text.Contains("@"))
        {
            GuideForIncorrectlyEnteredData(imageEmail, "메일 형식이 잘못되었습니다.");
            return;
        }

        btnFindID.interactable = false;
        SetMessage("메일 발송중입니다...");

        FindCustomID();
    }

    private void FindCustomID()
    {
        Backend.BMember.FindCustomID(inputFieldEmail.text, callback =>
        {
            btnFindID.interactable = true;
            if(callback.IsSuccess())
            {
                SetMessage($"{inputFieldEmail.text} 주소로 메일을 발송하였습니다.");
            }
            else
            {
                string message = string.Empty;

                switch(int.Parse(callback.GetStatusCode()))
                {
                    case 404: //해당 이메일의 유저가 없음
                        message = "해당 이메일을 사용하는 유저가 없습니다.";
                        break;
                    case 429: //24시간 이내 5회 이상 같은 이메일로 아이디/비밀번호 찾기 시도
                        message = "24시간 이내에 5회 이상 아이디/비밀번호 찾기를 시도했습니다.";
                        break;
                    default:
                        //code 400 => 프로젝트명에 특수문자가 추가된 경우(안내 메일 미발송 및 에러 발생)
                        message = callback.GetMessage();
                        break;
                }

                if(message.Contains("이메일"))
                {
                    GuideForIncorrectlyEnteredData(imageEmail, message);
                }
                else
                {
                    SetMessage(message);
                }
            }
        });
    }
}

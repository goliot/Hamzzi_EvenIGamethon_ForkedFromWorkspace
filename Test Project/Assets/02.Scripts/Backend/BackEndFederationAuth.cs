using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using BackEnd;
using UnityEngine.SceneManagement;

public class BackEndFederationAuth : LoginBase
{
    // GPGS 로그인 
    void Start()
    {
        Debug.Log("구글 로그인 시도");
        // GPGS 플러그인 설정
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
            .Builder()
            .RequestServerAuthCode(false)
            .RequestEmail() // 이메일 권한을 얻고 싶지 않다면 해당 줄(RequestEmail)을 지워주세요.
            .RequestIdToken()
            .Build();
        //커스텀 된 정보로 GPGS 초기화
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true; // 디버그 로그를 보고 싶지 않다면 false로 바꿔주세요.
                                                  //GPGS 시작.
        PlayGamesPlatform.Activate();
        //GPGSLogin();
        string message = string.Empty;
    }

    public void GPGSLogin()
    {
        // 이미 로그인 된 경우
        if (Social.localUser.authenticated == true)
        {
            BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "gpgs");
            
        }
        else
        {
            Social.localUser.Authenticate((bool success) => {
                if (success)
                {
                    // 로그인 성공 -> 뒤끝 서버에 획득한 구글 토큰으로 가입 요청
                    BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "gpgs");

                    //BackendGameData.Instance.GameDataInsert();
                    //SceneManager.LoadScene("Lobby");
                    SceneManager.LoadScene("FirstPlay");
                }
                else
                {
                    // 로그인 실패
                    Debug.Log("Login failed for some reason");
                }
            });
        }
    }

    // 구글 토큰 받아옴
    public string GetTokens()
    {
        string message = string.Empty;

        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            // 유저 토큰 받기 첫 번째 방법
            string _IDtoken = PlayGamesPlatform.Instance.GetIdToken();
            // 두 번째 방법
            // string _IDtoken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
            return _IDtoken;
        }
        else
        {
            message = "접속되어 있지 않습니다. PlayGamesPlatform.Instance.localUser.authenticated :  fail";
            Debug.Log("접속되어 있지 않습니다. PlayGamesPlatform.Instance.localUser.authenticated :  fail");
            return null;
        }
    }

    public void OnClickGPGSLogin()
    {
        string message = string.Empty;
        BackendReturnObject bro = Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "gpgs로 만든 계정");
        if(bro.IsSuccess())
        {
            Debug.Log("구글 토큰으로 뒤끝 로그인 성공 - 동기");
        }
        else
        {
            switch(bro.GetStatusCode())
            {
                case "200":
                    message = "이미 회원가입된 회원";
                    Debug.Log("이미 회원가입된 회원");
                    break;
                case "403":
                    message = "차단된 사용자, : " + bro.GetErrorCode();
                    Debug.Log("차단된 사용자, : " + bro.GetErrorCode());
                    break;
                default:
                    message = "서버 공통 에러 " + bro.GetErrorCode();
                    Debug.Log("서버 공통 에러 발생" + bro.GetMessage());
                    break;
            }
        }
    }

    //이미 가입된 회원의 이메일 정보 저장
    public void OnClickUpdateEmail()
    {
        BackendReturnObject bro = Backend.BMember.UpdateFederationEmail(GetTokens(), FederationType.Google);
        if(bro.IsSuccess())
        {
            Debug.Log("이메일 주소 저장 성공");
        }
        else
        {
            if (bro.GetStatusCode() == "404") Debug.Log("아이디 찾을 수 없음");
        }
    }

    public void OnClickCheckUserAuthenticate()
    {
        BackendReturnObject bro = Backend.BMember.CheckUserInBackend(GetTokens(), FederationType.Google);
        if(bro.GetStatusCode() == "200")
        {
            Debug.Log("가입중인 계정임 " + bro.GetReturnValue());
        }
        else
        {
            Debug.Log("가입된 계정이 아님");
        }
    }

    //커스텀 계정을 페더레이션 계정으로 변경
    public void OnClickChangeCustomToFederation()
    {
        BackendReturnObject bro = Backend.BMember.ChangeCustomToFederation(GetTokens(), FederationType.Google);
    }
}

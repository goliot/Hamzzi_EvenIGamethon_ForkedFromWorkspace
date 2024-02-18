using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using BackEnd;
using UnityEngine.SceneManagement;

public class BackEndFederationAuth : LoginBase
{
    private const string BoolKey = "HaveGoogleLogined";
    private bool defaultValue = false; // 저장된 값이 없을 때 반환할 기본값

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

        // Bool 값을 검색합니다. 저장된 값이 없으면 기본값을 사용합니다.
        bool myBool = GetBool(BoolKey, defaultValue);
        Debug.Log("My bool value: " + myBool);

        // 저장된 값이 없으면 기본값을 저장합니다.
        SetBool(BoolKey, myBool);

        if (myBool) GPGSLogin(); //구글 로그인 한 적이 있다면 자동 로그인

        string message = string.Empty;
    }

    private bool GetBool(string key, bool defaultValue)
    {
        if (PlayerPrefs.HasKey(key)) // 키가 존재하는 경우
        {
            return IntToBool(PlayerPrefs.GetInt(key));
        }
        else // 키가 존재하지 않는 경우
        {
            return defaultValue;
        }
    }

    private void SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, BoolToInt(value));
        PlayerPrefs.Save();
    }

    private int BoolToInt(bool value)
    {
        return value ? 1 : 0;
    }

    private bool IntToBool(int value)
    {
        return value == 1;
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
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_UI);
                    // 로그인 성공 -> 뒤끝 서버에 획득한 구글 토큰으로 가입 요청
                    BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "gpgs");

                    //BackendGameData.Instance.GameDataInsert();
                    //SceneManager.LoadScene("Lobby");
                    //SceneManager.LoadScene("FirstPlay");
                    SetBool(BoolKey, true); //구글로그인 해봤다고 저장

                    BackendGameData.Instance.GameDataLoad();
                    BackendGameData.Instance.TowerDataLoad();
                    //BackendGameData.Instance.DogamDataLoad();
                    BackendGameData.Instance.ClearDataLoad();
                    BackendGameData.Instance.StarDataLoad();

                    SceneManager.LoadScene("CutScene"); // 컷씬으로 넘어가게
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

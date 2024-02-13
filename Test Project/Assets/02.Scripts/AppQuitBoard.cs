using BackEnd;
using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppQuitBoard : MonoBehaviour
{
    public void OnClickYes()
    {
        if (Backend.IsInitialized && Backend.UserInDate != null) //서버가 연결되어있고, 로그인 상태일 경우
        {
            Backend.BMember.Logout((callback) =>
            {
                if (callback.IsSuccess())
                {
                    if (PlayGamesPlatform.Instance.IsAuthenticated())
                    {
                        Debug.Log("구글로그아웃");
                        PlayGamesPlatform.Instance.SignOut();
                        Debug.Log("게임 종료");
#if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
#else
                        Application.Quit();
#endif
                    }
                }
                else
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                        Application.Quit();
#endif
                }
            });
        }
        else
        {
            Debug.Log("게임 종료");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    public void OnClickNo()
    {
        Destroy(gameObject);
        AppController.instance.isOn = false;
    }

    private void OnApplicationQuit()
    {
#if !UNITY_EDITOR
        System.Diagnostics.Process.GetCurrentProcess().Kill();
#endif
    }
}

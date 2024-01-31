using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoScenario : MonoBehaviour
{
    [SerializeField]
    private Progress progress;

    private void Awake()
    {
        SystemSetup();
    }

    private void SystemSetup()
    {
        Application.runInBackground = true; //백그라운드에서 게임이 진행됨

        //int width = Screen.width;                         
        //int height = (int)(Screen.width * 16 / 9);
        //Screen.SetResolution(width, height, true);        // 스크린 해상도 강제 설정 해제

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        progress.Play(OnAfterProgress);
    }

    private void OnAfterProgress()
    {
        SceneManager.LoadScene("Login");
    }
}
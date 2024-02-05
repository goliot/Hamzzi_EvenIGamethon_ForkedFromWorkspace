using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    public static StageSelect instance;

    public int chapter;
    public int stage;
    public int max_chapter;
    public int min_chapter;

    //public bool speedIncreased; // 퍄괴되지 않는 정보 임시 저장

    private void Awake()
    {
        Initialize();
        //speedIncreased = false;
        chapter = 1;
        stage = 1;
        max_chapter = 4;
        min_chapter = 1;
    }

    protected void Initialize()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 전환되어도 게임오브젝트를 파괴하지 않는다 
        }
        else
        {
            Destroy(this);
        }
    }

    public void SceneLoad()
    {
        GameManager.Inst.Resume();          // 게임 재개
        StartCoroutine(Loading());
    }

    IEnumerator Loading()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Battle_Proto");

        // 로딩이 완료될 때까지 대기
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // 로딩이 완료된 후에 호출
        UIManager.Inst.UpdateSpeedControllBtn();
    }

    public void OnClickStage1()
    {
        stage = 1;
    }

    public void OnClickStage2()
    {
        stage = 2;
    }

    public void OnClickStage3()
    {
        stage = 3;
    }

    public void OnClickStage4()
    {
        stage = 4;
    }

    public void OnClickStage5()
    {
        stage = 5;
    }
}

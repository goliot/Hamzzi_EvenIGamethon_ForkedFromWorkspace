using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        chapter = 1;
        stage = 1;
        max_chapter = 5;
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
        SceneManager.LoadScene("Battle_Proto");
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

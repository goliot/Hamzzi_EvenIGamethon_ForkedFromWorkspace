using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour
{
    public static StageSelect instance;

    public int chapter;
    public int stage;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
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

    public void OnClickChapter1()
    {
        chapter = 1;
    }

    public void OnClickChapter2()
    {
        chapter = 2;
    }

    public void OnClickChapter3()
    {
        chapter = 3;
    }

    public void OnClickChapter4()
    {
        chapter = 4;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageButtonHandler : MonoBehaviour
{
    public enum Stage { Stage1, Stage2, Stage3, Stage4, Stage5 }
    public Stage stage;

    Button button;

    public void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnClickStage()
    {
        switch (stage)
        {
            case Stage.Stage1:
                StageSelect.instance.OnClickStage1();
                
                break;
            case Stage.Stage2:
                StageSelect.instance.OnClickStage2();
                StageSelect.instance.SceneLoad();
                break;
            case Stage.Stage3:
                StageSelect.instance.OnClickStage3();
                StageSelect.instance.SceneLoad();
                break;
            case Stage.Stage4:
                StageSelect.instance.OnClickStage4();
                StageSelect.instance.SceneLoad();
                break;
            case Stage.Stage5:
                StageSelect.instance.OnClickStage5();
                StageSelect.instance.SceneLoad();
                break;

        }
    }
}

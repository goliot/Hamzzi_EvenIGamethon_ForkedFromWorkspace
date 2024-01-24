using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Inst.PlayBgm(AudioManager.BGM.BGM_Title, true);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            SceneLoader.Inst.ChangeScene(3);
        }
    }
}

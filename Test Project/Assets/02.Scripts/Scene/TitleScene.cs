using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Inst.PlayBgm(AudioManager.BGM.BGM_Opening);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            //SceneLoader.Inst.ChangeScene(3);
            SceneManager.LoadScene("Loading");
        }
    }
}

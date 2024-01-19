using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ConvertingNextScene());        
    }

    IEnumerator ConvertingNextScene()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("Title");
    }
}

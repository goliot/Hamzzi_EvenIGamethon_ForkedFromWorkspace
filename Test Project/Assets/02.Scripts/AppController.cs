using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AppController : MonoBehaviour
{
    public static AppController instance;

    public GameObject appQuitBoard;
    public bool isOn;

#if UNITY_ANDROID
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        isOn = false;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "Battle_Proto" && SceneManager.GetActiveScene().name != "Title" && SceneManager.GetActiveScene().name != "Loading" && !isOn)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameObject canvas = GameObject.Find("Canvas");
                GameObject popUp = Instantiate(appQuitBoard, new Vector3(0, 0, 0), Quaternion.identity, canvas.transform);
                isOn = true;
            }
        }
    }
#endif
}

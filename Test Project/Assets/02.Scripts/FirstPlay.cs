using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstPlay : MonoBehaviour
{
    public GameObject profileUI;

    public void OnSettingStart()
    {
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(1.5f);
        profileUI.SetActive(true);
    }

    public void SecondBtn()
    {
        SceneManager.LoadScene("Lobby");
    }
}

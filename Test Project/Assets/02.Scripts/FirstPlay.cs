using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstPlay : MonoBehaviour
{
    public GameObject profileUI;

    [SerializeField]
    private UserInfo user = new UserInfo();
    [SerializeField]
    private TextMeshProUGUI textNickname;

    private void Awake()
    {
        //user.onUserInfoEvent.AddListener(isFirstTime);
        user.GetUserInfoFromBackend();
    }

    public void OnSettingStart()
    {
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(1.5f);
        profileUI.SetActive(true);
        /*textNickname.GetComponent<TextMeshProUGUI>().text = UserInfo.Data.nickname == null ?
                            UserInfo.Data.gamerId : UserInfo.Data.nickname;*/
    }

    public void SecondBtn()
    {
        SceneManager.LoadScene("Lobby");
    }

    /*private void isFirstTime()
    {
        if (UserInfo.Data.nickname == null)
        {
            Debug.Log("처음이시군요");
            return;
        }
        else
        {
            Debug.Log("로비로 가라");
            SecondBtn(); //처음 아니면 그냥 로비로 가라
        }
    }*/
}

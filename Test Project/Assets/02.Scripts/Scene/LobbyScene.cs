using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : MonoBehaviour
{
    [SerializeField]
    private UserInfo user;

    private void Awake()
    {
        user.GetUserInfoFromBackend();
    }
    void Start()
    {
        AudioManager.Inst.PlayBgm(AudioManager.BGM.BGM_Title, false);
        AudioManager.Inst.PlayBgm(AudioManager.BGM.BGM_Lobby, true);

        //AudioManager.Inst.PlayBgm();        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.Inst.PlaySfx(AudioManager.SFX.Range);
            Debug.Log("Å×½ºÆ® SFX");
        }
    }
}

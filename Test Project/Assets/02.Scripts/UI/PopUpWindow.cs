using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpWindow : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_UI);
        PopUpBGMPlay();
    }

    // 팝업 창 끄는 함수
    public void OnClose()
    {
        LobbyBGMPlay();                             // 로비 BGM 다시 시작
        PopUpManager.Inst.ClosePopUp(this);
        Destroy(gameObject);
    }

    void PopUpBGMPlay()
    {
        if (gameObject.name == PopUpManager.Inst.PopUpNames.strShopUI)
        {
            Debug.Log(gameObject.name); // 디버깅
            AudioManager.Inst.StopBgm();
            AudioManager.Inst.PlayBgm(AudioManager.BGM.BGM_Shop);
        }
        // 다른 팝업 (도감)
    }

    void LobbyBGMPlay()
    {
        if (gameObject.name == PopUpManager.Inst.PopUpNames.strShopUI)
        {
            AudioManager.Inst.StopBgm();
            AudioManager.Inst.PlayBgm(AudioManager.BGM.BGM_Lobby);
        }
    }
}

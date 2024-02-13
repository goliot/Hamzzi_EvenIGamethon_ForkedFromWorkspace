using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplainUI : MonoBehaviour
{
    public void OnClickCornAd()
    {
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_UI);
        AdmobManager.instance.ShowRewardedAd("corn");
    }

    public void OnClickThreadmillAd()
    {
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_UI);
        AdmobManager.instance.ShowRewardedAd("threadmill");
    }
}

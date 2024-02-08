using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChapterButtonHandler: MonoBehaviour
{
    [SerializeField] Button button;
    public UnityEvent OnChapterChanged;


    public void OnClickNextChapter()
    {
        if (StageSelect.instance.chapter < StageSelect.instance.max_chapter)
        {
            AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_UI);
            Debug.Log("Next Chapter");
            StageSelect.instance.chapter++;
            StageSelect.instance.stage = 1;
            //UpdateChapterButtons();
            OnChapterChanged.Invoke();
        }
    }

    public void OnClickPrevChapter()
    {
        if (StageSelect.instance.chapter > StageSelect.instance.min_chapter)
        {
            AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_UI);
            Debug.Log("Prev Chapter");
            StageSelect.instance.chapter--;
            StageSelect.instance.stage = 1;
            //UpdateChapterButtons();
            OnChapterChanged.Invoke();
        }
    }

    public void OnClickGameStart()
    {
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_UI);
        BackendGameData.Instance.UserGameData.threadmill -= 1;
        //BackendGameData.Instance.GameDataUpdate();
        StageSelect.instance.SceneLoad();
    }

    //private void UpdateChapterButtons()
    //{
    //    UpdateNextChapterButton();
    //    UpdatePrevChapterButton();
    //}

    //private void UpdateNextChapterButton()
    //{
    //    button.interactable = (StageSelect.instance.chapter < StageSelect.instance.max_chapter);
    //    Debug.Log($"Next Chapter Button Interactable: {button.interactable}");
    //}

    //private void UpdatePrevChapterButton()
    //{
    //    button.interactable = (StageSelect.instance.chapter > StageSelect.instance.min_chapter);
    //    Debug.Log($"Prev Chapter Button Interactable: {button.interactable}");
    //}
}

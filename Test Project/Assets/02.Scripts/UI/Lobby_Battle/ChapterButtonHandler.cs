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
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Wheel);
        //BackendGameData.Instance.UserGameData.threadmill -= 1;
        //BackendGameData.Instance.GameDataUpdate();
        if (Threadmill.instance.m_HeartAmount < 1)
        {
            GameObject board = FindChildByName(transform, "WarningThreadmillBoard");
            if (board != null) board.SetActive(true);

            return;
        }
        else
        {
            Threadmill.instance.OnClickUseHeart();
            //Threadmill.instance.OnClickUseHeart();

            //BackendGameData.Instance.GameDataUpdate();
            StageSelect.instance.SceneLoad();
        }
    }

    GameObject FindChildByName(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.name == childName)
            {
                return child.gameObject;
            }

            // 재귀적으로 자식의 자식까지 찾기
            GameObject foundChild = FindChildByName(child, childName);
            if (foundChild != null)
            {
                return foundChild;
            }
        }

        // 찾지 못한 경우
        return null;
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

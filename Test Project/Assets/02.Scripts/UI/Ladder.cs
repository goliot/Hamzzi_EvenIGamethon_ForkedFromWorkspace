using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ladder : MonoBehaviour
{
    [SerializeField] GameObject myPanel;
    [SerializeField] RectTransform panelRect;
    [SerializeField] float topPosY, targetPosY;
    [SerializeField] float tweenDuration;

    void Awake()
    {
        panelRect.anchoredPosition = new Vector2(panelRect.anchoredPosition.x, topPosY);
    }

    //// ÆË¾÷
    //void Start()
    //{
    //    Intro();
    //}

    // ÀÎ°ÔÀÓUI
    private void OnEnable()
    {
        Intro();
    }

    private void OnDisable()
    {
        
    }

    void Intro()
    {
        panelRect.DOAnchorPosY(targetPosY, tweenDuration).SetUpdate(true);
    }

    public void Outro()
    {
        StartCoroutine(OutroCoroutine());
    }

    public IEnumerator OutroCoroutine()
    {
        Tweener tweener = panelRect.DOAnchorPosY(topPosY, tweenDuration);

        yield return tweener.WaitForCompletion();

        Debug.Log("Animation Complete");

        if (myPanel != null)
        {
            myPanel.SetActive(false);
            panelRect.anchoredPosition = new Vector2(panelRect.anchoredPosition.x, topPosY);
        }
    }
}

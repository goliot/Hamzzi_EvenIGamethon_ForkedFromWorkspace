using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ladder : MonoBehaviour
{
    [SerializeField] RectTransform panelRect;
    [SerializeField] float topPosY, targetPosY;
    [SerializeField] float tweenDuration;

    private void Awake()
    {
        //panelRect = new Rect(0, topPosY);
    }

    void Start()
    {
        Intro();
    }

    void Update()
    {
        
    }

    void Intro()
    {
        panelRect.DOAnchorPosY(targetPosY, tweenDuration);
    }

    public IEnumerator Outro()
    {
        Tweener tweener = panelRect.DOAnchorPosY(topPosY, tweenDuration);

        yield return tweener.WaitForCompletion();

        Debug.Log("Animation Complete");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragPlayer : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    
    public void OnPointerClick(PointerEventData eventData)
    {
        //// 팝업 UI를 활성화하는 로직 추가
        //if (eventData.clickCount == 1)
        //{
        //    ShowPopUp();
        //}
    }

    public void OnDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    private void ShowPopUp()
    {
        //// PopUpHandler 컴포넌트 가져오기
        //PopUpHandler popUpHandler = GetComponent<PopUpHandler>();

        //if (popUpHandler != null)
        //{
        //    popUpHandler.OnClickPopUpProfile();
        //    Debug.LogError("프로필UI 팝업 찾을 수 없습니다.");

        //}
        //else
        //{
        //    Debug.LogError("PopUpHandler를 찾을 수 없습니다.");
        //}
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 팝업 전체를 관리하는 매니저
public class PopUpManager : MonoBehaviour
{
    public GameObject myNoTouch;                                        // 팝업 창이 여러개 뜬다면 뒤쪽 창을 뜨지 못하게 하는 빈 Image UI 오브젝트
    public Stack<PopUpWindow> popUpList = new Stack<PopUpWindow>();     // 여러 개의 팝업 창이 열릴 경우 Stack 형태로 먼저 열린 창부터 close 하게 끔 구현
    public UnityAction allClose = null;                               // 전체 팝업 창을 다 닫는 기능(현재는 적용 X)

    // PopUpNames 객체를 가져와 사용
    public PopUpNames PopUpNames { get; private set; } = new PopUpNames("StageSelectUI", "SettingsUI", "ExplainStaminaUI", "ExplainCornUI", "ProfileUI", "TowerUI", "LevelUpUI", "StageStartUI", "TowerUpgradeSellUI", "ShopUI");

    public static PopUpManager Inst { get; private set; }               // 스태틱 프로퍼티로 사용

    private void Awake()
    {
        Inst = this;
    }

    // 팝업 생성
    public void CreatePopup(string popUp)
    {
        myNoTouch.SetActive(true);
        myNoTouch.transform.SetAsLastSibling();
        PopUpWindow scp = (Instantiate(Resources.Load(popUp), transform) as GameObject).GetComponent<PopUpWindow>(); // 프리팹으로 미리 만들어 놓은 UI를 생성하고, PopUpWindow 컴포넌트를 scp에 할당
        allClose += scp.OnClose;
        popUpList.Push(scp);
    }

    // 팝업 끄기
    public void ClosePopUp(PopUpWindow pw)
    {
        allClose -= pw.OnClose;
        popUpList.Pop();
        if(popUpList.Count == 0)
        {
            myNoTouch.SetActive(false);
        }
        else
        {
            myNoTouch.transform.SetSiblingIndex(myNoTouch.transform.GetSiblingIndex() - 1);
        }
    }

    // 팝업창 다 지우는 키 들어간다면 Update문에서 진행
    private void Update()
    {
        
    }
}

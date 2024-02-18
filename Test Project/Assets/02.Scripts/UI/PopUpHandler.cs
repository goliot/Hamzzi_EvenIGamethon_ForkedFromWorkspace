using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;

// PopUpManager에서 팝업 이름(PopUpNames)을 가져와 사용할 수 있다
// 이미 이름을 PopUpManager에서 생성 및 초기화 해놔서, 동작 함수만 추가 작성하면 됨
public class PopUpHandler : MonoBehaviour
{
    Ladder ladder;

    [System.Serializable]
    public class LobbyLoadEvent : UnityEvent { }
    public LobbyLoadEvent lobbyLoadEvent = new LobbyLoadEvent();

    public UnityEvent onShopBackground;
    public UnityEvent onDogamBackground;
    public static UnityEvent<int> OnDogamMonsterButtonClicked = new UnityEvent<int>();
    public static UnityEvent<int> OnDogamSkillButtonClicked = new UnityEvent<int>();

    private static PopUpHandler instance = null;
    public static PopUpHandler Inst
    {
        get
        {
            if (instance == null)
            {
                instance = new PopUpHandler();
            }

            return instance;
        }
    }

    void Awake()
    {
        Init();
    }

    void Init()
    {
        if (ladder == null)
        {
            ladder = FindObjectOfType<Ladder>();
        }
        else return;
    }

    #region PopUpButton
    public void OnClickPopUpStageSelect()
    {
        PopUpManager.Inst.CreatePopup(PopUpManager.Inst.PopUpNames.strStageSelectUI);
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Battle_Effect);
    }

    public void OnClickPopUpSettings()
    {
        PopUpManager.Inst.CreatePopup(PopUpManager.Inst.PopUpNames.strSettingsUI);
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_UI);
    }

    public void OnClickPopUpExplainStamina()
    {
        PopUpManager.Inst.CreatePopup(PopUpManager.Inst.PopUpNames.strExplainStaminaUI);
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Wheel);

    }

    public void OnClickPopUpExplainCorn()
    {
        PopUpManager.Inst.CreatePopup(PopUpManager.Inst.PopUpNames.strExplainCornUI);
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Corn);
    }

    public void OnClickPopUpProfile()
    {
        PopUpManager.Inst.CreatePopup(PopUpManager.Inst.PopUpNames.strProfileUI);
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_UI);
    }

    public void OnClickPopUpLevelUp() //레벨업 하시겠습니까?
    {
        PopUpManager.Inst.CreatePopup(PopUpManager.Inst.PopUpNames.strLevelUpUI);
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_UI);
    }

    public void OnClickPopUpStart()
    {
        PopUpManager.Inst.CreatePopup(PopUpManager.Inst.PopUpNames.strStageStartUI);
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Battle_Effect);
    }

    public void OnClickPopUpShop()
    {
        PopUpManager.Inst.CreatePopup(PopUpManager.Inst.PopUpNames.strShopUI);
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Shop_Effect);
        onShopBackground.Invoke();
    }

    public void OnClickPopUpDogam()
    {
        PopUpManager.Inst.CreatePopup(PopUpManager.Inst.PopUpNames.strDogamUI);
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Book_Effect);
        onDogamBackground.Invoke();
    }

    public void OnClickPopUpDogamMonster()
    {
        // 클릭된 버튼의 이름을 기준으로 데이터를 찾음
        string buttonName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        int dataIndex = GetIndexFromButtonName(buttonName);

        PopUpManager.Inst.CreatePopup(PopUpManager.Inst.PopUpNames.strDogamMonsterUI);
        OnDogamMonsterButtonClicked?.Invoke(dataIndex);
    }

    public void OnClickPopUpDogamSkill()
    {
        string buttonName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        int dataIndex = GetIndexFromButtonName(buttonName);

        PopUpManager.Inst.CreatePopup(PopUpManager.Inst.PopUpNames.strDogamSkillUI);
        OnDogamSkillButtonClicked?.Invoke(dataIndex);
    }

    public void OnClickPopUpLobbyTutorial()
    {
        PopUpManager.Inst.CreatePopup(PopUpManager.Inst.PopUpNames.strLobbyTutorialUI);
    }

    #endregion

    // 버튼 이름에서 숫자 부분을 추출하여 데이터 인덱스로 반환하는 함수
    private int GetIndexFromButtonName(string buttonName)
    {
        // 버튼 이름에서 "Monster" 부분을 제거하여 숫자 부분만 추출
        string numberString = "";
        if(buttonName.Contains("Monster"))
        {
            numberString = buttonName.Replace("Monster", "");
        }
        else if(buttonName.Contains("Skill"))
        {
            numberString = buttonName.Replace("Skill", "");
        }

        // 추출한 숫자 부분을 정수로 변환하여 데이터 인덱스로 사용
        return int.Parse(numberString) - 1; // 인덱스는 0부터 시작하므로 1을 뺌
    }

    public void OnClickExit()
    {
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_UI);
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            Debug.Log("로비 소환!");
            lobbyLoadEvent.Invoke();
        }
        else if (SceneManager.GetActiveScene().name == "Battle_Proto" && gameObject.name == "TowerUIExitButton")
        {
            if (ObjectDetector.instance.towerText != null)
            {
                ObjectDetector.instance.towerText.gameObject.SetActive(false);
            }
        }
        else if (SceneManager.GetActiveScene().name == "Battle_Proto" && gameObject.name == "StageUIExitButton")
        {

        }
        PopUpManager.Inst.popUpList.Peek().OnClose();
    }

    public void OnClickExitWithLadder()
    {
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_UI);
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            Debug.Log("로비 소환!");
            lobbyLoadEvent.Invoke();

        }
        StartCoroutine(OutroAndClosePopUp());
    }

    IEnumerator OutroAndClosePopUp()
    {
        yield return ladder.OutroCoroutine();
        // 애니메이션이 완료된 후에 PopUp을 닫음
        PopUpManager.Inst.popUpList.Peek().OnClose();
    }
    public void OnClickLevelUp()
    {
        int level = BackendGameData.Instance.UserGameData.level;
        int bread = BackendGameData.Instance.UserGameData.bread;
        int corn = BackendGameData.Instance.UserGameData.corn;

        //if() ==> 레벨업이 가능한 경우
        if (bread >= BackendGameData.Instance.UserGameData.levelUpData[level - 1] && corn >= BackendGameData.Instance.UserGameData.cornCostToLevelUp[level - 1] && level < 20)
        {
            BackendGameData.Instance.UserGameData.bread -= BackendGameData.Instance.UserGameData.levelUpData[level - 1];
            BackendGameData.Instance.UserGameData.corn -= BackendGameData.Instance.UserGameData.cornCostToLevelUp[level - 1];
            BackendGameData.Instance.UserGameData.level += 1;

            LevelUpUI.Inst.ChangeCornText();
            BackendGameData.Instance.GameDataUpdate();
            //BackendGameData.Instance.GameDataLoad();
            AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Lobby_Hamster_Level_Up);
        }
        //else ==> 레벨업이 불가능한 경우
        else
        {
            //임시
            AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_UI);
            if(bread < BackendGameData.Instance.UserGameData.levelUpData[level - 1])
            {
                AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_UI);
                //빵이 부족합니다 메시지 띄우기
                StartCoroutine(NotEnoughBread());
            }
            else if (corn < BackendGameData.Instance.UserGameData.cornCostToLevelUp[level - 1])
            {
                StartCoroutine(NotEnoughCorn());
            }
        }
    }

    IEnumerator NotEnoughCorn()
    {
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_UI);
        transform.parent.parent.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        transform.parent.parent.GetChild(1).gameObject.SetActive(false);
    }

    IEnumerator NotEnoughBread()
    {
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_UI);
        transform.parent.parent.GetChild(2).gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        transform.parent.parent.GetChild(2).gameObject.SetActive(false);
    }


}

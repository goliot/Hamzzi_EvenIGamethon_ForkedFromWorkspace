using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DogamUI : MonoBehaviour
{
    public Button t_HamzziBtn;
    public Button t_MonsterBtn;
    public Button t_Skill;

    public GameObject hamzzi;
    public GameObject monster;
    public GameObject skill;

    public Button h_PrevBtn;
    public Button h_NextBtn;
    public GameObject[] hamzzis;
    int h_curIdx;

    public Button m_PrevBtn;
    public Button m_NextBtn;
    public GameObject[] monsters;
    int m_curIdx;
    public TextMeshProUGUI chapterText;

    public Button s_PrevBtn;
    public Button s_NextBtn;
    public GameObject[] skills;
    int s_curIdx;
    public TextMeshProUGUI skillText;

    // 맵
    public GameObject lobbyGrid; // 로비 그리드
    public GameObject dogamGrid; // 도감 그리드

    void Awake()
    {
        Init();
    }

    void Init()
    {
        // 도감 상단 버튼
        t_HamzziBtn.onClick.AddListener(() => OnClickTopBtn(hamzzi));
        t_MonsterBtn.onClick.AddListener(() => OnClickTopBtn(monster));
        t_Skill.onClick.AddListener(() => OnClickTopBtn(skill));
        t_HamzziBtn.onClick.Invoke();   // 도감 햄스터로 초기화

        // 햄스터 버튼
        h_curIdx = 0;
        h_PrevBtn.onClick.AddListener(() => OnClickPrevBtn());
        h_NextBtn.onClick.AddListener(() => OnClickNextBtn());

        // 몬스터 버튼 및 챕터
        m_curIdx = 0;
        m_PrevBtn.onClick.AddListener(() => OnClickPrevBtn_M());
        m_NextBtn.onClick.AddListener(() => OnClickNextBtn_M());

        // 스킬 버튼 및 텍스트
        s_curIdx = 0;
        s_PrevBtn.onClick.AddListener(() => OnClickPrevBtn_S());
        s_NextBtn.onClick.AddListener(() => OnClickNextBtn_S());

        // 맵
        lobbyGrid = FindObjectOfType<Grid>().gameObject.transform.GetChild(0).gameObject;
        dogamGrid = FindObjectOfType<Grid>().gameObject.transform.GetChild(2).gameObject;

        OnPopupOpened();
    }

    private void LateUpdate()
    {
        chapterText.text = string.Format($"0{m_curIdx + 1}");
        if (s_curIdx == 0) skillText.text = string.Format($"선택 스킬");
        else skillText.text = string.Format($"용병단 스킬");
    }

    private void OnClickTopBtn(GameObject objectToActivate)
    {
        hamzzi.SetActive(objectToActivate == hamzzi);
        monster.SetActive(objectToActivate == monster);
        skill.SetActive(objectToActivate == skill);

        // 도감 BGM 사운드 작업 여기에
        if (objectToActivate == hamzzi)
        {
            AudioManager.Inst.StopBgm();
            AudioManager.Inst.PlayBgm(AudioManager.BGM.BGM_IllustratedGuideHamster);
        }
        else if (objectToActivate == monster)
        {
            AudioManager.Inst.StopBgm();
            AudioManager.Inst.PlayBgm(AudioManager.BGM.BGM_IllustratedGuideMonster);
        }
        else if (objectToActivate == skill)
        {
            AudioManager.Inst.StopBgm();
            AudioManager.Inst.PlayBgm(AudioManager.BGM.BGM_IllustratedGuideArchive);
        }

    }

    //private void OnClickNextBtn(int idx, GameObject[] gameObjects)
    //{
    //    idx = (idx + 1) % gameObjects.Length;
    //    Debug.Log(idx);
    //    UpdateHamsters(idx, gameObjects);
    //}

    //private void OnClickPrevBtn(int idx, GameObject[] gameObjects)
    //{
    //    idx = (idx - 1 + gameObjects.Length) % gameObjects.Length;
    //    Debug.Log(idx);
    //    UpdateHamsters(idx, gameObjects);
    //}

    //private void UpdateHamsters(int idx, GameObject[] gameObjects)
    //{
    //    for (int i = 0; i < gameObjects.Length; i++)
    //    {
    //        gameObjects[i].SetActive(i == idx);
    //        Debug.Log(gameObjects[i].gameObject.activeSelf);
    //    }
    //}

    // 햄스터
    private void OnClickNextBtn()
    {
        h_curIdx = (h_curIdx + 1) % hamzzis.Length;
        Debug.Log(h_curIdx);
        UpdateHamsters();
    }

    private void OnClickPrevBtn()
    {
        h_curIdx = (h_curIdx - 1 + hamzzis.Length) % hamzzis.Length;
        Debug.Log(h_curIdx);
        UpdateHamsters();
    }

    private void UpdateHamsters()
    {
        for (int i = 0; i < hamzzis.Length; i++)
        {
            hamzzis[i].SetActive(i == h_curIdx);
            Debug.Log(hamzzis[i].gameObject.activeSelf);
        }
    }

    // 몬스터
    private void OnClickNextBtn_M()
    {
        m_curIdx = (m_curIdx + 1) % monsters.Length;
        Debug.Log(m_curIdx);
        UpdateMonsters();
    }

    private void OnClickPrevBtn_M()
    {
        m_curIdx = (m_curIdx - 1 + monsters.Length) % monsters.Length;
        Debug.Log(m_curIdx);
        UpdateMonsters();
    }

    private void UpdateMonsters()
    {
        for (int i = 0; i < monsters.Length; i++)
        {
            monsters[i].SetActive(i == m_curIdx);
            Debug.Log(monsters[i].gameObject.activeSelf);
        }
    }

    // 스킬
    private void OnClickNextBtn_S()
    {
        s_curIdx = (s_curIdx + 1) % skills.Length;
        Debug.Log(s_curIdx);
        UpdateSkills();
    }

    private void OnClickPrevBtn_S()
    {
        s_curIdx = (s_curIdx - 1 + skills.Length) % skills.Length;
        Debug.Log(s_curIdx);
        UpdateSkills();
    }

    private void UpdateSkills()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            skills[i].SetActive(i == s_curIdx);
            Debug.Log(skills[i].gameObject.activeSelf);
        }
    }

    // 맵
    public void OnPopupOpened()
    {
        lobbyGrid.SetActive(false);
        dogamGrid.SetActive(true);
    }

    public void OnPopupClosed()
    {
        lobbyGrid.SetActive(true);
        dogamGrid.SetActive(false);
        AudioManager.Inst.StopBgm();
        AudioManager.Inst.PlayBgm(AudioManager.BGM.BGM_Lobby);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// 제네릭 Signleton 구현으로 인해 스태틱 변수를 만들 필요가 없음
    /// </summary>
    //public static GameManager instance; 

    [Header("Game Control")]
    public bool isLive;
    public float gameTime;
    public float waveChangeTime;

    [Header("Game Object")]
    public Player player;
    public PoolManager pool;
    public Scanner scanner;
    public LevelUp uiLevelUp;

    /// <summary>
    /// 몬스터를 처치하고 레벨업을 하게 구현
    /// </summary>
    [Header("Player Info")]
    public int level; // 플레이어의 현재 레벨
    public int kill; // 플레이어의 현재 킬수(UI 상 표기하진 않지만 우선 기록)
    public int exp; // 현재까지 쌓은 경험치 0~100% 까지 표기
    public int[] nextExp; // 다음 레벨에 필요한 경험치량 임의로 설정 Test용

    #region
    /// <summary>
    /// Player 현재 체력, 최대 체력에 관련된 Data 인데 이거 구조체로 만들어 두신 곳으로 빼내야 할지 논의 필요
    /// 임시 UI Test용 임시 작성이라 생각하시면 됩니다.
    /// </summary>
    /// 
    [HideInInspector]
    public Wall wall; // 벽 참조, HP 데이터를 벽이 가지고 있음
    #endregion

    private void Awake()
    {
        base.Initialize();
        //instance = this; // 제네릭 Singleton 스크립트 안에 Initialize()를 통해 자기 자신에 할당하는 함수를 미리 생성해놓음
    }

    private void Start()
    {
        kill = 0;
        exp = 0;
        level = 0;
        nextExp = new int[]{ 120, 140, 160, 180, 200, 220, 240, 260, 280, 300, 320, 340, 360, 380, 400, 420, 440, 460, 480, 500 };
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        gameTime += Time.deltaTime;
    }

    // 경험치 증가 함수
    public void GetExp(int killExp)
    {
        exp += killExp;
        Debug.Log("경험치 획득" + killExp);
        // 필요 경험치에 도달하면 레벨업
        if(exp >= nextExp[level] && level < 20)
        {
            level++;
            exp -= nextExp[level-1];          // 경험치 초기화
            uiLevelUp.Show(); // 레벨업 UI 켜기
        }
    }

    // 게임 시간 정지
    public void Stop()
    {
        Time.timeScale = 0.0f;
    }

    // 게임 시간 재개
    public void Resume()
    {
        Time.timeScale = 1.0f;
    }

}
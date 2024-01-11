using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// 제네릭 Signleton 구현으로 인해 스태틱 변수를 만들 필요가 없음
    /// </summary>
    //public static GameManager instance; 

    public Player player;
    public PoolManager pool;
    public Scanner scanner;
    public float gameTime;
    public float waveChangeTime;

    private void Awake()
    {
        base.Initialize();
        //instance = this; // 제네릭 Singleton 스크립트 안에 Initialize()를 통해 자기 자신에 할당하는 함수를 미리 생성해놓음
    }

    private void Update()
    {
        gameTime += Time.deltaTime;
    }

}
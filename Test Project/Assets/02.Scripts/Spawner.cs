using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;

    public int currentWave;
    public int maxWave = 20;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    private void Start()
    {
        InvokeRepeating("IncreaseWaveAndWaveStart", 0f, GameManager.Inst.waveChangeTime);
    }

    private void Update()
    {
        float gameTime = GameManager.Inst.gameTime;
        if (currentWave >= maxWave)
        {
            CancelInvoke("IncreaseWave");
            Debug.Log("Wave가 최대에 도달하여 InvokeRepeating이 종료되었습니다.");
        }
    }

    private void IncreaseWaveAndWaveStart()
    {
        currentWave++;
        Debug.Log("Wave " + currentWave + " 시작");
        StartCoroutine(SpawnWaveEnemies(currentWave));
    }

    void Spawn()
    {
        GameObject enemy = GameManager.Inst.pool.Get(0);    
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[Random.Range(0, spawnData.Length)]);
    }
    
    IEnumerator SpawnWaveEnemies(int wave)
    {
        for(int i=0; i<wave; i++)
        {
            Spawn();
            yield return new WaitForSeconds(0.5f);
        }
    }
}

[System.Serializable]
public class SpawnData //몬스터 능력치 데이터
{
    public int spriteType;
    public int health;
    public float speed;
    public int exp;
}

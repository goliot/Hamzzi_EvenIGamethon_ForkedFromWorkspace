using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System;
using System.Xml.Serialization;

public class Spawner : MonoBehaviour //웨이브별 몬스터 스폰
{
    public Transform[] spawnPoint;
    public List<SpawnData> spawnData = new List<SpawnData>();
    string xmlFileName = "MobData";

    public int currentWave;
    public int maxWave = 20;

    public int[,] waveInfo = new int[,]
    {
        {2, 0},
        {3, 0},
        {4, 0},
        {5, 0},
        {6, 0},
        {6, 0},
        {6, 0},
        {6, 0},
        {6, 0},
        {6, 0},
        {6, 0},
        {6, 0},
        {6, 0},
        {6, 0},
        {6, 0},
        {6, 0},
        {6, 0},
        {6, 0},
        {6, 0},
        {6, 0},
    };

    void Start()
    {
        LoadXML(xmlFileName);
        InvokeRepeating("IncreaseWaveAndWaveStart", 0f, GameManager.Inst.waveChangeTime);
    }

    private void LoadXML(string _fileName)
    {
        TextAsset txtAsset = (TextAsset)Resources.Load(_fileName);
        if (txtAsset == null)
        {
            Debug.LogError("Failed to load XML file: " + _fileName);
            return;
        }

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(txtAsset.text);

        // 전체 아이템 가져오기 예제.
        XmlNodeList all_nodes = xmlDoc.SelectNodes("root/Sheet1");
        foreach (XmlNode node in all_nodes)
        {
            SpawnData newData = new SpawnData();

            newData.spriteType = int.Parse(node.SelectSingleNode("spriteType").InnerText);
            newData.health = float.Parse(node.SelectSingleNode("health").InnerText);
            newData.damage = float.Parse(node.SelectSingleNode("damage").InnerText);
            newData.atkSpeed = float.Parse(node.SelectSingleNode("atkSpeed").InnerText);
            newData.speed = float.Parse(node.SelectSingleNode("speed").InnerText);

            spawnData.Add(newData);
        }
    }

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
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
        //Debug.Log("Wave " + currentWave + " 시작");
        StartCoroutine(SpawnWaveEnemies(currentWave));
    }

    void Spawn()
    {
        GameObject enemy = GameManager.Inst.pool.Get(0);    
        enemy.transform.position = spawnPoint[UnityEngine.Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[UnityEngine.Random.Range(0, spawnData.Count)]);
    }
    
    IEnumerator SpawnWaveEnemies(int wave)
    {
        int mobsThisWave = waveInfo[wave-1, 0]; //현재 웨이브에 나와야 할 몹 수
        //Debug.Log(mobsThisWave);
        for(int i=0; i<mobsThisWave; i++)
        {
            Spawn();
            yield return new WaitForSeconds(0.5f);
        }
    } //여기는 나중에 스테이지-챕터까지 다 구현이 된다면 xml로 받아오게 수정
}

[System.Serializable]
public class SpawnData //몬스터 능력치 데이터
{
    public int spriteType;
    public float health;
    public float damage;
    public float atkSpeed;
    public float speed;
}

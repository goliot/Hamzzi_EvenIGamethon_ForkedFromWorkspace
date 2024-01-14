using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System;
using System.Xml.Serialization;
using UnityEngine.Events;

public class Spawner : MonoBehaviour //웨이브별 몬스터 스폰
{
    public Transform[] spawnPoint;
    public List<SpawnData> spawnData = new List<SpawnData>();
    string xmlFileName = "MobData";

    public event UnityAction<int> OnWaveChanged; // 웨이브 바뀔 때 알려주는 UnityAction

    public int currentWave;
    public int maxWave = 20;

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
        Debug.Log("Wave " + currentWave + " 시작");


        // 추가 작성 부분
        if (OnWaveChanged != null) 
            OnWaveChanged.Invoke(currentWave);          // 웨이브 바뀔 때, 


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
        //여기에 for문에 웨이브별 몬스터 수 정보를 넣으면 된다
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
    public float health;
    public float damage;
    public float atkSpeed;
    public float speed;
}

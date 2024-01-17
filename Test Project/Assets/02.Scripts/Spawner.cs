using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System;
using System.Xml.Serialization;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour //웨이브별 몬스터 스폰
{
    public Transform[] spawnPoint;
    public List<SpawnData> spawnData = new List<SpawnData>();
    string xmlFileName = "MobData";
    string stageXmlFileName;

    [Header("#StageInfo")]
    public int chapter;
    public int stage;
    public List<StageWaveData> stageWaveData = new List<StageWaveData>();
    public List<MobMagnificationData> mobMagnificationData = new List<MobMagnificationData>();
    string magXmlFileName = "MobStatMagnification";

    public event UnityAction<int> OnWaveChanged; // 웨이브 바뀔 때 알려주는 UnityAction

    public int currentWave;
    public int maxWave = 20;
    public int stageMobCount = 0; //현재 스테이지에서 나오는 총 몹의 수 -> 승리 로직에 사용

    void Start()
    {
        LoadXML(xmlFileName);
        InvokeRepeating("IncreaseWaveAndWaveStart", 0f, GameManager.Inst.waveChangeTime);

        //웨이브 정보 관련 함수
        chapter = StageSelect.instance.chapter;
        stage = StageSelect.instance.stage;
        stageXmlFileName = "Chapter" + chapter;
        LoadStageXml(stageXmlFileName);

        //스테이지별 몹 능력치 배율
        LoadMagXml(magXmlFileName);
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

    private void LoadStageXml(string _fileName)
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
            StageWaveData newData = new StageWaveData();

            newData.wave = int.Parse(node.SelectSingleNode("wave").InnerText);
            newData.mob1 = int.Parse(node.SelectSingleNode("mob1").InnerText);
            newData.mob2 = int.Parse(node.SelectSingleNode("mob2").InnerText);
            newData.mob3 = int.Parse(node.SelectSingleNode("mob3").InnerText);
            newData.semiBoss = int.Parse(node.SelectSingleNode("semiBoss").InnerText);
            newData.boss = int.Parse(node.SelectSingleNode("boss").InnerText);

            stageWaveData.Add(newData);
        }
    }

    private void LoadMagXml(string _fileName)
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
            MobMagnificationData newData = new MobMagnificationData();

            newData.chpater = int.Parse(node.SelectSingleNode("chapter").InnerText);
            newData.stage = int.Parse(node.SelectSingleNode("stage").InnerText);
            newData.mobHealth = float.Parse(node.SelectSingleNode("mobHealth").InnerText);
            newData.mobDamage = float.Parse(node.SelectSingleNode("mobDamage").InnerText);

            mobMagnificationData.Add(newData);
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

    void Victory()
    {
        //승리 로직
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

    void Spawn(int index)
    {
        GameObject enemy = GameManager.Inst.pool.Get(0);
        enemy.transform.position = spawnPoint[UnityEngine.Random.Range(1, spawnPoint.Length)].position;
        //enemy.GetComponent<Enemy>().Init(spawnData[UnityEngine.Random.Range(0, spawnData.Count)]);
        enemy.GetComponent<Enemy>().Init(spawnData[index]);
        enemy.GetComponent<Enemy>().health *= mobMagnificationData[(chapter - 1) * 5 + (stage - 1)].mobHealth;
        enemy.GetComponent<Enemy>().damage *= mobMagnificationData[(chapter - 1) * 5 + (stage - 1)].mobDamage;

        SpriteRenderer spriteRenderer = enemy.GetComponent<SpriteRenderer>();
        spriteRenderer.transform.localScale = new Vector3(4, 4, 1); //scale 초기화값

        if (index % 5 == 4) // 보스 몹일 경우
        {
            if (spriteRenderer != null)
            {
                // 현재 scale 값을 가져옴
                Vector3 currentScale = spriteRenderer.transform.localScale;

                // scale 값을 현재 값에 2를 곱함
                spriteRenderer.transform.localScale = new Vector3(currentScale.x * 2, currentScale.y * 2, currentScale.z);
            }
        }
    }

    IEnumerator SpawnWaveEnemies(int wave)
    {
        int[] eachMobThisWave = new int[5];
        eachMobThisWave[0] = stageWaveData[wave - 1].mob1;
        eachMobThisWave[1] = stageWaveData[wave - 1].mob2;
        eachMobThisWave[2] = stageWaveData[wave - 1].mob3;
        eachMobThisWave[3] = stageWaveData[wave - 1].semiBoss;
        eachMobThisWave[4] = stageWaveData[wave - 1].boss;

        //각 챕터별로 mob1, 2, 3, semi, boss가 프리팹에서 몇번째꺼를 불러오는지 알아야 함
        //그 데이터를 받아서 리스트에 숫자별로 저장한 다음, 섞어서 소환
        //각 몹별로 spawnData에서 몇번째 원소인지 알아보자
        int[] eachIndex = new int[5]; //mob1, mob2, mob3, semiBoss, boss
        switch(chapter)
        {
            case 1:
                for(int i=0; i<5; i++)
                {
                    eachIndex[i] = i;
                }
                break;
            case 2: 
                for(int i=0; i<5; i++)
                {
                    eachIndex[i] = i + 5;
                }
                break;
            case 3:
                for(int i=0; i<5; i++)
                {
                    eachIndex[i] = i + 10;
                }
                break;
            case 4: 
                for(int i=0; i<5; i++)
                {
                    eachIndex[i] = i + 15;
                }
                break;
            default:
                for (int i = 0; i < 5; i++)
                {
                    eachIndex[i] = i;
                }
                break;
        }

        List<int> spawnList = new List<int>();
        for(int i=0; i<4; i++)
        {
            for(int j=0; j < eachMobThisWave[i]; j++)
            {
                spawnList.Add(eachIndex[i]);
            }
        }
        string output = string.Join(" ", spawnList);
        Debug.Log("Origin Spawn: " + output);

        ShuffleList(spawnList);

        foreach(var index in spawnList)
        {
            Spawn(index);
            yield return new WaitForSeconds(0.5f);
        }

        /*//여기에 for문에 웨이브별 몬스터 수 정보를 넣으면 된다
        for (int i = 0; i < mobThisWave; i++)
        {
            Spawn(i);
            yield return new WaitForSeconds(0.5f);
        }*/
    }

    //순서 무작위로 섞기 - FisherYate 알고리즘
    void ShuffleList<T>(List<T> list)
    {
        System.Random random = new System.Random();

        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);

            // Swap list[i] and list[j]
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        string output = string.Join(" ", list);
        Debug.Log("Shuffled Spawn: " + output);
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

[System.Serializable]
public class StageWaveData
{ //스테이지, 웨이브별 각 몬스터의 소환 수
    public int wave;
    public int mob1;
    public int mob2;
    public int mob3;
    public int semiBoss;
    public int boss;
}

[System.Serializable]
public class MobMagnificationData
{
    public int chpater;
    public int stage;
    public float mobHealth;
    public float mobDamage;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;
using System.Net.Http.Headers;

public class Spawner : MonoBehaviour //웨이브별 몬스터 스폰
{
    public Transform[] spawnPoint;
    public List<SpawnData> spawnData = new List<SpawnData>();
    public GameObject shadowObject;
    string xmlFileName = "MobData";
    string stageXmlFileName;

    [Header("#StageInfo")]
    public int chapter;
    public int stage;
    public List<StageWaveData> stageWaveData = new List<StageWaveData>();
    public List<MobMagnificationData> mobMagnificationData = new List<MobMagnificationData>();
    string magXmlFileName = "MobStatMagnification";
    private bool isGameLive;
    public TextMeshProUGUI cornText;
    public TextMeshProUGUI breadText;

    [Header("Boss Effect")]
    public Image redLightImage;
    public Image warningImage;
    public GameObject bossHPBar;

    public event UnityAction<int> OnWaveChanged; // 웨이브 바뀔 때 알려주는 UnityAction

    [Header("Wave Info")]
    public int currentWave;
    public int maxWave = 20;
    public int stageMobCount = 0; //현재 스테이지에서 나오는 총 몹의 수 -> 승리 로직에 사용
    public GameObject[] tilemaps;

    [Header("Reward")]
    int[,] cornReward = { { 30, 40, 50, 60, 70 },
                          { 80, 90, 100, 110, 120 },
                          { 130, 140, 150, 160, 170 },
                          { 180, 190, 200, 210, 220 } };
    int[,] breadReward = { { 20, 30 ,40, 50, 60 },
                            {70, 80, 90, 100, 110 },
                           {120, 130, 140, 150, 160 },
                           {170, 180, 190, 200, 210 } };

    void Start()
    {
        AudioManager.Inst.StopBgm();
        AudioManager.Inst.PlayBgm(AudioManager.BGM.BGM_Chapter01);

        LoadXML(xmlFileName);
        InvokeRepeating("IncreaseWaveAndWaveStart", 0f, GameManager.Inst.waveChangeTime);

        //웨이브 정보 관련 함수
        if (GameObject.Find("StageManager") == null)
        {
            chapter = 1;
            stage = 1;
        }
        else
        {
            chapter = StageSelect.instance.chapter;
            stage = StageSelect.instance.stage;
        }

        //챕터에 맞는 브금 재생
        AudioManager.Inst.StopBgm();
        switch (chapter)
        {
            case 1:
                AudioManager.Inst.PlayBgm(AudioManager.BGM.BGM_Chapter01);
                break;
            case 2:
                AudioManager.Inst.PlayBgm(AudioManager.BGM.BGM_Chapter02);
                break;
            case 3:
                AudioManager.Inst.PlayBgm(AudioManager.BGM.BGM_Chapter03);
                break;
            case 4:
                AudioManager.Inst.PlayBgm(AudioManager.BGM.BGM_Chapter04);
                break;
        }

        stageXmlFileName = "Chapter" + chapter;
        LoadStageXml(stageXmlFileName);

        //스테이지별 몹 능력치 배율
        LoadMagXml(magXmlFileName);

        /*
        LoadXML(xmlFileName);
        InvokeRepeating("IncreaseWaveAndWaveStart", 0f, GameManager.Inst.waveChangeTime);

        //웨이브 정보 관련 함수
        chapter = StageSelect.instance.chapter;
        stage = StageSelect.instance.stage;
        stageXmlFileName = "Chapter" + chapter;
        LoadStageXml(stageXmlFileName);

        //스테이지별 몹 능력치 배율
        LoadMagXml(magXmlFileName);
        */

        for(int i=0; i<20; i++)
        {
            if(stage == 5 && i == 9) //5스테이지 10웨이브의 경우는 보스 1마리만 추가
            {
                stageMobCount++;
                continue;
            }
            else if (stage >= 3)
            {
                stageMobCount += stageWaveData[i].mob1;
                stageMobCount += stageWaveData[i].mob2;
                stageMobCount += stageWaveData[i].mob3;
                stageMobCount += stageWaveData[i].semiBoss;
                stageMobCount += stageWaveData[i].boss;
            }
            else //스테이지 1, 2에서는 세미몹 카운트 x
            {
                stageMobCount += stageWaveData[i].mob1;
                stageMobCount += stageWaveData[i].mob2;
                stageMobCount += stageWaveData[i].mob3;
            }
        }

        tilemaps[(chapter - 1) * 5 + (stage - 1)].SetActive(true);
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
        isGameLive = true;
    }

    private void Update()
    {
        float gameTime = GameManager.Inst.gameTime;

        if (currentWave >= maxWave)
        {
            CancelInvoke("IncreaseWaveAndWaveStart");
            Debug.Log("Wave가 최대에 도달하여 InvokeRepeating이 종료되었습니다.");
        }

        if(GameManager.Inst.kill >= stageMobCount || Input.GetKeyDown(KeyCode.V))
        {
            if (isGameLive)
            {
                isGameLive = false;
                GameManager.Inst.Stop();
                Victory();
            }
            else return;
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            currentWave = 9;
        }
    }

    void Victory()
    {
        int breadRewardThisStage = breadReward[chapter - 1, stage - 1];
        int cornRewardThisStage = cornReward[chapter - 1, stage - 1];
        cornText.text = cornRewardThisStage.ToString();
        breadText.text = breadRewardThisStage.ToString();

        BackendGameData.Instance.UserGameData.bread += breadRewardThisStage;
        BackendGameData.Instance.UserGameData.corn += cornRewardThisStage;
        BackendGameData.Instance.GameDataUpdate();

        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Stage_Clear);

        UIManager.Inst.victoryUI.SetActive(true);          // VictoryUI를 켜기만 한다
    }

    private void IncreaseWaveAndWaveStart()
    {
        currentWave++;
        //Debug.Log("Wave " + currentWave + " 시작");
        // 추가 작성 부분
        if (OnWaveChanged != null)
            OnWaveChanged.Invoke(currentWave);          // 웨이브 바뀔 때, 

        if (stage == 5 && currentWave == 10) //각 챕터 스테이지 5, 10웨이브에서
        {
            //경고문구가 뜬 다음에 웨이브가 진행되도록, 별도의 보스 스폰 함수 사용할것
            StartCoroutine(BossStageEffect());
        }
        else StartCoroutine(SpawnWaveEnemies(currentWave));
    }

    IEnumerator BossStageEffect()
    {
        redLightImage.gameObject.SetActive(true);
        warningImage.gameObject.SetActive(true);
        // 2. 화면 전체가 반투명한 빨간빛으로 3회 깜빡임
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Boss_Warning);

        Spawn((chapter * 5 - 1)); //보스 소환

        for (int i = 0; i < 3; i++)
        {
            SetRedLightImage(new Color(1f, 0f, 0f, 0.5f)); // 빨간빛 이미지 색상 조정
            yield return new WaitForSeconds(0.5f); // 0.5초 대기
            SetRedLightImage(Color.clear); // 빨간빛 이미지 색상을 원래 색으로 설정
            yield return new WaitForSeconds(0.5f); // 0.5초 대기
        }
        redLightImage.gameObject.SetActive(false);
        warningImage.gameObject.SetActive(false);

    }

    // 빨간빛 이미지의 색상을 조정하는 함수
    void SetRedLightImage(Color color)
    {
        redLightImage.color = color;
    }

    void Spawn(int index)
    {
        GameObject enemy = GameManager.Inst.pool.Get(0);

        //그림자 생성 후 약간 투명해지도록 하는 코드 블록
        GameObject shadow = Instantiate(shadowObject, enemy.transform.position, Quaternion.identity);
        shadow.transform.SetParent(enemy.transform);
        shadow.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        shadow.transform.localPosition = new Vector3(shadow.transform.localPosition.x, shadow.transform.localPosition.y - 0.12f, shadow.transform.localPosition.z);
        SpriteRenderer shadowRenderer = shadow.GetComponent<SpriteRenderer>();
        Material newMaterial = new Material(Shader.Find("Sprites/Default"));
        newMaterial.color = new Color(1f, 1f, 1f, 0.7f);
        shadowRenderer.material = newMaterial;

        enemy.transform.position = spawnPoint[UnityEngine.Random.Range(1, spawnPoint.Length)].position;
        //enemy.GetComponent<Enemy>().Init(spawnData[UnityEngine.Random.Range(0, spawnData.Count)]);
        enemy.GetComponent<Enemy>().Init(spawnData[index]);
        enemy.GetComponent<Enemy>().health *= mobMagnificationData[(chapter - 1) * 5 + (stage - 1)].mobHealth;
        enemy.GetComponent<Enemy>().maxHealth *= mobMagnificationData[(chapter - 1) * 5 + (stage - 1)].mobHealth;
        enemy.GetComponent<Enemy>().damage *= mobMagnificationData[(chapter - 1) * 5 + (stage - 1)].mobDamage;

        SpriteRenderer spriteRenderer = enemy.GetComponent<SpriteRenderer>();
        spriteRenderer.transform.localScale = new Vector3(4, 4, 1); //scale 초기화값

        if(index % 5 == 3) //준보스
        {
            if (spriteRenderer != null)
            {
                // 현재 scale 값을 가져옴
                Vector3 currentScale = spriteRenderer.transform.localScale;

                // scale 값을 현재 값에 2를 곱함
                spriteRenderer.transform.localScale = new Vector3(currentScale.x * 1.5f, currentScale.y * 1.5f, currentScale.z);
            }
        }
        else if (index % 5 == 4) // 보스 몹일 경우
        {
            if (spriteRenderer != null)
            {
                // 현재 scale 값을 가져옴
                Vector3 currentScale = spriteRenderer.transform.localScale;

                // scale 값을 현재 값에 2를 곱함
                spriteRenderer.transform.localScale = new Vector3(currentScale.x * 2, currentScale.y * 2, currentScale.z);

                //여기에 hp바 넣기

            }
        }
    }

    IEnumerator SpawnWaveEnemies(int wave)
    {
        int[] eachMobThisWave = new int[5]; //각원소 = 이번 웨이브에서 각 몬스터의 소환 수
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
            if (stage < 3 && i == 3) break; //스테이지1, 2에서는 세미몹 안나오도록
            for (int j=0; j < eachMobThisWave[i]; j++)
            {
                spawnList.Add(eachIndex[i]);
            }
        }
        string output = string.Join(" ", spawnList);
        //Debug.Log("Origin Spawn: " + output);

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
        //Debug.Log("Shuffled Spawn: " + output);
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
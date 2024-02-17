using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using System.Xml;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;

    public List<TowerData> towerData = new List<TowerData>();
    public Dictionary<Transform, GameObject> InstalledTower = new Dictionary<Transform, GameObject>();
    public GameObject seedUI;       // 설치 불가 UI

    string xmlFileName = "SubHamData";

    private void Start()
    {
        LoadXML(xmlFileName);
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

        XmlNodeList all_nodes = xmlDoc.SelectNodes("root/Sheet1");
        foreach (XmlNode node in all_nodes)
        {
            TowerData newData = new TowerData();

            newData.towerType = int.Parse(node.SelectSingleNode("towerType").InnerText);
            newData.atkSpeed = float.Parse(node.SelectSingleNode("atkSpeed").InnerText);
            newData.damage = float.Parse(node.SelectSingleNode("damage").InnerText);
            newData.splashRange = float.Parse(node.SelectSingleNode("splashRange").InnerText);
            newData.duration = float.Parse(node.SelectSingleNode("duration").InnerText);
            newData.barrier = float.Parse(node.SelectSingleNode("barrier").InnerText);
            newData.heal = float.Parse(node.SelectSingleNode("heal").InnerText);
            newData.atkRange = float.Parse(node.SelectSingleNode("atkRange").InnerText);

            towerData.Add(newData);
        }
    }

    
    public void SpawnTower(Transform tileTransform, int index)
    {
        if (GameManager.Inst.seed >= 40)
        {
            GameObject tower = GameManager.Inst.pool.Get(2);
            /*if(tileTransform.position.x < 0)
            {
                //왼쪽
                tower.transform.Find("CoolLeft").gameObject.SetActive(true);
                tower.transform.Find("CoolRight").gameObject.SetActive(false);
            }
            else
            {
                //오른쪽
                tower.transform.Find("CoolLeft").gameObject.SetActive(false);
                tower.transform.Find("CoolRight").gameObject.SetActive(true);
            }*/

            Tile tile = tileTransform.GetComponent<Tile>();
            if (tile.IsBuildTower == true) return;           // 현재 타워 건설되어 있으면 타워건설 X
            tile.IsBuildTower = true;                        // 타워 건설되어 있음으로 설정

            tower.transform.position = tileTransform.position;
            tower.GetComponent<Tower>().Init(towerData[index]);
            InstalledTower.Add(tile.transform, tower);
            GameManager.Inst.seed -= 40;
            /*switch (index)
            {
                case 0:
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Sub_Hamster_Voice_01);
                    break;
                case 1:
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Sub_Hamster_Voice_02);
                    break;
                case 2:
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Sub_Hamster_Voice_03);
                    break;
                case 3:
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Sub_Hamster_Voice_04);
                    break;
                case 4:
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Sub_Hamster_Voice_05);
                    break;
            }*/ //도감만들때 갖다 쓰세요
            AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Main_Hamster_Sub_Hamster_Build);
        }
        else
        {
            ObjectDetector.instance.towerText.text = "씨앗이 부족합니다!";
            StartCoroutine(TextClose());
            Debug.Log("Not Enough Seeds");
        }
    }

    public IEnumerator TextClose()
    {
        ObjectDetector.instance.towerText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        ObjectDetector.instance.towerText.gameObject.SetActive(false);
    }

    public void SellTower(Transform tileTransform)
    {
        Tile tile = tileTransform.GetComponent<Tile>();

        if (InstalledTower.ContainsKey(tileTransform))
        {
            // 타워가 건설되어 있으면 판매 처리
            tile.IsBuildTower = false;  // 타워가 건설되어 있지 않음으로 설정

            // 추가: 판매된 타워를 오브젝트 풀에 반환
            GameObject tower;
            if (InstalledTower.TryGetValue(tileTransform, out tower))
            {
                AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Sub_Hamster_Sell);   //  타워 철거 소리 추가
                InstalledTower.Remove(tileTransform);
                GameManager.Inst.pool.Release(tower);
                GameManager.Inst.seed += 30;
            }
        }
    }

    public void UpgradeTower(Transform tileTransform)
    {
        Tile tile = tileTransform.GetComponent<Tile>();
        if(InstalledTower.ContainsKey(tileTransform))
        {
            GameObject tower;
            if(InstalledTower.TryGetValue(tileTransform, out tower))
            {
                int towerType = tower.GetComponent<Tower>().towerType;
                Tower towerComponent = tower.GetComponent<Tower>();
                if (towerComponent.level >= 5)
                {
                    ObjectDetector.instance.towerText.text = "최대 레벨입니다!";
                    StartCoroutine(TextClose()); 
                    return;
                }
                switch (towerType)
                {
                    case 0:
                        towerComponent.atkSpeed -= 0.5f;
                        towerComponent.damage += 10;
                        towerComponent.level++;
                        break;
                    case 1:
                        towerComponent.atkRange += 0.4f;
                        towerComponent.damage += 5;
                        towerComponent.level++;
                        break;
                    case 2:
                        towerComponent.atkSpeed -= 0.5f;
                        towerComponent.damage += 2;
                        towerComponent.duration += 1;
                        towerComponent.level++;
                        break;
                    case 3:
                        towerComponent.barrier += 200;
                        towerComponent.atkSpeed -= 5;
                        towerComponent.level++;
                        break;
                    case 4:
                        towerComponent.heal += 200;
                        towerComponent.atkSpeed -= 5;
                        towerComponent.level++;
                        break;
                }
                GameManager.Inst.seed -= 20;
            }
        }
    }
    
}

[System.Serializable]
public class TowerData
{
    public int towerType;
    public float atkSpeed;
    public float damage;
    public float splashRange;
    public float duration;
    public float barrier;
    public float heal;
    public float atkRange;
}
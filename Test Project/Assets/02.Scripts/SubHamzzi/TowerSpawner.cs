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
        if(GameManager.Inst.seed >= 40)
        {
            GameObject tower = GameManager.Inst.pool.Get(2);

            Tile tile = tileTransform.GetComponent<Tile>();
            if (tile.IsBuildTower == true) return;           // 현재 타워 건설되어 있으면 타워건설 X
            tile.IsBuildTower = true;                        // 타워 건설되어 있음으로 설정

            tower.GetComponent<Tower>().Init(towerData[index]);
            tower.transform.position = tileTransform.position;
            InstalledTower.Add(tile.transform, tower);
            GameManager.Inst.seed -= 40;
            switch (index)
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
            }
        }
        else
        {
            Debug.Log("Not Enough Seeds");
        }
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
                InstalledTower.Remove(tileTransform);
                GameManager.Inst.pool.Release(tower);
                GameManager.Inst.seed += 30;
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
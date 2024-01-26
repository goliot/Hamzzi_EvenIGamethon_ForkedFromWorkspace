using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;

    public List<TowerData> towerData = new List<TowerData>();

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
        GameObject tower = GameManager.Inst.pool.Get(2);

        Tile tile = tileTransform.GetComponent<Tile>();
        if (tile.IsBuildTower == true) return;           // 현재 타워 건설되어 있으면 타워건설 X
        tile.IsBuildTower = true;                        // 타워 건설되어 있음으로 설정

        tower.GetComponent<Tower>().Init(towerData[index]);
        tower.transform.position = tileTransform.position;
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
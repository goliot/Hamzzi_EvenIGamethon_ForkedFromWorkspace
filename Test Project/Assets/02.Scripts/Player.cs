using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System;
using System.Xml.Serialization;

public class Player : MonoBehaviour
{
    public List<PlayerData> playerData = new List<PlayerData>();
    public Transform fireArea;
    string xmlFileName = "PlayerData";

    void Start()
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

        // 전체 아이템 가져오기 예제.
        XmlNodeList all_nodes = xmlDoc.SelectNodes("root/Sheet1");
        foreach (XmlNode node in all_nodes)
        {
            PlayerData newData = new PlayerData();

            newData.skillId = int.Parse(node.SelectSingleNode("skillId").InnerText);
            newData.penetrate = int.Parse(node.SelectSingleNode("penetrate").InnerText);
            newData.damage = float.Parse(node.SelectSingleNode("damage").InnerText);
            newData.atkSpeed = float.Parse(node.SelectSingleNode("atkSpeed").InnerText);
            newData.bulletSpeed = float.Parse(node.SelectSingleNode("bulletSpeed").InnerText);

            playerData.Add(newData);
        }
    }

    private void Update()
    {
        Transform target = gameObject.GetComponent<Scanner>().nearestTarget;
        if(Input.GetKeyUp(KeyCode.Space))
        {
            Attack(target);
        }
    }
    void Attack(Transform target) //유도탄이 아닌 공격 당시의 위치로 그냥 발사
    {
        //총알을 생성하는데 -> 생성한 총알에 Init을 해야됨 -> 완료
        BulletSpawn(target);
        //이제 총알의 움직임을 구현하자
    }

    void BulletSpawn(Transform target)
    {
        GameObject bullet = GameManager.Inst.pool.Get(1);
        bullet.transform.position = fireArea.position;
        bullet.GetComponent<Bullet>().Init(playerData[0]);
        bullet.GetComponent<Bullet>().target = target;
    }


}

[System.Serializable]
public class PlayerData //메인캐릭터 능력치(스킬) 데이터
{
    public int skillId; //무슨 공격인지
    public int penetrate; //관통 횟수
    public float damage; //공격력
    public float atkSpeed; //공격 속도
    public float bulletSpeed; //투사체 속도
}

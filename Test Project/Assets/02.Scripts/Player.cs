using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.UI;
using System.Linq;

public class Player : MonoBehaviour
{
    public List<PlayerData> playerData = new List<PlayerData>();
    public Transform fireArea;
    public Transform target;

    public Image[] skillImages;

    public Transform[] momenstoPoint;
    public Transform nextMomenstoLocation;

    string xmlFileName = "PlayerData";

    void Start()
    {
        LoadXML(xmlFileName);

        int level = BackendGameData.Instance.UserGameData.level;
        PlayerUpgrade(level);
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
            newData.secondDelay = float.Parse(node.SelectSingleNode("secondDelay").InnerText);
            newData.duration = float.Parse(node.SelectSingleNode("duration").InnerText);
            newData.bulletSpeed = float.Parse(node.SelectSingleNode("bulletSpeed").InnerText);
            newData.atkRange = float.Parse(node.SelectSingleNode("atkRange").InnerText);
            newData.explodeDamage = float.Parse(node.SelectSingleNode("explodeDamage").InnerText);
            newData.isExplode = bool.Parse(node.SelectSingleNode("isExplode").InnerText);
            newData.isUnlocked = bool.Parse(node.SelectSingleNode("isUnlocked").InnerText);
            newData.isPenetrate = bool.Parse(node.SelectSingleNode("isPenetrate").InnerText);
            newData.splashRange = float.Parse(node.SelectSingleNode("splashRange").InnerText);

            playerData.Add(newData);
        }
    }

    private void PlayerUpgrade(int level) //서버에서 레벨 받아오기
    {
        foreach(PlayerData data in playerData)
        {
            data.damage += BackendGameData.Instance.UserGameData.damageUpgradeAmount[level - 1];
            data.explodeDamage += BackendGameData.Instance.UserGameData.damageUpgradeAmount[level - 1];
        }
        Debug.Log("레벨 업그레이드 적용 " + level);
    }

    private void Update()
    {
        target = gameObject.GetComponent<Scanner>().nearestTarget;

        if (target == null) return;

        Vector3 offset = target.position - transform.position;
        float distance = offset.magnitude;
        /*if(Input.GetKeyUp(KeyCode.Space))
        {
            Attack(target);
        }*/

        foreach(PlayerData data in playerData)
        {
            if (data.isUnlocked)
            {
                skillImages[data.skillId].transform.parent.gameObject.SetActive(true);
                skillImages[data.skillId].gameObject.SetActive(true);
                skillImages[data.skillId].transform.SetAsFirstSibling();
                //Debug.Log(skillImages[data.skillId].name);
                data.UpdateCooldown();
                skillImages[data.skillId].fillAmount = data.cooldownTimer / data.atkSpeed;

                if (data.skillId == 5 && data.CanUseSkill()) //모멘스토일경우 다른 로직 사용
                {
                    nextMomenstoLocation = momenstoPoint[Random.Range(0, momenstoPoint.Length)];
                    BulletSpawn(data);
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Main_Hamster_Dark_Attack);
                    data.StartCoolDown();
                }
                else if (data.CanUseSkill() && distance < data.atkRange)
                {
                    BulletSpawn(data);
                    data.StartCoolDown();
                    switch(data.skillId)
                    {
                        case 0:
                            int rand = 0;
                            rand = Random.Range(0, 3);
                            if(rand == 0) AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Main_Hamster_Attack1);
                            else if(rand == 1) AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Main_Hamster_Attack2);
                            else AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Main_Hamster_Attack3);
                            break;
                        case 1:
                            AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Main_Hamster_Fire_Attack);
                            break;
                        case 2:
                            AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Main_Hamster_Ice_Attack);
                            break;
                        case 3:
                            AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Main_Hamster_Electric_Attack);
                            break;
                        case 4:
                            AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Main_Hamster_Lightning_Attack);
                            break;
                        case 6:
                            AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Main_Hamster_Missile_Attack);
                            break;

                    }
                }
            }
        }
    }

    void BulletSpawn(PlayerData data)
    {
        GameObject bullet = GameManager.Inst.pool.Get(1);
        if (data.skillId == 5) //모멘스토만 다른 로직
        {
            bullet.transform.position = nextMomenstoLocation.position;
        }
        else
        {
            bullet.transform.position = fireArea.position;
        }
        bullet.GetComponent<Bullet>().Init(data);
        //bullet.GetComponent<Bullet>().target = target;
    }
}

[System.Serializable]
public class PlayerData //메인캐릭터 능력치(스킬) 데이터
{
    public int skillId; //무슨 공격인지
    public int penetrate; //관통 횟수
    public float damage; //공격력
    public float atkSpeed; //공격 속도
    public float secondDelay; //두번째 공격이 있는 스킬의 두번째 공격 딜레이
    public float duration; //지속데미지 스킬의 지속시간
    public float bulletSpeed; //투사체 속도
    public float atkRange; //사정거리
    public float explodeDamage; //폭발 데미지
    public bool isExplode; //폭발형인지 여부
    public bool isUnlocked; //해금됐는지 여부
    public bool isPenetrate; //관통형인지 여부
    public float splashRange; //범위스킬 범위

    public float cooldownTimer = 0f;

    public bool CanUseSkill()
    {
        return cooldownTimer <= 0f;
    }

    public void StartCoolDown()
    {
        cooldownTimer = atkSpeed;
    }

    public void UpdateCooldown()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            //Debug.Log(skillId + "번 스킬 쿨타임 : " + cooldownTimer);
        }
    }
}

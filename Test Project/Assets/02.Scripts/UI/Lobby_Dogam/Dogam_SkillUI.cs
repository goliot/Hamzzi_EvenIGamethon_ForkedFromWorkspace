using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dogam_SkillUI : MonoBehaviour
{
    public Image myImage;
    public TextMeshProUGUI myTitle;
    public TextMeshProUGUI myEngTitle;
    public TextMeshProUGUI myDesc;
    public Sprite[] mySprites;

    public List<Dogam_SkillData> dogamSkillData = new List<Dogam_SkillData>();

    string xmlFileName = "DogamSkillData";

    private void Awake()
    {
        LoadXML(xmlFileName);
    }

    private void OnEnable()
    {
        PopUpHandler.OnDogamSkillButtonClicked.AddListener(SetPopUpData);
    }

    private void OnDisable()
    {
        PopUpHandler.OnDogamSkillButtonClicked.RemoveListener(SetPopUpData);
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
            Dogam_SkillData newData = new Dogam_SkillData();

            newData.myNo = int.Parse(node.SelectSingleNode("order").InnerText);
            newData.myName = node.SelectSingleNode("name").InnerText;
            newData.myEngName = node.SelectSingleNode("name_eng").InnerText;
            newData.myDesc = node.SelectSingleNode("description").InnerText;

            dogamSkillData.Add(newData);
        }
    }

    public void SetPopUpData(int dataIndex)
    {
        if (dataIndex >= 0 && dataIndex < dogamSkillData.Count)
        {
            myTitle.text = dogamSkillData[dataIndex].myName;
            myDesc.text = dogamSkillData[dataIndex].myDesc;
            if (dogamSkillData[dataIndex].myEngName != " ") myEngTitle.text = dogamSkillData[dataIndex].myEngName;
            myImage.sprite = mySprites[dataIndex];

            switch (dataIndex)
            {
                case 0:
                    int random = Random.Range(0, 2);
                    if(random == 0)
                    {
                        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Main_Hamster_Attack1);
                    }
                    else if (random == 1)
                    {
                        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Main_Hamster_Attack2);
                    }
                    else
                    {
                        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Main_Hamster_Attack3);
                    }
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
                case 5:
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Main_Hamster_Dark_Attack);
                    break;
                case 6:
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Main_Hamster_Missile_Attack);
                    break;
                case 7:
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Sub_Hamster_Arrow_Attack);
                    break;
                case 8:
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Sub_Hamster_Cannon_Attack);
                    break;
                case 9:
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Sub_Hamster_Black_Magic_Spell);
                    break;
                case 10:
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Sub_Hamster_Sheild_Spell);
                    break;
                case 11:
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Sub_Hamster_Heal_Spell);
                    break;
            }
        }
        else
        {
            Debug.LogError("Invalid data index: " + dataIndex);
        }
    }
}

[System.Serializable]
public class Dogam_SkillData
{
    public int myNo;
    public string myName;
    public string myEngName;
    public string myDesc;
}

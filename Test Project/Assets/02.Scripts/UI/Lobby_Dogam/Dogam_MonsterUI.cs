using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dogam_MonsterUI : MonoBehaviour
{
    public Image myImage;
    public TextMeshProUGUI myTitle;
    public TextMeshProUGUI myDesc;
    public Sprite[] mySprites;

    public List<Dogam_MonsterData> dogamMonsterData = new List<Dogam_MonsterData>();
    int currentDataIndex;

    string xmlFileName = "DogamMonsterData";

    private void Awake()
    {
        LoadXML(xmlFileName);
    }

    private void OnEnable()
    {
        PopUpHandler.OnDogamMonsterButtonClicked.AddListener(SetPopUpData);
    }

    private void OnDisable()
    {
        PopUpHandler.OnDogamMonsterButtonClicked.RemoveListener(SetPopUpData);
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
            Dogam_MonsterData newData = new Dogam_MonsterData();

            newData.myNo = int.Parse(node.SelectSingleNode("order").InnerText);
            newData.myName = node.SelectSingleNode("name").InnerText;
            newData.myDesc = node.SelectSingleNode("description").InnerText;

            dogamMonsterData.Add(newData);
        }
    }

    public void SetPopUpData(int dataIndex)
    {
        if (dataIndex >= 0 && dataIndex < dogamMonsterData.Count)
        {
            myTitle.text = dogamMonsterData[dataIndex].myName;
            myDesc.text = dogamMonsterData[dataIndex].myDesc;
            myImage.sprite = mySprites[dataIndex];

            switch(dataIndex)
            {
                case 0:
                case 1:
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Insam_Gamsam_Voice);
                    break;
                case 2:
                case 3:
                case 4:
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Appuri_Chopuri_Gampuri_Voice);
                    break;
                case 5:
                case 6:
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Injangyi_Gamjangyi_Voice);
                    break;
                case 7:
                case 8:
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Ifari_Gamfari_Voice);
                    break;
                case 9:
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Poison_Mushroom_Voice);
                    break;
                case 10:
                case 11:
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Erumi_Pinkuri_Voice);
                    break;
                case 12:
                case 13:
                case 14:
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_BlueKnight_BlackKnight_KimKnight_Voice);
                    break;
                case 15:
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Huindungyi_Voice);
                    break;
                case 16:
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Gomtaengyi_Voice);
                    break;
                case 17:
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Taepungyi_Voice);
                    break;
                case 18:
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Ippeunyi_Voice);
                    break;
                case 19:
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Haebaragisa_Voice);
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
public class Dogam_MonsterData
{
    public int myNo;
    public string myName;
    public string myDesc;
}


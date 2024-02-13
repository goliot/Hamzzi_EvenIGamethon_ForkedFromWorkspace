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


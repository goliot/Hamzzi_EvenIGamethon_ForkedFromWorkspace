using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerUI : MonoBehaviour
{
    public Button buttonArrow;
    public Button buttonBomb;
    public Button buttonBlack;
    public Button buttonTank;
    public Button buttonHeal;

    public Button buttonSell;
    public Button buttonUpgrade;

    public Sprite arrowImage;
    public Sprite bombImage;
    public Sprite blackImage;
    public Sprite tankImage;
    public Sprite healImage;
    public Sprite arrowNullImage;
    public Sprite bombNullImage;
    public Sprite blackNullImage;
    public Sprite tankNullImage;
    public Sprite healNullImage;

    [SerializeField] private TextMeshProUGUI towerText;

    TowerSpawner spawner;
    ObjectDetector objectDetector;
    PopUpWindow popUpWindow;

    private void Awake()
    {
        objectDetector = FindObjectOfType<ObjectDetector>();
        spawner = FindObjectOfType<TowerSpawner>();
        popUpWindow = GetComponent<PopUpWindow>();

        if (gameObject.name == "TowerUI")
        {
            buttonArrow.interactable = true;
            buttonBomb.interactable = true;
            buttonBlack.interactable = true;
            buttonTank.interactable = true;
            buttonHeal.interactable = true;
            buttonArrow.GetComponent<Image>().sprite = arrowImage;
            buttonBomb.GetComponent<Image>().sprite = bombImage;
            buttonBlack.GetComponent<Image>().sprite = blackImage;
            buttonTank.GetComponent<Image>().sprite = tankImage;
            buttonHeal.GetComponent<Image>().sprite = healImage;

            towerText = GameObject.Find("TowerText").GetComponent<TextMeshProUGUI>();
        }
    }

    private void Start()
    {
        Init();
    }

    void Init()
    {
        if (objectDetector == null || spawner == null)
        {
            Debug.LogError("ObjectDetector or TowerSpawner not assigned!");
            return;
        }

        if (gameObject.name == "TowerUI")
        {
            if (!BackendGameData.Instance.TowerDB.t0)
            {
                //buttonArrow.interactable = false;
                buttonArrow.GetComponent<Image>().sprite = arrowNullImage;
            }
            if (!BackendGameData.Instance.TowerDB.t1)
            {
                //buttonBomb.interactable = false;
                buttonBomb.GetComponent<Image>().sprite = bombNullImage;
            }
            if (!BackendGameData.Instance.TowerDB.t2)
            {
                //buttonBlack.interactable = false;
                buttonBlack.GetComponent<Image>().sprite = blackNullImage;
            }
            if (!BackendGameData.Instance.TowerDB.t3)
            {
                //buttonTank.interactable = false;
                buttonTank.GetComponent<Image>().sprite = tankNullImage;
            }
            if (!BackendGameData.Instance.TowerDB.t4)
            {
                //buttonHeal.interactable = false;
                buttonHeal.GetComponent<Image>().sprite = healNullImage;
            }


            if (buttonArrow != null)
            {
                buttonArrow.onClick.AddListener(() =>
                {
                    if (objectDetector.TilePos != null)
                    {
                        if(buttonArrow.GetComponent<Image>().sprite == arrowNullImage)
                        {
                            ObjectDetector.instance.towerText.text = "상점에서 용병을 구입할 수 있습니다!";
                            StartCoroutine(TextClose());
                            return;
                        }
                        spawner.SpawnTower(objectDetector.TilePos.transform, 0);
                        StartCoroutine(ClosePopUpAfterDelay());
                    }
                });
            }
            if (buttonBomb != null)
            {
                buttonBomb.onClick.AddListener(() =>
                {
                    if (objectDetector.TilePos != null)
                    {
                        if (buttonBomb.GetComponent<Image>().sprite == bombNullImage)
                        {
                            ObjectDetector.instance.towerText.text = "상점에서 용병을 구입할 수 있습니다!";
                            StartCoroutine(TextClose());
                            return;
                        }
                        spawner.SpawnTower(objectDetector.TilePos.transform, 1);
                        StartCoroutine(ClosePopUpAfterDelay());
                    }
                });
            }
            if (buttonBlack != null)
            {
                buttonBlack.onClick.AddListener(() =>
                {
                    if (objectDetector.TilePos != null)
                    {
                        if (buttonBlack.GetComponent<Image>().sprite == blackNullImage)
                        {
                            ObjectDetector.instance.towerText.text = "상점에서 용병을 구입할 수 있습니다!";
                            StartCoroutine(TextClose());
                            return;
                        }
                        spawner.SpawnTower(objectDetector.TilePos.transform, 2);
                        StartCoroutine(ClosePopUpAfterDelay());
                    }
                });
            }
            if (buttonTank != null)
            {
                buttonTank.onClick.AddListener(() =>
                {
                    if (objectDetector.TilePos != null)
                    {
                        if (buttonTank.GetComponent<Image>().sprite == tankNullImage)
                        {
                            ObjectDetector.instance.towerText.text = "상점에서 용병을 구입할 수 있습니다!";
                            StartCoroutine(TextClose());
                            return;
                        }
                        spawner.SpawnTower(objectDetector.TilePos.transform, 3);
                        StartCoroutine(ClosePopUpAfterDelay());
                    }
                });
            }
            if (buttonHeal != null)
            {
                buttonHeal.onClick.AddListener(() =>
                {
                    if (objectDetector.TilePos != null)
                    {
                        if (buttonHeal.GetComponent<Image>().sprite == healNullImage)
                        {
                            ObjectDetector.instance.towerText.text = "상점에서 용병을 구입할 수 있습니다!";
                            StartCoroutine(TextClose());
                            return;
                        }
                        spawner.SpawnTower(objectDetector.TilePos.transform, 4);
                        StartCoroutine(ClosePopUpAfterDelay());
                    }
                });
            }
        }
        else
        {
            if (buttonSell != null)
            {
                buttonSell.onClick.AddListener(() =>
                {
                    if (objectDetector.TilePos != null)
                    {
                        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Purchase_Effect);
                        spawner.SellTower(objectDetector.TilePos.transform);
                        StartCoroutine(ClosePopUpAfterDelay());
                    }
                });
            }

            if (buttonUpgrade != null)
            {
                buttonUpgrade.onClick.AddListener(() =>
                {
                    if (objectDetector.TilePos != null)
                    {
                        if(GameManager.Inst.seed < 20)
                        {
                            AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_UI);
                            ObjectDetector.instance.towerText.text = "씨앗이 부족합니다!";
                            StartCoroutine(TextClose());
                            return;
                        }
                        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Lobby_Hamster_Level_Up);
                        spawner.UpgradeTower(objectDetector.TilePos.transform);
                        StartCoroutine(ClosePopUpAfterDelay());
                    }
                });
            }
        }
    }

    public IEnumerator ClosePopUpAfterDelay()
    {
        yield return new WaitForSeconds(0.3f); // 원하는 대기 시간 설정
        popUpWindow.OnClose();
    }

    public IEnumerator TextClose()
    {
        ObjectDetector.instance.towerText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        ObjectDetector.instance.towerText.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
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

    TowerSpawner spawner;
    ObjectDetector objectDetector;
    PopUpWindow popUpWindow;

    private void Awake()
    {
        objectDetector = FindObjectOfType<ObjectDetector>();
        spawner = FindObjectOfType<TowerSpawner>();
        popUpWindow = GetComponent<PopUpWindow>();
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

        if (buttonArrow != null)
        {
            buttonArrow.onClick.AddListener(() =>
            {
                if (objectDetector.TilePos != null)
                {
                    spawner.SpawnTower(objectDetector.TilePos.transform, 0);
                    StartCoroutine(ClosePopUpAfterDelay());
                }
            });
        }
        if(buttonBomb != null)
        {
            buttonBomb.onClick.AddListener(() =>
            {
                if (objectDetector.TilePos != null)
                {
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
                    spawner.SpawnTower(objectDetector.TilePos.transform, 4);
                    StartCoroutine(ClosePopUpAfterDelay());
                }
            });
        }

        if (buttonSell != null)
        {
            buttonSell.onClick.AddListener(() =>
            {
                if (objectDetector.TilePos != null)
                {
                    spawner.SellTower(objectDetector.TilePos.transform);
                    StartCoroutine(ClosePopUpAfterDelay());
                }
            });
        }
    }

    public IEnumerator ClosePopUpAfterDelay()
    {
        yield return new WaitForSeconds(0.3f); // 원하는 대기 시간 설정
        popUpWindow.OnClose();
    }

}

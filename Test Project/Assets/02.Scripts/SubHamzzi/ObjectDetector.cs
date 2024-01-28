using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField] TowerSpawner towerSpawner;
    Camera mainCamera;
    Ray ray;
    RaycastHit hit;
    private Transform hitTransform;

    public Tile TilePos { get; set; }

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 카메라 위치에서 화면의 마우스 위치를 관통하는 광선 생성
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // 2D 모니터를 통해 3D 월드의 오브젝트를 마우스로 선택하는 방법
            // 광선에 부딪히는 오브젝트를 검출해서 hit에 저장
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.CompareTag("Tile"))
                {
                    TilePos = hit.transform.GetComponent<Tile>();
                    Debug.Log(TilePos);

                    if(!GameManager.Inst.isSelectingCard && PopUpManager.Inst.popUpList.Count < 1)         // 카드 선택창이 열려있지 않고, 팝업창이 열려 있지 않다면 (중복UI 생성 막기)
                    {
                        if (TilePos != null && !TilePos.IsBuildTower) // 타워가 건설되어 있지 않다면
                        {
                            PopUpManager.Inst.CreatePopup(PopUpManager.Inst.PopUpNames.strTowerUI);
                        }
                        else if(TilePos != null && TilePos.IsBuildTower)    // 타워가 건설되어 있다면
                        {
                            Debug.Log("업그레이드 UI 팝업");
                            // 업그레이드 UI
                        }
                        else return;
                    }
                    else return;
                }
            }
        }
    }

    public Transform GetHitTransform()
    {
        return hitTransform;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;

    public void SpawnTower(Transform tileTransform)
    {
        Tile tile = tileTransform.GetComponent<Tile>();
        if (tile.IsBuildTower == true) return;           // 현재 타워 건설되어 있으면 타워건설 X

        tile.IsBuildTower = true;                        // 타워 건설되어 있음으로 설정

        Instantiate(towerPrefab, tileTransform.position, Quaternion.identity);  // 테스트 임시
        // GameManager.Inst.pool.Get(2);                 // 풀매니저 활용
                                                         // 포지션, 위치 매개변수로 넘겨줄 필요 있음
    }
}

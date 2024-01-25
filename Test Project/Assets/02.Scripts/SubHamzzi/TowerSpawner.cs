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

        Instantiate(towerPrefab, tileTransform.position, Quaternion.identity);  // 임시
        // GameManager.Inst.pool.Get(2); 풀매니저 활용
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public PoolManager poolManager;
    public Transform nearestTarget;

    private void FixedUpdate()
    {
        nearestTarget = GetNearest(poolManager.transform);
    }

    public Transform GetNearest(Transform poolManager)
    {
        if (poolManager == null || poolManager.childCount == 0) return null;

        Transform lowestChild = null;
        float lowestY = float.MaxValue;

        foreach (Transform child in poolManager)
        {
            if (child.CompareTag("Enemy") && child.gameObject.activeSelf) // 활성화된 Enemy에 대해서만 고려
            {
                float childY = child.position.y;
                if (childY < lowestY)
                {
                    lowestY = childY;
                    lowestChild = child;
                }
            }
        }

        return lowestChild;
    }
}

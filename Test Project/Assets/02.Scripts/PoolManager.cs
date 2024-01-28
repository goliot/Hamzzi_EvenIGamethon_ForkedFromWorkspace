using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    //프리팹 보관 변수
    public GameObject[] prefabs;
    //풀 담당 리스트
    List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for(int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    /// <summary>
    /// 선택한 풀의 놀고 있는(비활성화된) 게임 오브젝트 접근
    /// 발견하면 select에 할당
    /// 못찾으면 새로 생성
    /// </summary>
    public GameObject Get(int index)
    {
        GameObject select = null;

        foreach(GameObject item in pools[index])
        {
            if(!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }
        if(!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }
        return select;
    }

    public void Release(GameObject obj)
    {
        obj.SetActive(false);
    }

}

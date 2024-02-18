using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventListenerManager : MonoBehaviour
{
    private static EventListenerManager instance;

    // 각 이벤트 리스너에 대한 부울값들
    private bool isGameDataLoadListenerAdded = false;
    private bool isClearDataLoadListenerAdded = false;
    private bool isTowerDataLoadListenerAdded = false;
    private bool isStarDataLoadListenerAdded = false;

    // 인스턴스 접근을 위한 프로퍼티
    public static EventListenerManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EventListenerManager>();
                if (instance == null)
                {
                    GameObject singleton = new GameObject(typeof(EventListenerManager).Name);
                    instance = singleton.AddComponent<EventListenerManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        // 새로운 씬에서 EventListenerManager가 파괴되지 않도록 설정
        DontDestroyOnLoad(gameObject);
    }

    // 각 이벤트 리스너에 대한 상태를 반환하는 메서드들
    public bool IsGameDataLoadListenerAdded => isGameDataLoadListenerAdded;
    public bool IsClearDataLoadListenerAdded => isClearDataLoadListenerAdded;
    public bool IsTowerDataLoadListenerAdded => isTowerDataLoadListenerAdded;
    public bool IsStarDataLoadListenerAdded => isStarDataLoadListenerAdded;

    // 각 이벤트 리스너에 대한 상태를 설정하는 메서드들
    public void SetGameDataLoadListenerAdded(bool value) => isGameDataLoadListenerAdded = value;
    public void SetClearDataLoadListenerAdded(bool value) => isClearDataLoadListenerAdded = value;
    public void SetTowerDataLoadListenerAdded(bool value) => isTowerDataLoadListenerAdded = value;
    public void SetStarDataLoadListenerAdded(bool value) => isStarDataLoadListenerAdded = value;
}


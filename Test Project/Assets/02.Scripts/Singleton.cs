using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    static T _inst = null;

    // 제네릭 기반으로 Inst 프로퍼티를 정의
    public static T Inst
    {
        get
        {
            if(_inst == null)
            {
                _inst = FindObjectOfType<T>();
                if(_inst == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).ToString();
                    _inst = obj.AddComponent<T>();
                }
            }
            return _inst;
        }
    }

    protected void Initialize()
    {
        if(_inst == null)
        {
            _inst = this as T;
            DontDestroyOnLoad(gameObject); // 씬이 전환되어도 게임오브젝트를 파괴하지 않는다
        }
        else
        {
            Destroy(this);
        }
    }


}

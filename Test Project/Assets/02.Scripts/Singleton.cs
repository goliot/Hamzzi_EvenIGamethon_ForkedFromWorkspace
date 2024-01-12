using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    static T _inst = null;

    // ���׸� ������� Inst ������Ƽ�� ����
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
            //DontDestroyOnLoad(gameObject); // ���� ��ȯ�Ǿ ���ӿ�����Ʈ�� �ı����� �ʴ´� 
                                             // ���� �ʿ���� ������ ��Ȱ��ȭ
        }
        else
        {
            Destroy(this);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

//public enum TowerType { Arrow, Bomb, Black, Tank, Heal }

public class Tower : MonoBehaviour //스폰된 후의 동작들
{
    [Header("#Info")]
    public int towerType;
    public float atkSpeed;
    public float damage;
    public float splashRange;
    public float duration;
    public float barrier;
    public float heal;
    public float atkRange;

    public void Init(TowerData data)
    {
        towerType = data.towerType;
        atkSpeed = data.atkSpeed;
        damage = data.damage;
        splashRange = data.splashRange;
        duration = data.duration;
        barrier = data.barrier;
        heal = data.heal;
        atkRange = data.atkRange;
    }
}
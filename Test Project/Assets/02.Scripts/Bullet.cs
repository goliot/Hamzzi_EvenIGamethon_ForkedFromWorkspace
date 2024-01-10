using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int atkSpeed;

    public void Init(float damage, int atkSpeed)
    {
        this.damage = damage;
        this.atkSpeed = atkSpeed;
    }
}

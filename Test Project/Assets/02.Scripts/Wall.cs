using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public float health;

    public void getDamage(float damage)
    {
        health -= damage;
        Debug.Log("벽 피격 : " + damage);

        if(health <= 0)
        {
            gameOver();
        }
    }

    public void gameOver()
    {
        //게임 오버 루틴 -> UI를 띄운다던가 하는 로직
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(health > 0)
        {
            health -= Time.deltaTime * 10;
        }
    }
}

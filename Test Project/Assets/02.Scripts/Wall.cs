using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wall : MonoBehaviour
{
    public float health;
    public float maxHealth;

    public SpriteRenderer wallImage;
    public Sprite[] wallImages;

    private void Awake()
    {
        wallImage = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        maxHealth = 3500f;
        health = maxHealth;
    }

    private void Update()
    {
        if (health > maxHealth * 0.8f)
        {
            wallImage.sprite = wallImages[0];
        }
        else if (health < maxHealth * 0.8f && health > maxHealth * 0.6f)
        {
            wallImage.sprite = wallImages[1];
        }
        else if (health < maxHealth * 0.6f && health > maxHealth * 0.3f)
        {
            wallImage.sprite = wallImages[2];
        }
        else if (health < maxHealth * 0.3f && health > 0)
        {
            wallImage.sprite = wallImages[3];
        }
        else
        {
            wallImage.sprite = wallImages[4];
        }
    }



    public void getDamage(float damage)
    {
        health -= damage;
        //Debug.Log("벽 피격 : " + damage + " 현재 체력: " + health);

        if(health <= 0)
        {
            wallImage.sprite = Resources.Load<Sprite>("04.Images/Wall/Castle_0percent.png");
            gameOver();
        }
    }

    public void gameOver()
    {
        //게임 오버 루틴 -> UI를 띄운다던가 하는 로직
        UIManager.Inst.gameOverUI.SetActive(true);      // 게임 오버 UI 켜지게
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(health > 0)
        {
            health -= Time.deltaTime * 10;
        }
    }
}

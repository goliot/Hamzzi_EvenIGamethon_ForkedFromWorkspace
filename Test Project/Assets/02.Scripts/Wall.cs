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

    private bool wall80;
    private bool wall60;
    private bool wall30;
    private bool wall0;

    private void Awake()
    {
        wallImage = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        maxHealth = 3500f;
        health = maxHealth;

        wall80 = false;
        wall60 = false;
        wall30 = false;
        wall0 = false;
    }

    private void Update()
    {
        int rand = Random.Range(0, 2);
        if (health > maxHealth * 0.8f)
        {
            wallImage.sprite = wallImages[0];
            if (!wall80)
            {
                if (rand == 0) AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Castle_Brake_01);
                else AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Castle_Brake_02);
            }
            wall80 = true;
        }
        else if (health < maxHealth * 0.8f && health > maxHealth * 0.6f)
        {
            wallImage.sprite = wallImages[1];
            if (!wall60)
            {
                if (rand == 0) AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Castle_Brake_01);
                else AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Castle_Brake_02);
            }
            wall60 = true;
        }
        else if (health < maxHealth * 0.6f && health > maxHealth * 0.3f)
        {
            wallImage.sprite = wallImages[2];
            if (rand == 0) AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Castle_Brake_01);
            else AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Castle_Brake_02);
        }
        else if (health < maxHealth * 0.3f && health > 0)
        {
            wallImage.sprite = wallImages[3];
            if (!wall30)
            {
                if (rand == 0) AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Castle_Brake_01);
                else AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Castle_Brake_02);
            }
            wall30 = true;
        }
        else
        {
            wallImage.sprite = wallImages[4];
            if (!wall0)
            {
                if (rand == 0) AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Castle_Brake_01);
                else AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Castle_Brake_02);
            }
            wall0 = true;
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
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Stage_Fail);
        //게임 오버 루틴 -> UI를 띄운다던가 하는 로직
        GameManager.Inst.Stop();                        // 게임 시간 정지
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
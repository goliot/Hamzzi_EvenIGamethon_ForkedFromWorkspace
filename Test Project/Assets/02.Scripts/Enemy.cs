using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public float damage;
    public float atkSpeed;
    public RuntimeAnimatorController[] animCon;

    bool isWallHit = false;
    bool isLive = false;
    bool isWallAttackInProgress = false;

    Rigidbody2D rb;
    Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();    
    }

    private void OnEnable()
    {
        isLive = true;
        isWallHit = false;
        health = maxHealth;
    }

    /// <summary>
    /// 몬스터 능력치 삽입
    /// </summary>
    /// <param name="data"></param>
    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        health = data.health;
        maxHealth = data.health;
        damage = data.damage;
        atkSpeed = data.atkSpeed;
        speed = data.speed;
    }

    private void Update()
    {
        if (!isWallHit)
        {
            // 아래로 이동
            Vector2 movement = Vector2.down * speed * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
        }

        if(speed == 0f && !isWallAttackInProgress) //벽에 도달
        {
            GameObject wall = GameObject.Find("Wall");
            StartCoroutine(WallAttack(wall));
        }
    }

    IEnumerator WallAttack(GameObject wall)
    {
        isWallAttackInProgress = true; // 공격이 시작됨을 표시

        yield return new WaitForSeconds(atkSpeed);
        wall.GetComponent<Wall>().getDamage(damage);

        isWallAttackInProgress = false; // 공격이 끝남을 표시
    }

    private void OnCollisionEnter2D(Collision2D collision) //벽과 충돌 로직
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            speed = 0f;  // 벽에 도달하면 속도를 0으로 설정
            isWallHit = true;
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            // "Enemy"와 충돌했을 때는 무시하고 그냥 겹치게 둠
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet")) return;

        health -= collision.GetComponent<Bullet>().damage;
        Debug.Log("피격");

        if (health > 0) //피격 후 생존
        {

        }
        else //죽었을 때
        {
            Dead();
        }
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
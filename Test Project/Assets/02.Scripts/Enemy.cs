using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header ("#Info")]
    public float speed;
    public float health;
    public float maxHealth;
    public float damage;
    public float atkSpeed;
    public RuntimeAnimatorController[] animCon;

    [Header("#Damage Pop Up")]
    public GameObject dmgText;
    public Text popupText;
    public GameObject dmgCanvas;

    [Header("#Color")]
    public Color hitColor = new Color(1f, 0.5f, 0.5f, 1f);  // 피격 시 적용할 색상
    SpriteRenderer spriteRenderer;
    Color originalColor;

    bool isWallHit = false;
    bool isLive = false;
    bool isWallAttackInProgress = false;

    Rigidbody2D rb;
    Animator anim;
    WaitForFixedUpdate wait;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();    
        wait = new WaitForFixedUpdate();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    private void OnEnable()
    {
        isLive = true;
        isWallHit = false;
        health = maxHealth;
        dmgCanvas = GameObject.Find("DmgPopUpCanvas");
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

        /*if(speed == 0f && !isWallAttackInProgress) //벽에 도달
        {
            GameObject wall = GameObject.Find("Wall");
            StartCoroutine(WallAttack(wall));
        }*/
    }

    IEnumerator WallAttack(GameObject wall)
    {
        while (true)
        {
            if (gameObject.activeSelf)
            {
                isWallAttackInProgress = true; // 공격이 시작됨을 표시
                anim.SetTrigger("Attack");
                wall.GetComponent<Wall>().getDamage(damage);
                //공격모션이 있다면 SetTrigger로 해보자
                isWallAttackInProgress = false; // 공격이 끝남을 표시
                yield return new WaitForSeconds(atkSpeed);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) //벽과 충돌 로직
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            speed = 0f;  // 벽에 도달하면 속도를 0으로 설정
            isWallHit = true;
            GameObject wall = GameObject.Find("Wall");
            StartCoroutine(WallAttack(wall));
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // "Enemy"와 충돌했을 때는 무시하고 그냥 겹치게 둠
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive) return;

        health -= collision.GetComponent<Bullet>().damage;
        Debug.Log("피격");

        Vector3 pos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f, 0);
        GameObject popupTextObejct = Instantiate(dmgText, pos, Quaternion.identity, dmgCanvas.transform);
        popupText.text = collision.GetComponent<Bullet>().damage.ToString();

        if (health > 0)
        {
            // 피격 후 생존
            StartCoroutine(HitEffect());
        }
        else
        {
            // 죽었을 때
            Dead();
            GameManager.Inst.kill++;
            GameManager.Inst.GetExp();
        }
    }

    IEnumerator HitEffect()
    {
        // SpriteRenderer의 색상을 변경하여 어두워지는 효과 부여
        spriteRenderer.color = hitColor;

        yield return new WaitForSeconds(0.5f);

        // 다시 원래 색상으로 돌아오게 함
        spriteRenderer.color = originalColor;
    }

    IEnumerator KnockBack()
    {
        yield return wait;
        float knockBackDistance = 0.5f; // Adjust as needed
    }


    void Dead()
    {
        gameObject.SetActive(false);
    }
}
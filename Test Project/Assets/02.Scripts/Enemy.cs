using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("#Info")]
    public int spriteType;
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

    [Header("#State")]
    public bool isParalyzed = false;
    public bool isKnockback = false;
    public float knockBackSpeed = 10f;

    bool isWallHit = false;
    bool isLive = false;
    bool isWallAttackInProgress = false;
    Coroutine coroutineInfo;

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
        dmgCanvas = GameObject.Find("DmgPopUpCanvas");
    }

    /// <summary>
    /// 몬스터 능력치 삽입
    /// </summary>
    /// <param name="data"></param>
    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        spriteType = data.spriteType;
        health = data.health;
        maxHealth = data.health;
        damage = data.damage;
        atkSpeed = data.atkSpeed;
        speed = data.speed;
    }

    private void Update()
    {
        if (!isWallHit && !isKnockback)
        {
            // 아래로 이동
            rb.velocity = Vector2.down.normalized * speed * Time.deltaTime * 50;
        }
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
                isWallAttackInProgress = false; // 공격이 끝남을 표시
                yield return new WaitForSeconds(atkSpeed);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameObject.activeSelf) return;
        if (collision.gameObject.CompareTag("Wall"))
        {
            rb.velocity = Vector2.zero;  // 벽에 도달하면 속도를 0으로 설정
            isWallHit = true;
            GameObject wall = GameObject.Find("Wall");
            StartCoroutine(WallAttack(wall));
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            // "Enemy"와 충돌했을 때는 무시하고 그냥 겹치게 둠
            //Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
            return;
        }
    }

    /*private void OnCollisionEnter2D(Collision2D collision) //벽과 충돌 로직
    {
        if (!gameObject.activeSelf) return;
        if (collision.gameObject.CompareTag("Wall"))
        {
            rb.velocity = Vector2.zero;  // 벽에 도달하면 속도를 0으로 설정
            isWallHit = true;
            GameObject wall = GameObject.Find("Wall");
            StartCoroutine(WallAttack(wall));
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // "Enemy"와 충돌했을 때는 무시하고 그냥 겹치게 둠
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }*/

    private void OnCollisionExit2D(Collision2D collision)
    {
        isWallHit = false;
        rb.velocity = Vector2.down.normalized * speed * Time.deltaTime * 50;
    }
    /*private void OnTriggerEnter2D(Collider2D collision)
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
    }*/

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive) return;

        TakeDamage(collision.GetComponent<Bullet>().damage);
    }*/

    public void TakeDamage(float damage, int skillId, float duration)
    {
        if(skillId == 4) //엑서니아경우
        {
            //지속 데미지
            if (coroutineInfo != null)  StopCoroutine(Aegsonia(damage, duration));
            coroutineInfo = StartCoroutine(Aegsonia(damage, duration));
        }
        else if (skillId == 5) //모멘스토일경우 -> 데미지 없이 넉백만 들어감
        {
            StartCoroutine(KnockBack());
        }
        else
        {
            Debug.Log("TakeDamage 호출 " + damage);
            health -= damage;
            Debug.Log("피격" + damage);

            //팝업 생성하는 부분
            Vector3 pos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f, 0);
            GameObject popupTextObejct = Instantiate(dmgText, pos, Quaternion.identity, dmgCanvas.transform);
            popupText.text = damage.ToString();

            if (health > 0)
            {
                if (skillId == 3 && !isParalyzed) //루모스일경우
                {
                    StartCoroutine(Paralyze(duration));
                }
                // 피격 후 생존
                StartCoroutine(HitEffect());
            }
            else
            {
                // 죽었을 때
                spriteRenderer.color = originalColor;
                Dead();
                GameManager.Inst.kill++;
                int killExp;
                if (spriteType % 5 < 3)
                {
                    killExp = 30;
                }
                else if (spriteType % 5 == 3)
                {
                    killExp = 60;
                }
                else killExp = 80;
                GameManager.Inst.GetExp(killExp);
            }
        }
    }

    /*private void OnCollisionStay2D(Collision2D collision) //장판딜 구현할때 쓸 친구
    {
        if (!collision.CompareTag("Bullet") || !isLive) return;

        health -= collision.GetComponent<Bullet>().explodeDamage;
    }*/

    IEnumerator HitEffect()
    {
        Color nowOrigin = spriteRenderer.color;
        // SpriteRenderer의 색상을 변경하여 어두워지는 효과 부여
        spriteRenderer.color = hitColor;

        yield return new WaitForSeconds(0.1f);

        // 다시 원래 색상으로 돌아오게 함
        spriteRenderer.color = nowOrigin;
    }

    IEnumerator Paralyze(float duration) //마비상태
    {
        isParalyzed = true;
        spriteRenderer.color = new Color(1f, 1f, 0f); //노란빛
        speed *= 0.8f;
        yield return new WaitForSeconds(duration);  
        spriteRenderer.color = originalColor;
        speed /= 0.8f;
        isParalyzed = false;
    }

    IEnumerator Pinesta(float secondDamage, float duration)
    {
        yield return new WaitForSeconds(duration);
    }

    IEnumerator Aegsonia(float damage, float duration)
    {
        while (duration > 0f)
        {
            // 데미지를 입히는 작업 수행
            Debug.Log("액서니아 타격 " + damage);
            health -= damage;
            Debug.Log("피격" + damage);

            //팝업 생성하는 부분
            Vector3 pos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f, 0);
            GameObject popupTextObejct = Instantiate(dmgText, pos, Quaternion.identity, dmgCanvas.transform);
            popupText.text = damage.ToString();

            // 1초 대기
            yield return new WaitForSeconds(1f);

            // 경과된 시간 감소
            duration -= 1f;

            if (health > 0)
            {
                // 피격 후 생존
                StartCoroutine(HitEffect());
            }
            else
            {
                // 죽었을 때
                spriteRenderer.color = originalColor;
                Dead();
                GameManager.Inst.kill++;
                int killExp;
                if (spriteType % 5 < 3)
                {
                    killExp = 30;
                }
                else if (spriteType % 5 == 3)
                {
                    killExp = 60;
                }
                else killExp = 80;
                GameManager.Inst.GetExp(killExp);
            }
        }
    }

    IEnumerator KnockBack()
    {
        isKnockback = true;
        float knockBackDuration = 0.1f; // 넉백 지속 시간 (코루틴이 돌아갈 시간)
        float timer = 0f;

        while (timer < knockBackDuration)
        {
            timer += Time.deltaTime;
            rb.velocity = Vector2.up.normalized * speed * Time.deltaTime * knockBackSpeed * 50;
            //spriteRenderer.color = new Color(0.5f, 0f, 0.5f, 1f);
            yield return null;
        }
        //spriteRenderer.color = originalColor;
        isKnockback = false;
    }



    void Dead()
    {
        gameObject.SetActive(false);
    }
}
using BackEnd;
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
    public Slider hpSlider;
    /*public GameObject hpBar;
    public Slider bossHPBar;*/

    [Header("#Color")]
    public Color hitColor = new Color(1f, 0.5f, 0.5f, 1f);  // 피격 시 적용할 색상
    SpriteRenderer spriteRenderer;
    Color originalColor;

    [Header("#State")]
    public bool isParalyzed = false;
    public bool isKnockback = false;
    public float knockBackSpeed = 10f;
    public bool isAegsoniaRunning = false;
    public bool isPinestarRunning = false;
    public bool angry = false; //보스 체력 50퍼 효과 나온적 있는지

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
        //bossHPBar =  hpBar.GetComponentInChildren<Slider>();
        originalColor = spriteRenderer.color;
    }

    private void OnEnable()
    {
        isLive = true;
        isWallHit = false;
        dmgCanvas = GameObject.Find("DmgPopUpCanvas");
        isAegsoniaRunning = false;
        isParalyzed = false;
        isKnockback = false;
        isPinestarRunning = false;
        angry = false;
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

        originalColor = spriteRenderer.color;

        if (spriteType % 5 == 4)
        {
            hpSlider.gameObject.SetActive(true);
        }
        else
        {
            hpSlider.gameObject.SetActive(false);
        }
        //bossHPBar.value = 1;
    }

        private void Update()
    {
        if (!isWallHit && !isKnockback)
        {
            // 아래로 이동
            rb.velocity = Vector2.down.normalized * speed * Time.deltaTime * 10;
            //AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Grass_Effect);
        }
        if(spriteType % 5 == 4 && health < maxHealth / 2 && !angry) //반피 이하로 내려가면
        {
            damage *= 2;
            atkSpeed /= 2;

            angry = true;
        }
        if(spriteType % 5 == 4 && angry)
        {
            spriteRenderer.color = Color.red;
        }
        if (spriteType % 5 == 4)
        {
            hpSlider.value = Mathf.Clamp01(health / maxHealth);

        }
        //hpSlider.value = Mathf.Clamp01(health / maxHealth);
    }

    IEnumerator WallAttack(GameObject wall)
    {
        while (true)
        {
            if (gameObject.activeSelf)
            {
                isWallAttackInProgress = true; // 공격이 시작됨을 표시
                anim.SetTrigger("Attack");

                if (0 <= spriteType && spriteType <= 8)
                {
                    int rand = Random.Range(0, 3);
                    if (rand == 0) AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Smash_Castle_01);
                    else if (rand == 1) AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Smash_Castle_02);
                    else AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Smash_Castle_03);
                }
                else if(spriteType == 9)
                {
                    AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Smash_Castle_04);
                }
                else if(spriteType == 10 || spriteType == 11)
                {
                    int rand = Random.Range(0, 2);
                    if (rand == 0) AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Smash_Castle_05);
                    else AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Smash_Castle_12);
                }
                else if(12 <= spriteType && spriteType <= 18) 
                {
                    int rand = Random.Range(0, 3);
                    if (rand == 0) AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Smash_Castle_06);
                    else if(rand == 1) AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Smash_Castle_07);
                    else AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Smash_Castle_08);
                }
                else
                {
                    int rand = Random.Range(0, 3);
                    if(rand == 0) AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Smash_Castle_09);
                    else if(rand == 1) AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Smash_Castle_10);
                    else AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Smash_Castle_11);
                }

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

    private void OnCollisionExit2D(Collision2D collision)
    {
        isWallHit = false;
        rb.velocity = Vector2.down.normalized * speed * Time.deltaTime * 10;
    }

    public void TakeDamage(float damage, float explodeDamage, int skillId, float duration)
    {
        if (!gameObject.activeSelf) return;

        if(skillId == 4) //엑서니아경우
        {
            //지속 데미지
            if (!isAegsoniaRunning)
            {
                StartCoroutine(Aegsonia(damage, duration));
            }
            else return;
        }
        else if (skillId == 5) //모멘스토일경우
        {
            if (!isKnockback) StartCoroutine(KnockBack());
            else return;
        }
        else if(skillId == 6) //피네스타
        {
            if (!isPinestarRunning) StartCoroutine(Pinesta(damage, explodeDamage, duration));
            else return;
        }
        else
        {
            Debug.Log("TakeDamage 호출 " + damage);
            health -= damage;
            AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Hit);
            Debug.Log("피격" + damage);

            //팝업 생성하는 부분
            Vector3 pos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f, 0);
            int intDamage = Mathf.FloorToInt(damage);
            popupText.text = intDamage.ToString();
            GameObject popupTextObejct = Instantiate(dmgText, pos, Quaternion.identity, dmgCanvas.transform);

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
                int seed;
                if (spriteType % 5 < 3)
                {
                    killExp = 30;
                    seed = 5;
                }
                else if (spriteType % 5 == 3)
                {
                    killExp = 60;
                    seed = 7;
                }
                else
                {
                    killExp = 80;
                    seed = 10;
                }
                GameManager.Inst.GetExp(killExp);
                GameManager.Inst.GetSeed(seed);
            }
        }
    }

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
        gameObject.GetComponent<Enemy>().speed *= 0.5f;
        yield return new WaitForSeconds(duration);  
        spriteRenderer.color = originalColor;
        gameObject.GetComponent<Enemy>().speed /= 0.5f;
        isParalyzed = false;
    }

    IEnumerator Pinesta(float damage, float explodeDamage, float duration)
    {
        isPinestarRunning = true;
        Debug.Log("TakeDamage 호출 " + damage);
        health -= damage;
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Hit);
        Debug.Log("피격" + damage);

        //팝업 생성하는 부분
        Vector3 pos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f, 0);
        int intDamage = Mathf.FloorToInt(damage);
        popupText.text = intDamage.ToString();
        GameObject popupTextObejct = Instantiate(dmgText, pos, Quaternion.identity, dmgCanvas.transform);

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
            int seed;
            if (spriteType % 5 < 3)
            {
                killExp = 30;
                seed = 5;
            }
            else if (spriteType % 5 == 3)
            {
                killExp = 60;
                seed = 7;
            }
            else
            {
                killExp = 80;
                seed = 10;
            }
            GameManager.Inst.GetExp(killExp);
            GameManager.Inst.GetSeed(seed);
        }

        yield return new WaitForSeconds(duration);

        Debug.Log("TakeDamage 호출 " + explodeDamage);
        health -= explodeDamage;
        Debug.Log("피격" + explodeDamage);

        //팝업 생성하는 부분
        popupTextObejct = Instantiate(dmgText, pos, Quaternion.identity, dmgCanvas.transform);
        int intExplodeDamage = Mathf.FloorToInt(explodeDamage);
        popupText.text = intExplodeDamage.ToString();

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
            int seed;
            if (spriteType % 5 < 3)
            {
                killExp = 30;
                seed = 5;
            }
            else if (spriteType % 5 == 3)
            {
                killExp = 60;
                seed = 7;
            }
            else
            {
                killExp = 80;
                seed = 10;
            }
            GameManager.Inst.GetExp(killExp);
            GameManager.Inst.GetSeed(seed);
        }
        isPinestarRunning = false;
    }

    IEnumerator Aegsonia(float damage, float duration)
    {
        isAegsoniaRunning = true;
        while (duration > 0f)
        {
            yield return new WaitForSeconds(1f);

            // 데미지를 입히는 작업 수행
            Debug.Log("액서니아 타격 " + damage);
            health -= damage;
            AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Hit);
            Debug.Log("피격" + damage);

            //팝업 생성하는 부분
            Vector3 pos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f, 0);
            int intDamage = Mathf.FloorToInt(damage);
            popupText.text = intDamage.ToString();
            GameObject popupTextObejct = Instantiate(dmgText, pos, Quaternion.identity, dmgCanvas.transform);

            // 경과된 시간 감소
            duration -= 1f;

            if (health > 0)
            {
                // 피격 후 생존
                StartCoroutine(HitEffect());
            }
            else
            {
                spriteRenderer.color = originalColor;
                Dead();
                GameManager.Inst.kill++;
                int killExp;
                int seed;
                if (spriteType % 5 < 3)
                {
                    killExp = 30;
                    seed = 5;
                }
                else if (spriteType % 5 == 3)
                {
                    killExp = 60;
                    seed = 7;
                }
                else
                {
                    killExp = 80;
                    seed = 10;
                }
                GameManager.Inst.GetExp(killExp);
                GameManager.Inst.GetSeed(seed);
            }
        }
        isAegsoniaRunning = false;
    }

    IEnumerator KnockBack()
    {
        Debug.Log("TakeDamage 호출 " + damage);
        health -= damage;
        AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Hit);
        Debug.Log("피격" + damage);

        //팝업 생성하는 부분
        Vector3 pos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f, 0);
        int intDamage = Mathf.FloorToInt(damage);
        popupText.text = intDamage.ToString();
        GameObject popupTextObejct = Instantiate(dmgText, pos, Quaternion.identity, dmgCanvas.transform);

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
            int seed;
            if (spriteType % 5 < 3)
            {
                killExp = 30;
                seed = 5;
            }
            else if (spriteType % 5 == 3)
            {
                killExp = 60;
                seed = 7;
            }
            else
            {
                killExp = 80;
                seed = 10;
            }
            GameManager.Inst.GetExp(killExp);
            GameManager.Inst.GetSeed(seed);
        }

        isKnockback = true;
        float knockBackDuration = 0.1f; // 넉백 지속 시간 (코루틴이 돌아갈 시간)
        float timer = 0f;

        while (timer < knockBackDuration)
        {
            timer += Time.deltaTime;
            rb.velocity = Vector2.up.normalized * Time.unscaledDeltaTime * knockBackSpeed * 50;
            //spriteRenderer.color = new Color(0.5f, 0f, 0.5f, 1f);
            yield return null;
        }
        //spriteRenderer.color = originalColor;
        isKnockback = false;
    }

    void Dead()
    {
        int rand = Random.Range(0, 3);
        switch (spriteType) 
        {
            //챕터 1
            case 0:
                AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Die_01);
                break;
            case 1:
                AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Die_01);
                break;
            case 2:
                AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Die_02);
                break;
            case 3:
                AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Die_02);
                break;
            case 4:
                AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Die_03);
                break;
            //챕터 2
            case 5:
                AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Die_04);
                break;
            case 6:
                AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Die_04);
                break;
            case 7:
                AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Die_05);
                break;
            case 8:
                AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Die_05);
                break;
            case 9:
                AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Die_06);
                break;
            //챕터 3
            case 10:
                AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Die_07);
                break;
            case 11:
                AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Die_07);
                break;
            case 12:
                AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Die_08);
                break;
            case 13:
                AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Die_08);
                break;
            case 14:
                AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Die_08);
                break;
            //챕터 4
            case 15:
                AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Die_09);
                break;
            case 16:
                AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Die_10);
                break;
            case 17:
                AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Die_11);
                break;
            case 18:
                AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Die_12);
                break;
            case 19:
                AudioManager.Inst.PlaySfx(AudioManager.SFX.SFX_Monster_Die_13);
                break;
        }


        /*foreach (Transform child in transform)
        {
            //Destroy(child.gameObject);
        }*/

        gameObject.SetActive(false);
    }
}
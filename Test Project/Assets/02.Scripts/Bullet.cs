using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int skillId; //스킬 스프라이트 결정 아이디
    public int penetrate; //관통 횟수 -> 0이라면 한번 부딪히면 끝, -1이면 무한
    public float damage;
    public float atkSpeed;
    public float secondDelay;
    public float duration;
    public float bulletSpeed; //투사체 속도
    public float atkRange;
    public float explodeDamage;
    public bool isExplode;
    public bool isUnlocked;
    public float splashRange;
    public Vector3 targetPosition;
    public GameObject target;
    public List<Transform> momenstoPoint = new List<Transform>();

    [Header("#Skill Effect")]
    public RuntimeAnimatorController[] animCon;
    Animator anim;

    Rigidbody2D rb;
    CapsuleCollider2D capsuleCollider;
    Collider2D[] hitColliders;
    private Vector3 initialDirection;
    private Vector3 initialLocation;
    private bool isTargetLocked = false;

    Vector2 currentSize;
    Vector2 newSize;

    bool bombardaExplodeCoroutineStarted = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    public void Init(PlayerData playerData)
    {
        anim.runtimeAnimatorController = animCon[playerData.skillId];
        skillId = playerData.skillId;
        penetrate = playerData.penetrate;
        damage = playerData.damage;
        atkSpeed = playerData.atkSpeed;
        secondDelay = playerData.secondDelay;
        duration = playerData.duration;
        bulletSpeed = playerData.bulletSpeed;
        atkRange = playerData.atkRange;
        explodeDamage = playerData.explodeDamage;
        isExplode = playerData.isExplode;
        isUnlocked = playerData.isUnlocked;
        splashRange = playerData.splashRange;

        if (gameObject.GetComponent<Bullet>().skillId == 1) //봄바르다일경우 콜라이더 잠깐 끄기
        {
            capsuleCollider.enabled = false;
        }
    }

    private void OnEnable() //Start로하면 재활용될때 target정보가 업데이트되지 않음
    {
        initialLocation = GameManager.Inst.player.fireArea.position;
        targetPosition = GameManager.Inst.player.target.position;
        initialDirection = targetPosition - initialLocation;
        target = GameManager.Inst.player.target.gameObject;

        currentSize = capsuleCollider.size;
        newSize = new Vector2(currentSize.x * 8f, currentSize.y * 4f);
        capsuleCollider.isTrigger = true;
        capsuleCollider.size = new Vector2(1f, 1f);
    }

    private void Update()
    {
        Vector3 dir = target.transform.position - transform.position;
        /*if(target == null || !target.gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            return;
        }*/
        /*AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        // "MainEffect" 애니메이션이 재생 중인지 확인
        if (stateInfo.IsName("MainEffect"))
        {
            rb.velocity = Vector2.zero;
        }*/
        RotateTowardsMovementDirection();

        if (gameObject.activeSelf)
        {
            if (skillId == 1) //봄바르다
            {
                rb.velocity = dir.normalized * bulletSpeed;
                //gameObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, initialDirection);
                transform.localScale = new Vector3(6, 6, 6);
                capsuleCollider.size = new Vector2(0.33f, 0.33f);
                // 일정 오차 범위 내에 위치가 일치하면
                float positionError = 0.1f; // 적절한 오차 범위를 설정하세요

                if (!capsuleCollider.enabled && Vector2.Distance(transform.position, target.transform.position) < positionError)
                {
                    transform.localScale = Vector3.one;
                    anim.speed = 2;
                    anim.SetTrigger("MainEffect"); //왜 1프레임 오른쪽으로 돌지?
                    RotateTowardsMovementDirection();
                    //capsuleCollider.size = newSize;
                    capsuleCollider.enabled = true; //딱 도달했을 때 터지게 해야지 콜라이더만 켜서는 안된다.
                    bulletSpeed = 0f;
                }
            }
            else if(skillId == 2) //아구아멘티
            {
                
            }
            else if (skillId == 3) //루모스
            {
                transform.localScale = new Vector3(0.7f, 2f, 0.7f);
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy").Where(obj => obj.transform.position.y <= 2.5f).ToArray();
                if (enemies.Length > 0 && !isTargetLocked)
                {
                    isTargetLocked = true;
                    target = enemies[Random.Range(0, enemies.Length)];
                    targetPosition = target.transform.position;
                }
                transform.position = new Vector2(target.transform.position.x, target.transform.position.y + 0.5f);
                anim.speed = 3;
            }
            else if (skillId == 4) //엑서니아
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90)); //수직으로 나오게
                transform.position = new Vector3(target.transform.position.x, 0.7f, 0);
                transform.localScale = new Vector3(5.5f, 5.5f, 5.5f);
                capsuleCollider.size = new Vector2(1.55f, 0.1f);
            }
            else if (skillId == 5) //모멘스토
            {
                bulletSpeed = 0f;
            }
            else
            {
                rb.velocity = dir.normalized * bulletSpeed;
                //gameObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, initialDirection);
            }
        }
        hitColliders = Physics2D.OverlapCapsuleAll(transform.position, capsuleCollider.size, CapsuleDirection2D.Vertical, 0f);
        for(int i=0; i<hitColliders.Length; i++)
        {
            Debug.Log(hitColliders[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || penetrate == -1) return;
        //관통력이 기본 -1이라면 무한관통
        Debug.Log("Bullet OnTriggerEnter with: " + collision.gameObject.name);

        switch (gameObject.GetComponent<Bullet>().skillId)
        {
            case 0: //매직볼
                collision.gameObject.GetComponent<Enemy>().TakeDamage(damage, skillId, duration);
                penetrate--;
                if (penetrate == -1)
                {
                    bulletSpeed = 0;
                    gameObject.SetActive(false);
                }
                break;
            case 1: //봄바르다
                //collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                penetrate--;
                if (penetrate == -1)
                {
                    //Collider2D[] hitColliders = Physics2D.OverlapCapsuleAll(transform.position, capsuleCollider.size, CapsuleDirection2D.Vertical, 0f);
                    foreach(var hitCollider in hitColliders)
                    {
                        Debug.Log(hitCollider.name);
                        if (hitCollider.CompareTag("Enemy"))
                        {
                            Enemy enemy = hitCollider.GetComponent<Enemy>();
                            if (enemy)
                            {
                                enemy.TakeDamage(explodeDamage, skillId, duration);
                            }
                        }
                    }
                }
                break;
            case 2: //아구아멘티
                break;
            case 3: //루모스
                //별도로 존재
                break;
            case 4: //액소니아
                Collider2D[] aegsoniaColliders = Physics2D.OverlapCapsuleAll(transform.position, capsuleCollider.size, CapsuleDirection2D.Horizontal, 0f);
                foreach (var hitCollider in aegsoniaColliders)
                {
                    if (hitCollider.gameObject == gameObject)
                        continue;

                    Debug.Log(hitCollider.name);
                    if (hitCollider.CompareTag("Enemy"))
                    {
                        var enemy = hitCollider.GetComponent<Enemy>();
                        if (enemy)
                        {
                            enemy.TakeDamage(damage, skillId, duration);
                        }
                    }
                }
                break;
            case 5: //모멘스토
                capsuleCollider.offset = new Vector2(0, -0.2f);
                capsuleCollider.size *= 1.5f;
                var momenstoColliders = Physics2D.OverlapCapsuleAll(transform.position, capsuleCollider.size, CapsuleDirection2D.Horizontal, 0f);
                foreach (var hitCollider in momenstoColliders)
                {
                    if (hitCollider.CompareTag("Enemy"))
                    {
                        var enemy = hitCollider.GetComponent<Enemy>();
                        if (enemy)
                        {
                            enemy.TakeDamage(explodeDamage, skillId, duration);
                        }
                    }
                }
                capsuleCollider.offset = Vector2.one;
                capsuleCollider.size = Vector2.one;
                momenstoColliders = null;
                break;
            case 6: //피네스타
                collision.gameObject.GetComponent<Enemy>().TakeDamage(damage, skillId, duration);
                collision.gameObject.GetComponent<Enemy>().TakeDamage(explodeDamage, skillId, duration);
                penetrate--;
                if (penetrate == -1)
                {
                    bulletSpeed = 0;
                    gameObject.SetActive(false);
                }
                break;
            default:
                penetrate--;
                if (penetrate == -1)
                {
                    bulletSpeed = 0;
                    gameObject.SetActive(false);
                }
                break;
        }
    }

    public void OnAnimationEnd() //애니메이션 이벤트
    {
        // 애니메이션 끝까지 재생되면 호출되는 함수
        capsuleCollider.size = currentSize;
        anim.speed = 1;
        transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        isTargetLocked = false;
        gameObject.SetActive(false);
    }

    public void OnAnimationLumos()
    {
        target.GetComponent<Enemy>().TakeDamage(damage, skillId, duration);

        gameObject.transform.rotation = Quaternion.identity;
        capsuleCollider.size = currentSize;
        anim.speed = 1;
        transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        isTargetLocked = false;
        gameObject.SetActive(false);
    }

    void OnDrawGizmos()
    {
        Vector2 capsuleColliderSize = capsuleCollider.size;
        // 그리기 전에 먼저 Physics2D.OverlapCapsuleAll을 사용하여 콜라이더를 얻습니다.
        var aegsoniaColliders = Physics2D.OverlapCapsuleAll(transform.position, capsuleColliderSize, CapsuleDirection2D.Horizontal, 0f);

        // 그림을 그리기 전에 색상을 설정할 수 있습니다.
        Gizmos.color = Color.red;

        // 얻은 콜라이더들을 기즈모로 그립니다.
        foreach (var collider in aegsoniaColliders)
        {
            Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
        }
    }

    void RotateTowardsMovementDirection()
    {
        // 현재 총알의 이동 방향을 구함
        Vector2 direction = GetComponent<Rigidbody2D>().velocity.normalized;

        // 이동 방향으로 총알 회전
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
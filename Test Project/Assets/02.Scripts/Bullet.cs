using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public bool isPenetrate;
    public float splashRange;
    public Vector3 targetPosition;
    public GameObject target;
    public List<Transform> momenstoPoint = new List<Transform>();
    public float positionError = 0.1f;

    [Header("#Skill Effect")]
    public RuntimeAnimatorController[] animCon;
    Animator anim;
    public List<GameObject> enemies = new List<GameObject>();
    List<GameObject> bombardaEnemies = new List<GameObject>();
    List<GameObject> momenstoEnemies = new List<GameObject>();

    [Header("#Skill State")]
    Vector3 straightDir;

    List<GameObject> targetedEnemies = new List<GameObject>();

    Rigidbody2D rb;
    CapsuleCollider2D capsuleCollider;
    Collider2D[] hitColliders;
    SpriteRenderer spriteRenderer;
    private Vector3 initialDirection;
    private Vector3 initialLocation;
    private bool isTargetLocked = false;
    public bool isAguaOn = false;
    private float distance;

    Vector2 currentSize;
    Vector2 newSize;

    bool bombardaExplodeCoroutineStarted = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        isPenetrate = playerData.isPenetrate;
        splashRange = playerData.splashRange;

        capsuleCollider.enabled = true;
        if (skillId == 1) //봄바르다일경우 콜라이더 잠깐 끄기
        {
            capsuleCollider.enabled = false;
        }
        /*else if(gameObject.GetComponent<Bullet>().skillId == 5) //모멘스토 위치 설정
        {
            transform.position = momenstoPoint[Random.Range(0, momenstoPoint.Count)].transform.position;
        }*/
        else if(skillId == 2)
        {
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 2, 0);
            isAguaOn = false;
        }

        straightDir = target.transform.position - transform.position;
        if(skillId != 2) spriteRenderer.sortingOrder = 10;
    }

    private void OnEnable() //Start로하면 재활용될때 target정보가 업데이트되지 않음 -> 얘가 먼저, Init이 나중
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

    private void FixedUpdate()
    {
        if (enemies != null) enemies.Clear();
        enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        enemies = enemies.Where(enemy => enemy.activeSelf).ToList();
        //enemies = GameObject.FindGameObjectsWithTag("Enemy").Where(enemy => enemy.activeSelf).ToList();
        //if (targetedEnemies.Count > 0)
        //{
        //    // targetedEnemies 리스트 안에 있는 GameObject들을 enemies 리스트에서 제외
        //    foreach (GameObject targetedEnemy in targetedEnemies)
        //    {
        //        if (enemies.Contains(targetedEnemy))
        //        {
        //            enemies.Remove(targetedEnemy);
        //        }
        //    }
        //}

        //if (enemies.Count == 0) OnAnimationEnd(); //적이 없으면 그냥 꺼버리기

        Vector3 dir = target.transform.position - transform.position;
        distance = dir.magnitude;
        RotateTowardsMovementDirection();

        if (gameObject.activeSelf)
        {
            if (skillId == 1) //봄바르다
            {
                rb.velocity = dir.normalized * bulletSpeed;
                transform.localScale = new Vector3(6, 6, 6);
                // 일정 오차 범위 내에 위치가 일치하면
                if (distance < positionError)
                {
                    anim.speed = 2;
                    foreach (GameObject enemy in enemies)
                    {
                        if (enemy != null && distance < splashRange && !bombardaEnemies.Contains(enemy))
                        {
                            bombardaEnemies.Add(enemy);
                        }
                    }
                    anim.SetTrigger("MainEffect");
                }
            }
            else if (skillId == 2) //아구아멘티
            {
                spriteRenderer.sortingOrder = 4;
                transform.localScale = new Vector3(3, 15, 3);
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                bulletSpeed = 0;

                if (!isAguaOn) StartCoroutine(Aguamenti(duration));
                else return;
            }
            else if (skillId == 3) //루모스
            {
                transform.localScale = new Vector3(2, 4, 2);
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                GameObject[] onLumosEnemies = GameObject.FindGameObjectsWithTag("Enemy").Where(obj => obj.transform.position.y <= 2.5f).ToArray();
                if (onLumosEnemies.Length > 0 && !isTargetLocked)
                {
                    isTargetLocked = true;
                    target = onLumosEnemies[Random.Range(0, onLumosEnemies.Length)];
                    targetPosition = target.transform.position;
                }
                transform.position = new Vector2(target.transform.position.x, target.transform.position.y + 0.5f);
                //anim.speed = 2;
            }
            else if (skillId == 4) //엑서니아
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90)); //수직으로 나오게
                transform.position = new Vector3(target.transform.position.x, 0.7f, 0);
                transform.localScale = new Vector3(5.5f, 5.5f, 5.5f);

                foreach (GameObject enemy in enemies)
                {
                    if (enemy.transform.position.x == transform.position.x)
                    {
                        enemy.GetComponent<Enemy>().TakeDamage(damage, explodeDamage, skillId, duration);
                    }
                }
            }
            else if (skillId == 5) //모멘스토
            {
                transform.localScale = new Vector3(6, 6, 6);
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                bulletSpeed = 0f;

                foreach (GameObject enemy in enemies)
                {
                    if (enemy != null && distance < splashRange && !momenstoEnemies.Contains(enemy))
                    {
                        momenstoEnemies.Add(enemy);
                    }
                }
            }
            else if(skillId == 6) //피네스타
            {
                // 현재 총알의 이동 방향을 구함
                Vector2 direction = GetComponent<Rigidbody2D>().velocity.normalized;

                // 이동 방향으로 총알 회전
                float angle =  Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

                transform.localScale = new Vector3(2, 2, 2);
                rb.velocity = straightDir.normalized * bulletSpeed; //타겟방향 직선운동하게 수정
                capsuleCollider.size = new Vector2(0.1f, 0.1f);
                //gameObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, initialDirection);
                if (distance < positionError)
                {
                    penetrate--;
                    target.GetComponent<Enemy>().TakeDamage(damage, explodeDamage, skillId, duration);

                    if (penetrate == -1)
                    {
                        bulletSpeed = 0;
                        OnAnimationEnd();
                    }
                }
            }
            else //기본공격
            {
                transform.localScale = new Vector3(4, 4, 4);
                rb.velocity = straightDir.normalized * bulletSpeed; //타겟방향 직선운동하게 수정
                capsuleCollider.size = new Vector2(0.1f, 0.1f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Border")) OnAnimationEnd();
        if (!collision.CompareTag("Enemy") || penetrate == -1) return;

        if (gameObject.GetComponent<Bullet>().skillId == 0) //매직볼
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage, explodeDamage, skillId, duration);
            penetrate--;
            if(isExplode)
            {
                OnAnimationBombarda();
            }
            if (penetrate == -1)
            {
                bulletSpeed = 0;
                OnAnimationEnd();
            }
        }
        else if (gameObject.GetComponent<Bullet>().skillId == 6) //피네스타
        {
            penetrate--;
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage, explodeDamage, skillId, duration);

            if (penetrate == -1)
            {
                bulletSpeed = 0;
                OnAnimationEnd();
            }
        }
        else return;
    }

    public void OnAnimationBombarda()
    {
        foreach (GameObject enemy in enemies)
        {
            if ((transform.position - enemy.transform.position).magnitude < splashRange)
            {
                Debug.Log((transform.position - enemy.transform.position).magnitude);
                enemy.GetComponent<Enemy>().TakeDamage(damage, explodeDamage, skillId, duration);
            }
        }
        OnAnimationEnd();
    }

    public void OnAnimationMomensto()
    {
        foreach(GameObject enemy in enemies)
        {
            if((transform.position - enemy.transform.position).magnitude < splashRange)
            {
                Debug.Log((transform.position - enemy.transform.position).magnitude);
                enemy.GetComponent<Enemy>().TakeDamage(damage, explodeDamage, skillId, duration);
            }
        }
        OnAnimationEnd();
    }

    IEnumerator Aguamenti(float duration)
    {
        isAguaOn = true;
        while (duration > 0)
        {
            foreach (GameObject enemy in enemies)
            {
                Vector3 distance = enemy.transform.position - transform.position;
                if (-2.4f < distance.y && distance.y < 0.5f && -0.55f < distance.x && distance.x < 0.6f)
                {
                    enemy.GetComponent<Enemy>().TakeDamage(damage, explodeDamage, skillId, duration);
                }
            }
            yield return new WaitForSeconds(1);
            duration--;
        }
        isAguaOn = false;
        anim.speed = 1;
        //OnAnimationEnd();
    }

    public void OnAnimationAguamenti()
    {
        anim.speed = 0;
    }

    public void OnAnimationEnd() //애니메이션 이벤트, 비활성화로직
    {
        // 애니메이션 끝까지 재생되면 호출되는 함수
        capsuleCollider.size = currentSize;
        anim.speed = 1;
        transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        isTargetLocked = false;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        gameObject.SetActive(false);
    }

    public void OnAnimationLumos()
    {
        if(isExplode)
        {
            OnAnimationBombarda();
        }
        else target.GetComponent<Enemy>().TakeDamage(damage, explodeDamage, skillId, duration);

        capsuleCollider.size = currentSize;
        anim.speed = 1;
        transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        isTargetLocked = false;
        gameObject.SetActive(false);
    }

    private void RotateTowardsMovementDirection()
    {
        // 현재 총알의 이동 방향을 구함
        Vector2 direction = GetComponent<Rigidbody2D>().velocity.normalized;

        // 이동 방향으로 총알 회전
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private GameObject FindNextClosestEnemy()
    {
        if (enemies.Count == 0) return null;
        GameObject closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach(GameObject enemy in enemies)
        {
            if (targetedEnemies.Contains(enemy)) continue;
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if(distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
}
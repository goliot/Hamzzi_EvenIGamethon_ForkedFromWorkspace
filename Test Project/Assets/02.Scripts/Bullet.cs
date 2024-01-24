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
    public float splashRange;
    public Vector3 targetPosition;
    public GameObject target;
    public List<Transform> momenstoPoint = new List<Transform>();
    public float positionError = 0.1f;

    [Header("#Skill Effect")]
    public RuntimeAnimatorController[] animCon;
    Animator anim;
    List<GameObject> enemies = new List<GameObject>();
    List<GameObject> bombardaEnemies = new List<GameObject>();

    Rigidbody2D rb;
    CapsuleCollider2D capsuleCollider;
    Collider2D[] hitColliders;
    private Vector3 initialDirection;
    private Vector3 initialLocation;
    private bool isTargetLocked = false;
    private float distance;

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

    private void FixedUpdate()
    {
        if(enemies != null) enemies.Clear();
        enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        Vector3 dir = target.transform.position - transform.position;
        distance = dir.magnitude;
        //Debug.Log(distance);
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

                /*if (!capsuleCollider.enabled && Vector2.Distance(transform.position, target.transform.position) < positionError)
                {
                    transform.localScale = Vector3.one;
                    anim.speed = 2;
                    anim.SetTrigger("MainEffect"); //왜 1프레임 오른쪽으로 돌지?
                    RotateTowardsMovementDirection();
                    bulletSpeed = 0f;
                }*/
            }
            else if(skillId == 2) //아구아멘티
            {
                
            }
            else if (skillId == 3) //루모스
            {
                transform.localScale = new Vector3(0.7f, 2f, 0.7f);
                GameObject[] onLumosEnemies = GameObject.FindGameObjectsWithTag("Enemy").Where(obj => obj.transform.position.y <= 2.5f).ToArray();
                if (onLumosEnemies.Length > 0 && !isTargetLocked)
                {
                    isTargetLocked = true;
                    target = onLumosEnemies[Random.Range(0, onLumosEnemies.Length)];
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

                foreach(GameObject enemy in enemies)
                {
                    if(enemy.transform.position.x == transform.position.x)
                    {
                        enemy.GetComponent<Enemy>().TakeDamage(damage, skillId, duration);
                    }
                }
            }
            else if (skillId == 5) //모멘스토
            {
                bulletSpeed = 0f;

                foreach(GameObject enemy in enemies)
                {
                    if(distance < splashRange)
                    {
                        enemy.GetComponent<Enemy>().TakeDamage(damage, skillId, duration);
                    }
                }
            }
            else if(skillId == 6) //피네스타
            {

            }
            else //기본공격
            {
                rb.velocity = dir.normalized * bulletSpeed;
                //gameObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, initialDirection);
                if(distance < positionError)
                {
                    penetrate--;
                    target.GetComponent<Enemy>().TakeDamage(damage, skillId, duration);

                    if(penetrate == -1)
                    {
                        bulletSpeed = 0;
                        OnAnimationEnd();
                    }
                }
            }
        }
    }
    
    public void OnAnimationBombarda()
    {
        foreach (GameObject enemy in bombardaEnemies)
        {
            if((transform.position - enemy.transform.position).magnitude < splashRange)
            {
                Debug.Log((transform.position - enemy.transform.position).magnitude);
                enemy.GetComponent<Enemy>().TakeDamage(explodeDamage, skillId, duration);
            }
        }
        bombardaEnemies.Clear();
        OnAnimationEnd();
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
        target.GetComponent<Enemy>().TakeDamage(damage, skillId, duration);

        capsuleCollider.size = currentSize;
        anim.speed = 1;
        transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        isTargetLocked = false;
        gameObject.SetActive(false);
    }

    /*void OnDrawGizmos()
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
    }*/

    void RotateTowardsMovementDirection()
    {
        // 현재 총알의 이동 방향을 구함
        Vector2 direction = GetComponent<Rigidbody2D>().velocity.normalized;

        // 이동 방향으로 총알 회전
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
using System.Collections;
using System.Collections.Generic;
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
    public Transform target;

    [Header("#Skill Effect")]
    public RuntimeAnimatorController[] animCon;
    Animator anim;


    Rigidbody2D rb;
    CapsuleCollider2D capsuleCollider;
    private Vector3 initialDirection;
    private Transform initialLocation;

    Vector2 currentSize;
    Vector2 newSize;

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
    }

    private void OnEnable() //Start로하면 재활용될때 target정보가 업데이트되지 않음
    {
        initialLocation = GameManager.Inst.player.fireArea;
        target = GameManager.Inst.player.target;
        initialDirection = target.position - initialLocation.position;

        currentSize = capsuleCollider.size;
        newSize = new Vector2(currentSize.x * 2f, currentSize.y * 2f);
    }

    private void Update()
    {
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

        if (gameObject.activeSelf)
        {
            rb.velocity = initialDirection.normalized * bulletSpeed;
            gameObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, initialDirection);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || penetrate == -1) return; 
        //관통력이 기본 -1이라면 무한관통

        switch (gameObject.GetComponent<Bullet>().skillId)
        {
            case 0:
                penetrate--;
                if (penetrate == -1)
                {
                    bulletSpeed = 0;
                    gameObject.SetActive(false);
                }
                break;
            case 1:
                penetrate--;
                if (penetrate == -1)
                {
                    bulletSpeed = 0;
                    anim.SetTrigger("MainEffect"); //폭발 이펙트는 나옴

                    //폭파 이펙트 너무 느림 -> 2배속
                    anim.speed = 3.0f;

                    //콜라이더 크기 키우고, 폭파 데미지 추가
                    capsuleCollider.size = newSize;
                    StartCoroutine(BombardaExplode(collision.gameObject, explodeDamage));
                }
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
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

        //기본 그냥 맞으면 사라지기
        /*penetrate--;
        if (penetrate == -1)
        {
            bulletSpeed = 0;
            penetrate = maxPenetrate;
            gameObject.SetActive(false);
        }*/
    }

    IEnumerator BombardaExplode(GameObject enemyObject, float damage)
    {
        if (enemyObject == null)
        {
            Debug.LogError("Enemy GameObject is null in BombardaExplode coroutine.");
            yield break; // 적절한 처리를 한 후 코루틴을 종료합니다.
        }

        Enemy enemy = enemyObject.GetComponent<Enemy>();
        if (enemy == null)
        {
            Debug.LogError("Enemy component not found on the Enemy GameObject in BombardaExplode coroutine.");
            yield break; // 적절한 처리를 한 후 코루틴을 종료합니다.
        }
        else
        {
            Debug.Log("폭발데미지 시도");
            //yield return new WaitForSeconds(0.7f); 
            enemy.TakeDamage(damage); //딜레이 없애니까 됨.
            Debug.Log("폭발데미지 들어감");
            yield break;
        }
    }

    public void OnAnimationEnd() //애니메이션 이벤트
    {
        // 애니메이션 끝까지 재생되면 호출되는 함수
        gameObject.SetActive(false);
        capsuleCollider.size = currentSize;
    }
}
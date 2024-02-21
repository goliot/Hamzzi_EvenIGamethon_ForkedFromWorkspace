using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerBullet : MonoBehaviour
{
    [Header("#Info")]
    public int towerType;
    public float atkSpeed;
    public float damage;
    public float splashRange;
    public float duration;
    public float barrier;
    public float heal;
    public float atkRange;
    public RuntimeAnimatorController[] animCon;
    public GameObject targetFixed;
    public Transform targetPosition;

    Animator anim;
    Rigidbody2D rb;
    float bulletSpeed;
    float positionError;
    bool isBlackOn;
    List<GameObject> enemies = new List<GameObject>();
    bool isTargetDeadWhileGoing;
    bool isBooming;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        bulletSpeed = 5f;
        positionError = 0.1f;
        rb = GetComponent<Rigidbody2D>();   
    }

    public void Init(TowerData data, GameObject target)
    {
        anim.runtimeAnimatorController = animCon[data.towerType];
        towerType = data.towerType;
        atkSpeed = data.atkSpeed;
        damage = data.damage;
        splashRange = data.splashRange;
        duration = data.duration;
        barrier = data.barrier;
        heal = data.heal;
        atkRange = data.atkRange;
        targetFixed = target;
        targetPosition = target.transform;

        gameObject.transform.localScale = new Vector3(3, 3, 3);

        if(towerType == 0) //±√¬Ó
        {
            transform.localScale = new Vector3(4, 4, 4);
        }
        else if(towerType == 1) // ∆˜¬Ó
        {
            transform.localScale = new Vector3(4, 4, 4);
        }
        else if(towerType == 2) //»Ê¬Ó
        {
            transform.localScale = new Vector3(4, 4, 4);
        }
        else
        {
            transform.localScale = new Vector3(6, 6, 6);
        }
        isBlackOn = false;
        isTargetDeadWhileGoing = false;
        isBooming = false;
    }

    private void Update()
    {
        if (targetFixed == null) return;
        if (!targetFixed.gameObject.activeSelf) isTargetDeadWhileGoing = true;

        Vector3 dir = new Vector3();
        if (isTargetDeadWhileGoing) dir = targetPosition.position - transform.position;
        else dir = targetFixed.transform.position - transform.position;

        float distance = dir.magnitude;
        RotateTowardsMovementDirection();

        if (towerType == 0) //±√¬Ó
        {
            rb.velocity = dir.normalized * bulletSpeed;
            if (distance < positionError)
            {
                targetFixed.GetComponent<Enemy>().TakeDamage(damage, damage, 0, duration);
                //∏ﬁ¿Œ¿« ∆Ú≈∏∆«¡§
                OnAnimationEnd();
            }
        }
        else if (towerType == 1) //∆˜¬Ó
        {
            enemies = GameObject.FindGameObjectsWithTag("Enemy").Where(enemy => enemy.activeSelf).ToList();

            if (!isBooming) rb.velocity = dir.normalized * bulletSpeed;
            else rb.velocity = Vector3.zero;
            if (distance < positionError)
            {
                transform.rotation = Quaternion.identity;
                anim.SetTrigger("Effect");
                isBooming = true;
            }
        }
        else if (towerType == 2) //»Ê¬Ó
        {
            enemies = GameObject.FindGameObjectsWithTag("Enemy").Where(enemy => enemy.activeSelf).ToList();
            rb.velocity = Vector3.zero;
            if (!isBlackOn) StartCoroutine(Black(duration));
            else return;
        }
        else return;
    }

    public void OnAnimationBomb()
    {
        foreach(GameObject enemy in enemies)
        {
            if ((transform.position - enemy.transform.position).magnitude < splashRange)
            {
                enemy.GetComponent<Enemy>().TakeDamage(damage, damage, 1, duration); //∫ΩπŸ∏£¥Ÿ ∆«¡§
            }
        }
        OnAnimationEnd();
    }

    public void OnAnimationTank()
    {
        GameObject wall = GameObject.FindGameObjectWithTag("Wall");
        wall.GetComponent<Wall>().health += barrier;
    }

    public void OnAnimationHeal()
    {
        GameObject wall = GameObject.FindGameObjectWithTag("Wall");

        if (wall != null)
        {
            if (wall.GetComponent<Wall>().health >= wall.GetComponent<Wall>().maxHealth)
            {
                return;
            }
            else wall.GetComponent<Wall>().health += heal;
        }
    }

    IEnumerator Black(float duration)
    {
        //yield return new WaitForSeconds(0.5f);
        isBlackOn = true;
        while(duration > 0)
        {
            foreach(GameObject enemy in enemies)
            {
                Vector3 distance = enemy.transform.position - transform.position;
                if(-1.2f < distance.x && distance.x < 1.2 && -0.6f < distance.y && distance.y < 0.6f)
                //if(distance.magnitude < splashRange)
                {
                    enemy.GetComponent<Enemy>().TakeDamage(damage, damage, 2, duration);
                    Debug.Log("»Ê¬Ó");
                }
            }
            yield return new WaitForSeconds(1);
            duration--;
        }
        isBlackOn = false;
        anim.speed = 1;
    }

    public void OnAnimationEnd()
    {
        anim.speed = 1;
        gameObject.SetActive(false);
    }

    public void OnAnimationStay()
    {
        anim.speed = 0;
    }

    private void RotateTowardsMovementDirection()
    {
        // «ˆ¿Á √—æÀ¿« ¿Ãµø πÊ«‚¿ª ±∏«‘
        Vector2 direction = GetComponent<Rigidbody2D>().velocity.normalized;

        // ¿Ãµø πÊ«‚¿∏∑Œ √—æÀ »∏¿¸
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}

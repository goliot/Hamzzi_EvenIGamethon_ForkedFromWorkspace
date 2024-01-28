using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    public Transform targetFixed;

    Animator anim;
    Rigidbody2D rb;
    float bulletSpeed;
    float positionError;
    bool isBlackOn;
    List<GameObject> enemies = new List<GameObject>();  

    private void Awake()
    {
        anim = GetComponent<Animator>();
        bulletSpeed = 5f;
        positionError = 0.1f;
        rb = GetComponent<Rigidbody2D>();   
    }

    public void Init(TowerData data, Transform target)
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

        gameObject.transform.localScale = new Vector3(3, 3, 3);

        if(towerType == 0) //±√¬Ó
        {
            transform.localScale = new Vector3(3, 3, 3);
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
    }

    private void Update()
    {
        if (targetFixed == null) return;

        Vector3 dir = targetFixed.transform.position - transform.position;
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
            rb.velocity = dir.normalized * bulletSpeed;
            if (distance < positionError)
            {
                rb.velocity = Vector3.zero;
                transform.rotation = Quaternion.identity;
                anim.SetTrigger("Effect");
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
        if (wall.GetComponent<Wall>().health > wall.GetComponent<Wall>().maxHealth)
        {
            wall.GetComponent<Wall>().health = wall.GetComponent<Wall>().maxHealth;
        }
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

using UnityEngine;

public class DragPlayer : MonoBehaviour
{
    public RuntimeAnimatorController[] animCon; //0. ±âº», 1. ÆÄ´Ú, 2. »ç°ú, 3. Ãã, 4. Ã¥
    public BoxCollider2D[] zones;               //0. »ç°ú, 1. Ãã, 2. Ã¥

    private Vector3 offset;

    BoxCollider2D bc;
    Animator anim;

    private void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "FeedZone")
        {
            anim.runtimeAnimatorController = animCon[2];
        }
        else if (collision.gameObject.name == "DanceZone")
        {
            anim.runtimeAnimatorController = animCon[3];
        }
        else if (collision.gameObject.name == "ReadZone")
        {
            anim.runtimeAnimatorController = animCon[4];
        }
    }

    void OnMouseDown()
    {
        offset = gameObject.transform.position - GetMouseWorldPos();
    }

    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + offset;
        anim.runtimeAnimatorController = animCon[1];
    }

    private void OnMouseUp()
    {
        anim.runtimeAnimatorController = animCon[0];
        CheckCollision();
    }

    private void CheckCollision()
    {
        int cnt = 0;
        foreach (BoxCollider2D zone in zones)
        {
            if (bc.IsTouching(zone))
            {
                cnt++;
            }
        }
        if (cnt < 2)
        {
            foreach (BoxCollider2D zone in zones)
            {
                if (bc.IsTouching(zone))
                {
                    OnTriggerEnter2D(zone);
                }
            }
        }
        else anim.runtimeAnimatorController = animCon[0];
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}

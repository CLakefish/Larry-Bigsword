using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretObj : MonoBehaviour
{
    public GameObject projectile, aimObj;
    public float intervalTime, speed;
    public bool isHorizontal = false;
    public bool isLeft = false;
    GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("fireProjectile", 0, intervalTime);
    }

    void fireProjectile()
    {
        SpriteRenderer sp = gameObject.GetComponent<SpriteRenderer>();
        Vector2 dir;

        if (isHorizontal)
        {
            if (isLeft) dir = new Vector2(transform.position.x + sp.bounds.size.x, transform.position.y);
            else dir = new Vector2(transform.position.x - sp.bounds.size.x, transform.position.y);
        }
        else
        {
            if (isLeft) dir = new Vector2(transform.position.x, transform.position.y + sp.bounds.size.y);
            else dir = new Vector2(transform.position.x, transform.position.y - sp.bounds.size.y);
        }

        obj = Instantiate(projectile, dir, Quaternion.identity);

        obj.GetComponent<Rigidbody2D>().velocity = (aimObj.transform.position - transform.position).normalized * speed;
    }
}

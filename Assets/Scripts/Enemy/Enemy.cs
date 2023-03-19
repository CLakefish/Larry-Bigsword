using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Basic,
    Heavy,
    Boss
}

public class Enemy : MonoBehaviour
{
    #region Variables

    [Header("Assignables")]
    public EnemyType enemyType;
    private SpriteRenderer _spriteRenderer;
    public LayerMask playerLayer;
    public LayerMask groundLayer;
    [Space(3)]
    public GameObject player;
    public Rigidbody2D rb;
    public GameObject projectile;

    [Header("Movement Variables")]
    [Space(3)]
    public bool canMove;
    [Space(3)]
    public float enemySpeed;
    public float enemyRunSpeed;
    [Space(3)]
    public float bigEnemySpeed;
    public float bigEnemyRunSpeed;

    [Header("Gameplay Variables")]
    [Space(3)]
    public bool canShoot;
    public bool isHit = false;
    [Space(3)]
    public float attackInterval;
    public float attackWait;
    public float bulletAmount;
    public float bulletSpread;
    public float projectileSpeed;
    [Space(3)]
    public float healthAmount;
    public float damagedInvunerability;
    public float knockbackForce;

    [Header("Detection Variables")]
    [Space(3)]
    public bool canCheck;
    [Space(3)]
    public float enemyRun;
    public float enemyCheck;
    public float enemyAttack;
    public float enemyClump;
    bool isCheck, isAttack, isRun;


    Vector2 movement;
    Vector3 dir;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb.GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Area detection
        #region Detection

        if (canCheck)
        {
            Detection();
        }

        #endregion


        #region Projectile Raycasting

        // Raycasting
        if (Vector3.Distance(transform.position, player.transform.position) < (enemyAttack * 1.25f))
        {
            Vector2 dir = player.transform.position - transform.position;
            dir = dir.normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir);

            if (hit.collider.gameObject.layer != groundLayer)
            {
                canShoot = true;
            }
            else
            {
                canShoot = false;
            }

            Debug.DrawRay(transform.position, dir * (enemyAttack * 1.25f), Color.red, 1.0f);
        }

        #endregion

        // Get basic movement
        #region Movement

        if (canMove)
        {
            dir = player.transform.position - rb.transform.position;
            dir.Normalize();
            movement = dir;
        }

        #endregion
    }

    // Movement Handler
    private void FixedUpdate()
    {
        if (!canMove)
        {
            return;
        }

        if (isCheck)
        {
            if (isAttack)
            {
                if (isRun)
                {
                    MoveEnemy(-movement, true);
                }
                else if (canShoot && !isRun && !isHit)
                {
                    StartCoroutine(Bullets());
                }
                else
                {
                    MoveEnemy(Vector2.zero, false);
                }
            }
            else
            {
                MoveEnemy(movement, false);
            }
        }
        else
        {
            MoveEnemy(Vector2.zero, false);
        }
    }

    // Detect Player
    void Detection()
    {
        isCheck = Physics2D.OverlapCircle(transform.position, enemyCheck, playerLayer);
        isAttack = Physics2D.OverlapCircle(transform.position, enemyAttack, playerLayer);
        isRun = Physics2D.OverlapCircle(transform.position, enemyRun, playerLayer);
    }

    // Basic Knockback
    public IEnumerator knockBack(Vector2 dir)
    {
        canMove = canShoot = canCheck = false;
        isHit = true;

        rb.velocity = new Vector2(0f, 0f);

        rb.AddForce(dir * knockbackForce, ForceMode2D.Force);

        yield return new WaitForSeconds(damagedInvunerability);

        canMove = canShoot = canCheck = true;
        isHit = false;
    }

    // Enemy Movement
    public void MoveEnemy(Vector2 dir, bool run)
    {
        if (enemyType == EnemyType.Basic)
        {
            rb.velocity = new Vector2(0f, 0f);

            if (run)
            {
                rb.MovePosition((Vector2)transform.position + (enemyRunSpeed * Time.fixedDeltaTime * dir));
            }
            else
            {
                rb.MovePosition((Vector2)transform.position + (enemySpeed * Time.fixedDeltaTime * dir));
            }
        }
        
        if (enemyType == EnemyType.Heavy)
        {
            rb.velocity = new Vector2(0f, 0f);

            if (run)
            {
                rb.MovePosition((Vector2)transform.position + (bigEnemyRunSpeed * Time.fixedDeltaTime * dir));
            }
            else
            {
                rb.MovePosition((Vector2)transform.position + (bigEnemySpeed * Time.fixedDeltaTime * dir));
            }
        }
    }

    #region Bullets

    // Bullets
    IEnumerator Bullets()
    {
        for (int i = 0; i < bulletAmount; i++)
        {
            if (isHit)
            {
                yield return null;
            }
            StartCoroutine(FireBullet());
        }

        canShoot = false;

        yield return new WaitForSeconds(attackInterval);

        canShoot = true;
    }

    IEnumerator FireBullet()
    {
        canMove = false;
        canShoot = false;

        rb.velocity = new Vector2(0f, 0f);

        Vector3 fireDir = ((new Vector3((player.transform.position.x + Random.Range(-bulletSpread, bulletSpread)), (player.transform.position.y + Random.Range(-bulletSpread, bulletSpread))) - transform.position)).normalized;
        GameObject firedObj = Instantiate(projectile, transform.position, Quaternion.Euler(0,0,Mathf.Rad2Deg * Mathf.Atan2(fireDir.x, fireDir.y)));

        firedObj.GetComponent<Rigidbody2D>().AddForce(fireDir * projectileSpeed, ForceMode2D.Force);

        if (enemyType == EnemyType.Heavy)
        {
            rb.velocity = new Vector2(knockbackForce, knockbackForce);
        }

        yield return new WaitForSeconds(attackWait);

        canMove = true;
        canShoot = true;
    }

    #endregion
}

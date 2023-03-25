using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BetterEnemy : MonoBehaviour
{
    #region Enums

    // Meant for sprites
    internal enum EnemyType
    {
        Standard,
        Shotgunner,
        Sniper
    }

    // Meant for States
    internal enum States 
    { 
        checking,
        attacking,
        running,
        reloading,
        parried,
        none
    }

    #endregion

    #region Variables

    [Space(3), Header("Enemy Type"), Space(3)]
    [SerializeField] private EnemyType type;

    [Space(3), Header("Checks"), Space(3)]
    [Space(3), SerializeField] LayerMask playerLayer;
    [SerializeField] private float checkRad;
    [SerializeField] private float attackRad, runRad;

    [Space(3), Header("Movement"), Space(3)]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runSpeed, reloadRunSpeed;
    public float knockbackForce;

    [Space(3), Header("Projectile Variables"), Space(3)]
    [Space(3), SerializeField] internal GameObject projectile;
    [Space(3), SerializeField] private float projectileSpeed; 
    [SerializeField] private float projectileStartTime;
    [Space(3), SerializeField] private float projectileCount;
    [SerializeField] private float projectileAmmoCount;
    [Space(3), SerializeField] private float projectileReloadTime;
    [SerializeField] float projectileInterval, projectileMinRandom, projectileMaxRandom;
    [Space(3), SerializeField] internal float projectileDamage;
    float projectileAmmoCountTemp, projectileStartTemp;
    internal bool isHit;

    [Header("Misc")]
    GameObject player;
    Rigidbody2D rb;

    Vector2 dir;

    internal States state, prevState;
    public float stateDur = 0f;

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        state = States.none;
        projectileAmmoCountTemp = projectileAmmoCount;
        projectileStartTemp = projectileStartTime;
    }

    // Update is called once per frame
    void Update()
    {
        void ChangeState(States newState)
        {
            prevState = state;
            state = newState;
            stateDur = 0f;
        }

        // Checks
        bool isCheck = Physics2D.OverlapCircle(transform.position, checkRad, playerLayer), isAttack = Physics2D.OverlapCircle(transform.position, attackRad, playerLayer), isRun = Physics2D.OverlapCircle(transform.position, runRad, playerLayer);

        #region State Machines

        // On state enter
        if (stateDur == 0)
        {
            switch (state)
            {
                case States.none:
                    rb.velocity = new Vector2(0f, 0f);
                    break;

                case States.checking:
                    projectileStartTemp = projectileStartTime;
                    break;

                case States.attacking:
                    rb.velocity = new Vector2(0f, 0f);
                    break;

                case States.running:
                    projectileStartTemp = projectileStartTime;
                    break;

                case States.reloading:
                    projectileStartTemp = projectileStartTime;
                    break;
            }
        }

        stateDur += Time.deltaTime;

        switch (state)
        {
            // When not doing anything else
            case States.none:

                if (projectileAmmoCountTemp <= 0) ChangeState(States.reloading);

                if (stateDur > .5f)
                {
                    if (isCheck) ChangeState(States.checking);
                }

                break;

            // When in view
            case States.checking:

                // Go to different states based off collisions
                if (!isCheck) ChangeState(States.none);
                if (isAttack) ChangeState(States.attacking);

                if (projectileAmmoCountTemp <= 0) ChangeState(States.reloading);

                if (stateDur > 1.5f && projectileAmmoCountTemp > 0)
                {
                    shootProjectile();
                    stateDur = 0f;
                }

                dir = (player.transform.position - rb.transform.position).normalized;
                break;

            // When attacking
            case States.attacking:

                if (!isAttack) ChangeState(States.checking);

                // For some reason the first shot from enemies right as you enter attack takes a long time. This fixes it
                if (stateDur > projectileStartTemp && projectileAmmoCountTemp > 0)
                {
                    projectileStartTemp += projectileInterval;
                    shootProjectile();
                }

                if (stateDur > projectileInterval && projectileAmmoCountTemp > 0) shootProjectile();
                if (stateDur > projectileInterval && projectileAmmoCountTemp <= 0) ChangeState(States.reloading);

                if (isRun && type != EnemyType.Shotgunner) ChangeState(States.running);

                break;

            // When running
            case States.running:

                if (!isRun) ChangeState(States.checking);

                if(stateDur > 0.7f && projectileAmmoCountTemp > 0)
                {
                    shootProjectile();
                    stateDur = 0f;
                }
                else if (projectileAmmoCountTemp <= 0) ChangeState(States.reloading);

                dir = (player.transform.position - rb.transform.position).normalized;
                break;

            // When reloading
            case States.reloading:

                if (stateDur > projectileReloadTime)
                {
                    projectileAmmoCountTemp = projectileAmmoCount;
                    ChangeState(States.checking);
                }

                dir = (player.transform.position - rb.transform.position).normalized;

                break;
        }
        #endregion
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case States.checking:
                MoveEnemy(dir, moveSpeed);
                break;

            case States.running:
                MoveEnemy(-dir, runSpeed);
                break;

            case States.reloading:
                MoveEnemy(-dir, reloadRunSpeed);
                break;
        }
    }

    internal void MoveEnemy(Vector2 dir, float speed)
    {
        rb.MovePosition((Vector2)transform.position + (speed * Time.deltaTime * dir));
    }

    #region Projectiles
    void shootProjectile()
    {
        Vector2 projectileDir;

        for (int i = 0; i < projectileCount; i++)
        {
            GameObject firedObj = Instantiate(projectile, transform.position, Quaternion.identity);

            firedObj.GetComponent<Projectiles>().projectileDamage = projectileDamage;

            if (type == EnemyType.Shotgunner || type == EnemyType.Sniper)
            {
                knockBack(player, -1);
            }

            if (interceptDir(player.transform.position, transform.position, player.GetComponent<Rigidbody2D>().velocity.normalized * Mathf.Min(player.GetComponent<Rigidbody2D>().velocity.magnitude, player.GetComponent<BetterMovement>().dashSpeed), projectileSpeed, out projectileDir))
            {
                firedObj.GetComponent<Rigidbody2D>().velocity = (projectileDir + rb.velocity).normalized * projectileSpeed;
            }
            else
            {
                firedObj.GetComponent<Rigidbody2D>().velocity = (player.transform.position - transform.position).normalized * projectileSpeed;
            }

            projectileAmmoCountTemp--;
        }

        stateDur = 0f;
    }

    public bool interceptDir(Vector2 playerObj, Vector2 enemyObj, Vector2 playerVel, float projectileSpeed, out Vector2 dir)
    {
        // Quadratic formula solution
        static int SolveQuadratic(float a, float b, float c, out float r1, out float r2)
        {
            float d = b * b - 4 * a * c;

            if (d < 0)
            {
                r1 = Mathf.Infinity;
                r2 = -r1;
                return 0;
            }

            r1 = (-b + Mathf.Sqrt(d)) / (2 * a);
            r2 = (-b - Mathf.Sqrt(d)) / (2 * a);

            return d > 0 ? 2 : 1;
        }

        // Get the direction from enemyObj to playerObj
        Vector2 dist = enemyObj - playerObj;
        float distMag = dist.magnitude;

        // Math
        var alpha = Vector2.Angle(dist, playerVel) * Mathf.Deg2Rad;
        var sA = playerVel.magnitude;
        var r = sA / projectileSpeed;

        // Ensure that if you character is stopped it won't fire it incorrectly
        if (SolveQuadratic(1 - r * r, 2 * r * distMag * Mathf.Cos(alpha), -(distMag * distMag), out var r1, out var r2) == 0)
        {
            dir = Vector2.zero * Random.Range(projectileMinRandom, projectileMaxRandom);
            return false;
        }

        var dA = Mathf.Max(r1, r2);
        var t = dA / projectileSpeed;
        var c = playerObj + new Vector2(Random.Range(projectileMinRandom, projectileMaxRandom), Random.Range(projectileMinRandom, projectileMaxRandom)) + playerVel * t;

        dir = (c - enemyObj).normalized;

        return true;
    }

    #endregion

    public void knockBack(GameObject objPos, int i)
    {
        Vector2 dir = (objPos.transform.position - rb.transform.position).normalized;

        Vector2 knockback = dir * i * knockbackForce;

        rb.velocity = new Vector2(0f, 0f);

        rb.AddForce(knockback, ForceMode2D.Force);
    }
}

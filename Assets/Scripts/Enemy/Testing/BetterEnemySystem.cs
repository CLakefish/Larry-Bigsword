using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BetterEnemySystem : MonoBehaviour
{
    public enum State
    {
        checking,
        attacking,
        running,
        reloading,
        none
    }

    public State currentStateM, currentStateA, previousStateM, previousStateA;
    private float stateDurMovement, stateDurAttack;

    [Space(3), Header("Enemy Type"), Space(3)]
    [SerializeField] private EnemyType type;

    [Space(3), Header("Checks"), Space(3)]
    [Space(3), SerializeField] LayerMask playerLayer;
    [SerializeField] private float checkRad;
    [SerializeField] private float attackRad, runRad;

    [Space(3), Header("Movement"), Space(3)]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runSpeed, reloadRunSpeed;
    [SerializeField] private float parriedStunTime;

    [Space(3), Header("Projectile Variables"), Space(3)]
    [Space(3), SerializeField] private GameObject projectile;
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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        player = GameObject.FindGameObjectWithTag("Player");

        projectileAmmoCountTemp = projectileAmmoCount;
        projectileStartTemp = projectileStartTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            return;
        }

        // Checks
        bool isCheck = Physics2D.OverlapCircle(transform.position, checkRad, playerLayer), 
             isAttack = Physics2D.OverlapCircle(transform.position, attackRad, playerLayer), 
             isRun = Physics2D.OverlapCircle(transform.position, runRad, playerLayer);

        stateDurAttack += Time.deltaTime;

        MovementStates(isCheck, isAttack, isRun);
    }

    private void FixedUpdate()
    {
        switch (currentStateM)
        {
            case State.none:
                rb.velocity = new Vector2(0f, 0f);
                break;

            case State.checking:

                if (Physics2D.OverlapCircle(transform.position, runRad * 1.25f).gameObject.tag == "enemy")
                {
                    GameObject obj = Physics2D.OverlapCircle(transform.position, runRad * 1.25f).gameObject;

                    MoveEnemy((dir + (Vector2)obj.transform.position).normalized, moveSpeed);
                }
                else 
                    MoveEnemy(dir.normalized, moveSpeed);

                break;
        }
    }

    internal void MovementStates(bool check, bool attack, bool run)
    {
        void ChangeState(State newState)
        {
            previousStateM = currentStateM;
            currentStateM = newState;
            stateDurMovement = 0f;
        }

        if (stateDurMovement == 0)
        {
            switch (currentStateM)
            {
                case State.none:

                    break;

                case State.checking:

                    break;

                case State.running:

                    break;
            }
        }

        stateDurMovement += Time.deltaTime;

        switch (currentStateM)
        {
            case State.none:
                if (check) ChangeState(State.checking);
                break;

            case State.checking:
                if (attack) ChangeState(State.attacking);

                dir = player.transform.position - rb.transform.position;
                break;

            case State.attacking:

                if (run) ChangeState(State.running);

                dir = new Vector2(0f, 0f);
                break;
        }

    }

    internal void MoveEnemy(Vector2 dir, float speed)
    {
        rb.MovePosition((Vector2)transform.position + (speed * Time.deltaTime * dir));
    }
}

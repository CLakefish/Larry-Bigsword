using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    internal enum States
    {
        checking,
        attacking,
        running
    }

    internal States state, previousState;
    float stateDur = 0f;



    [Space(3), Header("Radius"), Space(3)]
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask enemyLayer;
    [Space(3)]
    [SerializeField] float checkRadius;
    [SerializeField] float attackRadius, runRadius, correctionRadius;

    [Space(3), Header("Movement"), Space(3)]
    [SerializeField] float moveSpeed;
    [SerializeField] float runSpeed;

    GameObject player;
    Rigidbody2D rb;
    Vector2 dir;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        player = GameObject.FindGameObjectWithTag("Player");

        state = States.checking;
    }

    // Update is called once per frame
    void Update()
    {
        void StateChange(States newState)
        {
            previousState = state;
            state = newState;
            stateDur = 0f;
        }

        // Checks
        bool isCheck = Physics2D.OverlapCircle(transform.position, checkRadius, playerLayer),
             isAttack = Physics2D.OverlapCircle(transform.position, attackRadius, playerLayer),
             isRun = Physics2D.OverlapCircle(transform.position, runRadius, playerLayer),
             isClumped = Physics2D.OverlapCircle(transform.position, correctionRadius, enemyLayer);

        switch (state)
        {
            case States.checking:
                if (isAttack) StateChange(States.attacking);

                if (previousState == States.attacking)
                {
                    if (isClumped)
                    {
                        GameObject obj = Physics2D.OverlapCircle(transform.position, correctionRadius, enemyLayer).gameObject;

                        dir = (obj.transform.position + rb.transform.position).normalized;
                    }
                    else
                    {
                        dir = (player.transform.position - rb.transform.position).normalized;
                    }
                }
                else
                {
                    dir = (player.transform.position - rb.transform.position).normalized;
                }

                break;

            case States.attacking:

                if (!isAttack) StateChange(States.checking);
                if (isRun) StateChange(States.running);

                if(isClumped)
                {
                    GameObject obj = Physics2D.OverlapCircle(transform.position, correctionRadius, enemyLayer).gameObject;

                    dir = (obj.transform.position + rb.transform.position).normalized;
                }
                else
                {
                    dir = new Vector2(0f, 0f);
                }

                break;

            case States.running:
                if (!isRun) StateChange(States.attacking);

                dir = (player.transform.position - rb.transform.position).normalized;
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case States.checking:
                MoveEnemy(dir, moveSpeed);
                break;

            case States.attacking:
                MoveEnemy(dir, moveSpeed);
                break;

            case States.running:
                MoveEnemy(-dir, runSpeed);
                break;
        }
    }

    internal void MoveEnemy(Vector2 dir, float speed)
    {
        rb.MovePosition((Vector2)transform.position + (speed * Time.deltaTime * dir));
    }
}


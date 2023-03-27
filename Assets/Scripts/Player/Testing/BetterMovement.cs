using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterMovement : MonoBehaviour
{
    #region Variables

    [Space(3), Header("Keybinds"), Space(3)]
    [SerializeField] private KeyCode dashKey = KeyCode.Space;

    [Space(3), Header("Bools"), Space(3)]
    [SerializeField] internal bool canMove = true;
    [SerializeField] internal bool hasSword = true;
    internal bool canDash = true;

    [Space(3), Header("Movement"), Space(3)]
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float accelerationTime, decelerationTime;

    [Space(3), Header("Sword"), Space(3)]
    public GameObject swordObj;
    [SerializeField] public float swingDuration, swingCooldown;

    [Space(3), Header("Dash"), Space(3)]
    [SerializeField] internal float dashSpeed = 30f;
    [SerializeField] private float dashDuration, dashCooldown;

    [Space(3), Header("Parry"), Space(3)]
    [SerializeField] public float parryDuration;
    [SerializeField] public float parryCooldown;
    [SerializeField] public bool isParry, isInvincible = false;
    internal float impactTimeFreeze = 0.0000009f;
    float knockbackForce = 800f;
    internal int hitCount;


    [Space(3), Header("https://i.kym-cdn.com/entries/icons/original/000/023/977/cover3.jpg"), Space(3)]
    Vector2 input;

    [Header("Visuals")]
    public GameObject parryVisual, dashParticle;
    internal GameObject parryVFX, dashVFX;

    private Rigidbody2D rb;

    Vector2 currentVel;
    private float stateDur;
    float speed, speedVel;

    #endregion

    // Enum
    internal enum States
    {
        running,
        dashing,
        none
    }

    internal States state, prevState;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        state = States.running;
    }

    private void Update()
    {
        // Change State
        void ChangeState(States newState)
        {
            prevState = state;
            state = newState;
            stateDur = 0f;
        }

        #region Inputs

        // Inputs 
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        bool inputting = input != Vector2.zero;

        bool dashInput = Input.GetKeyDown(dashKey);

        #endregion

        #region Cooldowns

        // Cooldowns
        bool dashCooldownComplete = !(prevState == States.dashing && stateDur < dashCooldown);

        #endregion

        if (dashVFX != null) dashVFX.transform.position = rb.transform.position;

        #region Statemachine

        // On state enter
        if (stateDur == 0)
        {
            switch (state)
            {
                case States.none:

                    canMove = canDash = false;
                    rb.velocity = new Vector2(0f, 0f);
                    break;

                case States.running:

                    isInvincible = false;
                    break;

                case States.dashing:

                    isInvincible = true;
                    dashVFX = Instantiate(dashParticle, rb.transform.position, Quaternion.identity);
                    parryVFX = Instantiate(parryVisual, rb.position, Quaternion.identity);
                    break;
            }
        }

        stateDur += Time.deltaTime;

        // State checks & continuals
        switch (state) {

            case States.none:

                if (stateDur > 0.1f)
                {
                    canMove = canDash = true;
                    ChangeState(States.running);
                }

                break;

            // Run Case
            case States.running:

                speed = Mathf.SmoothDamp(speed, moveSpeed, ref speedVel, .075f);

                // Dash w/Cooldown
                if (dashInput && dashCooldownComplete && canDash) ChangeState(States.dashing);

                break;

            // Dash Case
            case States.dashing:

                if (parryVFX != null) parryVFX.transform.position = rb.transform.position;

                speed = Mathf.SmoothDamp(speed, dashSpeed, ref speedVel, .055f);

                if (stateDur > dashDuration)
                {
                    ChangeState(States.running);
                    if (parryVFX != null) Destroy(parryVFX);
                }

                break;
        }
        #endregion

        Vector2 desiredVel = input * speed;

        float acceleration = inputting ? accelerationTime : decelerationTime;

        if (canMove) rb.velocity = Vector2.SmoothDamp(rb.velocity, desiredVel, ref currentVel, acceleration);
    }

    public void knockBack(GameObject objPos)
    {
        prevState = state;
        state = States.none;
        stateDur = 0f;

        Vector2 dir = (objPos.transform.position - rb.transform.position).normalized;

        Vector2 knockback = dir * -1 * knockbackForce;

        rb.velocity = new Vector2(0f, 0f);

        rb.AddForce(knockback, ForceMode2D.Force);
    }
}
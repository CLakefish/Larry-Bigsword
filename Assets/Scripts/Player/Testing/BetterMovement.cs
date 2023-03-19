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

    [Space(3), Header("Movement"), Space(3)]
    [SerializeField] private float moveSpeed = 12f;

    [Space(3), Header("Sword"), Space(3)]
    public GameObject swordObj;
    [SerializeField] public float swingDuration, swingCooldown;

    [Space(3), Header("Dash"), Space(3)]
    [SerializeField] private float dashSpeed = 30f;
    [SerializeField] private float dashDuration, dashCooldown;

    [Space(3), Header("Parry"), Space(3)]
    [SerializeField] public float parryDuration;
    [SerializeField] public float parryCooldown;
    internal bool isParry, isInvincible = false;
    internal float impactTimeFreeze = 0.000001f;


    [Space(3), Header("https://i.kym-cdn.com/entries/icons/original/000/023/977/cover3.jpg"), Space(3)]
    Vector2 input;

    private Rigidbody2D rb;

    private float stateDur;

    #endregion

    // Enum
    internal enum States
    {
        running,
        dashing
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

        bool swordInput = Input.GetMouseButtonDown(0),
             dashInput = Input.GetKeyDown(dashKey);

        #endregion

        #region Cooldowns

        // Cooldowns
        bool dashCooldownComplete = !(prevState == States.dashing && stateDur < dashCooldown);

        #endregion

        if (canMove) rb.velocity = input * moveSpeed;

        #region Statemachine (ty Oliver)

        // On state enter
        if (stateDur == 0)
        {
            switch (state)
            {
                case States.running:
                    isInvincible = false;
                    rb.velocity = new Vector2(0f, 0f);
                    break;

                case States.dashing:
                    isInvincible = true;
                    rb.velocity = new Vector2(0f, 0f);
                    break;
            }
        }

        stateDur += Time.deltaTime;

        // State checks & continuals
        switch (state) { 

            // Run Case
            case States.running:

                // Dash w/Cooldown
                if (dashInput && dashCooldownComplete) ChangeState(States.dashing);

                break;

            // Dash Case
            case States.dashing:

                rb.velocity = input * dashSpeed;
                if (stateDur > dashDuration) ChangeState(States.running);

                break;
        }
        #endregion
    }
}
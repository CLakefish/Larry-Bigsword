using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordInput : MonoBehaviour
{
    private Camera cam;
    Vector2 mousePos;
    internal GameObject sword;
    BetterMovement p;
    Rigidbody2D rb;

    private float stateDur;

    private enum States
    {
        parrying,
        swinging,
        none
    }

    private States state, prevState;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        cam = FindObjectOfType<Camera>();
        p = gameObject.GetComponent<BetterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // ChangeState
        void ChangeState(States newState)
        {
            prevState = state;
            state = newState;
            stateDur = 0f;
        }

        // Inputs
        bool swordInput = Input.GetMouseButtonDown(0),
             parryInput = Input.GetMouseButtonDown(1);

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        // Cooldown
        bool parryCooldownComplete = !(prevState == States.parrying && stateDur < p.parryCooldown),
             swingCooldownComplete = !(prevState == States.swinging && stateDur < p.swingCooldown);

        if (stateDur == 0)
        {
            switch (state)
            {
                case States.parrying:
                    p.isInvincible = true;
                    p.isParry = true;
                    rb.velocity = new Vector2(0f, 0f);
                    break;

                case States.swinging:
                    sword = Instantiate(p.swordObj, rb.position, rb.transform.rotation);
                    break;
            }
        }

        stateDur += Time.deltaTime;

        switch (state)
        {
            case States.none:
                // Parry w/Cooldown
                if (parryInput && parryCooldownComplete) ChangeState(States.parrying);

                // Sword Swing w/Cooldown
                if (swordInput && p.hasSword && swingCooldownComplete) ChangeState(States.swinging);

                break;

            case States.swinging:

                // If sword is broken via projectile
                if (sword != null)
                {
                    SwordMovement();
                }

                // End of duration
                if (stateDur > p.swingDuration)
                {
                    ChangeState(States.none);
                    Destroy(sword);
                }

                break;

            case States.parrying:

                if (stateDur > p.parryDuration || (FindObjectOfType<Enemy>() && !FindObjectOfType<Enemy>().canCheck))
                {
                    p.isParry = false;
                    p.isInvincible = false;
                    ChangeState(States.none);
                }

                break;
        }
    }

    void SwordMovement()
    {
        // Mouse Position Rotations
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        sword.transform.rotation = rotation;
        sword.transform.position = rb.transform.position;
    }
}

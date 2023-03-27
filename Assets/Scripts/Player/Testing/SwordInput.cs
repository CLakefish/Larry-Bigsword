using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordInput : MonoBehaviour
{
    private Camera cam;
    Vector2 mousePos;
    public GameObject parryVisual;
    internal GameObject parryVFX;
    internal GameObject sword;
    BetterMovement p;
    Rigidbody2D rb;
    float angle;
    int i = 0;

    private float stateDur;

    private enum States
    {
        parrying,
        swinging,
        none
    }

    [SerializeField] private States state, prevState;

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
                    parryVFX = Instantiate(parryVisual, rb.position, Quaternion.identity);
                    p.isInvincible = true;
                    p.isParry = true;
                    rb.velocity = new Vector2(0f, 0f);
                    break;

                case States.swinging:
                    sword = Instantiate(p.swordObj, rb.position, rb.transform.rotation);
                    if (sword != null) SwordMovement();
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

                if (sword != null)
                {
                    sword.transform.position = rb.transform.position;

                    if (i == 0)
                    {
                        sword.transform.eulerAngles = Vector3.Lerp(sword.transform.eulerAngles, new Vector3(0f, 0f, sword.transform.eulerAngles.z + 180f), 4f * Time.deltaTime);
                        StartCoroutine(swordSwingIntDiff(1));
                    }

                    if (i == 1)
                    {
                        sword.transform.eulerAngles = Vector3.Lerp(sword.transform.eulerAngles, new Vector3(0f, 0f, sword.transform.eulerAngles.z - 180f), 4f * Time.deltaTime);
                        StartCoroutine(swordSwingIntDiff(0));
                    }
                }

                if (parryInput && parryCooldownComplete)
                {
                    if (sword != null) Destroy(sword);
                    ChangeState(States.parrying);
                }

                    // End of duration
                if (stateDur > p.swingDuration)
                {
                    ChangeState(States.none);
                    Destroy(sword);
                }

                break;

            case States.parrying:

                if(parryVFX != null) parryVFX.transform.position = rb.transform.position;

                if (stateDur > p.parryDuration || (FindObjectOfType<Enemy>() && !FindObjectOfType<Enemy>().canCheck))
                {
                    Destroy(parryVFX);
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

        if (i == 0) angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - 60f;
        if (i == 1) angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg + 60f;

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        sword.transform.rotation = rotation;
    }

    IEnumerator swordSwingIntDiff(int val)
    {
        yield return new WaitForSeconds(p.swingDuration);
        i = val;
    }
}

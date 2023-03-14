/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*
 Made by : Carson Lakefish & 
 
 Date : 3 / 10 / 2023

 Description : Basic Movement Top-Down Movement w/Dash
 *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CurrentDir
{
    public bool isUp, isDown;
    public bool isLeft, isRight;
    public void ResetBool()
    {
        isUp = isDown = false;
        isLeft = isRight = false;
    }
}

public class Movement : MonoBehaviour
{


    #region Variables

    public Rigidbody2D rb;
    public Camera cam;
    GameObject sword;

    [Header("Keybinds")]
    [Space(3)]
    public KeyCode dashKey = KeyCode.Space;

    [Header("Sprite Prefabs")]
    public GameObject swordSwing;

    [Header("Basic Variables")]
    [Space(3)]
    public bool isInvincible;
    [Space(3)]
    public float healthPoints;
    public float swingDamage;

    [Header("Basic Movement Variables")]
    [Space(3)]
    public bool canMove = true;
    [Space(3)]
    public float moveSpeed;

    [Header("Dash Variables")]
    [Space(3)]
    public bool canDash = true;
    [Space(3)]
    public float dashSpeed;
    public float dashCooldown;
    public float dashTime;

    [Header("Parry Variables")]
    [Space(3)]
    public bool canParry = true;
    [Space(3)]
    public float parryTime;
    public float parryCooldown;

    [Header("Melee Variables")]
    [Space(3)]
    public bool hasSword = true;
    [Space(3)]
    public bool canSwing = true;
    [Space(3)]
    public float swingTime;
    public float swingCooldown;
    bool isSwinging;


    [HideInInspector] public bool isParry;

    public CurrentDir facingDir;
    Vector2 input, moveDir, mousePos;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Input detection
        Inputs();
        // Used for Sprites
        CheckDir();


        // Sword Swing
        if (isSwinging)
        {
            sword.transform.position = rb.transform.position;

            // Rotations
            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            sword.transform.rotation = rotation;
        }

        moveDir = new Vector2(input.x, input.y).normalized;
    }

    private void FixedUpdate()
    {
        // Basic Movement
        if (canMove)
        {
            rb.velocity = (moveDir * moveSpeed);
        }
    }

    // For organization
    void Inputs()
    {
        if (canMove)
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        // Dash function
        if (Input.GetKeyDown(dashKey) && canDash)
        {
            StartCoroutine(Dash(input.x, input.y));
        }

        if (Input.GetMouseButtonDown(0) && hasSword && canSwing)
        {
            StartCoroutine(Sword(mousePos));
        }
    }

    // Basic Dash function
    #region Dash
    IEnumerator Dash(float x, float y)
    {
        float dashBeginning = Time.time;
        rb.velocity = Vector2.zero;

        canMove = false;
        canDash = false;

        // Dash in your current movement direction, if there is none, then you wont.
        if (x != 0 || y != 0)
        {
            moveDir = new Vector2(x, y);
        }
        else
        {
            if (canParry)
            {
                StartCoroutine(Parry());
            }
        }

        // Apply the force
        while (Time.time < dashBeginning + dashTime)
        {
            rb.velocity = moveDir.normalized * dashSpeed;
            yield return null;
        }

        canMove = true;
        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    #endregion

    // This could be altered in 3 different ways, I'll explain when needed
    #region Sprites
    void CheckDir()
    {
        // This will be used for animations later on, we could either use this for 8 directional sprites or 4 directional, since in either case we have the proper struct.

        if (rb.velocity.x == 0 && rb.velocity.y == 0)
        {
            return;
        }

        // Reset Bools
        facingDir.ResetBool();

        // Left & Right
        if (rb.velocity.x > 0)
        {
            facingDir.isLeft = true;
        }
        else if (rb.velocity.x < 0)
        {
            facingDir.isRight = true;
        }

        // Up & Down
        if (rb.velocity.y > 0)
        {
            facingDir.isUp = true;
        }
        else if (rb.velocity.y < 0)
        {
            facingDir.isDown = true;
        }
    }
    #endregion

    // Basic Parry
    #region Parry
    IEnumerator Parry()
    {
        // Reset your velocity, make sure you can't move. The Debug.Log is just for debugging
        rb.velocity = new Vector2(0f, 0f);

        canParry = canDash = canMove = false;
        isInvincible = true;

        yield return new WaitForSeconds(parryTime);

        Debug.Log("Parry");

        canDash = canMove = true;
        isInvincible = false;

        yield return new WaitForSeconds(parryCooldown);

        canParry = true;
    }

    #endregion

    // Basic sword swing
    #region Sword Move
    IEnumerator Sword(Vector2 mousePos)
    {
        // Make the gameObject
        sword = Instantiate(swordSwing, rb.position, rb.transform.rotation);

        // Get mouse rotation so it doesn't look weird on spawn
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Basic enables/disables
        canSwing = canParry = false;
        isInvincible = isSwinging = true;

        sword.transform.rotation = rotation;

        yield return new WaitForSeconds(swingTime);

        canParry = true;
        isInvincible = isSwinging = false;
        Destroy(sword);

        yield return new WaitForSeconds(swingCooldown);

        canSwing = true;
    }
    #endregion
}
/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*
 * Name: Carson Lakefish & Nico Sayed
 * Date: 3 / 22 / 2023
 * Desc: Health Points script
 *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPoints : MonoBehaviour
{
    [Header("Player")]
    [Space(3)]
    public bool isPlayer;
    [Space(3)]
    public HealthBar healthBar;

    [Header("Health")]
    [Space(3)]
    public bool destroyAtZero;
    [Space(3)]
    public float currentHP;
    public float maxHP;

    [Header("Death Effect")]
    [Space(3)]
    public bool deathEffect;
    [Space(3)]
    public GameObject deathObj;
    public float deathTime;
    bool invincibility;

    private void Start()
    {
        healthBar = GameObject.FindObjectOfType<HealthBar>().GetComponent<HealthBar>();
    }

    private void Update()
    {
        if (isPlayer)
        {
            invincibility = gameObject.GetComponent<BetterMovement>().isInvincible;

            healthBar.updateHealthBar(gameObject.GetComponent<HealthPoints>().maxHP, gameObject.GetComponent<HealthPoints>().currentHP);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isPlayer)
        {
            if (invincibility)
            {
                return;
            }

            currentHP -= damage;
            
            if (currentHP <= 0)
            {
                currentHP = 0;
            }

            StartCoroutine(invincibilityFrame());

            return;
        }
        else
        {
            currentHP -= damage;

            if (currentHP <= 0)
            {
                currentHP = 0f;

                if (destroyAtZero)
                {
                    if (deathEffect)
                    {
                        onDeath();
                        Destroy(gameObject);
                        return;
                    }

                    Destroy(gameObject);
                }
            }
        }
    }

    public void GainHealth(float health)
    {
        currentHP += health;

        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    void onDeath()
    {
        GameObject obj;

        if (deathObj != null)
        {
            Vector3 spawnPos = transform.position;
            obj = Instantiate(deathObj, spawnPos, transform.rotation);
            Destroy(obj, deathTime);
            return;
        }
    }

    //forgot the parry, make parry reflect bullets OR destroy all bullets and knockback enemies

    IEnumerator invincibilityFrame()
    {
        if (gameObject.GetComponent<BetterMovement>().isParry)
        {
            gameObject.GetComponent<BetterMovement>().isInvincible = true;

            yield return new WaitForSeconds(2f);

            gameObject.GetComponent<BetterMovement>().isInvincible = false;
        }
        else
        {
            gameObject.GetComponent<BetterMovement>().isInvincible = true;

            yield return new WaitForSeconds(.25f);

            gameObject.GetComponent<BetterMovement>().isInvincible = false;
        }
    }
}

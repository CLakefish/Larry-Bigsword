using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPoints : MonoBehaviour
{
    [Header("Player")]
    [Space(3)]
    public bool isPlayer;
    [Space(3)]

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

    private void Update()
    {
        if (isPlayer)
        {
            invincibility = gameObject.GetComponent<Movement>().isInvincible;
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

    IEnumerator invincibilityFrame()
    {
        gameObject.GetComponent<Movement>().isInvincible = true;

        yield return new WaitForSeconds(1f);

        gameObject.GetComponent<Movement>().isInvincible = false;
    }
}
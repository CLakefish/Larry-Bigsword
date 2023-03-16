using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    [Header("Damage Variables")]
    [Space(3)]
    public bool timedDestroy;
    [Space(3)]
    public float deathTime;
    public float projectileDamage;

    [Header("Collision Effects")]
    [Space(3)]
    public bool destroyOnCollide;
    public bool hasDeathEffect;
    [Space(3)]
    public GameObject deathEffect;
    [Space(3)]
    public float collisionDeathTime;

    private void Start()
    {
        if (timedDestroy)
        {
            Destroy(gameObject, deathTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HealthPoints hp = collision.gameObject.GetComponent<HealthPoints>();
        Movement player = collision.gameObject.GetComponent<Movement>();

        if (collision.gameObject.layer == 2)
        {
            return;
        }

        if (hp != null)
        {
            if (player != null)
            {
                if (!player.isInvincible)
                {
                    hp.TakeDamage(projectileDamage);
                }
            }
            else
            {
                hp.TakeDamage(projectileDamage);
            }
        }
        if (destroyOnCollide)
        {
            if (hasDeathEffect && deathEffect != null)
            {
                GameObject obj;

                Vector3 spawnPos = transform.position;
                obj = Instantiate(deathEffect, spawnPos, transform.rotation);

                Destroy(obj, collisionDeathTime);
            }

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthPoints hp = collision.gameObject.GetComponent<HealthPoints>();
        Movement player = collision.gameObject.GetComponent<Movement>();

        if (collision.gameObject.layer == 2)
        {
            return;
        }

        if (hp != null)
        {
            if (player != null)
            {
                if (!player.isInvincible)
                {
                    hp.TakeDamage(projectileDamage);
                }
            }
            else
            {
                hp.TakeDamage(projectileDamage);
            }
        }
        if (destroyOnCollide)
        {
            if (hasDeathEffect && deathEffect != null)
            {
                GameObject obj;

                Vector3 spawnPos = transform.position;
                obj = Instantiate(deathEffect, spawnPos, transform.rotation);

                Destroy(obj, collisionDeathTime);
            }

            Destroy(gameObject);
        }
    }
}
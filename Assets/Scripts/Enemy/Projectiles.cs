/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*
 * Name: Carson Lakefish & Nico Sayed
 * Date: 3 / 21 / 2023
 * Desc: Projectile Collisions Handler
 *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    public GameObject playerProjectile;

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
        BetterMovement player = collision.gameObject.GetComponent<BetterMovement>();
        BetterEnemy enemy = collision.gameObject.GetComponent<BetterEnemy>();
        CameraManager camera = GameObject.FindGameObjectWithTag("MainCamera").gameObject.GetComponent<CameraManager>();

        if (collision.gameObject.layer == 2 || collision.gameObject.layer == 10)
        {
            return;
        }

        // Enemy hits
        if (hp != null)
        {
            if (enemy != null)
            {
                if (enemy.isHit) return;

                camera.shakeDuration = .05f;
                camera.shakeMagnitude = .1f;

                hp.TakeDamage(1);
                enemy.knockBack(gameObject, -1, 3f);
                enemy.isHit = true;
                enemy.state = BetterEnemy.States.none;

                AudioHandler.PlaySound("eH");

                Destroy(gameObject);
            }
        }

        // Player Hits
        if (hp != null)
        {
            if (player != null)
            {
                if (player.isParry)
                {
                    if (player.GetComponent<SwordInput>().parryVFX != null)
                    {
                        Destroy(player.GetComponent<SwordInput>().parryVFX);
                    }

                    AudioHandler.PlaySound("pP");

                    player.GetComponent<HealthPoints>().GainHealth(1);

                    camera.shakeDuration = .1f;
                    camera.shakeMagnitude = .1f;

                    HitManager.ImpactHit();

                    // Destroy all projectiles
                    foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Projectile"))
                    {
                        Destroy(obj);
                    }

                    // Enemy knockback to prevent shooting
                    foreach (GameObject obj in GameObject.FindGameObjectsWithTag("enemy"))
                    {
                        if (obj.GetComponent<BetterEnemy>() != null)
                        {
                            enemy = obj.GetComponent<BetterEnemy>();

                            enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                            enemy.knockBack(gameObject, -1, 5);

                            enemy.state = BetterEnemy.States.none;
                        }

                        GameObject firedObj = Instantiate(playerProjectile, transform.position, Quaternion.identity);
                        firedObj.GetComponent<Rigidbody2D>().velocity = (enemy.transform.position - collision.gameObject.transform.position).normalized * 15f;
                    }

                    return;
                }

                if (player.isInvincible && player.state != BetterMovement.States.dashing && !player.isParry) return;

                if (player.isInvincible && player.state == BetterMovement.States.dashing)
                {
                    HitManager.ImpactHit();

                    if (player.parryVFX != null) Destroy(player.parryVFX);

                    AudioHandler.PlaySound("pH");
                    player.isInvincible = false;

                    return;
                }


                if (!player.isInvincible)
                {
                    camera.shakeDuration = .1f;
                    camera.shakeMagnitude = .1f;
                    HitManager.ImpactHit();
                    hp.TakeDamage(projectileDamage);

                    StartCoroutine(IFrame());

                    AudioHandler.PlaySound("pH");

                    player.knockBack(gameObject);
                }
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
        BetterMovement player = collision.gameObject.GetComponent<BetterMovement>();
        BetterEnemy enemy = collision.gameObject.GetComponent<BetterEnemy>();
        CameraManager camera = GameObject.FindGameObjectWithTag("MainCamera").gameObject.GetComponent<CameraManager>();

        if (collision.gameObject.layer == 2 || collision.gameObject.layer == 10)
        {
            return;
        }

        // Enemy hits
        if (hp != null)
        {
            if (enemy != null)
            {
                if (enemy.isHit) return;

                camera.shakeDuration = .05f;
                camera.shakeMagnitude = .1f;

                hp.TakeDamage(1);

                enemy.knockBack(gameObject, -1, 3f);
                enemy.isHit = true;
                enemy.state = BetterEnemy.States.none;

                Destroy(gameObject);
            }
        }

        // Player Hits
        if (hp != null)
        {
            if (player != null)
            {
                if (player.isParry)
                {
                    if (player.GetComponent<SwordInput>().parryVFX != null)
                    {
                        Destroy(player.GetComponent<SwordInput>().parryVFX);
                    }

                    AudioHandler.PlaySound("pP");

                    player.GetComponent<HealthPoints>().GainHealth(1);

                    camera.shakeDuration = .1f;
                    camera.shakeMagnitude = .1f;

                    HitManager.ImpactHit();

                    // Destroy all projectiles
                    foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Projectile"))
                    {
                        Destroy(obj);
                    }

                    // Enemy knockback to prevent shooting
                    foreach (GameObject obj in GameObject.FindGameObjectsWithTag("enemy"))
                    {
                        if (obj.GetComponent<BetterEnemy>() != null)
                        {
                            enemy = obj.GetComponent<BetterEnemy>();

                            enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                            enemy.knockBack(gameObject, -1, 5);

                            enemy.state = BetterEnemy.States.none;
                        }

                        GameObject firedObj = Instantiate(playerProjectile, transform.position, Quaternion.identity);
                        firedObj.GetComponent<Rigidbody2D>().velocity = (enemy.transform.position - collision.gameObject.transform.position).normalized * 15f;
                    }

                    return;
                }

                if (player.isInvincible && player.state != BetterMovement.States.dashing && !player.isParry) return;

                if (player.isInvincible && player.state == BetterMovement.States.dashing)
                {
                    HitManager.ImpactHit();

                    if (player.parryVFX != null) Destroy(player.parryVFX);

                    AudioHandler.PlaySound("pH");
                    player.isInvincible = false;

                    return;
                }


                if (!player.isInvincible)
                {
                    camera.shakeDuration = .1f;
                    camera.shakeMagnitude = .1f;
                    HitManager.ImpactHit();
                    hp.TakeDamage(projectileDamage);

                    StartCoroutine(IFrame());

                    AudioHandler.PlaySound("pH");

                    player.knockBack(gameObject);
                }
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

    IEnumerator IFrame()
    {
        BetterMovement player = GameObject.FindGameObjectWithTag("Player").GetComponent<BetterMovement>();

        player.isInvincible = true;

        yield return new WaitForSeconds(2f);

        player.isInvincible = false;
    }
}

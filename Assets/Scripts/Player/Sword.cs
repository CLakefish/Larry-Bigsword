/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*
 * Name: Carson Lakefish
 * Date: 3 / 23 / 2023
 * Desc: Sword Collisions
 *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sword : MonoBehaviour
{
    public GameObject player;
    public GameObject particleHit;
    internal BetterEnemy enemy;
    GameObject recentlyAttacked;

    public AudioClip hitSound, dieSound;
    AudioSource audioSrc;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        audioSrc = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthPoints hp = collision.gameObject.GetComponent<HealthPoints>();
        CameraManager camera = GameObject.FindGameObjectWithTag("MainCamera").gameObject.GetComponent<CameraManager>();

        if (hp != null && collision.gameObject != recentlyAttacked)
        {
            if (collision.gameObject.GetComponent<BetterEnemy>() != null && collision.gameObject.GetComponent<BetterEnemy>().isHit) return;
            if (collision.gameObject.tag == "enemy" && !collision.gameObject.GetComponent<BetterEnemy>().isHit)
            {
                enemy = collision.gameObject.GetComponent<BetterEnemy>();

                camera.shakeDuration = .07f;
                camera.shakeMagnitude = .07f;

                Instantiate(particleHit, enemy.transform.position, enemy.transform.rotation);

                audioSrc.PlayOneShot(hitSound, 10f);

                enemy.isHit = true;
                enemy.state = BetterEnemy.States.none;
                enemy.knockBack(player.gameObject, -1);

                HitManager.ImpactHit();

                if (hp.currentHP - 1 == 0) audioSrc.PlayOneShot(dieSound);

                hp.TakeDamage(1);

                return;
            }

        }
            
        recentlyAttacked = collision.gameObject;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HealthPoints hp = collision.gameObject.GetComponent<HealthPoints>();
        CameraManager camera = GameObject.FindGameObjectWithTag("MainCamera").gameObject.GetComponent<CameraManager>();

       if (hp != null && collision.gameObject != recentlyAttacked)
        {
            if (collision.gameObject.GetComponent<BetterEnemy>() != null && collision.gameObject.GetComponent<BetterEnemy>().isHit) return;
            if (collision.gameObject.tag == "enemy" && !collision.gameObject.GetComponent<BetterEnemy>().isHit)
            {
                enemy = collision.gameObject.GetComponent<BetterEnemy>();

                camera.shakeDuration = .07f;
                camera.shakeMagnitude = .07f;

                Instantiate(particleHit, enemy.transform.position, enemy.transform.rotation);

                audioSrc.PlayOneShot(hitSound, 10f);

                enemy.isHit = true;
                enemy.state = BetterEnemy.States.none;
                enemy.knockBack(player.gameObject, -1);

                HitManager.ImpactHit();

                if (hp.currentHP - 1 == 0) audioSrc.PlayOneShot(dieSound);

                hp.TakeDamage(1);

                return;
            }


            recentlyAttacked = collision.gameObject;
        }
    }
}

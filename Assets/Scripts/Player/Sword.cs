using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.PlayerSettings;

public class Sword : MonoBehaviour
{
    public GameObject player;
    GameObject recentlyAttacked;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthPoints hp = collision.gameObject.GetComponent<HealthPoints>();

        if (hp != null && collision.gameObject != recentlyAttacked)
        {
            // Projectile Deflect Case
            if (collision.gameObject.tag == "Projectile")
            {
                hp.TakeDamage(1);
                Destroy(gameObject);
                return;
            }
            if (collision.gameObject.tag == "enemy" && !collision.gameObject.GetComponent<Enemy>().isHit)
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();

                HitManager.ImpactHit();

                enemy.StartCoroutine(enemy.knockBack(transform.position));

                hp.TakeDamage(1);
                return;
            }

        }
            
        recentlyAttacked = collision.gameObject;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HealthPoints hp = collision.gameObject.GetComponent<HealthPoints>();

        if (hp != null && collision.gameObject != recentlyAttacked)
        {
            // Projectile Deflect Case
            if (collision.gameObject.tag == "Projectile")
            {
                hp.TakeDamage(1);
                Destroy(gameObject);
                return;
            }
            if (collision.gameObject.tag == "enemy" && !collision.gameObject.GetComponent<Enemy>().isHit)
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();

                HitManager.ImpactHit();

                enemy.StartCoroutine(enemy.knockBack(transform.position));

                hp.TakeDamage(1);
                return;
            }

        }

        recentlyAttacked = collision.gameObject;
    }
}

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthPoints hp = collision.gameObject.GetComponent<HealthPoints>();

        if (hp != null && collision.gameObject != recentlyAttacked)
        {
            // Projectile Deflect Case
            if (collision.gameObject.tag == "Projectile")
            {
                hp.TakeDamage(1);

                Instantiate(particleHit, collision.gameObject.transform.position, collision.gameObject.transform.rotation);

                Destroy(gameObject);
                return;
            }
            if (collision.gameObject.tag == "enemy" && !collision.gameObject.GetComponent<BetterEnemy>().isHit)
            {
                enemy = collision.gameObject.GetComponent<BetterEnemy>();

                Instantiate(particleHit, enemy.transform.position, enemy.transform.rotation);

                HitManager.ImpactHit();

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


                Instantiate(particleHit, collision.gameObject.transform.position, enemy.transform.rotation);
                Destroy(gameObject);
                return;
            }
            if (collision.gameObject.tag == "enemy" && !collision.gameObject.GetComponent<BetterEnemy>().isHit)
            {
                enemy = collision.gameObject.GetComponent<BetterEnemy>();

                Instantiate(particleHit, enemy.transform.position, collision.gameObject.transform.rotation);

                HitManager.ImpactHit();

                hp.TakeDamage(1);
                return;
            }

        }

        recentlyAttacked = collision.gameObject;
    }
}

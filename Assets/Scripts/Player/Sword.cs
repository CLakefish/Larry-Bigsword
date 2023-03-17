using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Sword : MonoBehaviour
{
    public GameObject player;
    GameObject recentlyAttacked;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemy" && !collision.gameObject.GetComponent<Enemy>().isHit)
        {
            Debug.Log(player.GetComponent<Movement>().swingDamage);
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            enemy.StartCoroutine(enemy.knockBack(transform.position));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthPoints hp = collision.gameObject.GetComponent<HealthPoints>();
        Movement obj = player.GetComponent<Movement>();

        if (hp != null && collision.gameObject != recentlyAttacked)
        {
            // Projectile Deflect Case
            if (collision.gameObject.tag == "Projectile")
            {
                hp.TakeDamage(obj.swingDamage);
                obj.isSwinging = false;
                obj.swingCooldown = 0.3f;
                Destroy(gameObject);
            }
            else
            {
                obj.swingCooldown = 0.1f;
            }

            hp.TakeDamage(obj.swingDamage);
        }

        recentlyAttacked = collision.gameObject;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HealthPoints hp = collision.gameObject.GetComponent<HealthPoints>();
        Movement obj = player.GetComponent<Movement>();

        if (hp != null && collision.gameObject != recentlyAttacked)
        {
            // Projectile Deflect Case
            if (collision.gameObject.tag == "Projectile")
            {
                hp.TakeDamage(obj.swingDamage);
                obj.isSwinging = false;
                obj.swingCooldown = 0.3f;
                Destroy(gameObject);
            }
            else
            {
                obj.swingCooldown = 0.1f;
            }

            hp.TakeDamage(obj.swingDamage);
        }

        recentlyAttacked = collision.gameObject;
    }
}

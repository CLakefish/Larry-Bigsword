using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private float projSpeed;
    public GameObject projectile;
    [SerializeField] private float minRandom, maxRandom;
    GameObject player;
    [SerializeField] private float bulletAmount;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        InvokeRepeating(nameof(ProjectileFire), Random.Range(.1f, 1), 0.5f);
    }

    void ProjectileFire()
    {
        GameObject firedObj = Instantiate(projectile, transform.position, Quaternion.identity);

        if (bulletInterceptCalculations(transform.position, projSpeed, out Vector2 dir))
        {
            for (int i = 0; i < bulletAmount; i++)
            {
                firedObj.GetComponent<Rigidbody2D>().velocity = dir * projSpeed;
            }
        }
        else
        {
            for (int i = 0; i < bulletAmount; i++)
            {
                firedObj.GetComponent<Rigidbody2D>().velocity = (player.transform.position - this.transform.position).normalized * projSpeed;
            }
        }
    }

    public bool bulletInterceptCalculations(Vector2 shootPos, float projectileSpeed, out Vector2 dir)
    {
        // Vectors "Oh yeah!"
        Vector2 playerVel = player.GetComponent<Rigidbody2D>().velocity, 
                playerPos = player.transform.position, 
                distanceEtP = playerPos - shootPos;

        // Abc
        float a = (playerVel.x * playerVel.x) + (playerVel.y * playerVel.y) - (projectileSpeed * projectileSpeed), 
              b = 2 * (playerVel.x * distanceEtP.x + playerVel.y * distanceEtP.y), 
              c = (distanceEtP.x * distanceEtP.x) + (distanceEtP.y * distanceEtP.y);

        // Discriminant
        float disc = (b * b) - 4 * a * c;

        // if disc is less than 0 don't even try it
        if (disc < 0 || a == 0)
        {
            dir = Vector2.zero;
            return false;
        }

        // t1 && t2
        float t1 = (-b + Mathf.Sqrt(disc)) / (2 * a), 
              t2 = (-b - Mathf.Sqrt(disc)) / (2 * a);

        // Choose the smaller positive value
        float t = Mathf.Min(t1 > 0 ? t1 : Mathf.Infinity, t2 > 0 ? t2 : Mathf.Infinity);

        // Projectile fired direction
        dir = (playerPos + Random.Range(minRandom, maxRandom) * t * playerVel - shootPos).normalized;
        return true;
    }
}
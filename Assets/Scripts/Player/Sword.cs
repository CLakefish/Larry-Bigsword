using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Sword : MonoBehaviour
{
    public GameObject player;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemy" && !collision.gameObject.GetComponent<Enemy>().isHit)
        {
            Debug.Log(player.GetComponent<Movement>().swingDamage);
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.StartCoroutine(enemy.knockBack(transform.position));
        }
    }
}

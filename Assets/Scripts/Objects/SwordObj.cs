using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordObj : MonoBehaviour
{
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.GetComponent<BetterMovement>().hasSword = true;
            Destroy(gameObject);
        }
    }
}

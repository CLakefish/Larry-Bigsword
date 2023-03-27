using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject[] DoorObjs;

    public GameObject healthObj;

    private GameObject player;
    internal CameraController cs;

    public int instance = 0;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        DoorObjs = GameObject.FindGameObjectsWithTag("Door");
        cs = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("enemy");

        if (enemies.Length <= 0)
        {
            foreach (GameObject obj in DoorObjs)
            {
                if (obj.activeSelf)
                {
                    obj.SetActive(false);
                    gameObject.GetComponent<AudioSource>().Play();
                }
            }

            if (instance == 2)
            {
                SpawnObj();
                instance--;
            }
        }
        else
        {
            foreach (GameObject obj in DoorObjs)
            {
                if (!obj.activeSelf)
                {
                    obj.SetActive(true);
                    gameObject.GetComponent<AudioSource>().Play();
                    instance++;
                }
            }
        }
    }

    void SpawnObj()
    {
        if (player.GetComponent<HealthPoints>().currentHP == 1)
        {
            GameObject obj = Instantiate(healthObj, player.transform.position - (Vector3.left * 3), Quaternion.identity);
        }
        else
        {
            int i;

            i = (int)Random.Range(0, 1);

            if (i == 0)
            {
                GameObject obj = Instantiate(healthObj, player.transform.position - (Vector3.left * 3), Quaternion.identity);
            }
        }
    }

    public void closeDoors()
    {
        foreach (GameObject obj in DoorObjs)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                gameObject.GetComponent<AudioSource>().Play();
            }
        }
    }
}

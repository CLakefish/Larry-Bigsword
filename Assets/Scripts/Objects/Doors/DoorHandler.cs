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

    int instance = 0;

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
                }
            }

            if (instance == 1)
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
                    instance++;
                }
            }
        }
    }

    void SpawnObj()
    {
        if (player.GetComponent<HealthPoints>().currentHP == 1)
        {
            GameObject obj = Instantiate(healthObj, cs.bounds.transform.position, Quaternion.identity);
        }
        else
        {
            GameObject obj;
            int i;

            i = (int)Random.Range(0, 1);

            if (i == 0) obj = Instantiate(healthObj, cs.bounds.transform.position, Quaternion.identity);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject[] DoorObjs;

    private void Awake()
    {
        DoorObjs = GameObject.FindGameObjectsWithTag("Door");
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
        }
        else
        {
            foreach (GameObject obj in DoorObjs)
            {
                if (!obj.activeSelf)
                {
                    obj.SetActive(true);
                }
            }
        }
    }
}

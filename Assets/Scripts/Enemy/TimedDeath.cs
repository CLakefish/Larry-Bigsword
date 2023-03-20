/*
 *Nico Sayed 
 *3/15/23
 * This code will be in control of time of death.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDeath : MonoBehaviour
{
    public float deathTime = 1;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, deathTime);
    }
}
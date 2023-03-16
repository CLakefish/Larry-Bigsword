/*nico Sayed
 * 3/15/2023
 * this code is resposeible for all deth animation. 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathEffects : MonoBehaviour
{
    [Tooltip("list of functions to be run if this object dies (and has something that invokes it)")]
    public UnityEvent deathEvent = new();
    [Tooltip("Add a prefab here if you want this object to drop it on death (leave empty if not but has other death events)")]
    public GameObject spawnOnDeath;
    // Start is called before the first frame update
    void Start()
    {
        //add the OnDeath function to the death event
        deathEvent.AddListener(OnDeath);
    }

    void OnDeath()
    {
        if (spawnOnDeath != null)
        {
            Instantiate(spawnOnDeath, transform.position, Quaternion.identity);
        }
    }
}